using ICICI_BANK_INTERFACE.Models;
using System.Data;

namespace ICICI_BANK_INTERFACE.Interface
{
    public interface IHomeService
    {
        string InsertBarCodeFromAnotherTable();
        DataTable GetPartyList(DateFilterModel model);
        DataTable GetItemList(ItemListFilter model);
        DataTable GetQrCodeList(ItemListFilter model);
        DataTable Login(UserMasterModel userMaster);
        string Register(RegisterUserModel registerUser);
        string DeleteQrCode(InsertBarcodeDetailsModel Model);
        ResponseModel InsertBarCode(InsertBarcodeDetailsModel Model);
        ResponseModel InsertBarCodeByBarcode(InsertBarcodeDetailsModel Model);
    }
}
