using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API_DEMO.DataBase;
using MBRS_API_DEMO.Models.StoreProcedure;
using MBRS_API_DEMO.Utils;
using Microsoft.Data.SqlClient;
using System.Data;
using static MBRS_API_DEMO.Utils.IConstants;

namespace MBRS_API.Repositories.Repository
{
    public class OrderFoodRepository : IOrderFoodRepository
    {
        private readonly string _sqlGetAllTypeFood = @"SELECT food_id,food_name,price,quantity,food_code,image_base64 FROM food
                                                       INNER JOIN type_food ON type_food.type_food_id = food.type_food_id
                                                       WHERE type_food.delete_flag = @delete_flag AND food.delete_flag=@delete_flag2 AND quantity != 0";

        private readonly string _sqlGetCustomerInformation = @"SELECT customer_id,full_name,email,phone_number,identify_number
                                                              FROM user_account
                                                              INNER JOIN customer ON customer.account_id = user_account.account_id
                                                              WHERE user_account.account_id= @account_id";

        private readonly string _sqlInsertOrderFood = @"INSERT INTO order_food (order_code,price,content_payment,status_payment,content_result_payment,bank_transaction_number,vnp_transaction_number,type_payment,bank_code,date_time,customer_id,delete_flag,status_food,notification_order_receptionist_id)
                                                        VALUES (@order_code,@price,@content_payment,@status_payment,@content_result_payment,@bank_transaction_number,@vnp_transaction_number,@type_payment,@bank_code,@date_time,@customer_id,@delete_flag,@status_food,@notification_order_receptionist_id); SELECT SCOPE_IDENTITY()";

        private readonly string _sqlInsertOrderFoodDetail = @"INSERT INTO order_food_detail (order_id,food_id,quantity) VALUES (@order_id0,@food_id0,@quantity0)";

        private readonly string _sqlGetRoomName = "SELECT room_name " +
                                                  "FROM customer " +
                                                  "INNER JOIN order_room ON order_room.customer_id = customer.customer_id " +
                                                  "INNER JOIN order_room_detail ON order_room_detail.order_id = order_room.order_id " +
                                                  "INNER JOIN room_information ON room_information.room_id = order_room_detail.room_id " +
                                                  "WHERE customer.customer_id = @customer_id AND @date_now BETWEEN order_room.check_in AND order_room.check_out";

        private readonly string _sqlGetCustomerInformationByIdentity = @"SELECT customer_id,full_name,email,phone_number,identify_number,day_of_birth
                                                                         FROM user_account
                                                                         INNER JOIN customer ON customer.account_id = user_account.account_id
                                                                         WHERE customer.identify_number= @identify_number";

        private readonly string _sqlGetCustomerInformationByRoomName = @"SELECT order_room.customer_id,full_name,email,phone_number,identify_number,day_of_birth
                                                                         FROM room_information
                                                                         INNER JOIN order_room_detail ON order_room_detail.room_id = room_information.room_id
                                                                         INNER JOIN order_room ON  order_room_detail.order_id = order_room.order_id
                                                                         INNER JOIN customer ON  order_room.customer_id = customer.customer_id
                                                                         INNER JOIN user_account ON customer.account_id = user_account.account_id
                                                                         WHERE room_name = @room_name  AND @date_now between order_room.check_in AND order_room.check_out";

        private readonly string _sqlUpdateFood = @"UPDATE food SET quantity = (food.quantity - @quantity0) WHERE food_id = @food_id0";

        private readonly string _sqlInsertNotificationReceptionist = @"INSERT INTO notification_order_receptionist (content_notification,date_time,status_notification,type_notification) VALUES (@content_notification,@date_time,@status_notification,@type_notification); SELECT SCOPE_IDENTITY()";

        private readonly SqlServerDBContext _sqlServerDbContext;
        private IConfiguration _configuration { get; }

        private readonly DBWorker dBWorker;
        public OrderFoodRepository(SqlServerDBContext sqlServer, IConfiguration configuration)
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
        public List<RoomInformation> getAllRoomNameByCustomerID(int customerID, string dateNow)
        {
            List<RoomInformation> roomNameList = new List<RoomInformation>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetRoomName;
                sqlParameters.Add(new SqlParameter("@customer_id", customerID));
                sqlParameters.Add(new SqlParameter("@date_now", dateNow));
                roomNameList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<RoomInformation>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return roomNameList;
        }
        public List<Food> getAllFood()
        {
            List<Food> foodList = new List<Food>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllTypeFood;
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                sqlParameters.Add(new SqlParameter("@delete_flag2", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                foodList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<Food>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return foodList;
        }

        public int createOrderFood(List<OrderFoodInformationViewModel> orderFoodInformationViewModels)
        {
            try
            {
                string additionStringContentPayment = string.Empty;
                string roomNameString = string.Empty;
                List<RoomInformation> roomNameList = new List<RoomInformation>();
                roomNameList = getAllRoomNameByCustomerID(orderFoodInformationViewModels[0].customerID, Common.ConvertUTCDateTime().ToString("yyyy-MM-dd") + " 12:00:00.000");
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString(IConstants.SqlString.SqlServerString)))
                {
                    string insertOrderDetail = _sqlInsertOrderFoodDetail;
                    string updateFood = _sqlUpdateFood;

                    for (int i = 0; i < orderFoodInformationViewModels.Count; i++)
                    {
                        if (i == 0)
                        {
                            additionStringContentPayment = " " + orderFoodInformationViewModels[i].quanity + "x " + orderFoodInformationViewModels[i].foodName;
                        }
                        else
                        {
                            insertOrderDetail = insertOrderDetail + $", (@order_id{i},@food_id{i},@quantity{i})";
                            updateFood = updateFood + $" ; UPDATE food SET quantity = (food.quantity - @quantity{i}) WHERE food_id = @food_id{i}";
                            additionStringContentPayment = additionStringContentPayment + ", " + orderFoodInformationViewModels[i].quanity + "x " + orderFoodInformationViewModels[i].foodName;
                        }
                    }
                    for (int j = 0; j < roomNameList.Count; j++)
                    {
                        if (j == 0)
                        {
                            roomNameString = " ("+ roomNameList[j].roomName;
                        }
                        else if(j == (roomNameList.Count-1))
                        {
                            roomNameString = roomNameString + "," + roomNameList[j].roomName + ")";
                        } 
                        else
                        {
                            roomNameString = roomNameString + "," + roomNameList[j].roomName;
                        }
                    }
                    string stringQueryNotificationReceptionist = _sqlInsertNotificationReceptionist;
                    string stringQueryCreateOrderFood = _sqlInsertOrderFood;
                    string stringQueryCreateOrderFoodDetail = insertOrderDetail;
                    string stringQueryUpdateFood = updateFood;

                    SqlCommand oCommand1 = new SqlCommand(stringQueryNotificationReceptionist, oConnection);
                    SqlCommand oCommand2 = new SqlCommand(stringQueryCreateOrderFood, oConnection);
                    SqlCommand oCommand3 = new SqlCommand(stringQueryCreateOrderFoodDetail, oConnection);
                    SqlCommand oCommand4 = new SqlCommand(stringQueryUpdateFood, oConnection);

                    oCommand1.CommandType = CommandType.Text;
                    oCommand2.CommandType = CommandType.Text;
                    oCommand3.CommandType = CommandType.Text;
                    oCommand4.CommandType = CommandType.Text;

                    oConnection.Open();
                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {
                            oCommand1.Parameters.Add(new SqlParameter("@content_notification", "Có đơn đặt đồ ăn " + orderFoodInformationViewModels[0].orderCode + " mới từ khách hàng"));
                            oCommand1.Parameters.Add(new SqlParameter("@date_time", Common.ConvertUTCDateTime().ToString("yyyy-MM-dd HH:mm") + ":00.000"));
                            oCommand1.Parameters.Add(new SqlParameter("@status_notification", true));
                            oCommand1.Parameters.Add(new SqlParameter("@type_notification", '2'));
                            oCommand1.Transaction = oTransaction;
                            decimal notificationID = (decimal)oCommand1.ExecuteScalar();

                            oCommand2.Parameters.Add(new SqlParameter("@order_code", orderFoodInformationViewModels[0].orderCode));
                            oCommand2.Parameters.Add(new SqlParameter("@price", orderFoodInformationViewModels[0].price));
                            oCommand2.Parameters.Add(new SqlParameter("@content_payment", orderFoodInformationViewModels[0].contentPayment + additionStringContentPayment + roomNameString));
                            oCommand2.Parameters.Add(new SqlParameter("@status_payment", orderFoodInformationViewModels[0].statusPayment));
                            oCommand2.Parameters.Add(new SqlParameter("@content_result_payment", orderFoodInformationViewModels[0].contentResultPayment));
                            oCommand2.Parameters.Add(new SqlParameter("@bank_transaction_number", orderFoodInformationViewModels[0].bankTransactionNumber));
                            oCommand2.Parameters.Add(new SqlParameter("@vnp_transaction_number", orderFoodInformationViewModels[0].vnpTransactionNumber));
                            oCommand2.Parameters.Add(new SqlParameter("@type_payment", orderFoodInformationViewModels[0].typePayment));
                            oCommand2.Parameters.Add(new SqlParameter("@bank_code", orderFoodInformationViewModels[0].bankCode));
                            oCommand2.Parameters.Add(new SqlParameter("@date_time", orderFoodInformationViewModels[0].dateTime));
                            oCommand2.Parameters.Add(new SqlParameter("@customer_id", orderFoodInformationViewModels[0].customerID));
                            oCommand2.Parameters.Add(new SqlParameter("@notification_order_receptionist_id", notificationID));
                            oCommand2.Parameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                            oCommand2.Parameters.Add(new SqlParameter("@status_food", '0'));
                            oCommand2.Transaction = oTransaction;
                            decimal orderID = (decimal)oCommand2.ExecuteScalar();

                            for (int i = 0; i < orderFoodInformationViewModels.Count; i++)
                            {
                                oCommand3.Parameters.Add(new SqlParameter("@order_id" + i, orderID));
                                oCommand3.Parameters.Add(new SqlParameter("@food_id" + i, orderFoodInformationViewModels[i].foodID));
                                oCommand3.Parameters.Add(new SqlParameter("@quantity" + i, orderFoodInformationViewModels[i].quanity));
                                oCommand4.Parameters.Add(new SqlParameter("@food_id" + i, orderFoodInformationViewModels[i].foodID));
                                oCommand4.Parameters.Add(new SqlParameter("@quantity" + i, orderFoodInformationViewModels[i].quanity));
                            }
                            oCommand3.Transaction = oTransaction;
                            oCommand3.ExecuteNonQuery();
                            oCommand4.Transaction = oTransaction;
                            oCommand4.ExecuteNonQuery();
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
                            oCommand3.Dispose();
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
        public List<CustomerViewModel> getCustomerInformationByIdentityNumber(string identityNumber)
        {
            List<CustomerViewModel> customerList = new List<CustomerViewModel>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetCustomerInformationByIdentity;

                sqlParameters.Add(new SqlParameter("@identify_number", identityNumber));
                customerList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<CustomerViewModel>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return customerList;
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
