using ICICI_BANK_INTERFACE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICICI_BANK_INTERFACE.Interface
{
    public interface IAdminUser
    {
        public string PostPlant(PlantMasterVM plantmaster);
    }
}
