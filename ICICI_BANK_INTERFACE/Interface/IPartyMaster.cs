using ICICI_BANK_INTERFACE.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ICICI_BANK_INTERFACE.Interface
{
    public interface IPartyMaster
    {
        DataTable GetItemListInward(ItemListFilter model);
        DataTable GetItemListOutward(ItemListFilter model);
        DataTable GetQrCodeListInward(ItemListFilter model);
        DataTable GetQrCodeListOutward(ItemListFilter model);
        string DeleteQrCodeInward(InsertBarcodeDetailsModel Model);
        string DeleteQrCodeOutward(InsertBarcodeDetailsModel Model);
        ResponseModel InsertBarCodeInward(InsertBarcodeDetailsModel Model);
        ResponseModel InsertBarCodeOutward(InsertBarcodeDetailsModel Model);
        ResponseModel InsertBarCodeByBarcodeInward(InsertBarcodeDetailsModel Model);
        ResponseModel InsertBarCodeByBarcodeOutward(InsertBarcodeDetailsModel Model);
    }
}
