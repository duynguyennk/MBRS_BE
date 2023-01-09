using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API_DEMO.DataBase;
using MBRS_API_DEMO.Models.StoreProcedure;
using MBRS_API_DEMO.Utils;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MBRS_API.Repositories.Repository
{
    public class OrderBikeRepository : IOrderBikeRepository
    {
        private readonly string _sqlGetAllTypeBike = @"SELECT type_bike.type_bike_id,type_bike.type_bike_name, type_bike.price,type_bike.number_of_seat,image_base64,total_bike
                                                       FROM type_bike
                                                       INNER JOIN
                                                       (
                                                       SELECT type_bike.type_bike_id,type_bike_name, price,number_of_seat,COUNT(t2.bike_id) as total_bike
                                                       FROM type_bike
                                                       INNER JOIN
                                                       (SELECT bike.bike_id, type_bike_id,bike.delete_flag
                                                       FROM bike
                                                       INNER JOIN
                                                       (SELECT bike_id
                                                       FROM bike
                                                       EXCEPT SELECT order_bike_detail.bike_id FROM order_bike 
                                                       INNER JOIN order_bike_detail ON order_bike_detail.order_id = order_bike.order_id
                                                       WHERE (order_bike.date_time_get_bike between @date_time_get_bike AND @date_time_back_bike)  OR delete_flag =  @orderBikeDeleteFlag) as t1 ON t1.bike_id = bike.bike_id) as t2 on t2.type_bike_id = type_bike.type_bike_id
                                                       WHERE t2.delete_flag = @bikeDeleteFlag AND type_bike.delete_flag = @typeBikeDeleteFlag 
                                                       GROUP BY type_bike.type_bike_id,type_bike_name, price,number_of_seat ) as t3 ON  t3.type_bike_id = type_bike.type_bike_id
                                                       WHERE t3.total_bike >= @total_bike";

        private readonly string _sqlGetCustomerInformation = @"SELECT customer_id,full_name,email,phone_number,identify_number
                                                              FROM user_account
                                                              INNER JOIN customer ON customer.account_id = user_account.account_id
                                                              WHERE user_account.account_id= @account_id";

        private readonly string _sqlGetBikeID = @"SELECT TOP(@number_select) bike.bike_id
                                                  FROM bike
                                                  INNER JOIN
                                                  (SELECT bike_id
                                                  FROM bike
                                                  EXCEPT  SELECT order_bike_detail.bike_id FROM order_bike INNER JOIN order_bike_detail ON order_bike_detail.order_id = order_bike.order_id WHERE (order_bike.date_time_get_bike between @date_time_get_bike AND @date_time_back_bike)  OR delete_flag =  @delete_flag) as t1 ON t1.bike_id = bike.bike_id
                                                  WHERE bike.type_bike_id = @type_bike_id";

        private readonly string _sqlInsertOrder = " INSERT INTO order_bike (order_code,price,content_payment,status_payment,content_result_payment,bank_transaction_number,vnp_transaction_number,type_payment,bank_code,date_time,date_time_get_bike,date_time_back_bike,number_hours_rent,customer_id,delete_flag,status_bike,notification_order_receptionist_id) VALUES (@order_code,@price,@content_payment,@status_payment,@content_result_payment,@bank_transaction_number,@vnp_transaction_number,@type_payment,@bank_code,@date_time,@date_time_get_bike,@date_time_back_bike,@number_hours_rent,@customer_id,@delete_flag,@status_bike,@notification_order_receptionist_id); SELECT SCOPE_IDENTITY()";

        private readonly string _sqlInsertOrderDetail = "INSERT INTO order_bike_detail (order_id,bike_id) VALUES (@order_id0,@bike_id0) ";

        private readonly string _sqlGetCustomerInformationByRoomName = @"SELECT order_room.customer_id,full_name,email,phone_number,identify_number,day_of_birth
                                                                         FROM room_information
                                                                         INNER JOIN order_room_detail ON order_room_detail.room_id = room_information.room_id
                                                                         INNER JOIN order_room ON  order_room_detail.order_id = order_room.order_id
                                                                         INNER JOIN customer ON  order_room.customer_id = customer.customer_id
                                                                         INNER JOIN user_account ON customer.account_id = user_account.account_id
                                                                         WHERE room_name = @room_name  AND @date_now between order_room.check_in AND order_room.check_out";

        private readonly string _sqlInsertNotificationReceptionist = @"INSERT INTO notification_order_receptionist (content_notification,date_time,status_notification,type_notification) VALUES (@content_notification,@date_time,@status_notification,@type_notification); SELECT SCOPE_IDENTITY()";

        private readonly SqlServerDBContext _sqlServerDbContext;
        private IConfiguration _configuration { get; }

        private readonly DBWorker dBWorker;
        public OrderBikeRepository(SqlServerDBContext sqlServer, IConfiguration configuration)
        {
            _sqlServerDbContext = sqlServer;
            _configuration = configuration;
            dBWorker = new DBWorker(configuration.GetConnectionString(IConstants.SqlString.SqlServerString));
        }

        public List<CustomerViewModel> getCustomerInformationByRoomName(string roomName)
        {
            List<CustomerViewModel> customerList = new List<CustomerViewModel>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetCustomerInformationByRoomName;
                sqlParameters.Add(new SqlParameter("@room_name", roomName));
                sqlParameters.Add(new SqlParameter("@date_now", Common.ConvertUTCDateTime().ToString("yyyy-MM-dd") + " 12:00:00.000"));
                customerList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<CustomerViewModel>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return customerList;
        }
        public List<TypeBike> getAllTypeBike(DateTime dateGet, string hoursGet, int hoursRent, int quantity)
        {
            List<TypeBike> typeRoomList = new List<TypeBike>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllTypeBike;
                if (hoursGet != null && hoursRent != 0 && quantity != 0)
                {
                    sqlParameters.Add(new SqlParameter("@date_time_get_bike", dateGet.ToString("yyyy-MM-dd") + " " + hoursGet + ".000"));
                    int totalNumberHours = Int32.Parse(hoursGet.Substring(0, 2)) + hoursRent;
                    if (totalNumberHours < 10)
                    {
                        sqlParameters.Add(new SqlParameter("@date_time_back_bike", dateGet.ToString("yyyy-MM-dd") + " 0" + totalNumberHours + ":00:00.000"));
                    }
                    else
                    {
                        sqlParameters.Add(new SqlParameter("@date_time_back_bike", dateGet.ToString("yyyy-MM-dd") + " " + totalNumberHours + ":00:00.000"));
                    }
                    sqlParameters.Add(new SqlParameter("@orderBikeDeleteFlag", IConstants.CHECKING_STATUS_DELETE_FLAG.Deleted));
                    sqlParameters.Add(new SqlParameter("@total_bike", quantity));
                    sqlParameters.Add(new SqlParameter("@bikeDeleteFlag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                    sqlParameters.Add(new SqlParameter("@typeBikeDeleteFlag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                    typeRoomList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<TypeBike>();
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return typeRoomList;
        }
        public List<Bike> getBikeID(int typeBikeID, int numberOfBike, string dateGetBike, string dateBackBike)
        {
            List<Bike> bikeIDList = new List<Bike>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetBikeID;
                sqlParameters.Add(new SqlParameter("@number_select", numberOfBike));
                sqlParameters.Add(new SqlParameter("@date_time_get_bike", dateGetBike));
                sqlParameters.Add(new SqlParameter("@date_time_back_bike", dateBackBike));
                sqlParameters.Add(new SqlParameter("@type_bike_id", typeBikeID));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.Deleted));
                bikeIDList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<Bike>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return bikeIDList;
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

        public int createOrderBike(OrderBikeInformationViewModel orderBikeInformationViewModel)
        {
            try
            {
                string multiInsertOrderDetail = _sqlInsertOrderDetail;
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString(IConstants.SqlString.SqlServerString)))
                {
                    List<Bike> bikeIDList = new List<Bike>();
                    string dateBackBike = string.Empty;
                    string dateGetBike = orderBikeInformationViewModel.dateTimeGetBike.ToString("yyyy-MM-dd") + " " + orderBikeInformationViewModel.hoursGetBike + ".000";
                    int totalNumberHours = Int32.Parse(orderBikeInformationViewModel.hoursGetBike.Substring(0, 2)) + orderBikeInformationViewModel.numberHoursRent;
                    if (totalNumberHours < 10)
                    {
                        dateBackBike = orderBikeInformationViewModel.dateTimeGetBike.ToString("yyyy-MM-dd") + " 0" + totalNumberHours + ":00:00.000";
                    }
                    else
                    {
                        dateBackBike = orderBikeInformationViewModel.dateTimeGetBike.ToString("yyyy-MM-dd") + " " + totalNumberHours + ":00:00.000";
                    }
                    bikeIDList = getBikeID(orderBikeInformationViewModel.typeBikeID, orderBikeInformationViewModel.numberOfBike, dateGetBike, dateBackBike);
                    if (bikeIDList.Count > 1)
                    {
                        for (int i = 1; i < bikeIDList.Count; i++)
                        {
                            multiInsertOrderDetail = multiInsertOrderDetail + $" ,(@order_id{i},@bike_id{i})";
                        }
                    }
                    string stringQueryNotificationReceptionist = _sqlInsertNotificationReceptionist;
                    string stringQueryCreateOrderBike = _sqlInsertOrder;
                    string stringQueryMultiCreateOrderDetail = multiInsertOrderDetail;

                    SqlCommand oCommand1 = new SqlCommand(stringQueryNotificationReceptionist, oConnection);
                    SqlCommand oCommand2 = new SqlCommand(stringQueryCreateOrderBike, oConnection);
                    SqlCommand oCommand3 = new SqlCommand(stringQueryMultiCreateOrderDetail, oConnection);

                    oCommand1.CommandType = CommandType.Text;
                    oCommand2.CommandType = CommandType.Text;
                    oCommand3.CommandType = CommandType.Text;

                    oConnection.Open();

                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {
                            oCommand1.Parameters.Add(new SqlParameter("@content_notification", "Có đơn đặt xe đạp " + orderBikeInformationViewModel.orderCode + " mới từ khách hàng"));
                            oCommand1.Parameters.Add(new SqlParameter("@date_time", Common.ConvertUTCDateTime().ToString("yyyy-MM-dd HH:mm") + ":00.000"));
                            oCommand1.Parameters.Add(new SqlParameter("@status_notification", true));
                            oCommand1.Parameters.Add(new SqlParameter("@type_notification", '3'));
                            oCommand1.Transaction = oTransaction;
                            decimal notificationID = (decimal)oCommand1.ExecuteScalar();

                            oCommand2.Parameters.Add(new SqlParameter("@order_code", orderBikeInformationViewModel.orderCode));
                            oCommand2.Parameters.Add(new SqlParameter("@price", orderBikeInformationViewModel.price));
                            oCommand2.Parameters.Add(new SqlParameter("@content_payment", orderBikeInformationViewModel.contentPayment));
                            oCommand2.Parameters.Add(new SqlParameter("@status_payment", orderBikeInformationViewModel.statusPayment));
                            oCommand2.Parameters.Add(new SqlParameter("@content_result_payment", orderBikeInformationViewModel.contentResultPayment));
                            oCommand2.Parameters.Add(new SqlParameter("@bank_transaction_number", orderBikeInformationViewModel.bankTransactionNumber));
                            oCommand2.Parameters.Add(new SqlParameter("@vnp_transaction_number", orderBikeInformationViewModel.vnpTransactionNumber));
                            oCommand2.Parameters.Add(new SqlParameter("@type_payment", orderBikeInformationViewModel.typePayment));
                            oCommand2.Parameters.Add(new SqlParameter("@bank_code", orderBikeInformationViewModel.bankCode));
                            oCommand2.Parameters.Add(new SqlParameter("@date_time", orderBikeInformationViewModel.dateTime));
                            oCommand2.Parameters.Add(new SqlParameter("@date_time_get_bike", dateGetBike));
                            oCommand2.Parameters.Add(new SqlParameter("@date_time_back_bike", dateBackBike));
                            oCommand2.Parameters.Add(new SqlParameter("@number_hours_rent", orderBikeInformationViewModel.numberHoursRent));
                            oCommand2.Parameters.Add(new SqlParameter("@customer_id", orderBikeInformationViewModel.customerID));
                            oCommand2.Parameters.Add(new SqlParameter("@notification_order_receptionist_id", notificationID));
                            oCommand2.Parameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                            oCommand2.Parameters.Add(new SqlParameter("@status_bike", '0'));
                            oCommand2.Transaction = oTransaction;
                            decimal orderID = (decimal)oCommand2.ExecuteScalar();

                            for (int i = 0; i < bikeIDList.Count; i++)
                            {
                                oCommand3.Parameters.Add(new SqlParameter("@order_id"+i, orderID));
                                oCommand3.Parameters.Add(new SqlParameter("@bike_id"+i, bikeIDList[i].bikeID));
                               
                            }
                            oCommand3.Transaction = oTransaction;
                            oCommand3.ExecuteNonQuery();

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
