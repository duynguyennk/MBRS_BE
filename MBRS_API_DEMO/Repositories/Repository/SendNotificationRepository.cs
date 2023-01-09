using MBRS_API.Models;
using MBRS_API.Repositories.IRepository;
using MBRS_API_DEMO.DataBase;
using MBRS_API_DEMO.Models.StoreProcedure;
using MBRS_API_DEMO.Utils;
using Microsoft.Data.SqlClient;

namespace MBRS_API.Repositories.Repository
{
    public class SendNotificationRepository : ISendNotificationRepository
    {
        private readonly string _sqlGetAllOrderNotificationReceptionist = "SELECT notification_order_receptionist_id,content_notification,status_notification,date_time,type_notification  FROM notification_order_receptionist  ORDER BY date_time DESC";
        private readonly string _sqlUpdateOrderNotificationReceptionist = "UPDATE notification_order_receptionist SET status_notification = @status_notification  WHERE notification_order_receptionist_id=@notification_order_receptionist_id";
        private readonly SqlServerDBContext _sqlServerDbContext;
        private IConfiguration _configuration { get; }

        private readonly DBWorker dBWorker;
        public SendNotificationRepository(SqlServerDBContext sqlServer, IConfiguration configuration)
        {
            _sqlServerDbContext = sqlServer;
            _configuration = configuration;
            dBWorker = new DBWorker(configuration.GetConnectionString(IConstants.SqlString.SqlServerString));
        }
        public List<NotificationBell> getAllOrderNotificationReceptionist()
        {
            List<NotificationBell> notificationOrderList = new List<NotificationBell>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllOrderNotificationReceptionist;
                notificationOrderList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<NotificationBell>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return notificationOrderList;
        }
        public int UpdateOrderNotificationReceptionist(int notificationID)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlUpdateOrderNotificationReceptionist;
                sqlParameters.Add(new SqlParameter("@notification_order_receptionist_id", notificationID));
                sqlParameters.Add(new SqlParameter("@status_notification", IConstants.STATUS_NOTIFICATION.NotActive));
                dBWorker.ExecuteNonQuery(commandText, sqlParameters.ToArray());
                return ErrorCodeResponse.SUCCESS_CODE;
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return ErrorCodeResponse.FAIL_CODE;
            }
        }
    }
}
