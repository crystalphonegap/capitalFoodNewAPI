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
    //[Route("api/[controller]")]
    [ApiController]
    [Route("[controller]")]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUpload _fileUpload;

        private readonly ILogger _ILogger;
        public FileUploadController(IFileUpload fileUploadService, IWebHostEnvironment environment)
        {
            _fileUpload = fileUploadService;
        }

        //[HttpPost("ProductMasterUpload")]
        //public IActionResult ProductMasterUpload([FromForm] UploadFile uploadFile)
        //{
        //    try
        //    {
        //        return Ok(_fileUpload.ProductMasterUpload(uploadFile));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpGet("GetPlant")]
        public IActionResult GetPlant()
        {
            try
            {
                return Ok(_fileUpload.GetPlant());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("STNOutwardUpload")]
        public IActionResult STNOutwardUpload([FromForm] UploadFile uploadFile)
        {
            try
            {
                return Ok(_fileUpload.STNOutwardUpload(uploadFile));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("STNInwarddUpload")]
        public IActionResult STNInwardUpload([FromForm] UploadFile uploadFile)
        {
            try
            {
                return Ok(_fileUpload.STNInwardUpload(uploadFile));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("partyMasterUpload")]
        public IActionResult PartyMasterUpload([FromForm] UploadFile uploadFile)
        {
            try
            {
                return Ok(_fileUpload.PartyMasterUpload(uploadFile));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("PartyListOutward")]
        public IActionResult GetPartyListForSTN([FromQuery] DateFilterModel model)
        {
            try
            {
                return Ok(_fileUpload.GetPartyListForSTN(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PartyListInward")]
        public IActionResult GetPartyListForInward([FromQuery] DateFilterModel model)
        {
            try
            {
                return Ok(_fileUpload.GetPartyListForInward(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
