using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API_DEMO.DataBase;
using MBRS_API_DEMO.Models.StoreProcedure;
using MBRS_API_DEMO.Utils;
using Microsoft.Data.SqlClient;

namespace MBRS_API.Repositories.Repository
{
    public class FilterServiceCustomerRepository : IFilterServiceCustomerRepository
    {
        private readonly string _sqlCheckUsingCustomerService = @"SELECT COUNT(user_account.account_id) as count
                                                                  FROM user_account
                                                                  INNER JOIN customer ON customer.account_id = user_account.account_id
                                                                  INNER JOIN order_room ON order_room.customer_id = customer.customer_id 
                                                                  INNER JOIN order_room_detail ON order_room_detail.order_id = order_room.order_id
                                                                  WHERE @date_now BETWEEN order_room.check_in AND order_room.check_out AND user_account.account_id = @account_id AND status_room =@status_room";
        private readonly SqlServerDBContext _sqlServerDbContext;
        private IConfiguration _configuration { get; }

        private readonly DBWorker dBWorker;
        public FilterServiceCustomerRepository(SqlServerDBContext sqlServer, IConfiguration configuration)
        {
            _sqlServerDbContext = sqlServer;
            _configuration = configuration;
            dBWorker = new DBWorker(configuration.GetConnectionString(IConstants.SqlString.SqlServerString));
        }
        public int checkUsingCustomerService(int accountID)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlCheckUsingCustomerService;
                sqlParameters.Add(new SqlParameter("@date_now", Common.ConvertUTCDateTime().ToString("yyyy-MM-dd") + " 12:00:00.000"));
                sqlParameters.Add(new SqlParameter("@account_id", accountID));
                sqlParameters.Add(new SqlParameter("@status_room", '2'));
                int count =(int) dBWorker.ExecuteScalar(commandText, sqlParameters.ToArray());
                if(count >=1)
                {
                    return ErrorCodeResponse.SUCCESS_CODE;
                }
                else
                {
                    return ErrorCodeResponse.FAIL_CODE;
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
