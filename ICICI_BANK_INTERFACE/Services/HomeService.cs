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
    public class HomeService : IHomeService
    {
        private readonly IHomeService _IHomeService;
        private static IWebHostEnvironment _hostEnvironment;

        private readonly IConfiguration _config;
        string DbName = "CMS1";
        public HomeService( IWebHostEnvironment environment, IConfiguration config)
         {
            _config = config;
            _hostEnvironment = environment;
        }
        public DataTable GetItemList(ItemListFilter model)
        {

            DeleteDuplicateBarcode();
            DataTable dt = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext")))
            {
                String StrQuery = "with CTE as (select Distinct dln1.itemcode as ItemCode, \n" +
                    " replace(dln1.dscription, ',', ' - ') as Description, sum(dln1.quantity) as Qty ,0 BalQty\n" +
                    " from odln inner join dln1 on odln.docentry = dln1.docentry\n" +
                    //" inner join[{0}].[dbo].[BarCodeUserMaster] BCUM on BCUM.Warehouse COLLATE DATABASE_DEFAULT =\n" +
                    //"  DLN1.WHSCODE  COLLATE DATABASE_DEFAULT or DLN1.WHSCODE COLLATE DATABASE_DEFAULT = BCUM.Location \n" +
                    //"  COLLATE DATABASE_DEFAULT " +
                    " inner join nnm1 on odln.series = nnm1.series \n" +
                    " WHERE  nnm1.seriesname+LTRIM(STR(odln.docnum)) COLLATE DATABASE_DEFAULT = '{1}' and\n" +
                    "  odln.cardname COLLATE DATABASE_DEFAULT = '{2}' \n" +
                    "   group by dln1.itemcode,dln1.dscription  \n" +
                    " UNION \n" +
                    " select[Item Code] COLLATE DATABASE_DEFAULT ItemCode, Description COLLATE DATABASE_DEFAULT , \n" +
                    "  0 Qty, count(*) AS BalQty  From[{0}].[dbo].[BarCodes]\n" +
                    " WHERE  ( [Dc Series]+'/'+[Dc Number]  COLLATE DATABASE_DEFAULT  ='{1}' or  [Dc Series] +[Dc Number]  COLLATE DATABASE_DEFAULT  ='{1}')  \n" +
                    "   and Customer COLLATE DATABASE_DEFAULT = '{2}'\n" +
                    " Group by[Item Code], Description )\n" +
                    //" select ItemCode, Description,  case when  SUM (Qty)/2 =0.5 then 1 else SUM (Qty)/2 end  as inwoard,SUM(BalQty) as outwpard, " +
                    " select ItemCode, Description,  SUM (Qty)  as inwoard,SUM(BalQty) as outwpard, " +
                    "    SUM (Qty) - SUM(BalQty)    BALQTY  \n " +
                    //"  case when  SUM (Qty)/2 =0.5 then 1 - SUM(BalQty) else SUM (Qty)/2  - SUM(BalQty) end      as BALQTY  \n " +
                    " from CTE group by ItemCode,Description  order by ItemCode \n";


                StrQuery = string.Format(StrQuery, DbName, model.DC, model.PartyName);

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

        public DataTable GetPartyList(DateFilterModel model)
        {

            DeleteDuplicateBarcode();
            DateTime fdate = DateTime.ParseExact(model.FromDate, "dd-MM-yyyy", null);
            DateTime tdate = DateTime.ParseExact(model.ToDate, "dd-MM-yyyy", null);
            DataTable dt = new DataTable();

            if (model.Keyword == "NoSearch" || string.IsNullOrEmpty(model.Keyword))
            {
                model.Keyword = "" ;
            }
            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext")))
            {
                using (SqlCommand sqlCmd = new SqlCommand("GetBarcodePartyListDetails", sqlConn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@FromDate",  Convert.ToDateTime(fdate.ToString("yyyy-MM-dd")));
                    sqlCmd.Parameters.AddWithValue("@ToDate", Convert.ToDateTime(tdate.ToString("yyyy-MM-dd")));
                    sqlCmd.Parameters.AddWithValue("@CardName", model.Keyword);
                    sqlCmd.Parameters.AddWithValue("@UserCode", model.UserCode);
                    using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
                    {
                        sqlAdapter.Fill(dt);
                    }
                }
            }
            return  dt ;
        }

        public DataTable GetQrCodeList(ItemListFilter model)
        {
            DataTable dt = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext")))
            {
                String StrQuery = "SELECT  [SerialId] ,[Barcode] FROM[{0}].[dbo].[BarCodes]" +
                    " where  ([Dc Series] + '/' +[Dc Number] = '{1}' or [Dc Series]  +[Dc Number] = '{1}') and [Customer] = '{2}' and[Item Code] = '{3}'" +
                    " order by SerialId";
                StrQuery = string.Format(StrQuery, DbName, model.DC, model.PartyName, model.ItemCode);

                using (SqlCommand sqlCmd = new SqlCommand(StrQuery, sqlConn))
                {
                    sqlCmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
                    {
                        sqlAdapter.Fill(dt);
                    }
                }
            }
            return  dt ;
        }
         
        public DataTable Login(UserMasterModel userMaster)
        {
        DataTable dt = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext3")))
            {
                String StrQuery = "SELECT TOP 1 [ID] ,[UserCode] ,[UserName] ,[Location] ,[Warehouse] ,[UserType] " +
                    " FROM [{0}].[dbo].[BarCodeUserMaster] where [UserCode] = '{1}' and [Password] ='{2}'";
                StrQuery = string.Format(StrQuery,DbName, userMaster.UserCode, userMaster.Password);

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

        public ResponseModel InsertBarCode(InsertBarcodeDetailsModel Model)
        {
            ResponseModel responseModel = new ResponseModel();
            string Result = "";
            var DocNo = Model.SeriesName.Split('/');
            if (!string.IsNullOrEmpty(Model.Barcode))
            {
                using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext2")))
                {
                    sqlConn.Open();
                    if (Model.SerialId == 0)
                    {
                        DataTable dt = new DataTable();

                        string StrQuery = "SELECT  * FROM [{0}].[dbo].[BarCodes]  " +
                            "    where  " +
                            //"  [Dc Series]='{1}' and  " +
                            //" [Dc Number] = '{2}' and " +
                            //" Customer = '{3}'  and " +
                            //" [Item Code] = '{4}' and " +
                            " Barcode = '{5}' ";
                        StrQuery = string.Format(StrQuery, DbName, DocNo[0].Trim(), DocNo[1].Trim(), Model.CardName.Trim(), Model.ItemCode.Trim(), Model.Barcode.Trim());

                        using (SqlCommand sqlCmd = new SqlCommand(StrQuery, sqlConn))
                        {
                            sqlCmd.CommandType = CommandType.Text;
                            using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
                            {
                                sqlAdapter.Fill(dt);
                            }
                        }

                        if(dt.Rows.Count == 0)
                        {
                            using (SqlCommand sqlCmd = new SqlCommand("InsertBarcodeDetails", sqlConn))
                            {
                                sqlCmd.CommandType = CommandType.StoredProcedure;
                                sqlCmd.Parameters.AddWithValue("@Type", "DC");
                                sqlCmd.Parameters.AddWithValue("@DocDate", Model.DocDate);
                                sqlCmd.Parameters.AddWithValue("@SeriesName", DocNo[0].Trim());
                                sqlCmd.Parameters.AddWithValue("@DocNum", DocNo[1].Trim());
                                sqlCmd.Parameters.AddWithValue("@CardName", Model.CardName.Trim());
                                sqlCmd.Parameters.AddWithValue("@ItemCode", Model.ItemCode.Trim());
                                sqlCmd.Parameters.AddWithValue("@Dscription", Model.Dscription.Trim());
                                sqlCmd.Parameters.AddWithValue("@QTY", Convert.ToInt32(Model.QTY));
                                sqlCmd.Parameters.AddWithValue("@Barcode", Model.Barcode.Trim());
                                sqlCmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                                sqlCmd.Parameters.AddWithValue("@Time", DateTime.Now.ToString("HH:mm:ss"));
                                Result = sqlCmd.ExecuteScalar().ToString();
                                Result = Result +Model.ItemCode.Trim() + " - " + Model.Dscription.Trim() + " Item Barcode Scanned";
                                //ItemListFilter model = new ItemListFilter(); 
                                //model.DC = Model.SeriesName;
                                //model.UserCode = Model.UserCode;
                                //model.PartyName = Model.CardName.Trim();
                                //responseModel.dataTable = GetItemList(model);
                            }

                        }
                        else
                        {
                            Result = "Barcode Already Exist for "+ dt.Rows[0]["Customer"] +"-"+ dt.Rows[0]["Dc Series"] + dt.Rows[0]["Dc Number"];
                        }
                    }
                    else
                    {
                        String StrQuery = "  Update  [{0}].[dbo].[barcodes] set  Barcode = '{1}' where [SerialId]={2} ";
                        StrQuery = string.Format(StrQuery, DbName, Model.Barcode, Model.SerialId);
                        using (SqlCommand sqlCmd = new SqlCommand(StrQuery, sqlConn))
                        {
                            sqlConn.Open();
                            sqlCmd.CommandType = CommandType.Text;
                            sqlCmd.ExecuteNonQuery();
                            Result = Model.SerialId.ToString();
                        }
                    }

                }

            }

            //Result = "'" + Result + "'";
            responseModel.data = Result;
            return responseModel;
        }

        public ResponseModel InsertBarCodeByBarcode(InsertBarcodeDetailsModel Model)
        {
            ResponseModel responseModel = new ResponseModel();
            string Result = "";
            var DocNo = Model.SeriesName.Split('/');
            if (!string.IsNullOrEmpty(Model.Barcode))
            {
                if (!Regex.IsMatch(Model.Barcode.Substring(0, 5), @"^\d+$"))
                { 
                    responseModel.data = "No item available for entered barcode";
                    return responseModel;
                }
                else
                {

                    using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext")))
                    {
                        DataTable dt = new DataTable();
                        string StrQuery = "with CTE as (" +
                            " select Distinct dln1.itemcode as ItemCode, \n" +
                           " replace(dln1.dscription, ',', ' - ') as Description,sum(dln1.quantity) as Qty ,0 BalQty\n" +
                           " from odln inner join dln1 on odln.docentry = dln1.docentry\n" +
                           " inner join[{0}].[dbo].[BarCodeUserMaster] BCUM on BCUM.Warehouse COLLATE DATABASE_DEFAULT =\n" +
                           " DLN1.WHSCODE  COLLATE DATABASE_DEFAULT or DLN1.WHSCODE COLLATE DATABASE_DEFAULT = BCUM.Location\n" +
                           " COLLATE DATABASE_DEFAULT\n" +
                           " inner join nnm1 on odln.series = nnm1.series\n" +
                           " INNER JOIN OITM ON OITM.ItemCode = dln1.itemcode\n" +
                           " WHERE nnm1.seriesname + LTRIM(STR(odln.docnum)) COLLATE DATABASE_DEFAULT = '{1}' and\n" +
                           " odln.cardname COLLATE DATABASE_DEFAULT = '{2}'  AND OITM.DocEntry ={3}\n" +
                           " group by  dln1.itemcode ,dln1.dscription ) \n" +
                           " select ItemCode, Description, sum(Qty/3) as QTY, BalQty from CTE group by ItemCode,Description,BalQty \n";

                        StrQuery = string.Format(StrQuery, DbName, Model.SeriesName, Model.CardName, Model.Barcode.Substring(0, 5));
                        sqlConn.Open();


                        using (SqlCommand sqlCmd = new SqlCommand(StrQuery, sqlConn))
                        {
                            sqlCmd.CommandType = CommandType.Text;
                            using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
                            {
                                dt = new DataTable();
                                sqlAdapter.Fill(dt);
                                if (dt.Rows.Count != 0)
                                {
                                    Model.ItemCode = dt.Rows[0]["ItemCode"].ToString();
                                    Model.Dscription = dt.Rows[0]["Description"].ToString();
                                    Model.QTY = dt.Rows[0]["QTY"].ToString();
                                }
                                else
                                {
                                     
                                    responseModel.data = "No item available for entered barcode";
                                    return responseModel;
                                }
                            }
                        }

                        //dt = new DataTable();

                        //StrQuery = "SELECT  * FROM [{0}].[dbo].[BarCodes]  " +
                        //   "  where  " +
                        //   " [Scan Date]=Convert(varchar(50), GETDATE(),103) and [Dc Series]= '{1}' and [Dc Number]='{2}' and [Customer]='{3}' and [Item Code]='{4}' and Barcode = '{5}'"; //[Scan Date]=Convert(varchar(50), GETDATE(),103) and [Dc Series]= '{1}' and [Dc Number]='{2}' and [Customer]='{3}' and [Item Code]='{4}' and
                        //StrQuery = string.Format(StrQuery, DbName, DocNo[0].Trim(), DocNo[1].Trim(), Model.CardName.Trim(), Model.ItemCode.Trim(), Model.Barcode.Trim());

                        //using (SqlCommand sqlCmd = new SqlCommand(StrQuery, sqlConn))
                        //{
                        //    sqlCmd.CommandType = CommandType.Text;
                        //    using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
                        //    {
                        //        sqlAdapter.Fill(dt);
                        //    }
                        //}


                        //if (dt.Rows.Count == 0)
                        //{
                        //StrQuery = "select count (*)  FROM [{0}].[dbo].[BarCodes] where  [Dc Series]='{3}' and [Dc Number] = '{4}' and " +
                        //   " [Customer] = '{1}' and [Item Code] = '{2}' ";

                        //StrQuery = string.Format(StrQuery, DbName, Model.CardName, Model.ItemCode, DocNo[0], DocNo[1]);

                        //using (SqlCommand sqlCmd = new SqlCommand(StrQuery, sqlConn))
                        //{
                        //    sqlCmd.CommandType = CommandType.Text;
                        //    var resut = sqlCmd.ExecuteScalar();
                        //    if (Convert.ToInt32(resut.ToString()) == Convert.ToInt32(Convert.ToDecimal(Model.QTY)))
                        //    { 
                        //        responseModel.data = "Barcode already exist for total quantuty"; 
                        //        return responseModel;
                        //    }

                        //}
                        decimal s = Convert.ToDecimal(Model.QTY);
                        SqlCommand cmd1 = new SqlCommand("[CMS1].[dbo].[PRC_SCAN_BARCODE_CHECK]", sqlConn);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@P_DCSERIES", DocNo[0]);
                        cmd1.Parameters.AddWithValue("@P_DC_NUMBER", DocNo[1]);
                        cmd1.Parameters.AddWithValue("@P_CUSTOMER", Model.CardName);
                        cmd1.Parameters.AddWithValue("@P_ITEMCODE", Model.ItemCode.Trim());
                        cmd1.Parameters.AddWithValue("@P_BARCODE", Model.Barcode.Trim());
                        cmd1.Parameters.AddWithValue("@P_QTY",Convert.ToDecimal(s));
                        cmd1.Parameters.AddWithValue("@Type", "DC");
                        cmd1.Parameters.AddWithValue("@DocDate", Model.DocDate);
                        cmd1.Parameters.AddWithValue("@SeriesName", DocNo[0].Trim());
                        cmd1.Parameters.AddWithValue("@DocNum", DocNo[1].Trim());
                        cmd1.Parameters.AddWithValue("@CardName", Model.CardName.Trim());
                        cmd1.Parameters.AddWithValue("@ItemCode", Model.ItemCode.Trim());
                        cmd1.Parameters.AddWithValue("@Dscription", Model.Dscription.Trim());
                        cmd1.Parameters.AddWithValue("@QTY", Convert.ToInt32(Convert.ToDecimal(Model.QTY)));
                        cmd1.Parameters.AddWithValue("@Barcode", Model.Barcode.Trim());
                        cmd1.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                        cmd1.Parameters.AddWithValue("@Time", DateTime.Now.ToString("HH:mm:ss"));
                        cmd1.Parameters.Add("@P_MESSAGE", SqlDbType.VarChar, 300);
                        cmd1.Parameters["@P_MESSAGE"].Direction = ParameterDirection.Output;
                        cmd1.ExecuteNonQuery();
                        string Messages= cmd1.Parameters["@P_MESSAGE"].Value.ToString();
                        responseModel.data = Messages;
                        return responseModel;
                        if (Messages != "SUCCESSFULL...!")
                        {
                            responseModel.data = Messages;
                            return responseModel;
                        }
                        else
                        {
                            Result = Model.ItemCode.Trim() + " - " + Model.Dscription.Trim() + " Item Barcode Scanned";
                        }




                        //using (SqlConnection sqlConn2 = new SqlConnection(_config.GetConnectionString("DatabaseContext2")))
                        //    {


                        //        sqlConn2.Open();
                        //        if (Model.SerialId == 0)
                        //        {
                        //            using (SqlCommand sqlCmd = new SqlCommand("InsertBarcodeDetails", sqlConn2))
                        //            {
                        //                sqlCmd.CommandType = CommandType.StoredProcedure;
                        //                //sqlCmd.Parameters.AddWithValue("@Type", "DC");
                        //                //sqlCmd.Parameters.AddWithValue("@DocDate", Model.DocDate);
                        //                //sqlCmd.Parameters.AddWithValue("@SeriesName", DocNo[0].Trim());
                        //                //sqlCmd.Parameters.AddWithValue("@DocNum", DocNo[1].Trim());
                        //                //sqlCmd.Parameters.AddWithValue("@CardName", Model.CardName.Trim());
                        //                //sqlCmd.Parameters.AddWithValue("@ItemCode", Model.ItemCode.Trim());
                        //                //sqlCmd.Parameters.AddWithValue("@Dscription", Model.Dscription.Trim());
                        //                //sqlCmd.Parameters.AddWithValue("@QTY", Convert.ToInt32(Convert.ToDecimal(Model.QTY)));
                        //                //sqlCmd.Parameters.AddWithValue("@Barcode", Model.Barcode.Trim());
                        //                //sqlCmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                        //                //sqlCmd.Parameters.AddWithValue("@Time", DateTime.Now.ToString("HH:mm:ss"));
                        //                Result = sqlCmd.ExecuteScalar().ToString();
                        //                Result = Model.ItemCode.Trim() + " - " + Model.Dscription.Trim() + " Item Barcode Scanned";
                        //                //ItemListFilter model = new ItemListFilter();
                        //                //model.DC = Model.SeriesName;
                        //                //model.UserCode = Model.UserCode;
                        //                //model.PartyName = Model.CardName.Trim();
                        //                //responseModel.dataTable = GetItemList(model);
                        //            }
                        //        }
                        //        else
                        //        {
                        //            StrQuery = "  Update  [{0}].[dbo].[barcodes] set  Barcode = '{1}' where [SerialId]={2} ";
                        //            StrQuery = string.Format(StrQuery, DbName, Model.Barcode, Model.SerialId);
                        //            using (SqlCommand sqlCmd = new SqlCommand(StrQuery, sqlConn2))
                        //            {
                        //                sqlCmd.CommandType = CommandType.Text;
                        //                sqlCmd.ExecuteNonQuery();
                        //                Result = Model.SerialId.ToString();
                        //            }
                        //        }

                        //    }

                        //}
                        //else
                        //{
                        //    Result = "Barcode Already Exist for " + dt.Rows[0]["Customer"] + "- " + dt.Rows[0]["Dc Series"] + dt.Rows[0]["Dc Number"];
                        //}
                    }

                }
            }
            //Result = "'" + Result + "'";
            responseModel.data = Result;
            return responseModel;
        }

        public string DeleteQrCode(InsertBarcodeDetailsModel Model)
        {
            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext")))
            {
                var DocNo = Model.SeriesName.Split('/');
                String StrQuery = "  delete from [{0}].[dbo].[barcodes] where [Dc Series]='{1}' and  " +
                    "[Dc Number] = '{2}' and Customer = '{3}' and [Item Code] = '{4}' and Barcode = '{5}'";
                StrQuery = string.Format(StrQuery, DbName, DocNo[0], DocNo[1], Model.CardName, Model.ItemCode, Model.Barcode);
                using (SqlCommand sqlCmd = new SqlCommand(StrQuery, sqlConn))
                {
                    sqlConn.Open();
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.ExecuteReader();
                }
            }
            return "Record Deleted Successfully";
        }
        public void DeleteDuplicateBarcode()
        {
            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext2")))
            { 
                using (SqlCommand sqlCmd = new SqlCommand("DeleteDuplicateBarcode", sqlConn))
                {
                    sqlConn.Open();
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.ExecuteNonQuery();
                }
            } 
        }

        public string InsertBarCodeFromAnotherTable()
        {
            string Result = "";

            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext2")))
            {
                sqlConn.Open();

                DataTable dt = new DataTable();

                string StrQuery = "SELECT   [Dc Date]  as DocDate ,[Dc Series] SeriesName ,[Dc Number] DocNum,[Customer] " +
            ",[Item Code] ItemCode,[Description] ,[Qty] ,[Barcode]  Barcode FROM [{0}].[dbo].[BarCodes]  ";
                StrQuery = string.Format(StrQuery, "CMSTest");

                using (SqlCommand sqlCmd = new SqlCommand(StrQuery, sqlConn))
                {
                    sqlCmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
                    {
                        sqlAdapter.Fill(dt);
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    for(int count=0; count< dt.Rows.Count;count ++)
                    {
                        using (SqlCommand sqlCmd = new SqlCommand("InsertBarcodeDetails", sqlConn))
                        {

                            sqlCmd.CommandType = CommandType.StoredProcedure;
                            sqlCmd.Parameters.AddWithValue("@Type", "DC");
                            sqlCmd.Parameters.AddWithValue("@DocDate", dt.Rows[count]["DocDate"] );
                            sqlCmd.Parameters.AddWithValue("@SeriesName", dt.Rows[count]["SeriesName"].ToString());
                            sqlCmd.Parameters.AddWithValue("@DocNum", dt.Rows[count]["DocNum"].ToString());
                            sqlCmd.Parameters.AddWithValue("@CardName", dt.Rows[count]["Customer"].ToString());
                            sqlCmd.Parameters.AddWithValue("@ItemCode", dt.Rows[count]["ItemCode"].ToString());
                            sqlCmd.Parameters.AddWithValue("@Dscription", dt.Rows[count]["Description"].ToString());
                            sqlCmd.Parameters.AddWithValue("@QTY", Convert.ToInt32(dt.Rows[count]["Qty"].ToString()));
                            sqlCmd.Parameters.AddWithValue("@Barcode", dt.Rows[count]["Barcode"].ToString());
                            sqlCmd.Parameters.AddWithValue("@Date",  DateTime.Now.ToString("yyyy -MM-dd"));
                            sqlCmd.Parameters.AddWithValue("@Time",  DateTime.Now.ToString("HH:mm:ss"));
                            Result = sqlCmd.ExecuteScalar().ToString();
                            Result = dt.Rows[count]["ItemCode"].ToString() + " - " + dt.Rows[count]["Description"].ToString() + " Item Barcode Scanned";
                            Console.WriteLine(Result);
                        }

                    }
                    Console.WriteLine("Barcode Insertion Done");
                }
                else
                {
                    Result = "Barcode Already Exist";
                }
            }
            //Result = "'" + Result + "'";
            return Result;
        }


        ///////
        ///
        //public ResponseModel InsertBarCodeByBarcode(InsertBarcodeDetailsModel Model)
        //{
        //    ResponseModel responseModel = new ResponseModel();
        //    string Result = "";
        //    var DocNo = Model.SeriesName.Split('/');
        //    if (!string.IsNullOrEmpty(Model.Barcode))
        //    {
        //        if (!Regex.IsMatch(Model.Barcode.Substring(0, 5), @"^\d+$"))
        //        {
        //            responseModel.data = "No item available for entered barcode";
        //            return responseModel;
        //        }
        //        else
        //        {

        //            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext")))
        //            {
        //                DataTable dt = new DataTable();
        //                string StrQuery = "with CTE as (" +
        //                    " select Distinct dln1.itemcode as ItemCode, \n" +
        //                   " replace(dln1.dscription, ',', ' - ') as Description,sum(dln1.quantity) as Qty ,0 BalQty\n" +
        //                   " from odln inner join dln1 on odln.docentry = dln1.docentry\n" +
        //                   " inner join[{0}].[dbo].[BarCodeUserMaster] BCUM on BCUM.Warehouse COLLATE DATABASE_DEFAULT =\n" +
        //                   " DLN1.WHSCODE  COLLATE DATABASE_DEFAULT or DLN1.WHSCODE COLLATE DATABASE_DEFAULT = BCUM.Location\n" +
        //                   " COLLATE DATABASE_DEFAULT\n" +
        //                   " inner join nnm1 on odln.series = nnm1.series\n" +
        //                   " INNER JOIN OITM ON OITM.ItemCode = dln1.itemcode\n" +
        //                   " WHERE nnm1.seriesname + LTRIM(STR(odln.docnum)) COLLATE DATABASE_DEFAULT = '{1}' and\n" +
        //                   " odln.cardname COLLATE DATABASE_DEFAULT = '{2}'  AND OITM.DocEntry ={3}\n" +
        //                   " group by  dln1.itemcode ,dln1.dscription ) \n" +
        //                   " select ItemCode, Description, sum(Qty/3) as QTY, BalQty from CTE group by ItemCode,Description,BalQty \n";

        //                StrQuery = string.Format(StrQuery, DbName, Model.SeriesName, Model.CardName, Model.Barcode.Substring(0, 5));
        //                sqlConn.Open();


        //                using (SqlCommand sqlCmd = new SqlCommand(StrQuery, sqlConn))
        //                {
        //                    sqlCmd.CommandType = CommandType.Text;
        //                    using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
        //                    {
        //                        dt = new DataTable();
        //                        sqlAdapter.Fill(dt);
        //                        if (dt.Rows.Count != 0)
        //                        {
        //                            Model.ItemCode = dt.Rows[0]["ItemCode"].ToString();
        //                            Model.Dscription = dt.Rows[0]["Description"].ToString();
        //                            Model.QTY = dt.Rows[0]["QTY"].ToString();
        //                        }
        //                        else
        //                        {

        //                            responseModel.data = "No item available for entered barcode";
        //                            return responseModel;
        //                        }
        //                    }
        //                }

        //                dt = new DataTable();

        //                StrQuery = "SELECT  * FROM [{0}].[dbo].[BarCodes]  " +
        //                   "  where  " +
        //                   " [Scan Date]=Convert(varchar(50), GETDATE(),103) and [Dc Series]= '{1}' and [Dc Number]='{2}' and [Customer]='{3}' and [Item Code]='{4}' and Barcode = '{5}'"; //[Scan Date]=Convert(varchar(50), GETDATE(),103) and [Dc Series]= '{1}' and [Dc Number]='{2}' and [Customer]='{3}' and [Item Code]='{4}' and
        //                StrQuery = string.Format(StrQuery, DbName, DocNo[0].Trim(), DocNo[1].Trim(), Model.CardName.Trim(), Model.ItemCode.Trim(), Model.Barcode.Trim());

        //                using (SqlCommand sqlCmd = new SqlCommand(StrQuery, sqlConn))
        //                {
        //                    sqlCmd.CommandType = CommandType.Text;
        //                    using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
        //                    {
        //                        sqlAdapter.Fill(dt);
        //                    }
        //                }


        //                if (dt.Rows.Count == 0)
        //                {
        //                    StrQuery = "select count (*)  FROM [{0}].[dbo].[BarCodes] where  [Dc Series]='{3}' and [Dc Number] = '{4}' and " +
        //                       " [Customer] = '{1}' and [Item Code] = '{2}' ";

        //                    StrQuery = string.Format(StrQuery, DbName, Model.CardName, Model.ItemCode, DocNo[0], DocNo[1]);

        //                    using (SqlCommand sqlCmd = new SqlCommand(StrQuery, sqlConn))
        //                    {
        //                        sqlCmd.CommandType = CommandType.Text;
        //                        var resut = sqlCmd.ExecuteScalar();
        //                        if (Convert.ToInt32(resut.ToString()) == Convert.ToInt32(Convert.ToDecimal(Model.QTY)))
        //                        {
        //                            responseModel.data = "Barcode already exist for total quantuty";
        //                            return responseModel;
        //                        }

        //                    }
        //                    using (SqlConnection sqlConn2 = new SqlConnection(_config.GetConnectionString("DatabaseContext2")))
        //                    {


        //                        sqlConn2.Open();
        //                        if (Model.SerialId == 0)
        //                        {
        //                            using (SqlCommand sqlCmd = new SqlCommand("InsertBarcodeDetails", sqlConn2))
        //                            {
        //                                sqlCmd.CommandType = CommandType.StoredProcedure;
        //                                sqlCmd.Parameters.AddWithValue("@Type", "DC");
        //                                sqlCmd.Parameters.AddWithValue("@DocDate", Model.DocDate);
        //                                sqlCmd.Parameters.AddWithValue("@SeriesName", DocNo[0].Trim());
        //                                sqlCmd.Parameters.AddWithValue("@DocNum", DocNo[1].Trim());
        //                                sqlCmd.Parameters.AddWithValue("@CardName", Model.CardName.Trim());
        //                                sqlCmd.Parameters.AddWithValue("@ItemCode", Model.ItemCode.Trim());
        //                                sqlCmd.Parameters.AddWithValue("@Dscription", Model.Dscription.Trim());
        //                                sqlCmd.Parameters.AddWithValue("@QTY", Convert.ToInt32(Convert.ToDecimal(Model.QTY)));
        //                                sqlCmd.Parameters.AddWithValue("@Barcode", Model.Barcode.Trim());
        //                                sqlCmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
        //                                sqlCmd.Parameters.AddWithValue("@Time", DateTime.Now.ToString("HH:mm:ss"));
        //                                Result = sqlCmd.ExecuteScalar().ToString();
        //                                Result = Model.ItemCode.Trim() + " - " + Model.Dscription.Trim() + " Item Barcode Scanned";
        //                                //ItemListFilter model = new ItemListFilter();
        //                                //model.DC = Model.SeriesName;
        //                                //model.UserCode = Model.UserCode;
        //                                //model.PartyName = Model.CardName.Trim();
        //                                //responseModel.dataTable = GetItemList(model);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            StrQuery = "  Update  [{0}].[dbo].[barcodes] set  Barcode = '{1}' where [SerialId]={2} ";
        //                            StrQuery = string.Format(StrQuery, DbName, Model.Barcode, Model.SerialId);
        //                            using (SqlCommand sqlCmd = new SqlCommand(StrQuery, sqlConn2))
        //                            {
        //                                sqlCmd.CommandType = CommandType.Text;
        //                                sqlCmd.ExecuteNonQuery();
        //                                Result = Model.SerialId.ToString();
        //                            }
        //                        }

        //                    }

        //                }
        //                else
        //                {
        //                    Result = "Barcode Already Exist for " + dt.Rows[0]["Customer"] + "- " + dt.Rows[0]["Dc Series"] + dt.Rows[0]["Dc Number"];
        //                }
        //            }

        //        }
        //    }
        //    //Result = "'" + Result + "'";
        //    responseModel.data = Result;
        //    return responseModel;
        //}
    }
}
