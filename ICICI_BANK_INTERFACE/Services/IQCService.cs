using Dapper;
using ICICI_BANK_INTERFACE.Interface;
using ICICI_BANK_INTERFACE.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace ICICI_BANK_INTERFACE.Services
{
    public class IQCService : IIQCService
    {

        private readonly IHomeService _IHomeService;
        private static IWebHostEnvironment _hostEnvironment;

        private readonly IConfiguration _config;
        string DbName = "Audiplus_Test1";

        public IQCService(IWebHostEnvironment environment, IConfiguration config)
        {
            _config = config;
            _hostEnvironment = environment;
        }


        public DataTable GETIQCPENDINGLIST(int PAGENUMBER, int PAGESIZE, string FROMDATE, string TODATE)
        {
            SqlConnection con = new SqlConnection(_config.GetConnectionString("DatabaseContext3"));
            SqlCommand cmd;
            DataTable dt = new DataTable();
            try
            {

                //IQC_HEADER_Model IQC_HEADER_Model = new IQC_HEADER_Model();
                cmd = new SqlCommand("PRC_IQC_GETIQUC_DOCUMENTLIST", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_ID", 0);
                cmd.Parameters.AddWithValue("@P_PAGENO", PAGENUMBER);
                cmd.Parameters.AddWithValue("@P_PAPGESIZE", PAGESIZE);
                cmd.Parameters.AddWithValue("@P_FROMDATE", Convert.ToDateTime(FROMDATE));
                cmd.Parameters.AddWithValue("@P_TODATE", Convert.ToDateTime(TODATE));

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);

            }
            catch (SqlException e)
            {

            }
            return dt;


        }

        public DataSet GETIQCDOCUMENTDETAILSBYID(string DOCUMENTID)
        {
            SqlConnection con = new SqlConnection(_config.GetConnectionString("DatabaseContext3"));
            SqlCommand cmd;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {

                //IQC_HEADER_Model IQC_HEADER_Model = new IQC_HEADER_Model();
                cmd = new SqlCommand("PRC_IQC_GETIDOCUMENTDETAILS_BYID", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_DOCUMENTID", DOCUMENTID);
                
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ds);

            }
            catch (SqlException e)
            {

            }
            return ds;


        }


        public DataSet DOCUMENTDEATISLINSERTUPDATE(IQC_DETAIL_Model model)
        {
            SqlConnection con = new SqlConnection(_config.GetConnectionString("DatabaseContext3"));
            SqlCommand cmd;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            con.Open();
            try
            {

                //IQC_HEADER_Model IQC_HEADER_Model = new IQC_HEADER_Model();
                cmd = new SqlCommand("PRC_IQC_DOCUMENTDETAILS_IUD", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@P_DCOENTRY", model.DocEntry);
                cmd.Parameters.AddWithValue("@P_USERIALNUMBER", model.U_SerialNumber);
                cmd.Parameters.AddWithValue("@P_ACTION", 'I');
                
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
              
                sda.Fill(ds);

            }
            catch (SqlException e)
            {

            }finally
            {
                con.Close();
            }
            return ds;


        }
    }
}
