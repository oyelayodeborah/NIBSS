using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
//using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MyNIBSS.Core.Models;
using MyNIBSS.Data.Repositories;
using Trx.Messaging;
using Trx.Messaging.Channels;
using Trx.Messaging.FlowControl;
//using Trx.Messaging.FlowControl;
using Trx.Messaging.Iso8583;

namespace MyNIBSS.Processor
{
    class Program
    {
        public static Processor processor = new Processor();
        public static List<Node> AllNodes = new NodeRepository().GetByStatus(Status.Active).ToList();   //select only the active nodes
        public static List<ClientPeer> ClientPeers = new List<ClientPeer>();


        static void Main(string[] args)
        {
            //InitializeTcpListener();

            //CbaListener.StartUpListener("1","192.168.43.160", 60012);
            //processor.BeginProcess();

            //processor.BeginProcess();

            //InitializeTcpListener();
            // Console.WriteLine("Started the processor");
            Console.WriteLine("Initializing listener");
            InitializeTcpListener();
            Console.ReadLine();
            Console.WriteLine("Transaction ended");
            Console.Read();
        }
        public static void ConnectSinkNode(int sinkNodeId)
        {
            try
            {
                FinancialInstitution sink = new FinancialInstitutionRepository().Get(sinkNodeId);
                Processor.ClientPeer(sink, ClientPeers);
                sink.Status = Status.Active;
                new FinancialInstitutionRepository().Update(sink);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static void InitializeTcpListener()
        {
            foreach (var source in AllNodes)
            {
                Console.WriteLine("About to call ListenerPeer method....");
                Processor.ListenerPeer(source);
                source.Status = Status.Active;
                new NodeRepository().Update(source);
            }
        }
    }
}
