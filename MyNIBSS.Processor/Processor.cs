using MyNIBSS.Core.Models;
using MyNIBSS.Data.Repositories;
using MyNIBSS.Logic;
using MyNIBSS.Processor.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Trx.Messaging;
using Trx.Messaging.Channels;
using Trx.Messaging.FlowControl;
using Trx.Messaging.Iso8583;

namespace MyNIBSS.Processor
{
    public class Processor
    {

        UtilityLogic utility = new UtilityLogic();

        static FinancialInstitution GetFinancialInstitution(Iso8583Message msg)
        {
            var accountNum1 = Convert.ToInt64(Convert.ToString(msg.Fields[102]));
            var accountNum2 = Convert.ToInt64(Convert.ToString(msg.Fields[103]));
            var getCustomerAccount1 = new CustomerAccountRepository().GetByAcctNum(accountNum1);
            var getCustomerAccount2 = new CustomerAccountRepository().GetByAcctNum(accountNum2);
            var InstitutionCode = Convert.ToString(msg.Fields[33]);
            var getFinancialInstitution = new FinancialInstitution();
            string transactionTypeCode = msg.Fields[MessageField.TRANSACTION_TYPE_FIELD].ToString().Substring(0, 2);
            if (InstitutionCode != null)
            {
                getFinancialInstitution = new FinancialInstitutionRepository().GetByInstitutionCode(InstitutionCode);
            }
            else if (getCustomerAccount1 != null && transactionTypeCode=="01" )
            {
                var institutionCode = getCustomerAccount1.InstitutionCode;
                getFinancialInstitution = new FinancialInstitutionRepository().GetByInstitutionCode(institutionCode);
                if (getFinancialInstitution == null)
                {
                    getFinancialInstitution = null;
                }
            }
            else if(getCustomerAccount1!=null && getCustomerAccount2!=null && transactionTypeCode == "71")
            {
                var institutionCode = getCustomerAccount2.InstitutionCode;// get theinstitution code of the bank we are transferring to
                getFinancialInstitution = new FinancialInstitutionRepository().GetByInstitutionCode(institutionCode);
                if (getFinancialInstitution == null)
                {
                    getFinancialInstitution = null;
                }

            }
            return getFinancialInstitution;
        }

        public static void Listener_Receive(object sender, ReceiveEventArgs e)
        {
            //MessageLogger.LogMessage("Receiving new Message...");
            Console.WriteLine("Receiving new Message...");
            var listenerPeer = sender as ListenerPeer;       //ATM
            int NodeId = Convert.ToInt32(listenerPeer.Name);  //the sourceNode's ID was used as the name of the listenerPeer
            Iso8583Message theIsoMessage = e.Message as Iso8583Message;
            var getFinancialInstitution = GetFinancialInstitution(theIsoMessage);
            if (getFinancialInstitution != null)
            {
               TransactionManager.LogTransaction(theIsoMessage, getFinancialInstitution);
            }
            //Log the incoming message before validating it
            //MessageLogger.LogMessage("Validating incoming message...");
            Console.WriteLine("Validating incoming message...");
            Program.ConnectSinkNode(getFinancialInstitution.Id);
            Iso8583Message responseFromFEP = TransactionManager.ProcessIncommingMessage(getFinancialInstitution.Id, theIsoMessage);

            //Log the response message as a transaction wether successful or not
            TransactionManager.LogTransaction(responseFromFEP, getFinancialInstitution);
                   //sends message to the client                    
            //MessageLogger.LogMessage("Sending response back to the source...");
            Console.WriteLine("Sending response back to the source...");
            SendMessage(listenerPeer, responseFromFEP);    //sends response msg back to the source, ATM in this case            
            //listenerPeer.Close();
            //listenerPeer.Dispose();
        }

        static void Listener_Connected(object sender, EventArgs e)
        {
            var listener = sender as ListenerPeer;
            UtilityLogic.LogMessage(listener.Name + " is now connected");
            //Console.WriteLine("Client Connected!");
        }
        static void Listener_Disconnected(object sender, EventArgs e)
        {
            var listener = sender as ListenerPeer;
            UtilityLogic.LogMessage(listener.Name + " is disonnected");
            //Console.WriteLine("Client Disconnected!");
        }

        public static void ListenerPeer(Node sourceNode)     //create conn
        {
            var port = Convert.ToInt32(sourceNode.Port);
            Console.WriteLine($"Port is {port}");
            TcpListener tcpListener = new TcpListener(port);
            tcpListener.LocalInterface = sourceNode.IPAddress;
            Console.WriteLine($"Logging IP Address --------- {tcpListener.LocalInterface} ------- {port}");
            ListenerPeer listener = new ListenerPeer(sourceNode.Id.ToString(),
                     new TwoBytesNboHeaderChannel(new Iso8583Ascii1987BinaryBitmapMessageFormatter()),
                     new BasicMessagesIdentifier(11, 41),
                     tcpListener);
            Console.WriteLine($"Successfully set up stuff ---- {port}");
            //Program.Connects(tcpListener.LocalInterface, port);
            listener.Receive += Listener_Receive;
            listener.Connected += Listener_Connected;

            listener.Connect();
            Console.WriteLine("Listening for connection.. on " + sourceNode.Port);
        }

        static Message SendMessage(Peer peer, Message msg)
        {
            int maxRetries = 3; int numberOfRetries = 1;
            while (numberOfRetries < maxRetries)
            {
                if (peer.IsConnected)
                {
                    break;
                }
                peer.Close();
                numberOfRetries++;
                peer.Connect();
                Thread.Sleep(2000);
            }

            if (peer.IsConnected)
            {
                try
                {
                    var request = new PeerRequest(peer, msg);

                    request.Send();

                    //At this point, the message has been sent to the SINK for processing
                    int serverTimeout = 100000000;
                    request.WaitResponse(serverTimeout);

                    var response = request.ResponseMessage;
                    return response;
                }
                catch (Exception ex)
                {
                    msg.Fields.Add(39, "06"); // ERROR
                    MessageLogger.LogError("Error sending message " + ex.Message + "   Inner Exception:  " + ex.InnerException);
                    return msg;
                }
            }
            else
            {
                msg.Fields.Add(39, ResponseCode.ISSUER_OR_SWITCH_INOPERATIVE); // ERROR
                return msg;
            }

        }
        static void Client_RequestCancelled(object sender, PeerRequestCancelledEventArgs e)
        {
            ///
            Console.WriteLine("Peer request cancelled");
        }

        static void Client_RequestDone(object sender, PeerRequestDoneEventArgs e)
        {
            //
            Console.WriteLine("Peer request done");
        }
        public static void ClientPeer(FinancialInstitution sinkNodeToConnect, List<ClientPeer> clientPeers)    //join conn
        {
            ClientPeer client = new ClientPeer(sinkNodeToConnect.Id.ToString(),
                                new TwoBytesNboHeaderChannel(new Iso8583Ascii1987BinaryBitmapMessageFormatter(), sinkNodeToConnect.IPAddress, Convert.ToInt32(sinkNodeToConnect.Port)), new BasicMessagesIdentifier(11, 41));
            client.RequestDone += new PeerRequestDoneEventHandler(Client_RequestDone);
            client.RequestCancelled += new PeerRequestCancelledEventHandler(Client_RequestCancelled);
            client.Connected += client_Connected;
            client.Receive += new PeerReceiveEventHandler(Client_Receive);
            clientPeers.Add(client);
            client.Connect();

            Console.WriteLine("Waiting for connection..");
        }

        static void Client_Receive(object sender, ReceiveEventArgs e)
        {
            ClientPeer clientPeer = sender as ClientPeer;
            //logger.Log("Connected to ==> " + clientPeer.Name);

            Iso8583Message receivedMsg = e.Message as Iso8583Message;
        }
        static void client_Connected(object sender, EventArgs e)
        {
            Console.WriteLine("Client Connected");
        }
    }
}
