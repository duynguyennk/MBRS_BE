using FluentEmail.Core;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API_DEMO.DataBase;
using MBRS_API_DEMO.Models.StoreProcedure;
using MBRS_API_DEMO.Utils;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MBRS_API.Repositories.Repository
{
    public class CustomerAccountRepository : ICustomerAccountRepository
    {

        private readonly string _sqlCreateCustomerAccount = @"INSERT INTO user_account (user_name,password,email,department_id,delete_flag) VALUES (@user_name,@password,@email,@department_id,@delete_flag); SELECT SCOPE_IDENTITY()";
        
        private readonly string _sqlCreateCustomerInformation = @"INSERT INTO customer (full_name,phone_number,day_of_birth,identify_number,account_id,delete_flag) VALUES (@full_name,@phone_number,@day_of_birth,@identify_number,@account_id,@delete_flag)";
        
        private readonly string _sqlGetCustomerInformation = @"SELECT customer_id,full_name,email,phone_number,identify_number,day_of_birth
                                                              FROM user_account
                                                              INNER JOIN customer ON customer.account_id = user_account.account_id
                                                              WHERE user_account.account_id= @account_id";

        private readonly string _sqlUpdateCustomerAccount = @"UPDATE user_account SET email=@email WHERE account_id=@account_id";

        private readonly string _sqlCheckUserName = @"SELECT COUNT(account_id) FROM user_account WHERE user_name = @user_name";

        private readonly string _sqlCheckEmail = @"SELECT COUNT(account_id) FROM user_account WHERE email = @email";

        private readonly string _sqlCheckIdentityNumber = @"SELECT COUNT(customer_id) FROM customer WHERE identify_number = @identify_number";

        private readonly string _sqlUpdateCustomer = @"UPDATE customer SET full_name=@full_name , phone_number=@phone_number , day_of_birth=@day_of_birth ,identify_number=@identify_number WHERE customer_id=@customer_id";

        private readonly SqlServerDBContext _sqlServerDbContext;
        private IConfiguration _configuration { get; }
        public readonly IFluentEmail _fluentEmail;
        private readonly DBWorker dBWorker;
        public CustomerAccountRepository(SqlServerDBContext sqlServer, IConfiguration configuration, IFluentEmail fluentEmail)
        {
            _sqlServerDbContext = sqlServer;
            _configuration = configuration;
            dBWorker = new DBWorker(configuration.GetConnectionString(IConstants.SqlString.SqlServerString));
            _fluentEmail = fluentEmail;
        }

        public int registerCustomerAccount(CustomerViewModel customerViewModel)
        {
            try
            {
                string HashPasswordMD5 = Common.GetMD5Password(customerViewModel.password);
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
                            _fluentEmail.To(customerViewModel.email).Subject("Chào Mừng Đến Westlake").Body("Cảm Ơn Đã Tin Tưởng Chúng Tôi").SendAsync();
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

        public int checkDuplicateUserName(string userName)
        {
            int checkDuplicate = 0;
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlCheckUserName;
                sqlParameters.Add(new SqlParameter("@user_name", userName));
                checkDuplicate = Convert.ToInt32( dBWorker.ExecuteScalar(commandText, sqlParameters.ToArray()));
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

        public List<CustomerViewModel> getCustomerInformationByID(int accountID)
        {
            List<CustomerViewModel> customerList = new List<CustomerViewModel>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetCustomerInformation;

                sqlParameters.Add(new SqlParameter("@account_id", accountID));
                customerList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<CustomerViewModel>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return customerList;
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
    }
}
