using ICICI_BANK_INTERFACE.Interface;
using ICICI_BANK_INTERFACE.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ICICI_BANK_INTERFACE.Services
{
    public class PartyMasterService : IPartyMaster
    {
        private static IWebHostEnvironment _hostEnvironment;

        private readonly IConfiguration _config;
        string DbName = "CF_BARCODE";
        public PartyMasterService(IWebHostEnvironment environment, IConfiguration config)
        {
            _config = config;
            _hostEnvironment = environment;
        }
        public DataTable GetItemListInward(ItemListFilter model)
        {
            //DeleteDuplicateBarcode();
            DataTable dt = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext2")))
            {
                using (SqlCommand sqlCmd = new SqlCommand("GetItemListInward", sqlConn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@DC", model.DC);
                    sqlCmd.Parameters.AddWithValue("@CardName", model.PartyName);
                    sqlCmd.Parameters.AddWithValue("@UserCode", model.UserCode);
                    using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
                    {
                        sqlAdapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public DataTable GetItemListOutward(ItemListFilter model)
        {
            //DeleteDuplicateBarcode();
            DataTable dt = new DataTable();
            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext2")))
            {
                using (SqlCommand sqlCmd = new SqlCommand("GetItemListOutward", sqlConn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@DC", model.DC);
                    sqlCmd.Parameters.AddWithValue("@CardName", model.PartyName);
                    sqlCmd.Parameters.AddWithValue("@UserCode", model.UserCode);
                    using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
                    {
                        sqlAdapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public ResponseModel InsertBarCodeByBarcode(InsertBarcodeDetailsModel Model)
        {
            throw new NotImplementedException();
        }

        public DataTable GetQrCodeListInward(ItemListFilter model)
        {
            DataTable dt = new DataTable();
            //using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext")))
            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext2")))
            {
                //String StrQuery = "SELECT  [SerialId] ,[Barcode] FROM[{0}].[dbo].[BarCodes]" +
                //    " where  ([Dc Series] + '/' +[Dc Number] = '{1}' or [Dc Series]  +[Dc Number] = '{1}') and [Customer] = '{2}' and[Item Code] = '{3}'" +
                //    " order by SerialId";

                String StrQuery = "SELECT  [Box Number]+' || '+[Batch]+ ' || ' +[Mfg Date]+ ' || ' + [Expiry Date] as 'NEWNAME', [SerialId] ,[SKU Code] ,[Batch] ,[Box Number] FROM [{0}].[dbo].[MaterialInwardDetails]" +
                    " where  [Supplying Depot] = '{1}' and [SKU Code] = '{2}'" +
                    " order by SerialId";

                StrQuery = string.Format(StrQuery, DbName, /*model.DC,*/ model.PartyName.Trim(), model.ItemCode);

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

        public DataTable GetQrCodeListOutward(ItemListFilter model)
        {
            DataTable dt = new DataTable();
            //using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext")))
            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext2")))
            {
                //String StrQuery = "SELECT  [SerialId] ,[Barcode] FROM[{0}].[dbo].[BarCodes]" +
                //    " where  ([Dc Series] + '/' +[Dc Number] = '{1}' or [Dc Series]  +[Dc Number] = '{1}') and [Customer] = '{2}' and[Item Code] = '{3}'" +
                //    " order by SerialId";

                String StrQuery = "SELECT  [Box Number]+' || '+[Batch]+ ' || ' +[Mfg Date]+ ' || ' + [Expiry Date] as 'NEWNAME', [SerialId] ,[SKU Code] ,[Batch] ,[Box Number] FROM [{0}].[dbo].[MaterialOutwardDetails]" +
                    " where  [Supplying Depot] = '{1}' and [SKU Code] = '{2}'" +
                    " order by SerialId";

                StrQuery = string.Format(StrQuery, DbName, /*model.DC,*/ model.PartyName.Trim(), model.ItemCode);

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

        public string DeleteQrCodeInward(InsertBarcodeDetailsModel Model)
        {
            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext")))
            {
                var DocNo = Model.SeriesName.Split('/');
                String StrQuery = "  delete from [{0}].[dbo].[MaterialInwardDetails] where [Dc Series]='{1}' and  " +
                    "[Dc Number] = '{2}' and [Supplying Depot] = '{3}' and [SKU Code] = '{4}' and Barcode = '{5}'";
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

        public string DeleteQrCodeOutward(InsertBarcodeDetailsModel Model)
        {
            using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext")))
            {
                var DocNo = Model.SeriesName.Split('/');
                String StrQuery = "  delete from [{0}].[dbo].[MaterialOutwardDetails] where [Dc Series]='{1}' and  " +
                    "[Dc Number] = '{2}' and [Supplying Depot] = '{3}' and [SKU Code] = '{4}' and Barcode = '{5}'";
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

        public ResponseModel InsertBarCodeInward(InsertBarcodeDetailsModel Model)
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

                        string StrQuery = "SELECT  * FROM [{0}].[dbo].[MaterialInwardDetails]  " +
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

                        if (dt.Rows.Count == 0)
                        {
                            using (SqlCommand sqlCmd = new SqlCommand("Usp_InsertBarcodeDetailsInward", sqlConn))
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
                                Result = Result + Model.ItemCode.Trim() + " - " + Model.Dscription.Trim() + " Item Barcode Scanned";

                            }

                        }
                        else
                        {
                            Result = "Barcode Already Exist for " + dt.Rows[0]["Customer"] + "-" + dt.Rows[0]["Dc Series"] + dt.Rows[0]["Dc Number"];
                        }
                    }
                    else
                    {
                        String StrQuery = "  Update  [{0}].[dbo].[MaterialInwardDetails] set  Barcode = '{1}' where [SerialId]={2} ";
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

        public ResponseModel InsertBarCodeOutward(InsertBarcodeDetailsModel Model)
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

                        string StrQuery = "SELECT  * FROM [{0}].[dbo].[MaterialOutwardDetails]  " +
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

                        if (dt.Rows.Count == 0)
                        {
                            using (SqlCommand sqlCmd = new SqlCommand("Usp_InsertBarcodeDetailsOutward", sqlConn))
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
                                Result = Result + Model.ItemCode.Trim() + " - " + Model.Dscription.Trim() + " Item Barcode Scanned";
                            }

                        }
                        else
                        {
                            Result = "Barcode Already Exist for " + dt.Rows[0]["Customer"] + "-" + dt.Rows[0]["Dc Series"] + dt.Rows[0]["Dc Number"];
                        }
                    }
                    else
                    {
                        String StrQuery = "  Update  [{0}].[dbo].[MaterialOutwardDetails] set  Barcode = '{1}' where [SerialId]={2} ";
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

        public ResponseModel InsertBarCodeByBarcodeInward(InsertBarcodeDetailsModel Model)
        {
            ResponseModel responseModel = new ResponseModel();
            string Result = "";
            var BarCodeSeries = Model.Barcode.Split(',');
            //string Mfgdate = BarCodeSeries[2].Trim().Substring(6) + "/" + BarCodeSeries[2].Trim().Substring(4, 2) + "/" + BarCodeSeries[2].Trim().Substring(0, 4);
            //string Expdate = BarCodeSeries[3].Trim().Substring(6) + "/" + BarCodeSeries[3].Trim().Substring(4, 2) + "/" + BarCodeSeries[3].Trim().Substring(0, 4);
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

                    using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext2")))
                    {
                        DataTable dt = new DataTable();
                        //string StrQuery = "with CTE as (" +
                        //    " select Distinct dln1.itemcode as ItemCode, \n" +
                        //   " replace(dln1.dscription, ',', ' - ') as Description,sum(dln1.quantity) as Qty ,0 BalQty\n" +
                        //   " from odln inner join dln1 on odln.docentry = dln1.docentry\n" +
                        //   " inner join[{0}].[dbo].[BarCodeUserMaster] BCUM on BCUM.Warehouse COLLATE DATABASE_DEFAULT =\n" +
                        //   " DLN1.WHSCODE  COLLATE DATABASE_DEFAULT or DLN1.WHSCODE COLLATE DATABASE_DEFAULT = BCUM.Location\n" +
                        //   " COLLATE DATABASE_DEFAULT\n" +
                        //   " inner join nnm1 on odln.series = nnm1.series\n" +
                        //   " INNER JOIN OITM ON OITM.ItemCode = dln1.itemcode\n" +
                        //   " WHERE nnm1.seriesname + LTRIM(STR(odln.docnum)) COLLATE DATABASE_DEFAULT = '{1}' and\n" +
                        //   " odln.cardname COLLATE DATABASE_DEFAULT = '{2}'  AND OITM.DocEntry ={3}\n" +
                        //   " group by  dln1.itemcode ,dln1.dscription ) \n" +
                        //   " select ItemCode, Description, sum(Qty/3) as QTY, BalQty from CTE group by ItemCode,Description,BalQty \n";
                        //string StrQuery = "select ItemCode ,replace(description, ',', ' - ') as description,Sum(Quantity) as QTY,0 as BalQty from BarCodeHDR " +
                        //                          "WHERE /*[Doc Series] + LTRIM(STR([Doc Num])) COLLATE DATABASE_DEFAULT ='{1}' and*/ [Card Name]='{2}' and [Sku Code] = '{3}' group by ItemCode,Quantity,Description";





                        string StrQuery = "select [SKU Code] ,replace(description, ',', ' - ') as description,Sum(Quantity) as QTY,0 as BalQty from MaterialInwardHDR " +
                                                  "WHERE [Card Name]='{0}' and [Sku Code] = '{1}'  group by [SKU Code],Quantity,Description";



                        //StrQuery = string.Format(StrQuery,/* DbName, *//*Model.SeriesName,*/ Model.CardName, 
                        //    /*Model.Barcode.Substring(0, 5)*/BarCodeSeries[0].Trim());

                        StrQuery = string.Format(StrQuery, Model.CardName, BarCodeSeries[0].Trim());
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
                                    Model.ItemCode = dt.Rows[0]["[SKU Code]"].ToString();
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

                        //}
                        decimal s = Convert.ToDecimal(Model.QTY);
                        SqlCommand cmd1 = new SqlCommand("[CF_BARCODE].[dbo].[PRC_SCAN_BARCODE_CHECKInward]", sqlConn);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@P_DCSERIES", DocNo[0]);
                        cmd1.Parameters.AddWithValue("@P_DC_NUMBER", DocNo[1]);
                        cmd1.Parameters.AddWithValue("@P_CUSTOMER", Model.CardName);
                        cmd1.Parameters.AddWithValue("@P_ITEMCODE", Model.ItemCode.Trim());
                        //cmd1.Parameters.AddWithValue("@P_BARCODE", Model.Barcode.Trim());
                        cmd1.Parameters.AddWithValue("@P_QTY", Convert.ToDecimal(s));
                        cmd1.Parameters.AddWithValue("@Type", "DC");
                        cmd1.Parameters.AddWithValue("@DocDate", Convert.ToDateTime(Model.DocDate));
                        cmd1.Parameters.AddWithValue("@SeriesName", DocNo[0].Trim());
                        cmd1.Parameters.AddWithValue("@DocNum", DocNo[1].Trim());
                        cmd1.Parameters.AddWithValue("@CardName", Model.CardName.Trim());
                        //cmd1.Parameters.AddWithValue("@ItemCode", Model.ItemCode.Trim());
                        cmd1.Parameters.AddWithValue("@Dscription", Model.Dscription.Trim());
                        cmd1.Parameters.AddWithValue("@QTY", Convert.ToInt32(Convert.ToDecimal(Model.QTY)));
                        //cmd1.Parameters.AddWithValue("@Barcode", Model.Barcode.Trim());
                        cmd1.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                        cmd1.Parameters.AddWithValue("@Time", DateTime.Now.ToString("HH:mm:ss"));

                        cmd1.Parameters.AddWithValue("@SKUCode", BarCodeSeries[0].Trim());
                        cmd1.Parameters.AddWithValue("@Batch", BarCodeSeries[1].Trim());
                        cmd1.Parameters.AddWithValue("@BoxNum", BarCodeSeries[4].Trim());
                        cmd1.Parameters.AddWithValue("@MFGDate", BarCodeSeries[2]);
                        cmd1.Parameters.AddWithValue("@UseByDate", BarCodeSeries[3]);
                        cmd1.Parameters.Add("@P_MESSAGE", SqlDbType.VarChar, 300);
                        cmd1.Parameters["@P_MESSAGE"].Direction = ParameterDirection.Output;
                        cmd1.ExecuteNonQuery();
                        string Messages = cmd1.Parameters["@P_MESSAGE"].Value.ToString();
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
                    }

                }
            }
            //Result = "'" + Result + "'";
            responseModel.data = Result;
            return responseModel;
        }

        public ResponseModel InsertBarCodeByBarcodeOutward(InsertBarcodeDetailsModel Model)
        {
            ResponseModel responseModel = new ResponseModel();
            string Result = "";
            var BarCodeSeries = Model.Barcode.Split(',');
            //string Mfgdate = BarCodeSeries[2].Trim().Substring(6) + "/" + BarCodeSeries[2].Trim().Substring(4, 2) + "/" + BarCodeSeries[2].Trim().Substring(0, 4);
            //string Expdate = BarCodeSeries[3].Trim().Substring(6) + "/" + BarCodeSeries[3].Trim().Substring(4, 2) + "/" + BarCodeSeries[3].Trim().Substring(0, 4);
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

                    using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext2")))
                    {
                        DataTable dt = new DataTable();
                        //string StrQuery = "with CTE as (" +
                        //    " select Distinct dln1.itemcode as ItemCode, \n" +
                        //   " replace(dln1.dscription, ',', ' - ') as Description,sum(dln1.quantity) as Qty ,0 BalQty\n" +
                        //   " from odln inner join dln1 on odln.docentry = dln1.docentry\n" +
                        //   " inner join[{0}].[dbo].[BarCodeUserMaster] BCUM on BCUM.Warehouse COLLATE DATABASE_DEFAULT =\n" +
                        //   " DLN1.WHSCODE  COLLATE DATABASE_DEFAULT or DLN1.WHSCODE COLLATE DATABASE_DEFAULT = BCUM.Location\n" +
                        //   " COLLATE DATABASE_DEFAULT\n" +
                        //   " inner join nnm1 on odln.series = nnm1.series\n" +
                        //   " INNER JOIN OITM ON OITM.ItemCode = dln1.itemcode\n" +
                        //   " WHERE nnm1.seriesname + LTRIM(STR(odln.docnum)) COLLATE DATABASE_DEFAULT = '{1}' and\n" +
                        //   " odln.cardname COLLATE DATABASE_DEFAULT = '{2}'  AND OITM.DocEntry ={3}\n" +
                        //   " group by  dln1.itemcode ,dln1.dscription ) \n" +
                        //   " select ItemCode, Description, sum(Qty/3) as QTY, BalQty from CTE group by ItemCode,Description,BalQty \n";
                        //string StrQuery = "select ItemCode ,replace(description, ',', ' - ') as description,Sum(Quantity) as QTY,0 as BalQty from BarCodeHDR " +
                        //                          "WHERE /*[Doc Series] + LTRIM(STR([Doc Num])) COLLATE DATABASE_DEFAULT ='{1}' and*/ [Card Name]='{2}' and [Sku Code] = '{3}' group by ItemCode,Quantity,Description";





                        string StrQuery = "select [SKU Code] ,replace(description, ',', ' - ') as description,Sum(Quantity) as QTY,0 as BalQty from MaterialOutwardHDR " +
                                                  "WHERE [Card Name]='{0}' and [Sku Code] = '{1}'  group by [SKU Code],Quantity,Description";



                        //StrQuery = string.Format(StrQuery,/* DbName, *//*Model.SeriesName,*/ Model.CardName, 
                        //    /*Model.Barcode.Substring(0, 5)*/BarCodeSeries[0].Trim());

                        StrQuery = string.Format(StrQuery, Model.CardName, BarCodeSeries[0].Trim());
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
                                    Model.ItemCode = dt.Rows[0]["[SKU Code]"].ToString();
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

                        //}
                        decimal s = Convert.ToDecimal(Model.QTY);
                        SqlCommand cmd1 = new SqlCommand("[CF_BARCODE].[dbo].[PRC_SCAN_BARCODE_CHECKOutward]", sqlConn);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@P_DCSERIES", DocNo[0]);
                        cmd1.Parameters.AddWithValue("@P_DC_NUMBER", DocNo[1]);
                        cmd1.Parameters.AddWithValue("@P_CUSTOMER", Model.CardName);
                        cmd1.Parameters.AddWithValue("@P_ITEMCODE", Model.ItemCode.Trim());
                        //cmd1.Parameters.AddWithValue("@P_BARCODE", Model.Barcode.Trim());
                        cmd1.Parameters.AddWithValue("@P_QTY", Convert.ToDecimal(s));
                        cmd1.Parameters.AddWithValue("@Type", "DC");
                        cmd1.Parameters.AddWithValue("@DocDate", Convert.ToDateTime(Model.DocDate));
                        cmd1.Parameters.AddWithValue("@SeriesName", DocNo[0].Trim());
                        cmd1.Parameters.AddWithValue("@DocNum", DocNo[1].Trim());
                        cmd1.Parameters.AddWithValue("@CardName", Model.CardName.Trim());
                        //cmd1.Parameters.AddWithValue("@ItemCode", Model.ItemCode.Trim());
                        cmd1.Parameters.AddWithValue("@Dscription", Model.Dscription.Trim());
                        cmd1.Parameters.AddWithValue("@QTY", Convert.ToInt32(Convert.ToDecimal(Model.QTY)));
                        //cmd1.Parameters.AddWithValue("@Barcode", Model.Barcode.Trim());
                        cmd1.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                        cmd1.Parameters.AddWithValue("@Time", DateTime.Now.ToString("HH:mm:ss"));

                        cmd1.Parameters.AddWithValue("@SKUCode", BarCodeSeries[0].Trim());
                        cmd1.Parameters.AddWithValue("@Batch", BarCodeSeries[1].Trim());
                        cmd1.Parameters.AddWithValue("@BoxNum", BarCodeSeries[4].Trim());
                        cmd1.Parameters.AddWithValue("@MFGDate", BarCodeSeries[2]);
                        cmd1.Parameters.AddWithValue("@UseByDate", BarCodeSeries[3]);
                        cmd1.Parameters.Add("@P_MESSAGE", SqlDbType.VarChar, 300);
                        cmd1.Parameters["@P_MESSAGE"].Direction = ParameterDirection.Output;
                        cmd1.ExecuteNonQuery();
                        string Messages = cmd1.Parameters["@P_MESSAGE"].Value.ToString();
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
                    }

                }
            }
            //Result = "'" + Result + "'";
            responseModel.data = Result;
            return responseModel;
        }
    }
}
