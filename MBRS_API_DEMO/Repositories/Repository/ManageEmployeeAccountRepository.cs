using FluentEmail.Core;
using MBRS_API.Models;
using MBRS_API_DEMO.DataBase;
using MBRS_API_DEMO.Models;
using MBRS_API_DEMO.Models.StoreProcedure;
using MBRS_API_DEMO.Models.ViewModels;
using MBRS_API_DEMO.Repositories.IRepository;
using MBRS_API_DEMO.Utils;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MBRS_API_DEMO.Repositories.Repository
{
    public class ManageEmployeeAccountRepository : IManageEmployeeAccountRepository
    {
        private readonly string _sqlGetAllEmployeeAccount = @"SELECT employee_id,full_name,phone_number,day_of_birth,cccd,e.account_id,u.user_name,d.department_name FROM employee e
                                                            LEFT JOIN user_account u  ON u.account_id = e.account_id
                                                            LEFT JOIN department d ON d.department_id = u.department_id 
                                                            WHERE (d.department_code = @department_code1 OR d.department_code = @department_code2  OR d.department_code = @department_code3)  AND e.delete_flag = @delete_flag ORDER BY employee_id DESC";

        private readonly string _sqlGetAllEmployeeAccountWithFilter = @"SELECT employee_id,full_name,phone_number,day_of_birth,cccd,e.account_id,u.user_name,d.department_name FROM employee e
                                                            LEFT JOIN user_account u  ON u.account_id = e.account_id
                                                            LEFT JOIN department d ON d.department_id = u.department_id 
                                                            WHERE (d.department_code = @department_code1 OR d.department_code = @department_code2  OR d.department_code = @department_code3)  AND e.delete_flag = @delete_flag";

        private readonly string _sqlCheckUserName = @"SELECT COUNT(account_id) FROM user_account WHERE user_name = @user_name AND delete_flag = 0";

        private readonly string _sqlCheckEmail = @"SELECT COUNT(account_id) FROM user_account WHERE email = @email AND delete_flag = 0";

        private readonly string _sqlCheckIdentityNumber = @"SELECT COUNT(employee_id) FROM employee WHERE cccd = @cccd AND delete_flag = 0";

        private readonly string _sqlDeleteEmployeeByID = @"UPDATE employee SET delete_flag = 1 WHERE employee_id =@employee_id";

        private readonly string _sqlDeleteAccountByID = @"UPDATE user_account SET delete_flag = 1 WHERE account_id =@account_id";

        private readonly string _sqlCreateEmployee = @"INSERT INTO employee (full_name,phone_number,day_of_birth,cccd,account_id,delete_flag) VALUES (@full_name,@phone_number,@day_of_birth,@cccd,@account_id,@delete_flag)";

        private readonly string _sqlCreateEmployeeAccount = @"INSERT INTO user_account (user_name,password,email,department_id,delete_flag) VALUES (@user_name,@password,@email,@department_id,@delete_flag); SELECT SCOPE_IDENTITY()";

        private readonly string _sqlUpdateEmployeeAccount = @"UPDATE user_account SET user_name=@user_name , email=@email , department_id=@department_id WHERE account_id=@account_id";

        private readonly string _sqlUpdateEmployee = @"UPDATE employee SET full_name=@full_name , phone_number=@phone_number , day_of_birth=@day_of_birth ,cccd=@cccd WHERE employee_id=@employee_id";

        private readonly string _sqlGetEmployeeByID = @"SELECT employee_id,full_name,phone_number,day_of_birth,cccd,employee.account_id,user_name,email,department_id FROM employee
                                                        LEFT JOIN user_account ON user_account.account_id = employee.account_id
                                                        WHERE employee_id = @employee_id";

        private readonly string _sqlGetListDepartment = @"SELECT * FROM department WHERE department_code <> 'CS'";

        private readonly SqlServerDBContext _sqlServerDbContext;
        private IConfiguration _configuration { get; }
        public readonly IFluentEmail _fluentEmail;

        private readonly DBWorker dBWorker;
        public ManageEmployeeAccountRepository(SqlServerDBContext sqlServer, IConfiguration configuration, IFluentEmail fluentEmail)
        {
            _sqlServerDbContext = sqlServer;
            _configuration = configuration;
            _fluentEmail = fluentEmail;
            dBWorker = new DBWorker(configuration.GetConnectionString(IConstants.SqlString.SqlServerString));
        }

        public List<EmployeeViewModel> getEmployeeInformationToUpdateByID(int employeeID)
        {
            List<EmployeeViewModel> employeeList = new List<EmployeeViewModel>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetEmployeeByID;
                sqlParameters.Add(new SqlParameter("@employee_id", employeeID));
                employeeList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<EmployeeViewModel>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return employeeList;
        }
        public int checkDuplicateUserName(string userName)
        {
            int checkDuplicate = 0;
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlCheckUserName;
                sqlParameters.Add(new SqlParameter("@user_name", userName));
                checkDuplicate = Convert.ToInt32(dBWorker.ExecuteScalar(commandText, sqlParameters.ToArray()));
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return checkDuplicate;
        }

        public int checkDuplicateEmail(string email)
        {
            int checkDuplicate = 0;
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlCheckEmail;
                sqlParameters.Add(new SqlParameter("@email", email));
                checkDuplicate = Convert.ToInt32(dBWorker.ExecuteScalar(commandText, sqlParameters.ToArray()));
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return checkDuplicate;
        }

        public int checkDuplicateIdentityNumber(string cccd)
        {
            int checkDuplicate = 0;
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlCheckIdentityNumber;
                sqlParameters.Add(new SqlParameter("@cccd", cccd));
                checkDuplicate = Convert.ToInt32(dBWorker.ExecuteScalar(commandText, sqlParameters.ToArray()));
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return checkDuplicate;
        }
        public int deleteEmloyeeByID(int employeeID, int accountID)
        {
            try
            {
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString(IConstants.SqlString.SqlServerString)))
                {
                    string stringQueryDeleteEmployeeByID = _sqlDeleteEmployeeByID;
                    string stringQueryDeleteAccountByID = _sqlDeleteAccountByID;
                    SqlCommand oCommand1 = new SqlCommand(stringQueryDeleteEmployeeByID, oConnection);
                    SqlCommand oCommand2 = new SqlCommand(stringQueryDeleteAccountByID, oConnection);
                    oCommand1.CommandType = CommandType.Text;
                    oCommand2.CommandType = CommandType.Text;
                    oCommand1.Parameters.Add(new SqlParameter("@employee_id", employeeID));
                    oCommand2.Parameters.Add(new SqlParameter("@account_id", accountID));
                    oConnection.Open();
                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {
                            oCommand1.Transaction = oTransaction;
                            oCommand1.ExecuteNonQuery();
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

        public List<Department> getListDepartment()
        {
            List<Department> listDepartment = new List<Department>();
            try
            {
                string commandText = _sqlGetListDepartment;
                listDepartment = dBWorker.GetDataTable(commandText).ToList<Department>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return listDepartment;
        }

        public List<Employee> getAllEmployeeAccount()
        {
            List<Employee> employeeList = new List<Employee>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllEmployeeAccount;
                sqlParameters.Add(new SqlParameter("@department_code1", "LT"));
                sqlParameters.Add(new SqlParameter("@department_code2", "MN"));
                sqlParameters.Add(new SqlParameter("@department_code3", "AM"));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                employeeList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<Employee>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return employeeList;
        }
        public List<Employee> getAllEmployeeAccountWithFilter(string filterName, string filterValue)
        {
            List<Employee> employeeList = new List<Employee>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllEmployeeAccountWithFilter;
                sqlParameters.Add(new SqlParameter("@department_code1", "LT"));
                sqlParameters.Add(new SqlParameter("@department_code2", "MN"));
                sqlParameters.Add(new SqlParameter("@department_code3", "AM"));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                if (filterName != "")
                {
                    commandText = _sqlGetAllEmployeeAccountWithFilter + " AND " + filterName + " LIKE @filterValue ORDER BY employee_id DESC";
                    sqlParameters.Add(new SqlParameter("@filterValue", "%" + filterValue.Trim() + "%"));
                }
                employeeList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<Employee>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return employeeList;
        }


        public int updateEmployeeAccount(EmployeeViewModel employeeViewModel, int employeeID, int accountID)
        {
            try
            {
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString(IConstants.SqlString.SqlServerString)))
                {
                    string stringQueryUpdateEmployee = _sqlUpdateEmployee;
                    string stringQueryUpdateEmployeeAccount = _sqlUpdateEmployeeAccount;
                    SqlCommand oCommand1 = new SqlCommand(stringQueryUpdateEmployee, oConnection);
                    SqlCommand oCommand2 = new SqlCommand(stringQueryUpdateEmployeeAccount, oConnection);
                    oCommand1.CommandType = CommandType.Text;
                    oCommand2.CommandType = CommandType.Text;
                    oConnection.Open();
                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {
                            oCommand1.Parameters.Add(new SqlParameter("@full_name", employeeViewModel.fullName));
                            oCommand1.Parameters.Add(new SqlParameter("@phone_number", employeeViewModel.phoneNumber));
                            oCommand1.Parameters.Add(new SqlParameter("@day_of_birth", employeeViewModel.dateOfBirth.ToString(IConstants.FormatDate.normalDate)));
                            oCommand1.Parameters.Add(new SqlParameter("@cccd", employeeViewModel.identifyNumber));
                            oCommand1.Parameters.Add(new SqlParameter("@employee_id", employeeID));

                            oCommand1.Transaction = oTransaction;
                            oCommand1.ExecuteNonQuery();

                            oCommand2.Parameters.Add(new SqlParameter("@user_name", employeeViewModel.userName));
                            oCommand2.Parameters.Add(new SqlParameter("@email", employeeViewModel.email));
                            oCommand2.Parameters.Add(new SqlParameter("@department_id", employeeViewModel.departmentID));
                            oCommand2.Parameters.Add(new SqlParameter("@account_id", accountID));
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
        public int createEmployee(EmployeeViewModel employeeViewModel)
        {
            string randomPassword = Common.RandomPassword();
            string HashPasswordMD5 = Common.GetMD5Password(randomPassword);
            try
            {
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString(IConstants.SqlString.SqlServerString)))
                {
                    string stringQueryCreateEmployee = _sqlCreateEmployee;
                    string stringQueryCreateEmployeeAccount = _sqlCreateEmployeeAccount;
                    SqlCommand oCommand1 = new SqlCommand(stringQueryCreateEmployeeAccount, oConnection);
                    SqlCommand oCommand2 = new SqlCommand(stringQueryCreateEmployee, oConnection);
                    oCommand1.CommandType = CommandType.Text;
                    oCommand2.CommandType = CommandType.Text;
                    oCommand1.Parameters.Add(new SqlParameter("@user_name", employeeViewModel.userName));
                    oCommand1.Parameters.Add(new SqlParameter("@password", HashPasswordMD5));
                    oCommand1.Parameters.Add(new SqlParameter("@email", employeeViewModel.email));
                    oCommand1.Parameters.Add(new SqlParameter("@department_id", Int32.Parse(employeeViewModel.role)));
                    oCommand1.Parameters.Add(new SqlParameter("@delete_flag", false));
                    oConnection.Open();
                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {
                            oCommand1.Transaction = oTransaction;
                            decimal accountID = (decimal)oCommand1.ExecuteScalar();

                            oCommand2.Parameters.Add(new SqlParameter("@full_name", employeeViewModel.fullName));
                            oCommand2.Parameters.Add(new SqlParameter("@phone_number", employeeViewModel.phoneNumber));
                            oCommand2.Parameters.Add(new SqlParameter("@day_of_birth", employeeViewModel.dateOfBirth.ToString(IConstants.FormatDate.normalDate)));
                            oCommand2.Parameters.Add(new SqlParameter("@cccd", employeeViewModel.identifyNumber));
                            oCommand2.Parameters.Add(new SqlParameter("@account_id", Convert.ToInt32(accountID)));
                            oCommand2.Parameters.Add(new SqlParameter("@delete_flag", false));
                            oCommand2.Transaction = oTransaction;
                            oCommand2.ExecuteNonQuery();
                            oTransaction.Commit();
                            _fluentEmail.To(employeeViewModel.email).Subject("Chào mừng bạn đến với Westlake Hotel").Body("Tài Khoản Westlake Của Bạn \n Tên Tài Khoản : " + employeeViewModel.userName + "\n Mật Khẩu : " + randomPassword).SendAsync();
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



