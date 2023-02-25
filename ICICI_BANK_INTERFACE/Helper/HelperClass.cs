using Dapper;
using ICICI_BANK_INTERFACE.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ICICI_BANK_INTERFACE.Helper
{
    
        public class HelperClass : IHelperClass
        {
            private readonly IConfiguration _config;

            public HelperClass(IConfiguration config)
            {
                _config = config;
            }

            public DbConnection GetConnection()
            {
                return new SqlConnection(_config.GetConnectionString("DatabaseContext"));
            }

            public T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
            {
                using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DatabaseContext")))
                {
                    return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
                }
            }

            public List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
            {
                using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DatabaseContext")))
                {
                    return db.Query<T>(sp, parms, commandType: commandType).ToList();
                }
            }

            public DataTable GetDataTable(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
            {
                DataTable dt = new DataTable();
                using (SqlConnection sqlConn = new SqlConnection(_config.GetConnectionString("DatabaseContext")))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(sp, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@Param", "SomeValueHereToPass");
                        sqlConn.Open();
                        using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
                        {
                            sqlAdapter.Fill(dt);
                        }
                    }
                }
                return dt;
            }

            public int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
            {
                using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DatabaseContext")))
                {
                    return db.Execute(sp, parms, commandType: commandType);
                }
            }

            public T Insert<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
            {
                T result;
                using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DatabaseContext")))
                {
                    try
                    {
                        if (db.State == ConnectionState.Closed)
                            db.Open();

                        using (var tran = db.BeginTransaction())
                        {
                            try
                            {
                                result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                                tran.Commit();
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                throw ex;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (db.State == ConnectionState.Open)
                            db.Close();
                    }

                    return result;
                }
            }

            public T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
            {
                T result;
                using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DatabaseContext")))
                {
                    try
                    {
                        if (db.State == ConnectionState.Closed)
                            db.Open();

                        using (var tran = db.BeginTransaction())
                        {
                            try
                            {
                                result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                                tran.Commit();
                            }
                            catch (Exception ex)
                            {
                                tran.Rollback();
                                throw ex;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (db.State == ConnectionState.Open)
                            db.Close();
                    }

                    return result;
                }
            }

            public void Dispose()
            {
                // throw new NotImplementedException();
            }

            public List<T> Count<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
            {
                using (IDbConnection db = new SqlConnection(_config.GetConnectionString("DatabaseContext")))
                {
                    return db.Query<T>(sp, parms, commandType: commandType).ToList();
                }
            }
        }
    
}
