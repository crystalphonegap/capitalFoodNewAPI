using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICICI_BANK_INTERFACE.Models
{
    public class InsertBarcodeDetailsModel
    {
        public int SerialId { get; set; }
        public string Type { get; set; }
        public string DocDate { get; set; }
        public string SeriesName { get; set; }
        public string CardName { get; set; }
        public string DocNum { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public string UserCode { get; set; }
        public string QTY { get; set; }
        public string Barcode { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }
}
