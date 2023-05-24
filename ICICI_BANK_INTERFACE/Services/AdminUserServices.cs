using ICICI_BANK_INTERFACE.Interface;
using ICICI_BANK_INTERFACE.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ICICI_BANK_INTERFACE.Services
{
    public class AdminUserServices : IAdminUser
    {
        private static IWebHostEnvironment _hostEnvironment;

        private readonly IConfiguration _config;
        string DbName = "CF_BARCODE";
        public AdminUserServices(IWebHostEnvironment environment, IConfiguration config)
        {
            _config = config;
            _hostEnvironment = environment;
        }
        public string PostPlant(PlantMasterVM plantmaster)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext2")))
                {
                    sqlConn.Open();
                    using (SqlCommand sqlCmd = new SqlCommand("USP_InsPlantMaster", sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@plantCode", plantmaster.PlantCode.Trim());
                        sqlCmd.Parameters.AddWithValue("@plantName", plantmaster.PlantName.Trim());
                       
                        sqlCmd.ExecuteNonQuery();
                        sqlConn.Close();
                        sqlConn.Dispose();
                    }
                }

            }
            catch (Exception ex)
            {
                return "Data is not uploaded properly";
            }

            return "Plant Uploaded Successfully!!!";
        }



    }
}
