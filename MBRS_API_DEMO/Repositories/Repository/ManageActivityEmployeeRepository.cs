using MBRS_API.Models;
using MBRS_API.Repositories.IRepository;
using MBRS_API_DEMO.DataBase;
using MBRS_API_DEMO.Models.StoreProcedure;
using MBRS_API_DEMO.Utils;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;

namespace MBRS_API.Repositories.Repository
{
    public class ManageActivityEmployeeRepository : IManageActivityEmployeeRepository
    {
        private readonly string _sqlGetAllActivityEmployee = "SELECT activity_employee_id,content_activity,full_name,date_time " +
                                                             "FROM activity_employee " +
                                                             "INNER JOIN employee ON employee.employee_id = activity_employee.employee_id ";

        private readonly string _sqlCreateActivityEmployee = "INSERT INTO activity_employee (employee_id,content_activity,date_time) VALUES (@employee_id,@content_activity,@date_time)";

        private readonly string _sqlGetEmployeeID = "SELECT employee_id FROM employee WHERE account_id=@account_id";

        private readonly SqlServerDBContext _sqlServerDbContext;
        private IConfiguration _configuration { get; }

        private readonly DBWorker dBWorker;
        public ManageActivityEmployeeRepository(SqlServerDBContext sqlServer, IConfiguration configuration)
        {
            _sqlServerDbContext = sqlServer;
            _configuration = configuration;
            dBWorker = new DBWorker(configuration.GetConnectionString(IConstants.SqlString.SqlServerString));
        }

        public List<ActivityEmployee> getAllActivityEmployee()
        {
            List<ActivityEmployee> notificationOrderList = new List<ActivityEmployee>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllActivityEmployee + " ORDER BY date_time DESC";
                notificationOrderList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<ActivityEmployee>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return notificationOrderList;
        }
        public List<ActivityEmployee> getAllActivityEmployeeWithFilter(string filterName, string filterValue)
        {
            List<ActivityEmployee> notificationOrderList = new List<ActivityEmployee>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllActivityEmployee;
                if (filterName != "")
                {
                    if (filterName == "date_time")
                    {
                        commandText = _sqlGetAllActivityEmployee + " WHERE " + "CONVERT(VARCHAR(10)," + filterName + ",120)" + " LIKE @filterValue";
                        if (filterValue != null)
                        {
                            string format = "dd/MM/yyyy";
                            filterValue = (DateTime.ParseExact(filterValue, format, CultureInfo.InvariantCulture)).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            filterValue = "";
                        }
                    }
                    else
                    {
                        commandText = _sqlGetAllActivityEmployee + " WHERE " + filterName + " LIKE @filterValue ORDER BY date_time DESC";
                    }    
                    sqlParameters.Add(new SqlParameter("@filterValue", "%" + filterValue.Trim() + "%"));
                }
                notificationOrderList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<ActivityEmployee>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return notificationOrderList;
        }

        public int createActivityEmployee(ActivityEmployee activityEmployee)
        {
            try
            {
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString(IConstants.SqlString.SqlServerString)))
                {
                    string stringQueryGetEmployeeID = _sqlGetEmployeeID;
                    string stringQueryCreateActivityEmployee = _sqlCreateActivityEmployee;

                    SqlCommand oCommand1 = new SqlCommand(stringQueryGetEmployeeID, oConnection);
                    SqlCommand oCommand2 = new SqlCommand(stringQueryCreateActivityEmployee, oConnection);

                    oCommand1.CommandType = CommandType.Text;
                    oCommand2.CommandType = CommandType.Text;

                    oConnection.Open();
                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {
                            oCommand1.Parameters.Add(new SqlParameter("@account_id", activityEmployee.employeeID));
                            oCommand1.Transaction = oTransaction;
                            int employeeID = Convert.ToInt32(oCommand1.ExecuteScalar());

                            oCommand2.Parameters.Add(new SqlParameter("@employee_id", employeeID));
                            oCommand2.Parameters.Add(new SqlParameter("@content_activity", activityEmployee.contentActivity));
                            oCommand2.Parameters.Add(new SqlParameter("@date_time", Common.ConvertUTCDateTime().ToString("yyyy-MM-dd HH:mm") + ":00.000"));
                            oCommand2.Transaction = oTransaction;
                            oCommand2.ExecuteNonQuery();
                            oTransaction.Commit();
                            return ErrorCodeResponse.SUCCESS_CODE;
                        }
                        catch (Exception ex)
                        {
                            LogUtil.Error(ex.Message, ex);
                            oTransaction.Rollback();
                            return ErrorCodeResponse.FAIL_CODE;
                        }
                        finally
                        {
                            if (oConnection.State == ConnectionState.Open)
                            {
                                oConnection.Close();
                            }
                            oConnection.Dispose();
                            oCommand1.Dispose();
                            oCommand2.Dispose();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return ErrorCodeResponse.FAIL_CODE;
            }
        }
    }
}
