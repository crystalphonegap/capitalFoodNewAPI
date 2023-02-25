using ICICI_BANK_INTERFACE.Models;
using System.Data;

namespace ICICI_BANK_INTERFACE.Interface
{
   public  interface IIQCService
    {
        DataTable GETIQCPENDINGLIST(int PAGENUMBER, int PAGESIZE, string FROMDATE, string TODATE);
        DataSet GETIQCDOCUMENTDETAILSBYID(string DOCUMENTID);
        DataSet DOCUMENTDEATISLINSERTUPDATE(IQC_DETAIL_Model model);
    }
}
