using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNIBSS.Core.Models
{
    public class FinancialInstitution:Entity
    {
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "IP Address")]

        public string IPAddress { get; set; }
        [Display(Name = "Port")]

        public int Port { get; set; }
        [Display(Name = "Institution Code")]

        public string InstitutionCode { get; set; }

        public Status Status { get; set; }
    }
}
