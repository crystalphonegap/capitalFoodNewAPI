using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICICI_BANK_INTERFACE.Models
{
    public class RegisterUserModel
    {
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string UserType { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string PlantCode { get; set; }
        public string Password { get; set; }
        public string Location { get; set; }
        public string Warehouse { get; set; }
        public string CreatedBy { get; set; }
        public ICollection<PlantMaster> Ids { get; set; }
    }

    public class PlantMaster
    {
        public int Id { get; set; }
        public string PlantCode { get; set; }
    }
}
