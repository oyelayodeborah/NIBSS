using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNIBSS.Core.Models
{
    public class Transaction
    {
        public int ID { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string FromAccountNumber { get; set; }
        public string ToAccountNumber { get; set; }

        public string CardPAN { get; set; }
        public string STAN { get; set; }
        public string ChannelCode { get; set; }

        public string InstitutionCode { get; set; }

        public string TransactionTypeCode { get; set; }

        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
    }
}
