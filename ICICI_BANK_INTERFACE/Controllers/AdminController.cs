using ICICI_BANK_INTERFACE.Interface;
using ICICI_BANK_INTERFACE.Models;
using Microsoft.AspNetCore.Hosting;
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
    public class AdminController : Controller
    {
        private readonly IAdminUser _adminuser;

        private readonly ILogger _ILogger;
        public AdminController(IAdminUser IAdminUser, IWebHostEnvironment environment)
        {
            _adminuser = IAdminUser;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("PostPlant")]
        public IActionResult PostPlant(PlantMasterVM model)
        {
            try
            {
                return Ok(_adminuser.PostPlant(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
