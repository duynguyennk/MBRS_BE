using FluentEmail.Core;
using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API_DEMO.DataBase;
using MBRS_API_DEMO.Models.StoreProcedure;
using MBRS_API_DEMO.Models.ViewModels;
using MBRS_API_DEMO.Utils;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MBRS_API.Repositories.Repository
{
    public class ManageCustomerAccountRepository : IManageCustomerAccountRepository
    {
        private readonly string _sqlGetAllCustomerAccount = @"SELECT customer_id,full_name,phone_number,day_of_birth,identify_number,c.account_id,u.user_name,u.email FROM customer c
                                                            left join user_account u  ON u.account_id = c.account_id
                                                            left join department d ON d.department_id = u.department_id 
                                                            WHERE d.department_code = @department_code AND c.delete_flag = @delete_flag";

        private readonly string _sqlCheckUserName = @"SELECT COUNT(account_id) FROM user_account WHERE user_name = @user_name AND delete_flag = 0";

        private readonly string _sqlCheckEmail = @"SELECT COUNT(account_id) FROM user_account WHERE email = @email AND delete_flag = 0";

        private readonly string _sqlCheckIdentityNumber = @"SELECT COUNT(customer_id) FROM customer WHERE identify_number = @identify_number AND delete_flag = 0";

        private readonly string _sqlDeleteCustomerInformationByID = @"UPDATE customer SET delete_flag = @delete_flag WHERE customer_id = @customer_id";

        private readonly string _sqlDeleteCustomerAccountByID = @"UPDATE user_account SET delete_flag = @delete_flag WHERE account_id =@account_id";

        private readonly string _sqlCreateCustomerInformation = @"INSERT INTO customer (full_name,phone_number,day_of_birth,identify_number,account_id,delete_flag) VALUES (@full_name,@phone_number,@day_of_birth,@identify_number,@account_id,@delete_flag)";

        private readonly string _sqlCreateCustomerAccount = @"INSERT INTO user_account (user_name,password,email,department_id,delete_flag) VALUES (@user_name,@password,@email,@department_id,@delete_flag); SELECT SCOPE_IDENTITY()";

        private readonly string _sqlUpdateCustomerAccount = @"UPDATE user_account SET user_name=@user_name , email=@email WHERE account_id=@account_id";

        private readonly string _sqlUpdateCustomer = @"UPDATE customer SET full_name=@full_name , phone_number=@phone_number , day_of_birth=@day_of_birth ,identify_number=@identify_number WHERE customer_id=@customer_id";

        private readonly string _sqlGetCustomerByID = @"SELECT customer_id,full_name,phone_number,day_of_birth,identify_number,customer.account_id,user_name,email,department_id FROM customer
                                                        LEFT JOIN user_account ON user_account.account_id = customer.account_id
                                                        WHERE customer_id = @customer_id";

        private readonly SqlServerDBContext _sqlServerDbContext;
        private IConfiguration _configuration { get; }
        public readonly IFluentEmail _fluentEmail;
        private readonly DBWorker dBWorker;
        public ManageCustomerAccountRepository(SqlServerDBContext sqlServer, IConfiguration configuration, IFluentEmail fluentEmail)
        {
            _sqlServerDbContext = sqlServer;
            _configuration = configuration;
            dBWorker = new DBWorker(configuration.GetConnectionString(IConstants.SqlString.SqlServerString));
            _fluentEmail = fluentEmail;
        }

        public List<CustomerViewModel> getCustomerInformationToUpdateByID(int customerID)
        {
            List<CustomerViewModel> customerList = new List<CustomerViewModel>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetCustomerByID;
                sqlParameters.Add(new SqlParameter("@customer_id", customerID));
                customerList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<CustomerViewModel>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return customerList;
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

        public int checkDuplicateIdentityNumber(string identityNumber)
        {
            int checkDuplicate = 0;
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlCheckIdentityNumber;
                sqlParameters.Add(new SqlParameter("@identify_number", identityNumber));
                checkDuplicate = Convert.ToInt32(dBWorker.ExecuteScalar(commandText, sqlParameters.ToArray()));
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return checkDuplicate;
        }
        public int deleteCustomerByID(int customerID, int accountID)
        {
            try
            {
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString(IConstants.SqlString.SqlServerString)))
                {
                    string stringQueryDeleteCustomerByID = _sqlDeleteCustomerInformationByID;
                    string stringQueryDeleteCustomerAccountByID = _sqlDeleteCustomerAccountByID;
                    SqlCommand oCommand1 = new SqlCommand(stringQueryDeleteCustomerByID, oConnection);
                    SqlCommand oCommand2 = new SqlCommand(stringQueryDeleteCustomerAccountByID, oConnection);
                    oCommand1.CommandType = CommandType.Text;
                    oCommand2.CommandType = CommandType.Text;
                    oCommand1.Parameters.Add(new SqlParameter("@customer_id", customerID));
                    oCommand1.Parameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.Deleted));
                    oCommand2.Parameters.Add(new SqlParameter("@account_id", accountID));
                    oCommand2.Parameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.Deleted));
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

        public List<Customer> getAllCustomerAccount()
        {
            List<Customer> customerList = new List<Customer>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllCustomerAccount + " ORDER BY customer_id DESC";
                sqlParameters.Add(new SqlParameter("@department_code", "CS"));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                customerList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<Customer>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return customerList;
        }

        public List<Customer> getAllCustomerAccountWithFilter(string filterName, string filterValue)
        {
            List<Customer> employeeList = new List<Customer>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllCustomerAccount;
                sqlParameters.Add(new SqlParameter("@department_code", "CS"));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                if (filterName != "")
                {
                    commandText = _sqlGetAllCustomerAccount + " AND " + filterName + " LIKE @filterValue ORDER BY customer_id DESC";
                    sqlParameters.Add(new SqlParameter("@filterValue", "%" + filterValue.Trim() + "%"));
                }
                employeeList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<Customer>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return employeeList;
        }


        public int updateCustomerAccount(CustomerViewModel customerViewModel, int customerID, int accountID)
        {
            try
            {
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString(IConstants.SqlString.SqlServerString)))
                {
                    string stringQueryUpdateEmployee = _sqlUpdateCustomer;
                    string stringQueryUpdateEmployeeAccount = _sqlUpdateCustomerAccount;
                    SqlCommand oCommand1 = new SqlCommand(stringQueryUpdateEmployee, oConnection);
                    SqlCommand oCommand2 = new SqlCommand(stringQueryUpdateEmployeeAccount, oConnection);
                    oCommand1.CommandType = CommandType.Text;
                    oCommand2.CommandType = CommandType.Text;
                    oConnection.Open();
                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {
                            oCommand1.Parameters.Add(new SqlParameter("@full_name", customerViewModel.fullName));
                            oCommand1.Parameters.Add(new SqlParameter("@phone_number", customerViewModel.phoneNumber));
                            oCommand1.Parameters.Add(new SqlParameter("@day_of_birth", customerViewModel.dateOfBirth.ToString(IConstants.FormatDate.normalDate)));
                            oCommand1.Parameters.Add(new SqlParameter("@identify_number", customerViewModel.identifyNumber));
                            oCommand1.Parameters.Add(new SqlParameter("@customer_id", customerID));

                            oCommand1.Transaction = oTransaction;
                            oCommand1.ExecuteNonQuery();

                            oCommand2.Parameters.Add(new SqlParameter("@user_name", customerViewModel.userName));
                            oCommand2.Parameters.Add(new SqlParameter("@email", customerViewModel.email));
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
        public int createCustomer(CustomerViewModel customerViewModel)
        {
            try
            {
                string randomPassword = Common.RandomPassword();
                string HashPasswordMD5 = Common.GetMD5Password(randomPassword);
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString(IConstants.SqlString.SqlServerString)))
                {
                    string stringQueryCreateEmployee = _sqlCreateCustomerInformation;
                    string stringQueryCreateEmployeeAccount = _sqlCreateCustomerAccount;
                    SqlCommand oCommand1 = new SqlCommand(stringQueryCreateEmployeeAccount, oConnection);
                    SqlCommand oCommand2 = new SqlCommand(stringQueryCreateEmployee, oConnection);
                    oCommand1.CommandType = CommandType.Text;
                    oCommand2.CommandType = CommandType.Text;
                    oCommand1.Parameters.Add(new SqlParameter("@user_name", customerViewModel.userName.ToString()));
                    oCommand1.Parameters.Add(new SqlParameter("@password", HashPasswordMD5));
                    oCommand1.Parameters.Add(new SqlParameter("@email", customerViewModel.email));
                    oCommand1.Parameters.Add(new SqlParameter("@department_id", (int)IConstants.CHECK_ROLE.CS));
                    oCommand1.Parameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                    oConnection.Open();
                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {
                            oCommand1.Transaction = oTransaction;
                            decimal accountID = (decimal)oCommand1.ExecuteScalar();

                            oCommand2.Parameters.Add(new SqlParameter("@full_name", customerViewModel.fullName));
                            oCommand2.Parameters.Add(new SqlParameter("@phone_number", customerViewModel.phoneNumber));
                            oCommand2.Parameters.Add(new SqlParameter("@day_of_birth", customerViewModel.dateOfBirth.ToString(IConstants.FormatDate.normalDate)));
                            oCommand2.Parameters.Add(new SqlParameter("@identify_number", customerViewModel.identifyNumber));
                            oCommand2.Parameters.Add(new SqlParameter("@account_id", Convert.ToInt32(accountID)));
                            oCommand2.Parameters.Add(new SqlParameter("@delete_flag", false));
                            oCommand2.Transaction = oTransaction;
                            oCommand2.ExecuteNonQuery();
                            oTransaction.Commit();
                            _fluentEmail.To(customerViewModel.email).Subject("Chào mừng bạn đến với Westlake Hotel").Body("Tài Khoản Westlake Của Bạn \n Tên Tài Khoản : " + customerViewModel.userName + "\n Mật Khẩu : " + randomPassword).SendAsync();
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
