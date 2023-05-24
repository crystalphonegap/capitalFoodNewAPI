using ICICI_BANK_INTERFACE.Interface;
using ICICI_BANK_INTERFACE.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICICI_BANK_INTERFACE.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class PartyMasterController : ControllerBase
    {
        private readonly IPartyMaster _partyMaster;

        private readonly ILogger _ILogger;
        public PartyMasterController(IPartyMaster partymaster, IWebHostEnvironment environment)
        {
            _partyMaster = partymaster;
            //_IHomeService.InsertBarCodeFromAnotherTable();
        }

        //Inward Item list
        [HttpGet("GetItemListInward")]
        public IActionResult GetItemListInward([FromQuery] ItemListFilter model)
        {
            try
            {
                return Ok(_partyMaster.GetItemListInward(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Outward Item list
        [HttpGet("GetItemListOutward")]
        public IActionResult GetItemListOutward([FromQuery] ItemListFilter model)
        {
            try
            {
                return Ok(_partyMaster.GetItemListOutward(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        ////Outward Item list
        //[HttpGet("GetItemListOutward")]
        //public IActionResult GetItemList(ItemListFilter model)
        //{
        //    try
        //    {
        //        return Ok(_partyMaster.GetItemListInward(model));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpGet("GetQrCodeListInward")]
        public IActionResult GetQrCodeListInward([FromQuery] ItemListFilter model)
        {
            try
            {
                return Ok(_partyMaster.GetQrCodeListInward(model));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetQrCodeListOutward")]
        public IActionResult GetQrCodeListOutward([FromQuery] ItemListFilter model)
        {
            try
            {
                return Ok(_partyMaster.GetQrCodeListOutward(model));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("InsertBarCodeInward")]
        public IActionResult InsertBarCodeInward(InsertBarcodeDetailsModel Model)
        {
            try
            {
                return Ok(_partyMaster.InsertBarCodeInward(Model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("InsertBarCodeOutward")]
        public IActionResult InsertBarCodeOutward(InsertBarcodeDetailsModel Model)
        {
            try
            {
                return Ok(_partyMaster.InsertBarCodeOutward(Model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("InsertBarCodeByBarcodeInward")]
        public IActionResult InsertBarCodeByBarcodeInward(InsertBarcodeDetailsModel Model)
        {
            try
            {
                return Ok(_partyMaster.InsertBarCodeByBarcodeInward(Model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("InsertBarCodeByBarcodeOutward")]
        public IActionResult InsertBarCodeByBarcodeOutward(InsertBarcodeDetailsModel Model)
        {
            try
            {
                return Ok(_partyMaster.InsertBarCodeByBarcodeOutward(Model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteQrCodeInward")]
        public IActionResult DeleteQrCodeInward(InsertBarcodeDetailsModel Model)
        {
            try
            {
                ResponseModel responseModel = new ResponseModel();
                responseModel.data = _partyMaster.DeleteQrCodeInward(Model).ToString().Trim();
                return Ok(responseModel);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteQrCodeOutward")]
        public IActionResult DeleteQrCodeOutward(InsertBarcodeDetailsModel Model)
        {
            try
            {
                ResponseModel responseModel = new ResponseModel();
                responseModel.data = _partyMaster.DeleteQrCodeOutward(Model).ToString().Trim();
                return Ok(responseModel);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
