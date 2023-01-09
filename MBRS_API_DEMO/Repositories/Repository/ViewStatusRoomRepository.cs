using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API_DEMO.DataBase;
using MBRS_API_DEMO.Models.StoreProcedure;
using MBRS_API_DEMO.Utils;
using Microsoft.Data.SqlClient;

namespace MBRS_API.Repositories.Repository
{
    public class ViewStatusRoomRepository : IViewStatusRoomRepository
    {
        private readonly string _sqlGetAllRoom = "SELECT room_information.room_id,room_information.room_name,t2.type_room_name,t2.full_name,t2.check_in,t2.check_out,t2.status_room,order_id,room_information.floor_id,order_room_detail_id FROM room_information\r\nINNER JOIN\r\n(SELECT room_information.room_id,room_information.room_name,type_room.type_room_name,t1.full_name,t1.check_in,t1.check_out, " +
                                                 "CASE " +
                                                 "WHEN status_room IS NULL THEN 0 " +
                                                 "ELSE status_room " +
                                                 "END " +
                                                 "as status_room,order_id,room_information.floor_id,order_room_detail_id FROM room_information " +
                                                 "LEFT JOIN " +
                                                 "(SELECT full_name,check_in,check_out,room_name,order_room_detail.room_id,status_room,order_room.order_id,order_room_detail.order_room_detail_id " +
                                                 "FROM order_room " +
                                                 "INNER JOIN order_room_detail ON order_room_detail.order_id = order_room.order_id " +
                                                 "INNER JOIN room_information ON room_information.room_id = order_room_detail.room_id " +
                                                 "LEFT JOIN customer ON customer.customer_id = order_room.customer_id " +
                                                 "WHERE ( @dateSearch between order_room.check_in AND order_room.check_out) AND order_room.delete_flag=0 AND status_room != 3) as t1 ON t1.room_id = room_information.room_id " +
                                                 "INNER JOIN type_room ON type_room.type_room_id = room_information.type_room_id " +
                                                 "WHERE room_information.delete_flag = 0) as t2 ON t2.room_id = room_information.room_id ";

        private readonly string _sqlGetAllFloor = "SELECT floor_id,floor_name FROM floor WHERE delete_flag = @delete_flag;";

        private readonly string _sqlGetCountRoomAlreadyCheckIn = "SELECT COUNT(room_information.room_id) as total_status_Room FROM room_information" +
                                                               " LEFT JOIN (SELECT full_name,check_in,check_out,room_name,order_room_detail.room_id,status_room,order_room.order_id FROM order_room INNER JOIN order_room_detail ON order_room_detail.order_id = order_room.order_id INNER JOIN room_information ON room_information.room_id = order_room_detail.room_id " +
                                                               " LEFT JOIN customer ON customer.customer_id = order_room.customer_id " +
                                                               " WHERE ( @dateSearch between order_room.check_in AND order_room.check_out) AND order_room.delete_flag=0) as t1 ON t1.room_id = room_information.room_id " +
                                                               " INNER JOIN type_room ON type_room.type_room_id = room_information.type_room_id " +
                                                               " WHERE room_information.delete_flag = 0 AND status_room = 2";

        private readonly string _sqlGetCountRoomHaveOrder = "SELECT COUNT(room_information.room_id) as total_status_Room FROM room_information" +
                                                               " LEFT JOIN (SELECT full_name,check_in,check_out,room_name,order_room_detail.room_id,status_room,order_room.order_id FROM order_room INNER JOIN order_room_detail ON order_room_detail.order_id = order_room.order_id INNER JOIN room_information ON room_information.room_id = order_room_detail.room_id " +
                                                               " LEFT JOIN customer ON customer.customer_id = order_room.customer_id " +
                                                               " WHERE ( @dateSearch between order_room.check_in AND order_room.check_out) AND order_room.delete_flag=0) as t1 ON t1.room_id = room_information.room_id " +
                                                               " INNER JOIN type_room ON type_room.type_room_id = room_information.type_room_id " +
                                                               " WHERE room_information.delete_flag = 0 AND status_room = 1";

        private readonly string _sqlGetCountRoomEmpty = "SELECT COUNT(room_information.room_id) as total_status_Room FROM room_information " +
                                                        "INNER JOIN " +
                                                        "(SELECT room_information.room_id,room_information.room_name,type_room.type_room_name,t1.full_name,t1.check_in,t1.check_out, " +
                                                        "CASE " +
                                                        "WHEN status_room IS NULL THEN 0 " +
                                                        "ELSE status_room " +
                                                        "END " +
                                                        "as status_room,order_id,room_information.floor_id,order_room_detail_id FROM room_information " +
                                                        "LEFT JOIN " +
                                                        "(SELECT full_name,check_in,check_out,room_name,order_room_detail.room_id,status_room,order_room.order_id,order_room_detail.order_room_detail_id " +
                                                        "FROM order_room " +
                                                        "INNER JOIN order_room_detail ON order_room_detail.order_id = order_room.order_id " +
                                                        "INNER JOIN room_information ON room_information.room_id = order_room_detail.room_id " +
                                                        "LEFT JOIN customer ON customer.customer_id = order_room.customer_id " +
                                                        "WHERE ( @dateSearch between order_room.check_in AND order_room.check_out) AND order_room.delete_flag=0 AND status_room != 3 ) as t1 ON t1.room_id = room_information.room_id " +
                                                        "INNER JOIN type_room ON type_room.type_room_id = room_information.type_room_id " +
                                                        "WHERE room_information.delete_flag = 0) as t2 ON t2.room_id = room_information.room_id " +
                                                        "WHERE t2.status_room = 0";

        private readonly string _sqlUpdateStatus = "UPDATE order_room_detail SET status_room=@status_room WHERE order_room_detail_id = @order_room_detail_id";

        private readonly SqlServerDBContext _sqlServerDbContext;
        private IConfiguration _configuration { get; }

        private readonly DBWorker dBWorker;
        public ViewStatusRoomRepository(SqlServerDBContext sqlServer, IConfiguration configuration)
        {
            _sqlServerDbContext = sqlServer;
            _configuration = configuration;
            dBWorker = new DBWorker(configuration.GetConnectionString(IConstants.SqlString.SqlServerString));
        }
        public List<StatusRoomViewModel> getAllRoom(DateTime selectDate)
        {
            List<StatusRoomViewModel> roomList = new List<StatusRoomViewModel>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllRoom;
                sqlParameters.Add(new SqlParameter("@dateSearch", selectDate.ToString("yyyy-MM-dd") + " 12:00:00.000"));
                roomList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<StatusRoomViewModel>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return roomList;
        }
        public int getCountRoomCheckIn(DateTime selectDate)
        {
            int roomCount = 0;
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetCountRoomAlreadyCheckIn;
                sqlParameters.Add(new SqlParameter("@dateSearch", selectDate.ToString("yyyy-MM-dd") + " 12:00:00.000"));
                roomCount = dBWorker.ExecuteScalar(commandText, sqlParameters.ToArray()).Convert_ToInt();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return roomCount;
        }

        public int updateStatusRoom(int valueStatus, int orderID)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlUpdateStatus;
                sqlParameters.Add(new SqlParameter("@status_room", valueStatus));
                sqlParameters.Add(new SqlParameter("@order_room_detail_id", orderID));
                dBWorker.ExecuteNonQuery(commandText, sqlParameters.ToArray());
                return ErrorCodeResponse.SUCCESS_CODE;
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return ErrorCodeResponse.FAIL_CODE;
            }
        }

        public int getCountRoomEmpty(DateTime selectDate)
        {
            int roomCount = 0;
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetCountRoomEmpty;
                sqlParameters.Add(new SqlParameter("@dateSearch", selectDate.ToString("yyyy-MM-dd") + " 12:00:00.000"));
                roomCount = dBWorker.ExecuteScalar(commandText, sqlParameters.ToArray()).Convert_ToInt();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return roomCount;
        }
        public int getCountRoomHaveOrder(DateTime selectDate)
        {
            int roomCount = 0;
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetCountRoomHaveOrder;
                sqlParameters.Add(new SqlParameter("@dateSearch", selectDate.ToString("yyyy-MM-dd") + " 12:00:00.000"));
                roomCount = dBWorker.ExecuteScalar(commandText, sqlParameters.ToArray()).Convert_ToInt();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return roomCount;
        }
        public List<Floor> getAllFloor()
        {
            List<Floor> floorList = new List<Floor>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllFloor;
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                floorList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<Floor>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return floorList;
        }
    }
}
