using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nuget.LogService.Models
{
    public class CreateErrorIn
    {

        public int SeverityID { get; set; }
        public string? Description { get; set; }
        public string? UserID { get; set; }
        public string? TransactionID { get; set; }
        public string? Code { get; set; }
        public string? Component { get; set; }
        public string? Machine { get; set; }
        public DateTime Date { get; set; }

    }
}
