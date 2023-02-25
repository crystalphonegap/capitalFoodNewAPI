using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICICI_BANK_INTERFACE.Models
{
    public class DateFilterModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Keyword { get; set; }
        public string UserCode { get; set; }
        public string UserType { get; set; }
    }
}
