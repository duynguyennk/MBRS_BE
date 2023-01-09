using FluentEmail.Core;
using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API_DEMO.DataBase;
using MBRS_API_DEMO.Models.StoreProcedure;
using MBRS_API_DEMO.Repositories.IRepository;
using MBRS_API_DEMO.Utils;
using Microsoft.Data.SqlClient;
using System.Data;
using User = MBRS_API_DEMO.Models.User;

namespace MBRS_API_DEMO.Repositories.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly string _sqlCheckLogin = @"SELECT u.account_id as AccountID,user_name as UserName, d.department_code as DepartmentCode, t.full_name as FullName, d.department_name as DepartmentName FROM user_account u
                                                LEFT JOIN 
                                                (SELECT full_name,account_id FROM customer 
                                                UNION
                                                SELECT full_name,account_id FROM employee) t ON u.account_id = t.account_id
                                                LEFT JOIN  department d ON d.department_id = u.department_id
                                                WHERE u.user_name=@user_name AND password = @password AND u.delete_flag =@delete_flag ";

        private readonly string _sqlCheckIsNotActive = @"SELECT delete_flag FROM user_account WHERE user_name = @user_name";

        private readonly string _sqlChangePassword = @"UPDATE user_account SET password = @password WHERE user_name = @user_name ";

        private readonly string _sqlPasswordCorrect = @"SELECT COUNT(user_name) FROM user_account WHERE user_name = @user_name AND  password = @password";

        private readonly string _sqlCheckExistEmail = @"SELECT COUNT(email)  FROM user_account WHERE email = @email AND user_name = @user_name";

        private readonly string _sqlUpdatePassword = @"UPDATE user_account SET password = @password WHERE email = @email";

        private readonly string _sqlGetCustomerInformation = @"SELECT customer_id,full_name,email,phone_number,identify_number,day_of_birth
                                                              FROM user_account
                                                              INNER JOIN customer ON customer.account_id = user_account.account_id
                                                              WHERE user_account.account_id= @account_id";

        private readonly SqlServerDBContext _sqlServerDbContext;
        private IConfiguration _configuration { get; }
        private readonly DBWorker dBWorker;
        public readonly IFluentEmail _fluentEmail;
        public LoginRepository(SqlServerDBContext sqlServer, IConfiguration configuration, IFluentEmail fluentEmail)
        {
            _sqlServerDbContext = sqlServer;
            _configuration = configuration;
            dBWorker = new DBWorker(configuration.GetConnectionString("SqlServerConnection"));
            _fluentEmail = fluentEmail;
        }

        public int ForgetPassword(User user)
        {
            int checkEmailExist = 0;
            try
            {
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString(IConstants.SqlString.SqlServerString)))
                {
                    string stringQueryCheckExistEmail = _sqlCheckExistEmail;
                    string stringQueryUpdatePassword = _sqlUpdatePassword;
                    SqlCommand oCommand1 = new SqlCommand(stringQueryCheckExistEmail, oConnection);
                    SqlCommand oCommand2 = new SqlCommand(stringQueryUpdatePassword, oConnection);
                    oCommand1.CommandType = CommandType.Text;
                    oCommand2.CommandType = CommandType.Text;
                    oConnection.Open();
                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {
                            oCommand1.Parameters.Add(new SqlParameter("@email", user.Email));
                            oCommand1.Parameters.Add(new SqlParameter("@user_name", user.UserName));
                            oCommand1.Transaction = oTransaction;
                            checkEmailExist = (int)oCommand1.ExecuteScalar();
                            if (checkEmailExist == 1)
                            {
                                string randomPassword = Common.RandomPassword();
                                string HashPasswordMD5 = Common.GetMD5Password(randomPassword);
                                oCommand2.Parameters.Add(new SqlParameter("@email", user.Email));
                                oCommand2.Parameters.Add(new SqlParameter("@password", HashPasswordMD5));
                                oCommand2.Transaction = oTransaction;
                                oCommand2.ExecuteNonQuery();
                                _fluentEmail.To(user.Email).Subject("Mật Khẩu Mới Của Bạn").Body(" Mật Khẩu Mới Của Bạn Là : " + randomPassword).SendAsync();
                            }
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
        public User CheckLoginByUser(User user)
        {
            User userLogin = new User();
            try
            {
                string password = Common.GetMD5Password(user.Password);
                SqlParameter[] _params = new SqlParameter[3];
                _params[0] = new SqlParameter("@user_name", user.UserName.Convert_ToString());
                _params[1] = new SqlParameter("@password", password);
                _params[2] = new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete);
                userLogin = dBWorker.GetDataTable(_sqlCheckLogin, _params).ToList<User>().SingleOrDefault();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return userLogin;
        }

        public int CheckNotActive(string UserName)
        {
            int checkFlag = 0;
            try
            {
                SqlParameter[] _params = new SqlParameter[1];
                _params[0] = new SqlParameter("@user_name", UserName.Convert_ToString());
                checkFlag = dBWorker.ExecuteScalar(_sqlCheckIsNotActive, _params).ConvertToInt();

            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return checkFlag;

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

        public int ChangePassword(ChangePassword changePassword)
        {
            int checkFlag = 0;
            try
            {
                string password = Common.GetMD5Password(changePassword.newPassword);
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString(IConstants.SqlString.SqlServerString)))
                {
                    string stringQueryChangePassword = _sqlChangePassword;
                    SqlCommand oCommand = new SqlCommand(stringQueryChangePassword, oConnection);
                    oCommand.CommandType = CommandType.Text;
                    oConnection.Open();
                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {
                            oCommand.Parameters.Add(new SqlParameter("@password", password));
                            oCommand.Parameters.Add(new SqlParameter("@user_name", changePassword.userName));
                            oCommand.Transaction = oTransaction;
                            checkFlag = oCommand.ExecuteNonQuery();
                            oTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            LogUtil.Error(ex.Message, ex);
                            oTransaction.Rollback();
                            return checkFlag;
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
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return checkFlag;
            }
            return checkFlag;
        }

        public int CheckPasswordCorrect(ChangePassword changePassword)
        {
            int checkFlag = 0;
            try
            {
                string password = Common.GetMD5Password(changePassword.oldPassword);
                SqlParameter[] _params = new SqlParameter[2];
                _params[0] = new SqlParameter("@user_name", changePassword.userName);
                _params[1] = new SqlParameter("@password", password);
                checkFlag = dBWorker.ExecuteScalar(_sqlPasswordCorrect, _params).ConvertToInt();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return checkFlag;

        }
    }

}

