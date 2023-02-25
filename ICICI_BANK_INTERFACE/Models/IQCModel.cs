using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICICI_BANK_INTERFACE.Models
{
    public class IQCModel
    {
    }
    public class IQC_HEADER_Model
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public int Period { get; set; }
        public int Instance { get; set; }
        public int Series { get; set; }
        public string Handwrtten { get; set; }
        public string Canceled { get; set; }
        public string Object { get; set; }
        public int UserSign { get; set; }
        public string Transfered { get; set; }
        public string Status { get; set; }
        public string CreateDate { get; set; }

        public string CreateTime { get; set; }
        public string UpdateDate { get; set; }
        public int VisOrder { get; set; }
        public string UpdateTime { get; set; }
        public string DataSource { get; set; }
        public string RequestStatus { get; set; }
        public string Creator { get; set; }
        public string Remark { get; set; }
        public int U_GRNDocEntry { get; set; }
        public string U_GRNNumber { get; set; }
        public string U_GRNDate { get; set; }
        public string U_CardCode { get; set; }

        public string U_CardName { get; set; }
        public string U_ItemCode { get; set; }
        public string U_ItemName { get; set; }
        public int U_IQCQTY { get; set; }
        public string U_ComplQty { get; set; }

        public int Total { get; set; }
        public string Pending { get; set; }


    }
    public class IQC_DETAIL_Model
    {

        public int DocEntry { get; set; }
        public int LineId { get; set; }
        public int VisOrder { get; set; }
        public string Object { get; set; }
        public int LogInst { get; set; }
        public string U_ItemCode { get; set; }
        public string U_SerialNumber { get; set; }
        public string U_QCDate { get; set; }
        public string U_Remarks { get; set; }
        public string U_ItemName { get; set; }
        public string U_QCResult { get; set; }
        public int U_isSTNDone { get; set; }
    }
}
