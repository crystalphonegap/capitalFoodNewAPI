using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICICI_BANK_INTERFACE.Models
{
    public class ItemListFilter
    {
        public int ID { get; set; }
        public string PartyName { get; set; }
        public string DC { get; set; }
        public string UserCode { get; set; }
        public string ItemCode { get; set; }
    }
}
