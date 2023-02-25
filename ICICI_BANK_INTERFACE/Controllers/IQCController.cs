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
    public class IQCController : Controller
    {
        private readonly IIQCService  _IQCService;

        private readonly ILogger _ILogger;
        public IQCController(IIQCService IQCService, IWebHostEnvironment environment)
        {
            _IQCService = IQCService;
           
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GETIQCPENDINGLIST/{PAGENUMBER},{PAGESIZE},{FROMDATE},{TODATE}")]
        public IActionResult GETIQCPENDINGLIST(int PAGENUMBER, int PAGESIZE,string FROMDATE,string TODATE)
        {


            try
            {
                return Ok(_IQCService.GETIQCPENDINGLIST(PAGENUMBER, PAGESIZE, FROMDATE, TODATE));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GETIQCDOCUMENTDETAILSBYID/{DOCUMENTID}")]
        public IActionResult GETIQCDOCUMENTDETAILSBYID(string DOCUMENTID)
        {


            try
            {
                return Ok(_IQCService.GETIQCDOCUMENTDETAILSBYID(DOCUMENTID));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DOCUMENTDEATISLINSERTUPDATE")]
        public IActionResult DOCUMENTDEATISLINSERTUPDATE(IQC_DETAIL_Model model)
        {
            try
            {
                return Ok(_IQCService.DOCUMENTDEATISLINSERTUPDATE(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

 
}
