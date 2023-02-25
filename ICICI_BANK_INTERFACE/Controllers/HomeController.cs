using ICICI_BANK_INTERFACE.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ICICI_BANK_INTERFACE.Models;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Org.BouncyCastle.Asn1.X509;
using System.Net;
using Org.BouncyCastle.Crypto.Tls;
using System.Text.RegularExpressions;

namespace ICICI_BANK_INTERFACE.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _IHomeService; 
         
        private readonly ILogger _ILogger; 
        public HomeController(IHomeService IHomeService, IWebHostEnvironment environment)
        { 
            _IHomeService = IHomeService;
             //_IHomeService.InsertBarCodeFromAnotherTable();
        }

        
        [HttpGet("Index")]
        public IActionResult Index()
        {
            var data = "Studio Master BarCode Scanner";
            return Ok(data);
        }


        //[HttpPost("GetData")]
        //public IActionResult GetData(DateFilterModel model)
        //{
        //    try
        //    {

        //        DateTime fdate = DateTime.ParseExact(model.FromDate, "dd-MM-yyyy", null);
        //        DateTime tdate = DateTime.ParseExact(model.ToDate, "dd-MM-yyyy", null);
        //        DataTable dt = new DataTable();
        //        using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext")))
        //        {
        //            String StrQuery = "INSERT INTO  [{0}].[dbo].[TempBarCodes] (Type,DcDate, DcSeries, DcNumber,Customer,ItemCode,Description,Qty,UserId)" +
        //                " select 'DC' as Type , odln.docdate as DcDate, nnm1.seriesname as DcSeries ,  LTRIM(STR(odln.docnum)) as DcNumber , " +
        //                " odln.cardname as Customer , dln1.itemcode as ItemCode,  replace(dln1.dscription, ',', ' - ') as Description, dln1.quantity as Qty,BCUM.ID" +
        //                " from odln inner join dln1 on odln.docentry = dln1.docentry" +
        //                " inner join[{0}].[dbo].[BarCodeUserMaster] BCUM on BCUM.Warehouse COLLATE DATABASE_DEFAULT = DLN1.WHSCODE  COLLATE DATABASE_DEFAULT  or" +
        //                " DLN1.WHSCODE COLLATE DATABASE_DEFAULT = BCUM.Location   COLLATE DATABASE_DEFAULT" +
        //                " inner join nnm1 on odln.series = nnm1.series" +
        //                " WHERE odln.DOCDATE >= CONVERT(DATETIME, '{1}', 102)  AND" +
        //                " odln.DOCDATE <= CONVERT(DATETIME,'{2}', 102)  AND " +
        //                " (BCUM.UserCode = '{3}')  ORDER BY odln.DOCDATE , odln.DOCNUM ";
        //            StrQuery = string.Format(StrQuery, DbName, fdate, tdate, model.UserCode);

        //            using (SqlCommand sqlCmd = new SqlCommand(StrQuery, sqlConn))
        //            {
        //                sqlCmd.CommandType = CommandType.Text;
        //                sqlConn.Open();
        //                sqlCmd.ExecuteNonQuery();

        //            }
        //            StrQuery = "SELECT distinct [Type] ,[DcDate],[DcSeries]+[DcNumber] as DcSeries ,[Customer]" +
        //             " ,0 as Approved   FROM [{0}].[dbo].[TempBarCodes]  TBC" +
        //             " inner join [{0}].[dbo].[BarCodeUserMaster] BCUM on BCUM.ID = TBC.UserId" +
        //             " where BCUM.UserCode ='{1}'";
        //            StrQuery = string.Format(StrQuery, DbName, model.UserCode);

        //            using (SqlCommand sqlCmd = new SqlCommand(StrQuery, sqlConn))
        //            {
        //                sqlCmd.CommandType = CommandType.Text;
        //                using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
        //                {
        //                    sqlAdapter.Fill(dt);
        //                }
        //            }
        //        }
        //        return Ok(dt);

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}


        [HttpPost("GetPartyList")]
        public IActionResult GetPartyList(DateFilterModel model)
        {
            try
            {
                return Ok(_IHomeService.GetPartyList(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("GetItemList")]
        public IActionResult GetItemList(ItemListFilter model)
        {
            try
            {
                return Ok(_IHomeService.GetItemList(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("DeleteQrCode")]
        public IActionResult DeleteQrCode(InsertBarcodeDetailsModel Model)
        {
            try
            { 
                ResponseModel responseModel = new ResponseModel();
                responseModel.data = _IHomeService.DeleteQrCode(Model).ToString().Trim();
                return Ok(responseModel);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPost("GetQrCodeList")]
        public IActionResult GetQrCodeList(ItemListFilter model)
        {
            try
            {
                return Ok(_IHomeService.GetQrCodeList(model));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Login")]
        public IActionResult Login(UserMasterModel userMaster)
        {
            try
            {
                return Ok(_IHomeService.Login(userMaster));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("InsertBarCode")]
        public IActionResult InsertBarCode(InsertBarcodeDetailsModel Model)
        {
            try
            { 
                return Ok(_IHomeService.InsertBarCode(Model) );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("InsertBarCodeByBarcode")]
        public IActionResult InsertBarCodeByBarcode(InsertBarcodeDetailsModel Model)
        {
            try
            {  
                    return Ok(_IHomeService.InsertBarCodeByBarcode(Model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
