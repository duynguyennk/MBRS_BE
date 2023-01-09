using MBRS_API.Models;
using MBRS_API.Repositories.IRepository;
using MBRS_API_DEMO.DataBase;
using MBRS_API_DEMO.Models.StoreProcedure;
using MBRS_API_DEMO.Utils;
using Microsoft.Data.SqlClient;
using System.Globalization;

namespace MBRS_API.Repositories.Repository
{
    public class ManageOrderRoomRepository : IManageOrderRoomRepository
    {
        private readonly string _sqlGetAllOrderRoom = @"SELECT order_id,order_code,check_in,check_out,price, full_name,identify_number,phone_number,content_payment,bank_transaction_number,vnp_transaction_number,bank_code,type_payment,date_time,status_payment,content_result_payment,type_order,order_room.delete_flag 
                                                        FROM order_room 
                                                        LEFT JOIN customer ON customer.customer_id = order_room.customer_id ";

        private readonly string _sqlChangeStatusPayment = @"UPDATE order_room SET type_order=@type_order WHERE order_id=@order_id";

        private readonly string _sqlDeleteOrderRoom = @"UPDATE order_room SET delete_flag = @delete_flag WHERE order_id = @order_id";

        private readonly SqlServerDBContext _sqlServerDbContext;
        private IConfiguration _configuration { get; }

        private readonly DBWorker dBWorker;
        public ManageOrderRoomRepository(SqlServerDBContext sqlServer, IConfiguration configuration)
        {
            _sqlServerDbContext = sqlServer;
            _configuration = configuration;
            dBWorker = new DBWorker(configuration.GetConnectionString(IConstants.SqlString.SqlServerString));
        }

        public List<OrderRoom> getAllOrderRoom()
        {
            List<OrderRoom> orderRoomList = new List<OrderRoom>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllOrderRoom + "ORDER BY order_room.date_time DESC";
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                orderRoomList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<OrderRoom>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return orderRoomList;
        }
        public int updateStatusPayment(int orderID)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlChangeStatusPayment;
                sqlParameters.Add(new SqlParameter("@type_order", '1'));
                sqlParameters.Add(new SqlParameter("@order_id", orderID));
                dBWorker.ExecuteNonQuery(commandText, sqlParameters.ToArray());
                return ErrorCodeResponse.SUCCESS_CODE;
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return ErrorCodeResponse.FAIL_CODE;
            }
        }

        public List<OrderRoom> getAllOrderRoomFilter(string filterName, string filterValue)
        {
            List<OrderRoom> orderRoomList = new List<OrderRoom>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllOrderRoom;
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                if (filterName != "")
                {
                    commandText = _sqlGetAllOrderRoom + " WHERE " + filterName + " LIKE @filterValue";
                    if (filterName == "check_in" || filterName == "check_out")
                    {
                        commandText = _sqlGetAllOrderRoom + " WHERE " + "CONVERT(VARCHAR(10)," + filterName + ",120)" + " LIKE @filterValue";
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
                    else if (filterName == "date_time")
                    {
                        if (filterValue != null)
                        {
                            string format = "dd/MM/yyyy";
                            filterValue = (DateTime.ParseExact(filterValue, format, CultureInfo.InvariantCulture)).ToString("yyyyMMdd");
                        }
                        else
                        {
                            filterValue = "";
                        }
                    }
                    sqlParameters.Add(new SqlParameter("@filterValue", "%" + filterValue.Trim() + "%"));
                    commandText = commandText + " ORDER BY order_room.date_time DESC";
                }
                orderRoomList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<OrderRoom>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return orderRoomList;
        }
        public int completedBackPayment(int orderID)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlDeleteOrderRoom;
                sqlParameters.Add(new SqlParameter("@order_id", orderID));
                sqlParameters.Add(new SqlParameter("@delete_flag", '3'));
                dBWorker.GetDataTable(commandText, sqlParameters.ToArray());
                return ErrorCodeResponse.SUCCESS_CODE;
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return ErrorCodeResponse.FAIL_CODE;
            }
        }

        public int deleteOrderRoom(int orderID)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlDeleteOrderRoom;
                sqlParameters.Add(new SqlParameter("@order_id", orderID));
                sqlParameters.Add(new SqlParameter("@delete_flag", '1'));
                dBWorker.GetDataTable(commandText, sqlParameters.ToArray());
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
