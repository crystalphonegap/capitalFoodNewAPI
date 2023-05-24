using ExcelDataReader;
using ICICI_BANK_INTERFACE.Interface;
using ICICI_BANK_INTERFACE.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ICICI_BANK_INTERFACE.Services
{
    public class FileUploadService : IFileUpload
    {
        private static IWebHostEnvironment _hostEnvironment;

        private readonly IConfiguration _config;
        IExcelDataReader reader;
        string DbName = "CF_BARCODE";
        public FileUploadService(IWebHostEnvironment environment, IConfiguration config)
        {
            _config = config;
            _hostEnvironment = environment;
        }

        #region Product Master Upload
        //public string ProductMasterUpload(UploadFile uploadFile)
        //{
        //    try
        //    {
        //        // Check the File is received

        //        if (uploadFile == null)
        //            throw new Exception("File is Not Received...");

        //        // Create the Directory if it is not exist
        //        string dirPath = Path.Combine(_hostEnvironment.WebRootPath, "ProductMaster");
        //        if (!Directory.Exists(dirPath))
        //        {
        //            Directory.CreateDirectory(dirPath);
        //        }

        //        // MAke sure that only Excel file is used 
        //        string dataFileName = Path.GetFileName(uploadFile.Attachment.FileName);

        //        string extension = Path.GetExtension(dataFileName);

        //        string[] allowedExtsnions = new string[] { ".xls", ".xlsx" };

        //        if (!allowedExtsnions.Contains(extension))
        //            throw new Exception("Sorry! This file is not allowed, make sure that file having extension as either.xls or.xlsx is uploaded.");

        //        // Make a Copy of the Posted File from the Received HTTP Request
        //        string saveToPath = Path.Combine(dirPath, dataFileName);

        //        using (FileStream stream = new FileStream(saveToPath, FileMode.Create))
        //        {
        //            uploadFile.Attachment.CopyTo(stream);
        //        }

        //        // USe this to handle Encodeing differences in .NET Core
        //        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        //        // read the excel file
        //        using (var stream = new FileStream(saveToPath, FileMode.Open))
        //        {
        //            if (extension == ".xls")
        //                reader = ExcelReaderFactory.CreateBinaryReader(stream);
        //            else
        //                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);

        //            DataSet ds = new DataSet();
        //            ds = reader.AsDataSet();
        //            reader.Close();
        //            if (ds != null && ds.Tables.Count > 0)
        //            {
        //                DataTable serviceDetails = ds.Tables[0];
        //                for (int i = 1; i < serviceDetails.Rows.Count; i++)
        //                {
        //                    using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext2")))
        //                    {
        //                        sqlConn.Open();
        //                        using (SqlCommand sqlCmd = new SqlCommand("Usp_InsProductMasterUploadTemp", sqlConn))
        //                        {

        //                            sqlCmd.CommandType = CommandType.StoredProcedure;
        //                            sqlCmd.Parameters.AddWithValue("@CLIENT", serviceDetails.Rows[i][0].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@SKU_CODE", serviceDetails.Rows[i][1].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@MATERIAL_DESC", serviceDetails.Rows[i][2].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@PARENT_SKU", serviceDetails.Rows[i][3].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@CATEGORY", serviceDetails.Rows[i][4].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@SUB_CATEGORY", serviceDetails.Rows[i][5].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@SUB_CATEGORY2", serviceDetails.Rows[i][6].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@GRAMMAGE", serviceDetails.Rows[i][7].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@UOM", serviceDetails.Rows[i][8].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@MAT_TYPE", serviceDetails.Rows[i][9].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@MAT_GROUP", serviceDetails.Rows[i][10].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@BRAND", serviceDetails.Rows[i][11].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@HSN_CODE", serviceDetails.Rows[i][12].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@SELF_LIFE", serviceDetails.Rows[i][13].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@UNITPERCASE", serviceDetails.Rows[i][14].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@MRP", serviceDetails.Rows[i][15].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@MRP_PP", serviceDetails.Rows[i][16].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@ACC_ASS_CODE", serviceDetails.Rows[i][17].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@ACC_ASSIGNMENT_GRP", serviceDetails.Rows[i][18].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@CROSS_DIS_CHAIN_CODE", serviceDetails.Rows[i][19].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@SD_STATUS", serviceDetails.Rows[i][20].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@CROSS_PLANT_MAT_STATUS", serviceDetails.Rows[i][21].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@PLANT_SPECIFIC_MAT_STATUS", serviceDetails.Rows[i][22].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@NET_WT", serviceDetails.Rows[i][23].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@GROSS_WT", serviceDetails.Rows[i][24].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@WT_UNIT", serviceDetails.Rows[i][25].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@LENGTH", serviceDetails.Rows[i][26].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@WIDTH", serviceDetails.Rows[i][27].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@HEIGHT", serviceDetails.Rows[i][28].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@UNIT_LBH", serviceDetails.Rows[i][29].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@VOLUME", serviceDetails.Rows[i][30].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@CREATED_ON", Convert.ToDateTime(serviceDetails.Rows[i][31].ToString()));
        //                            sqlCmd.Parameters.AddWithValue("@CREATED_BY", serviceDetails.Rows[i][32].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@UPDATED_AT", Convert.ToDateTime(serviceDetails.Rows[i][33].ToString()));
        //                            sqlCmd.Parameters.AddWithValue("@UPDATED_BY", serviceDetails.Rows[i][34].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@TAXPERCENT", serviceDetails.Rows[i][35].ToString());
        //                            sqlCmd.Parameters.AddWithValue("@DATE_TIME", Convert.ToDateTime(serviceDetails.Rows[i][36].ToString()));

        //                            sqlCmd.ExecuteNonQuery();

        //                            sqlConn.Close();
        //                            sqlConn.Dispose();
        //                        }
        //                    }

        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return "Data is not uploaded properly";
        //    }

        //    return "Data Uploaded Successfully!!!";
        //}
        #endregion

        public DataTable GetPlant()
        {
            //Get the STN list
            DataTable dt = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext2")))
            {
                String StrQuery = "select * from PlantMaster";

                //StrQuery = string.Format(StrQuery, DbName, model.DC, model.PartyName);
                using (SqlCommand sqlCmd = new SqlCommand(StrQuery, sqlConn))
                {
                    sqlCmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
                    {
                        sqlAdapter.Fill(dt);
                    }
                }
            }

            return dt;
        }

        #region Party List For STN(Outward)
        public DataTable GetPartyListForSTN(DateFilterModel model)
        {
            DateTime fdate = DateTime.ParseExact(model.FromDate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(model.ToDate, "dd-MM-yyyy", null);
            DataTable dt = new DataTable();

            if (model.Keyword == "NoSearch" || string.IsNullOrEmpty(model.Keyword))
            {
                model.Keyword = "";
            }
            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext2")))
            {
                using (SqlCommand sqlCmd = new SqlCommand("GetBarcodePartyListOutwardDetails", sqlConn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(fdate.ToString("yyyy-MM-dd")));
                    sqlCmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(tdate.ToString("yyyy-MM-dd")));
                    sqlCmd.Parameters.AddWithValue("@CardName", model.Keyword);
                    sqlCmd.Parameters.AddWithValue("@Usercode", model.UserCode);
                    using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
                    {
                        sqlAdapter.Fill(dt);
                    }
                }
            }
         return dt;
        }
        #endregion

        #region Party List For STN(Inward)
        public DataTable GetPartyListForInward(DateFilterModel model)
        {
            DateTime fdate = DateTime.ParseExact(model.FromDate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(model.ToDate, "dd-MM-yyyy", null);
            DataTable dt = new DataTable();

            if (model.Keyword == "NoSearch" || string.IsNullOrEmpty(model.Keyword))
            {
                model.Keyword = "";
            }
            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext2")))
            {
                using (SqlCommand sqlCmd = new SqlCommand("GetBarcodePartyListInwardDetails", sqlConn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@FromDate", Convert.ToDateTime(fdate.ToString("yyyy-MM-dd")));
                    sqlCmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(tdate.ToString("yyyy-MM-dd")));
                    sqlCmd.Parameters.AddWithValue("@CardName", model.Keyword);
                    sqlCmd.Parameters.AddWithValue("@Usercode", model.UserCode);
                    using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
                    {
                        sqlAdapter.Fill(dt);
                    }
                }
            }
            return dt;
        }
        #endregion
        #region STNOutwardUpload
        public string STNOutwardUpload(UploadFile uploadFile)
        {
            try
            {
                // Check the File is received

                if (uploadFile == null)
                    throw new Exception("File is Not Received...");

                // Create the Directory if it is not exist
                string dirPath = Path.Combine(_hostEnvironment.WebRootPath, "ProductMaster");
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                // MAke sure that only Excel file is used 
                string dataFileName = Path.GetFileName(uploadFile.Attachment.FileName);

                string extension = Path.GetExtension(dataFileName);

                string[] allowedExtsnions = new string[] { ".xls", ".xlsx" };

                if (!allowedExtsnions.Contains(extension))
                    throw new Exception("Sorry! This file is not allowed, make sure that file having extension as either.xls or.xlsx is uploaded.");

                // Make a Copy of the Posted File from the Received HTTP Request
                string saveToPath = Path.Combine(dirPath, dataFileName);

                using (FileStream stream = new FileStream(saveToPath, FileMode.Create))
                {
                    uploadFile.Attachment.CopyTo(stream);
                }

                // USe this to handle Encodeing differences in .NET Core
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                // reading the excel file
                using (var stream = new FileStream(saveToPath, FileMode.Open))
                {
                    if (extension == ".xls")
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    else
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                    DataSet ds = new DataSet();
                    ds = reader.AsDataSet();
                    reader.Close();

                    //Get the STN list
                    DataTable dt = new DataTable();
                    using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext2")))
                    {
                        String StrQuery = "select distinct DeliveryNumber, ShipTOParty, ShipToPartyName, ToDate, Material, Batch FROM TempStockOutward";

                        //StrQuery = string.Format(StrQuery, DbName, model.DC, model.PartyName);
                        using (SqlCommand sqlCmd = new SqlCommand(StrQuery, sqlConn))
                        {
                            sqlCmd.CommandType = CommandType.Text;
                            using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
                            {
                                sqlAdapter.Fill(dt);
                            }
                        }
                    }

                    //for check build 
                    int rowCount = 0;
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        DataTable serviceDetails = ds.Tables[0];
                        for (int i = 1; i < serviceDetails.Rows.Count; i++)
                        {
                            if (!(serviceDetails.Rows[i][2].ToString() == dt.Rows[0]["DeliveryNumber"].ToString())
                                && !(serviceDetails.Rows[i][6].ToString() == dt.Rows[0]["ShipTOParty"].ToString())
                                && !(serviceDetails.Rows[i][7].ToString() == dt.Rows[0]["ShipToPartyName"].ToString())
                                && !(serviceDetails.Rows[i][8].ToString() == dt.Rows[0]["ToDate"].ToString())
                                && !(serviceDetails.Rows[i][9].ToString() == dt.Rows[0]["Material"].ToString())
                                && !(serviceDetails.Rows[i][11].ToString() == dt.Rows[0]["Batch"].ToString())
                                )
                            {

                                //DataTable serviceDetails = ds.Tables[0];
                                //for (int i = 1; i < serviceDetails.Rows.Count; i++)
                                //{
                                    using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext2")))
                                    {
                                        sqlConn.Open();
                                        using (SqlCommand sqlCmd = new SqlCommand("Usp_InsStockTransferOutwardTemp", sqlConn))
                                        {
                                            sqlCmd.CommandType = CommandType.StoredProcedure;
                                            sqlCmd.Parameters.AddWithValue("@TO_NUMBER", Convert.ToInt64(serviceDetails.Rows[i][0].ToString()));
                                            sqlCmd.Parameters.AddWithValue("@TO_Item", serviceDetails.Rows[i][1].ToString());
                                            sqlCmd.Parameters.AddWithValue("@DELIVERY_NUMBER", serviceDetails.Rows[i][2].ToString());
                                            sqlCmd.Parameters.AddWithValue("@Document_Type", serviceDetails.Rows[i][3].ToString());
                                            sqlCmd.Parameters.AddWithValue("@Supplying_Depot", serviceDetails.Rows[i][4].ToString());
                                            sqlCmd.Parameters.AddWithValue("@Sales_Order", serviceDetails.Rows[i][5].ToString());
                                            sqlCmd.Parameters.AddWithValue("@Ship_To_Party", serviceDetails.Rows[i][6].ToString());
                                            sqlCmd.Parameters.AddWithValue("@Ship_To_Party_Name", serviceDetails.Rows[i][7].ToString());
                                            sqlCmd.Parameters.AddWithValue("@To_Date", serviceDetails.Rows[i][8].ToString());
                                            sqlCmd.Parameters.AddWithValue("@Material", serviceDetails.Rows[i][9].ToString());
                                            sqlCmd.Parameters.AddWithValue("@Description", serviceDetails.Rows[i][10].ToString());
                                            sqlCmd.Parameters.AddWithValue("@Batch", serviceDetails.Rows[i][11].ToString());
                                            sqlCmd.Parameters.AddWithValue("@MFG_Date", Convert.ToDateTime(serviceDetails.Rows[i][12].ToString()));
                                            sqlCmd.Parameters.AddWithValue("@ExpiryDate", Convert.ToDateTime(serviceDetails.Rows[i][13].ToString()));
                                            sqlCmd.Parameters.AddWithValue("@Qty", serviceDetails.Rows[i][14].ToString());
                                            sqlCmd.Parameters.AddWithValue("@UOM", serviceDetails.Rows[i][15].ToString());
                                            sqlCmd.Parameters.AddWithValue("@Tota_Gross_Weight", serviceDetails.Rows[i][16].ToString());
                                            sqlCmd.Parameters.AddWithValue("@Source_Storage_Type", serviceDetails.Rows[i][17].ToString());
                                            sqlCmd.Parameters.AddWithValue("@Source_Storage_Bin", serviceDetails.Rows[i][18].ToString());

                                            rowCount = sqlCmd.ExecuteNonQuery();

                                            sqlConn.Close();
                                            sqlConn.Dispose();
                                        }
                                    }

                                //}



                                if (rowCount > 0)
                                {
                                    using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext2")))
                                    {
                                        sqlConn.Open();
                                        using (SqlCommand sqlCmd = new SqlCommand("Usp_InsStockTransferOutwardHDR", sqlConn))
                                        {
                                            sqlCmd.ExecuteNonQuery();
                                            sqlConn.Close();
                                            sqlConn.Dispose();
                                        }
                                    }
                                }

                            }
                        }
                    }



                    //int rowCount = 0;
                    //if (ds != null && ds.Tables.Count > 0)
                    //{
                        //DataTable serviceDetails = ds.Tables[0];
                        //for (int i = 1; i < serviceDetails.Rows.Count; i++)
                        //{
                        //    using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext2")))
                        //    {
                        //        sqlConn.Open();
                        //        using (SqlCommand sqlCmd = new SqlCommand("Usp_InsStockTransferOutwardTemp", sqlConn))
                        //        {
                        //            sqlCmd.CommandType = CommandType.StoredProcedure;
                        //            sqlCmd.Parameters.AddWithValue("@TO_NUMBER", Convert.ToInt64(serviceDetails.Rows[i][0].ToString()));
                        //            sqlCmd.Parameters.AddWithValue("@TO_Item", serviceDetails.Rows[i][1].ToString());
                        //            sqlCmd.Parameters.AddWithValue("@DELIVERY_NUMBER", serviceDetails.Rows[i][2].ToString());
                        //            sqlCmd.Parameters.AddWithValue("@Document_Type", serviceDetails.Rows[i][3].ToString());
                        //            sqlCmd.Parameters.AddWithValue("@Supplying_Depot", serviceDetails.Rows[i][4].ToString());
                        //            sqlCmd.Parameters.AddWithValue("@Sales_Order", serviceDetails.Rows[i][5].ToString());
                        //            sqlCmd.Parameters.AddWithValue("@Ship_To_Party", serviceDetails.Rows[i][6].ToString());
                        //            sqlCmd.Parameters.AddWithValue("@Ship_To_Party_Name", serviceDetails.Rows[i][7].ToString());
                        //            sqlCmd.Parameters.AddWithValue("@To_Date", serviceDetails.Rows[i][8].ToString());
                        //            sqlCmd.Parameters.AddWithValue("@Material", serviceDetails.Rows[i][9].ToString());
                        //            sqlCmd.Parameters.AddWithValue("@Description", serviceDetails.Rows[i][10].ToString());
                        //            sqlCmd.Parameters.AddWithValue("@Batch", serviceDetails.Rows[i][11].ToString());
                        //            sqlCmd.Parameters.AddWithValue("@MFG_Date", Convert.ToDateTime(serviceDetails.Rows[i][12].ToString()));
                        //            sqlCmd.Parameters.AddWithValue("@ExpiryDate", Convert.ToDateTime(serviceDetails.Rows[i][13].ToString()));
                        //            sqlCmd.Parameters.AddWithValue("@Qty", serviceDetails.Rows[i][14].ToString());
                        //            sqlCmd.Parameters.AddWithValue("@UOM", serviceDetails.Rows[i][15].ToString());
                        //            sqlCmd.Parameters.AddWithValue("@Tota_Gross_Weight", serviceDetails.Rows[i][16].ToString());
                        //            sqlCmd.Parameters.AddWithValue("@Source_Storage_Type", serviceDetails.Rows[i][17].ToString());
                        //            sqlCmd.Parameters.AddWithValue("@Source_Storage_Bin", serviceDetails.Rows[i][18].ToString());

                        //            rowCount = sqlCmd.ExecuteNonQuery();

                        //            sqlConn.Close();
                        //            sqlConn.Dispose();
                        //        }
                        //    }

                        //}

                        //if (rowCount > 0)
                        //{
                        //    using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext2")))
                        //    {
                        //        sqlConn.Open();
                        //        using (SqlCommand sqlCmd = new SqlCommand("Usp_InsStockTransferOutwardHDR", sqlConn))
                        //        {
                        //            sqlCmd.ExecuteNonQuery();
                        //            sqlConn.Close();
                        //            sqlConn.Dispose();
                        //        }
                        //    }
                        //}

                    //}
                }
            }
            catch (Exception ex)
            {
                return "Data is not uploaded properly";
            }

            return "Data Uploaded Successfully!!!";
        }
        #endregion

        #region STNInwardUpload
        public string STNInwardUpload(UploadFile uploadFile)
        {
            try
            {
                // Check the File is received

                if (uploadFile == null)
                    throw new Exception("File is Not Received...");

                // Create the Directory if it is not exist
                string dirPath = Path.Combine(_hostEnvironment.WebRootPath, "ProductMaster");
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                // MAke sure that only Excel file is used 
                string dataFileName = Path.GetFileName(uploadFile.Attachment.FileName);

                string extension = Path.GetExtension(dataFileName);

                string[] allowedExtsnions = new string[] { ".xls", ".xlsx" };

                if (!allowedExtsnions.Contains(extension))
                    throw new Exception("Sorry! This file is not allowed, make sure that file having extension as either.xls or.xlsx is uploaded.");

                // Make a Copy of the Posted File from the Received HTTP Request
                string saveToPath = Path.Combine(dirPath, dataFileName);

                using (FileStream stream = new FileStream(saveToPath, FileMode.Create))
                {
                    uploadFile.Attachment.CopyTo(stream);
                }

                // USe this to handle Encoding differences in .NET Core
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                // read the excel file
                using (var stream = new FileStream(saveToPath, FileMode.Open))
                {
                    if (extension == ".xls")
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    else
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                    DataSet ds = new DataSet();
                    ds = reader.AsDataSet();
                    reader.Close();
                    int rowCount = 0;
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        DataTable serviceDetails = ds.Tables[0];
                        for (int i = 1; i < serviceDetails.Rows.Count; i++)
                        {
                            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext2")))
                            {
                                sqlConn.Open();
                                using (SqlCommand sqlCmd = new SqlCommand("Usp_InsStockTransferInwardTemp", sqlConn))
                                {

                                    sqlCmd.CommandType = CommandType.StoredProcedure;
                                    sqlCmd.Parameters.AddWithValue("@TO_NUMBER", Convert.ToInt64(serviceDetails.Rows[i][0].ToString()));
                                    sqlCmd.Parameters.AddWithValue("@TO_Item", serviceDetails.Rows[i][1].ToString());
                                    sqlCmd.Parameters.AddWithValue("@DELIVERY_NUMBER", serviceDetails.Rows[i][2].ToString());
                                    sqlCmd.Parameters.AddWithValue("@Document_Type", serviceDetails.Rows[i][3].ToString());
                                    sqlCmd.Parameters.AddWithValue("@Supplying_Depot", serviceDetails.Rows[i][4].ToString());
                                    sqlCmd.Parameters.AddWithValue("@Sales_Order", serviceDetails.Rows[i][5].ToString());
                                    sqlCmd.Parameters.AddWithValue("@Ship_To_Party", serviceDetails.Rows[i][6].ToString());
                                    sqlCmd.Parameters.AddWithValue("@Ship_To_Party_Name", serviceDetails.Rows[i][7].ToString());
                                    sqlCmd.Parameters.AddWithValue("@To_Date", serviceDetails.Rows[i][8].ToString());
                                    sqlCmd.Parameters.AddWithValue("@Material", serviceDetails.Rows[i][9].ToString());
                                    sqlCmd.Parameters.AddWithValue("@Description", serviceDetails.Rows[i][10].ToString());
                                    sqlCmd.Parameters.AddWithValue("@Batch", serviceDetails.Rows[i][11].ToString());
                                    sqlCmd.Parameters.AddWithValue("@MFG_Date", Convert.ToDateTime(serviceDetails.Rows[i][12].ToString()));
                                    sqlCmd.Parameters.AddWithValue("@ExpiryDate", Convert.ToDateTime(serviceDetails.Rows[i][13].ToString()));
                                    sqlCmd.Parameters.AddWithValue("@Qty", serviceDetails.Rows[i][14].ToString());
                                    sqlCmd.Parameters.AddWithValue("@UOM", serviceDetails.Rows[i][15].ToString());
                                    sqlCmd.Parameters.AddWithValue("@Tota_Gross_Weight", serviceDetails.Rows[i][16].ToString());
                                    sqlCmd.Parameters.AddWithValue("@Source_Storage_Type", serviceDetails.Rows[i][17].ToString());
                                    sqlCmd.Parameters.AddWithValue("@Source_Storage_Bin", serviceDetails.Rows[i][18].ToString());

                                    rowCount = sqlCmd.ExecuteNonQuery();

                                    sqlConn.Close();
                                    sqlConn.Dispose();
                                }

                            }

                        }

                        if (rowCount > 0)
                        {
                            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext2")))
                            {
                                sqlConn.Open();
                                using (SqlCommand sqlCmd = new SqlCommand("Usp_InsStockTransferInwardHDR", sqlConn))
                                {
                                    sqlCmd.ExecuteNonQuery();
                                    sqlConn.Close();
                                    sqlConn.Dispose();
                                }
                            }
                        }
                        

                    }
                }
            }
            catch (Exception ex)
            {
                return "Data is not uploaded properly";
            }

            return "Data Uploaded Successfully!!!";
        }
        #endregion
        #region Party Master Upload
        public string PartyMasterUpload(UploadFile uploadFile)
        {
            try
            {
                // Check the File is received

                if (uploadFile == null)
                    throw new Exception("File is Not Received...");

                // Create the Directory if it is not exist
                string dirPath = Path.Combine(_hostEnvironment.WebRootPath, "ProductMaster");
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                // MAke sure that only Excel file is used 
                string dataFileName = Path.GetFileName(uploadFile.Attachment.FileName);

                string extension = Path.GetExtension(dataFileName);

                string[] allowedExtsnions = new string[] { ".xls", ".xlsx" };

                if (!allowedExtsnions.Contains(extension))
                    throw new Exception("Sorry! This file is not allowed, make sure that file having extension as either.xls or.xlsx is uploaded.");

                // Make a Copy of the Posted File from the Received HTTP Request
                string saveToPath = Path.Combine(dirPath, dataFileName);

                using (FileStream stream = new FileStream(saveToPath, FileMode.Create))
                {
                    uploadFile.Attachment.CopyTo(stream);
                }

                // USe this to handle Encodeing differences in .NET Core
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                // read the excel file
                using (var stream = new FileStream(saveToPath, FileMode.Open))
                {
                    if (extension == ".xls")
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    else
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);

                    DataSet ds = new DataSet();
                    ds = reader.AsDataSet();
                    reader.Close();
                    int rowCount = 0;
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        DataTable serviceDetails = ds.Tables[0];
                        for (int i = 1; i < serviceDetails.Rows.Count; i++)
                        {
                            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext2")))
                            {
                                sqlConn.Open();
                                using (SqlCommand sqlCmd = new SqlCommand("Usp_InsPartyMasterUploadTemp", sqlConn))
                                {
                                    sqlCmd.CommandType = CommandType.StoredProcedure;
                                    sqlCmd.Parameters.AddWithValue("@TO_NUMBER", Convert.ToInt64(serviceDetails.Rows[i][0].ToString()));
                                    sqlCmd.Parameters.AddWithValue("@TO_Item", serviceDetails.Rows[i][1].ToString());
                                    sqlCmd.Parameters.AddWithValue("@DELIVERY_NUMBER", serviceDetails.Rows[i][2].ToString());
                                    sqlCmd.Parameters.AddWithValue("@Document_Type", serviceDetails.Rows[i][3].ToString());
                                    sqlCmd.Parameters.AddWithValue("@Supplying_Depot", serviceDetails.Rows[i][4].ToString());
                                    sqlCmd.Parameters.AddWithValue("@Sales_Order", serviceDetails.Rows[i][5].ToString());
                                    sqlCmd.Parameters.AddWithValue("@Ship_To_Party", serviceDetails.Rows[i][6].ToString());
                                    sqlCmd.Parameters.AddWithValue("@Ship_To_Party_Name", serviceDetails.Rows[i][7].ToString());
                                    sqlCmd.Parameters.AddWithValue("@To_Date", Convert.ToDateTime(serviceDetails.Rows[i][8].ToString()).ToString("dd-MM-yyyy"));
                                    sqlCmd.Parameters.AddWithValue("@Material", serviceDetails.Rows[i][9].ToString());
                                    sqlCmd.Parameters.AddWithValue("@Description", serviceDetails.Rows[i][10].ToString());
                                    sqlCmd.Parameters.AddWithValue("@Batch", serviceDetails.Rows[i][11].ToString());
                                    sqlCmd.Parameters.AddWithValue("@MFG_Date", Convert.ToDateTime(serviceDetails.Rows[i][12].ToString()));
                                    sqlCmd.Parameters.AddWithValue("@ExpiryDate", Convert.ToDateTime(serviceDetails.Rows[i][13].ToString()));
                                    sqlCmd.Parameters.AddWithValue("@Qty", serviceDetails.Rows[i][14].ToString());
                                    sqlCmd.Parameters.AddWithValue("@UOM", serviceDetails.Rows[i][15].ToString());
                                    sqlCmd.Parameters.AddWithValue("@Tota_Gross_Weight", serviceDetails.Rows[i][16].ToString());
                                    sqlCmd.Parameters.AddWithValue("@Source_Storage_Type", serviceDetails.Rows[i][17].ToString());
                                    sqlCmd.Parameters.AddWithValue("@Source_Storage_Bin", serviceDetails.Rows[i][18].ToString());

                                    rowCount = rowCount + sqlCmd.ExecuteNonQuery();

                                    sqlConn.Close();
                                    sqlConn.Dispose();
                                }
                            }

                        }

                        if (rowCount > 0)
                        {
                            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext2")))
                            {
                                sqlConn.Open();
                                using (SqlCommand sqlCmd = new SqlCommand("Usp_InsPartyMasterUpload", sqlConn))
                                {
                                    sqlCmd.ExecuteNonQuery();
                                    sqlConn.Close();
                                    sqlConn.Dispose();
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                return "Data is not uploaded properly";
            }

            return "Data Uploaded Successfully!!!";
        }
        #endregion
    }
}

