using MBRS_API_DEMO.Utils;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MBRS_API_DEMO.Models.StoreProcedure
{
    public class DBWorker
    {
        private readonly string _connection;

        public DBWorker(string connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Get DataTable
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string command, SqlParameter[] parameters = null)
        {
            DataTable _dataTable = new DataTable();
            using (SqlConnection oConnection = new SqlConnection(_connection))
            {
                SqlCommand oCommand = new SqlCommand(command, oConnection);
                oCommand.CommandType = CommandType.Text;

                if (parameters != null)
                {
                    oCommand.Parameters.AddRange(parameters);
                }
                oConnection.Open();
                SqlDataAdapter oAdapter = new SqlDataAdapter();
                oAdapter.SelectCommand = oCommand;
                oAdapter.Fill(_dataTable);
                oCommand.Parameters.Clear();
            }
            return _dataTable;
        }

        /// <summary>
        /// GET DataSet
        /// </summary>
        /// <param name="commantText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string commantText, SqlParameter[] parameters = null)
        {
            DataSet _dataSet = new DataSet();
            using (SqlConnection oConnection = new SqlConnection(_connection))
            {
                SqlCommand oCommand = new SqlCommand(commantText, oConnection);
                oCommand.CommandType = CommandType.Text;
                if (parameters != null)
                {
                    oCommand.Parameters.AddRange(parameters);
                }
                SqlDataAdapter oAdapter = new SqlDataAdapter();
                oAdapter.SelectCommand = oCommand;
                oConnection.Open();

                using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                {
                    try
                    {
                        oAdapter.SelectCommand.Transaction = oTransaction;
                        oAdapter.Fill(_dataSet);
                        oTransaction.Commit();
                        oCommand.Parameters.Clear();
                    }
                    catch (Exception ex)
                    {
                        oTransaction.Rollback();
                        LogUtil.Error(ex.Message, ex);
                    }
                    finally
                    {
                        if (oConnection.State == ConnectionState.Open)
                        {
                            oConnection.Close();
                        }
                        oConnection.Dispose();
                        oAdapter.Dispose();
                    }
                }
            }
            return _dataSet;
        }

        public object ExecuteScalar(String commantText, SqlParameter[] parameters = null)
        {
            object oReturnValue = null;

            using (SqlConnection oConnection = new SqlConnection(_connection))
            using (SqlCommand oCommand = new SqlCommand(commantText, oConnection))
            {
                oCommand.CommandType = CommandType.Text;
                if (parameters != null)
                {
                    oCommand.Parameters.AddRange(parameters);
                }

                oConnection.Open();

                using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                {
                    try
                    {
                        oCommand.Transaction = oTransaction;
                        oReturnValue = oCommand.ExecuteScalar();
                        oTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        oTransaction.Rollback();
                        LogUtil.Error(ex.Message, ex);
                    }
                    finally
                    {
                        if (oConnection.State == ConnectionState.Open)
                        {
                            oConnection.Close();
                        }
                        oCommand.Parameters.Clear();
                    }
                }
            }

            return oReturnValue;
        }

        public void ExecuteNonQuery(string commantText, SqlParameter[] parameters = null)
        {
            using (SqlConnection oConnection = new SqlConnection(_connection))
            {
                SqlCommand oCommand = new SqlCommand(commantText, oConnection);
                oCommand.CommandType = CommandType.Text;
                if (parameters != null)
                {
                    oCommand.Parameters.AddRange(parameters);
                }
                oConnection.Open();
                using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                {
                    try
                    {
                        oCommand.Transaction = oTransaction;
                        oCommand.ExecuteNonQuery();
                        oTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        oTransaction.Rollback();
                        oCommand.Parameters.Clear();
                        LogUtil.Error(ex.Message, ex);
                    }
                    finally
                    {
                        oCommand.Parameters.Clear();
                        if (oConnection.State == ConnectionState.Open)
                        {
                            oConnection.Close();
                        }
                        oConnection.Dispose();
                        oCommand.Dispose();
                    }
                }
            }
        }

        public void ExecuteNonQueryNoIdentity(string commantText, SqlParameter[] parameters = null)
        {
            using (SqlConnection oConnection = new SqlConnection(_connection))
            {
                SqlCommand oCommand = new SqlCommand(commantText, oConnection);
                oCommand.CommandType = CommandType.Text;
                if (parameters != null)
                {
                    oCommand.Parameters.AddRange(parameters);
                }

                oConnection.Open();
                using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                {
                    try
                    {
                        oCommand.Transaction = oTransaction;
                        oCommand.ExecuteNonQuery();
                        oTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        oTransaction.Rollback();
                        oCommand.Parameters.Clear();
                        LogUtil.Error(ex.Message, ex);
                    }
                    finally
                    {
                        oCommand.Parameters.Clear();
                        if (oConnection.State == ConnectionState.Open)
                        {
                            oConnection.Close();
                        }
                        oConnection.Dispose();
                        oCommand.Dispose();
                    }
                }
            }
        }

        public DataSet ExecuteSqlString(string SqlQuery)
        {
            DataSet _dataSet = new DataSet();
            using (SqlConnection oConnection = new SqlConnection(_connection))
            {
                SqlCommand oCommand = new SqlCommand("execute_all_data", oConnection);
                oCommand.CommandType = CommandType.Text;
                if (!string.IsNullOrEmpty(SqlQuery))
                {
                    oCommand.Parameters.AddWithValue("@SqlCommand", SqlQuery);
                }
                SqlDataAdapter oAdapter = new SqlDataAdapter();
                oAdapter.SelectCommand = oCommand;
                oConnection.Open();

                using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                {
                    try
                    {
                        oAdapter.SelectCommand.Transaction = oTransaction;
                        oAdapter.Fill(_dataSet);
                        oTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        oTransaction.Rollback();
                        LogUtil.Error(ex.Message, ex);
                    }
                    finally
                    {
                        if (oConnection.State == ConnectionState.Open)
                        {
                            oConnection.Close();
                        }
                        oConnection.Dispose();
                        oAdapter.Dispose();
                    }
                }
            }
            return _dataSet;
        }

        public object ExecuteScalarProc(String commantText, SqlParameter[] parameters = null)
        {
            object oReturnValue = null;
            using (SqlConnection oConnection = new SqlConnection(_connection))
            {
                SqlCommand oCommand = new SqlCommand(commantText, oConnection);
                oCommand.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    oCommand.Parameters.AddRange(parameters);
                }
                var returnParameter = oCommand.Parameters.Add("@ReturnVal", SqlDbType.Variant);
                returnParameter.Direction = ParameterDirection.ReturnValue;
                oConnection.Open();

                using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                {
                    try
                    {
                        oCommand.Transaction = oTransaction;
                        oCommand.ExecuteNonQuery();
                        oReturnValue = returnParameter.Value;
                        oTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        oTransaction.Rollback();
                        LogUtil.Error(ex.Message, ex);
                    }
                    finally
                    {
                        if (oConnection.State == ConnectionState.Open)
                        {
                            oConnection.Close();
                        }
                        oConnection.Dispose();
                        oCommand.Dispose();
                    }
                }
            }
            return oReturnValue;
        }
    }
}
