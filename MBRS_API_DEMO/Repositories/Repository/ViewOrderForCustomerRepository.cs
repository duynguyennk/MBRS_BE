using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API_DEMO.DataBase;
using MBRS_API_DEMO.Models.StoreProcedure;
using MBRS_API_DEMO.Utils;
using Microsoft.Data.SqlClient;

namespace MBRS_API.Repositories.Repository
{
    public class ViewOrderForCustomerRepository : IViewOrderForCustomerRepository
    {
        private readonly string _sqlGetAllOrderRoom = "SELECT order_room.order_id,check_in,check_out,order_code,order_room.price,status_payment,order_room.date_time,type_room.type_room_name,type_room.number_of_child,type_room.number_of_bed,type_room.number_of_view,type_room.number_of_adult,type_room.number_of_bath_room,status_room,rating_id,type_order,order_room.content_payment,order_room.delete_flag,first_image_base64,COUNT(order_room_detail.room_id) as total_room " +
                                                  "FROM order_room " +
                                                  "INNER JOIN order_room_detail on order_room_detail.order_id = order_room.order_id " +
                                                  "INNER JOIN room_information on room_information.room_id = order_room_detail.room_id " +
                                                  "INNER JOIN type_room ON type_room.type_room_id = room_information.type_room_id " +
                                                  "LEFT JOIN rating_room ON rating_room.order_id = order_room.order_id " +
                                                  "WHERE order_room.customer_id = @customer_id " +
                                                  "GROUP BY order_room.order_id,check_in,check_out,order_code,order_room.price,status_payment,order_room.date_time,type_room.type_room_name,type_room.number_of_child,type_room.number_of_bed,type_room.number_of_view,type_room.number_of_adult,type_room.number_of_bath_room,status_room,rating_id,type_order,order_room.content_payment,order_room.delete_flag,first_image_base64 ORDER BY date_time DESC";

        private readonly string _sqlGetAllOrderFood = "SELECT order_food.order_id,order_food.order_code,order_food.price,order_food.status_payment,order_food.date_time,SUBSTRING(order_food.content_payment, 15, LEN(order_food.content_payment)) as content_payment,status_food,rating_id " +
                                                      "FROM order_food " +
                                                      "LEFT JOIN rating_food ON rating_food.order_id=order_food.order_id "+
                                                      "WHERE customer_id = @customer_id ORDER BY date_time DESC";

        private readonly string _sqlGetAllOrderBike = "SELECT order_bike.order_id,order_bike.date_time_get_bike,order_bike.date_time_back_bike,order_bike.order_code,order_bike.price,order_bike.status_payment,order_bike.date_time,SUBSTRING(order_bike.content_payment, 15, LEN(order_bike.content_payment)) as content_payment,status_bike,rating_id " +
                                                      "FROM order_bike " +
                                                      "LEFT JOIN rating_bike ON rating_bike.order_id = order_bike.order_id " +
                                                      "WHERE customer_id = @customer_id ORDER BY date_time DESC";

        private readonly string _sqlGetCustomerInformation = @"SELECT customer_id
                                                              FROM user_account
                                                              INNER JOIN customer ON customer.account_id = user_account.account_id
                                                              WHERE user_account.account_id= @account_id";

        private readonly string _sqlCancelOrderRoom = @"UPDATE order_room SET delete_flag = @delete_flag,content_payment = @content_payment WHERE order_id = @order_id";

        private readonly SqlServerDBContext _sqlServerDbContext;
        private IConfiguration _configuration { get; }

        private readonly DBWorker dBWorker;
        public ViewOrderForCustomerRepository(SqlServerDBContext sqlServer, IConfiguration configuration)
        {
            _sqlServerDbContext = sqlServer;
            _configuration = configuration;
            dBWorker = new DBWorker(configuration.GetConnectionString(IConstants.SqlString.SqlServerString));
        }

        public List<CustomerOrderRoom> getAllOrderRoom(int customerID)
        {
            List<CustomerOrderRoom> roomOrderList = new List<CustomerOrderRoom>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllOrderRoom;
                sqlParameters.Add(new SqlParameter("@customer_id", customerID));
                roomOrderList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<CustomerOrderRoom>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return roomOrderList;
        }
        public List<CustomerOrderBike> getAllOrderBike(int customerID)
        {
            List<CustomerOrderBike> bikeOrderList = new List<CustomerOrderBike>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllOrderBike;
                sqlParameters.Add(new SqlParameter("@customer_id", customerID));
                bikeOrderList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<CustomerOrderBike>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return bikeOrderList;
        }
        public List<CustomerOrderFood> getAllOrderFood(int customerID)
        {
            List<CustomerOrderFood> foodOrderList = new List<CustomerOrderFood>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllOrderFood;
                sqlParameters.Add(new SqlParameter("@customer_id", customerID));
                foodOrderList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<CustomerOrderFood>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return foodOrderList;
        }
        public int cancelOrderRoom(CancelOrderRoomCustomer cancelOrderRoomCustomer)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlCancelOrderRoom;
                sqlParameters.Add(new SqlParameter("@order_id", cancelOrderRoomCustomer.orderID));
                sqlParameters.Add(new SqlParameter("@content_payment", cancelOrderRoomCustomer.contentPayment));
                sqlParameters.Add(new SqlParameter("@delete_flag", '2'));
                dBWorker.GetDataTable(commandText, sqlParameters.ToArray());
                return ErrorCodeResponse.SUCCESS_CODE;
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return ErrorCodeResponse.FAIL_CODE;
            }
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
    }
}
