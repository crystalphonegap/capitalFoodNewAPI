using ICICI_BANK_INTERFACE.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ICICI_BANK_INTERFACE.Interface
{
    public interface IFileUpload
    {
        //public string ProductMasterUpload(UploadFile uploadFile);
        public DataTable GetPlant();
        public string STNOutwardUpload(UploadFile uploadFile);
        public string STNInwardUpload(UploadFile uploadFile);
        public string PartyMasterUpload(UploadFile uploadFile);
        public DataTable GetPartyListForSTN(DateFilterModel model);
        public DataTable GetPartyListForInward(DateFilterModel model);
    }
}
