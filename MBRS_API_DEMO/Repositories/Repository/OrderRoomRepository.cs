using FluentEmail.Core;
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
    public class OrderRoomRepository : IOrderRoomRepository
    {
        private readonly string _sqlGetAllTypeRoom = @"SELECT type_room.type_room_id,type_room.type_room_name,type_room.number_of_bed,type_room.number_of_bath_room,type_room.number_of_adult,type_room.number_of_child,type_room.price,type_room.number_of_view,first_image_base64,t3.total_room
                                                       FROM type_room
                                                       INNER JOIN 
                                                       (SELECT type_room.type_room_id,type_room_name,number_of_bed,number_of_bath_room,number_of_adult,number_of_child,type_room.price,number_of_view,COUNT(t2.room_id) as total_room
                                                       FROM type_room
                                                       INNER JOIN
                                                       (SELECT room_information.room_id,room_information.type_room_id
                                                       FROM room_information
                                                       INNER JOIN  (SELECT room_information.room_id FROM room_information
                                                       EXCEPT SELECT order_room_detail.room_id FROM order_room_detail
                                                       INNER JOIN order_room ON order_room.order_id = order_room_detail.order_id
                                                       WHERE (((@check_in BETWEEN order_room.check_in AND order_room.check_out) OR (@check_out BETWEEN order_room.check_in AND order_room.check_out)) OR ((order_room.check_in BETWEEN @check_in AND @check_out)  OR (order_room.check_out BETWEEN @check_in AND @check_out))) AND order_room.delete_flag = 0) as t1 ON t1.room_id = room_information.room_id
                                                       WHERE room_information.delete_flag = @delete_flag) as t2 ON t2.type_room_id = type_room.type_room_id
                                                       GROUP BY type_room.type_room_id,type_room_name,number_of_bed,number_of_bath_room,number_of_adult,number_of_child,type_room.price,number_of_view) as t3 ON t3.type_room_id = type_room.type_room_id
                                                       WHERE t3.total_room >= @total_room AND type_room.number_of_child >= @number_of_child AND type_room.number_of_adult >= @number_of_adult";

        private readonly string _sqlGetInformationTypeRoom = @"SELECT type_room_id,type_room_name,number_of_child,number_of_bed,number_of_adult,number_of_bedroom,number_of_view,number_of_adding_bed,number_of_bath_room,price,type_room_code,content_Introduce_Room,room_square,safety_box,dryer,wifi,iron,tivi,fridge,heater_bath,bathtub,air_condition,heater_room,dinner_table,type_room.list_utilities_id,first_image_base64,second_image_base64,third_image_base64,fourth_image_base64,fifth_image_base64
                                                    FROM type_room
                                                    INNER JOIN list_utilities ON list_utilities.list_utilities_id = type_room.list_utilities_id
                                                    WHERE type_room.delete_flag = @delete_flag and type_room_id = @type_room_id";

        private readonly string _sqlGetListRating = @"SELECT *
                                                      FROM rating_room
                                                      INNER JOIN customer ON customer.customer_id = rating_room.customer_id
                                                      WHERE rating_room.type_room_id = @type_room_id";

        private readonly string _sqlGetRatingPercent = @"SELECT SUM(number_rating_conveniences) as total_number_rating_conveniences,SUM(number_rating_Interior) as total_number_rating_interior,SUM(number_rating_employee) as total_number_rating_employee,SUM(number_rating_service) as total_number_rating_service,SUM(number_rating_hygiene) as total_number_rating_hygiene,SUM(number_rating_view) as total_number_rating_view, COUNT(rating_id) as total_rating
                                                         FROM rating_room
                                                         WHERE type_room_id = @type_room_id";

        private readonly string _sqlCreateGuest = @"INSERT INTO guest (full_name,identity_number,phone_number,day_of_birth,email) VALUES (@full_name,@identity_number,@phone_number,@day_of_birth,@email); SELECT SCOPE_IDENTITY()";

        private readonly string _sqlCreateCustomer = @"INSERT INTO customer (full_name,identify_number,phone_number,day_of_birth,account_id,delete_flag) VALUES (@full_name,@identify_number,@phone_number,@day_of_birth,@account_id,@delete_flag); SELECT SCOPE_IDENTITY()";

        private readonly string _sqlCreateAccount = @"INSERT INTO user_account (user_name,password,email,department_id,delete_flag) VALUES (@user_name,@password,@email,@department_id,@delete_flag); SELECT SCOPE_IDENTITY()";

        private readonly string _sqlCreateOderForGuest = @"INSERT INTO order_room (order_code,check_in,check_out,price,guest_id,content_payment,status_payment,content_result_payment,bank_transaction_number,vnp_transaction_number,type_payment,bank_code,date_time,delete_flag,type_order) VALUES (@order_code,@check_in,@check_out,@price,@guest_id,@content_payment,@status_payment,@content_result_payment,@bank_transaction_number,@vnp_transaction_number,@type_payment,@bank_code,@date_time,@delete_flag,@type_order); SELECT SCOPE_IDENTITY()";

        private readonly string _sqlCreateOderRoomReceptionist = @"INSERT INTO order_room (order_code,check_in,check_out,price,customer_id,content_payment,status_payment,content_result_payment,bank_transaction_number,vnp_transaction_number,type_payment,bank_code,date_time,delete_flag,type_order,notification_order_receptionist_id) VALUES (@order_code,@check_in,@check_out,@price,@customer_id,@content_payment,@status_payment,@content_result_payment,@bank_transaction_number,@vnp_transaction_number,@type_payment,@bank_code,@date_time,@delete_flag,@type_order,@notification_order_receptionist_id); SELECT SCOPE_IDENTITY()";

        private readonly string _sqlCreateOderRoomCash = @"INSERT INTO order_room (order_code, check_in, check_out, price, customer_id, content_payment, status_payment, bank_transaction_number, date_time, delete_flag, type_order,notification_order_receptionist_id) VALUES (@order_code, @check_in, @check_out, @price, @customer_id, @content_payment, @status_payment, @bank_transaction_number, @date_time, @delete_flag, @type_order,@notification_order_receptionist_id); SELECT SCOPE_IDENTITY()";

        private readonly string _sqlCreateOderUnpaymentForCustomer = @"INSERT INTO order_room (order_code,check_in,check_out,price,customer_id,content_payment,content_result_payment,date_time,delete_flag,type_order,bank_transaction_number,status_payment) VALUES (@order_code,@check_in,@check_out,@price,@customer_id,@content_payment,@content_result_payment,@date_time,@delete_flag,@type_order,@bank_transaction_number,@status_payment); SELECT SCOPE_IDENTITY()";

        private readonly string _sqlCreateOderForCustomer = @"INSERT INTO order_room (order_code,check_in,check_out,price,customer_id,content_payment,status_payment,content_result_payment,bank_transaction_number,vnp_transaction_number,type_payment,bank_code,date_time,delete_flag,type_order,notification_order_receptionist_id) VALUES (@order_code,@check_in,@check_out,@price,@customer_id,@content_payment,@status_payment,@content_result_payment,@bank_transaction_number,@vnp_transaction_number,@type_payment,@bank_code,@date_time,@delete_flag,@type_order,@notification_order_receptionist_id); SELECT SCOPE_IDENTITY()";

        private readonly string _sqlCreateOderUnpaymentForGuest = @"INSERT INTO order_room (order_code,check_in,check_out,price,guest_id,content_payment,content_result_payment,date_time,delete_flag) VALUES (@order_code,@check_in,@check_out,@price,@guest_id,@content_payment,@content_result_payment,@date_time,@delete_flag); SELECT SCOPE_IDENTITY()";

        private readonly string _sqlGetRoomID = @"SELECT TOP(@number_of_room) room_information.room_id,room_information.room_name
                                                  FROM room_information
                                                  INNER JOIN
                                                  (
                                                 SELECT room_information.room_id FROM room_information
                                                 EXCEPT SELECT order_room_detail.room_id FROM order_room_detail
                                                 INNER JOIN order_room ON order_room.order_id = order_room_detail.order_id
                                                  WHERE (((@check_in BETWEEN order_room.check_in AND order_room.check_out) OR (@check_out BETWEEN order_room.check_in AND order_room.check_out)) OR ((order_room.check_in BETWEEN @check_in AND @check_out)  OR (order_room.check_out BETWEEN @check_in AND @check_out))) AND order_room.delete_flag = 0) as t1 ON t1.room_id = room_information.room_id
                                                  WHERE room_information.type_room_id = @type_room_id";

        private readonly string _sqlGetCustomerInformation = @"SELECT customer_id,full_name,email,phone_number,identify_number,day_of_birth
                                                              FROM user_account
                                                              INNER JOIN customer ON customer.account_id = user_account.account_id
                                                              WHERE user_account.account_id= @account_id";

        private readonly string _sqlGetCustomerInformationByIdentity = @"SELECT customer_id,full_name,email,phone_number,identify_number,day_of_birth
                                                                         FROM user_account
                                                                         INNER JOIN customer ON customer.account_id = user_account.account_id
                                                                         WHERE customer.identify_number= @identify_number";

        private readonly string _sqlInsertNotificationReceptionist = @"INSERT INTO notification_order_receptionist (content_notification,date_time,status_notification,type_notification) VALUES (@content_notification,@date_time,@status_notification,@type_notification); SELECT SCOPE_IDENTITY()";

        private readonly string _sqlCreateOrderRoomDetail = @"INSERT INTO order_room_detail (order_id,room_id,status_room) VALUES (@order_id0,@room_id0,@status_room0)";
        private readonly SqlServerDBContext _sqlServerDbContext;
        public readonly IFluentEmail _fluentEmail;
        private IConfiguration _configuration { get; }

        private readonly DBWorker dBWorker;
        public OrderRoomRepository(SqlServerDBContext sqlServer, IConfiguration configuration, IFluentEmail fluentEmail)
        {
            _fluentEmail = fluentEmail;
            _sqlServerDbContext = sqlServer;
            _configuration = configuration;
            dBWorker = new DBWorker(configuration.GetConnectionString(IConstants.SqlString.SqlServerString));
        }
        public List<RoomInformation> getTheRoomID(int typeRoomID, string checkIn, string checkOut, int numberOfRoom)
        {
            List<RoomInformation> typeRoomList = new List<RoomInformation>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetRoomID;
                sqlParameters.Add(new SqlParameter("@number_of_room", numberOfRoom));
                sqlParameters.Add(new SqlParameter("@check_in", checkIn));
                sqlParameters.Add(new SqlParameter("@check_out", checkOut));
                sqlParameters.Add(new SqlParameter("@type_room_id", typeRoomID));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                typeRoomList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<RoomInformation>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return typeRoomList;
        }
        public List<TypeRoomViewModel> getAllTypeRoom(DateTime checkInt, DateTime checkOut, int numberOfRoom, int numberOfChild, int numberOfAdult)
        {
            List<TypeRoomViewModel> typeRoomList = new List<TypeRoomViewModel>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllTypeRoom;
                sqlParameters.Add(new SqlParameter("@check_in", checkInt.ToString("yyyy-MM-dd") + " 12:01:00.000"));
                sqlParameters.Add(new SqlParameter("@check_out", checkOut.ToString("yyyy-MM-dd") + " 11:59:00.000"));
                sqlParameters.Add(new SqlParameter("@total_room", numberOfRoom));
                sqlParameters.Add(new SqlParameter("@number_of_child", numberOfChild));
                sqlParameters.Add(new SqlParameter("@number_of_adult", numberOfAdult));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                typeRoomList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<TypeRoomViewModel>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return typeRoomList;
        }
        public List<TypeRoom> getTypeRoomInformation(int typeRoomID)
        {
            List<TypeRoom> typeRoomList = new List<TypeRoom>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetInformationTypeRoom;
                sqlParameters.Add(new SqlParameter("@type_room_id", typeRoomID));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                typeRoomList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<TypeRoom>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return typeRoomList;
        }
        public List<RatingViewModel> getAllListRating(int typeRoomID)
        {
            List<RatingViewModel> ratingList = new List<RatingViewModel>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetListRating;
                sqlParameters.Add(new SqlParameter("@type_room_id", typeRoomID));
                ratingList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<RatingViewModel>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return ratingList;
        }
        public List<RatingPercentViewModel> getRatingPercent(int typeRoomID)
        {
            List<RatingPercentViewModel> ratingList = new List<RatingPercentViewModel>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetRatingPercent;
                sqlParameters.Add(new SqlParameter("@type_room_id", typeRoomID));
                ratingList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<RatingPercentViewModel>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return ratingList;
        }
        public int createOrderRoomCash(OrderRoomUnpayment orderRoomUnpayment)
        {
            try
            {
                string _mutiSqlCreateOrderRoomDetail = string.Empty;
                string roomName = string.Empty;
                string userName = "westlake" + Common.RandomNumber();
                string randomPassword = Common.RandomPassword();
                string HashPasswordMD5 = Common.GetMD5Password(randomPassword);
                string randomCodeOrder = Common.RandomString();
                List<RoomInformation> roomIDList = new List<RoomInformation>();
                string checkInConvert = orderRoomUnpayment.checkIn.ToString("yyyy-MM-dd") + " 12:00:00.000";
                string checkOutConvert = orderRoomUnpayment.checkOut.ToString("yyyy-MM-dd") + " 12:00:00.000";
                roomIDList = getTheRoomID(orderRoomUnpayment.typeRoomID, checkInConvert, checkOutConvert, orderRoomUnpayment.numberOfRoom);
                if (roomIDList.Count > 1)
                {
                    for (int j = 1; j < roomIDList.Count; j++)
                    {
                        _mutiSqlCreateOrderRoomDetail = _sqlCreateOrderRoomDetail + $", (@order_id{j},@room_id{j},@status_room{j})";
                        if (j == 1)
                        {
                            roomName = roomName + roomIDList[j].roomName;
                        }
                        else
                        {
                            roomName = roomName + "," + roomIDList[j].roomName;
                        }
                    }
                }
                else
                {
                    _mutiSqlCreateOrderRoomDetail = _sqlCreateOrderRoomDetail;
                    roomName = roomIDList[0].roomName;
                }
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString(IConstants.SqlString.SqlServerString)))
                {
                    string stringQueryNotificationReceptionist = _sqlInsertNotificationReceptionist;
                    string stringQueryCreateAccountInformation = _sqlCreateAccount;
                    string stringQueryCreateCustomerInformation = _sqlCreateCustomer;
                    string stringQueryCreateOrderRoom = _sqlCreateOderRoomCash;
                    string stringQueryCreateOrderRoomDetail = _mutiSqlCreateOrderRoomDetail;

                    SqlCommand oCommand1 = new SqlCommand(stringQueryNotificationReceptionist, oConnection);
                    SqlCommand oCommand2 = new SqlCommand(stringQueryCreateAccountInformation, oConnection);
                    SqlCommand oCommand3 = new SqlCommand(stringQueryCreateCustomerInformation, oConnection);
                    SqlCommand oCommand4 = new SqlCommand(stringQueryCreateOrderRoom, oConnection);
                    SqlCommand oCommand5 = new SqlCommand(stringQueryCreateOrderRoomDetail, oConnection);

                    oCommand1.CommandType = CommandType.Text;
                    oCommand2.CommandType = CommandType.Text;
                    oCommand3.CommandType = CommandType.Text;
                    oCommand4.CommandType = CommandType.Text;
                    oCommand5.CommandType = CommandType.Text;

                    oConnection.Open();
                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {
                            oCommand1.Parameters.Add(new SqlParameter("@content_notification", "Có đơn đặt phòng "+ randomCodeOrder + " mới từ khách hàng"));
                            oCommand1.Parameters.Add(new SqlParameter("@date_time", Common.ConvertUTCDateTime().ToString("yyyy-MM-dd HH:mm") + ":00.000"));
                            oCommand1.Parameters.Add(new SqlParameter("@status_notification",true));
                            oCommand1.Parameters.Add(new SqlParameter("@type_notification", '1'));
                            oCommand1.Transaction = oTransaction;
                            decimal notificationID = (decimal)oCommand1.ExecuteScalar();

                            oCommand2.Parameters.Add(new SqlParameter("@user_name", userName));
                            oCommand2.Parameters.Add(new SqlParameter("@password", HashPasswordMD5));
                            oCommand2.Parameters.Add(new SqlParameter("@email", orderRoomUnpayment.email));
                            oCommand2.Parameters.Add(new SqlParameter("@department_id", '2'));
                            oCommand2.Parameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));

                            oCommand2.Transaction = oTransaction;
                            decimal accountID = (decimal)oCommand2.ExecuteScalar();

                            oCommand3.Parameters.Add(new SqlParameter("@full_name", orderRoomUnpayment.fullName));
                            oCommand3.Parameters.Add(new SqlParameter("@identify_number", orderRoomUnpayment.identifyNumber));
                            oCommand3.Parameters.Add(new SqlParameter("@phone_number", orderRoomUnpayment.phoneNumber));
                            oCommand3.Parameters.Add(new SqlParameter("@day_of_birth", orderRoomUnpayment.dateOfBirth.ToString("yyyy-MM-dd")));
                            oCommand3.Parameters.Add(new SqlParameter("@account_id", accountID));
                            oCommand3.Parameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));

                            oCommand3.Transaction = oTransaction;
                            decimal customerID = (decimal)oCommand3.ExecuteScalar();
                           
                            oCommand4.Parameters.Add(new SqlParameter("@order_code", randomCodeOrder));
                            oCommand4.Parameters.Add(new SqlParameter("@check_in", checkInConvert));
                            oCommand4.Parameters.Add(new SqlParameter("@check_out", checkOutConvert));
                            oCommand4.Parameters.Add(new SqlParameter("@type_order", '3'));
                            oCommand4.Parameters.Add(new SqlParameter("@price", orderRoomUnpayment.price));
                            oCommand4.Parameters.Add(new SqlParameter("@customer_id", customerID));
                            oCommand4.Parameters.Add(new SqlParameter("@content_payment", "Trả tiền mặt" + orderRoomUnpayment.numberOfRoom + " phòng " + orderRoomUnpayment.typeRoomName + " " + orderRoomUnpayment.numberOfDay + " Đêm(" + roomName + ")"));
                            oCommand4.Parameters.Add(new SqlParameter("@status_payment", "00"));
                            oCommand4.Parameters.Add(new SqlParameter("@bank_transaction_number", "VNP00000000"));
                            oCommand4.Parameters.Add(new SqlParameter("@date_time", Common.ConvertUTCDateTime().ToString("yyyyMMddHHmmss")));
                            oCommand4.Parameters.Add(new SqlParameter("@notification_order_receptionist_id",notificationID));
                            oCommand4.Parameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));

                            oCommand4.Transaction = oTransaction;
                            decimal orderID = (decimal)oCommand4.ExecuteScalar();

                            for (int i = 0; i < roomIDList.Count; i++)
                            {
                                oCommand5.Parameters.Add(new SqlParameter($"@order_id{i}", orderID));
                                oCommand5.Parameters.Add(new SqlParameter($"@room_id{i}", roomIDList[i].roomID));
                                oCommand5.Parameters.Add(new SqlParameter($"@status_room{i}", IConstants.STATUS_ROOM.OrderRoom));
                            }
                            oCommand5.Transaction = oTransaction;
                            oCommand5.ExecuteNonQuery();

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
                            oCommand3.Dispose();
                            oCommand4.Dispose();
                            oCommand5.Dispose();
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
        public int createOrderRoomReceptionist(OrderRoomInformationViewModel orderInformationViewModel)
        {
            try
            {
                string _mutiSqlCreateOrderRoomDetail = string.Empty;
                string roomName = string.Empty;
                string userName = "westlake" + Common.RandomNumber();
                string randomPassword = Common.RandomPassword();
                string HashPasswordMD5 = Common.GetMD5Password(randomPassword);
                List<RoomInformation> roomIDList = new List<RoomInformation>();
                string checkIn = orderInformationViewModel.checkIn.ToString("yyyy-MM-dd") + " 12:00:00.000";
                string checkOut = orderInformationViewModel.checkOut.ToString("yyyy-MM-dd") + " 12:00:00.000";
                roomIDList = getTheRoomID(orderInformationViewModel.typeRoomID, checkIn, checkOut, orderInformationViewModel.numberOfRoom);
                if (roomIDList.Count > 1)
                {
                    for (int j = 1; j < roomIDList.Count; j++)
                    {
                        _mutiSqlCreateOrderRoomDetail = _sqlCreateOrderRoomDetail + $", (@order_id{j},@room_id{j},@status_room{j})";
                        if (j == 1)
                        {
                            roomName = roomName + roomIDList[j].roomName;
                        }
                        else
                        {
                            roomName = roomName + "," + roomIDList[j].roomName;
                        }
                    }
                }
                else
                {
                    _mutiSqlCreateOrderRoomDetail = _sqlCreateOrderRoomDetail;
                    roomName = roomIDList[0].roomName;
                }
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString(IConstants.SqlString.SqlServerString)))
                {

                    string stringQueryNotificationReceptionist = _sqlInsertNotificationReceptionist;
                    string stringQueryCreateAccountInformation = _sqlCreateAccount;
                    string stringQueryCreateCustomerInformation = _sqlCreateCustomer;
                    string stringQueryCreateOrderRoom = _sqlCreateOderRoomReceptionist;
                    string stringQueryCreateOrderRoomDetail = _mutiSqlCreateOrderRoomDetail;

                    SqlCommand oCommand1 = new SqlCommand(stringQueryNotificationReceptionist, oConnection);
                    SqlCommand oCommand2 = new SqlCommand(stringQueryCreateAccountInformation, oConnection);
                    SqlCommand oCommand3 = new SqlCommand(stringQueryCreateCustomerInformation, oConnection);
                    SqlCommand oCommand4 = new SqlCommand(stringQueryCreateOrderRoom, oConnection);
                    SqlCommand oCommand5 = new SqlCommand(stringQueryCreateOrderRoomDetail, oConnection);

                    oCommand1.CommandType = CommandType.Text;
                    oCommand2.CommandType = CommandType.Text;
                    oCommand3.CommandType = CommandType.Text;
                    oCommand4.CommandType = CommandType.Text;
                    oCommand5.CommandType = CommandType.Text;

                    oConnection.Open();
                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {
                            oCommand1.Parameters.Add(new SqlParameter("@content_notification", "Có đơn đặt phòng " + orderInformationViewModel.codeOrder + " mới từ khách hàng"));
                            oCommand1.Parameters.Add(new SqlParameter("@date_time", Common.ConvertUTCDateTime().ToString("yyyy-MM-dd HH:mm") + ":00.000"));
                            oCommand1.Parameters.Add(new SqlParameter("@status_notification", true));
                            oCommand1.Parameters.Add(new SqlParameter("@type_notification", '1'));
                            oCommand1.Transaction = oTransaction;
                            decimal notificationID = (decimal)oCommand1.ExecuteScalar();

                            oCommand2.Parameters.Add(new SqlParameter("@user_name", userName));
                            oCommand2.Parameters.Add(new SqlParameter("@password", HashPasswordMD5));
                            oCommand2.Parameters.Add(new SqlParameter("@email", orderInformationViewModel.email));
                            oCommand2.Parameters.Add(new SqlParameter("@department_id", '2'));
                            oCommand2.Parameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));

                            oCommand2.Transaction = oTransaction;
                            decimal accountID = (decimal)oCommand2.ExecuteScalar();

                            oCommand3.Parameters.Add(new SqlParameter("@full_name", orderInformationViewModel.fullName));
                            oCommand3.Parameters.Add(new SqlParameter("@identify_number", orderInformationViewModel.identifyNumber));
                            oCommand3.Parameters.Add(new SqlParameter("@phone_number", orderInformationViewModel.phoneNumber));
                            oCommand3.Parameters.Add(new SqlParameter("@day_of_birth", orderInformationViewModel.dateOfBirth.ToString("yyyy-MM-dd")));
                            oCommand3.Parameters.Add(new SqlParameter("@account_id", accountID));
                            oCommand3.Parameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));

                            oCommand3.Transaction = oTransaction;
                            decimal customerID = (decimal)oCommand3.ExecuteScalar();

                            oCommand4.Parameters.Add(new SqlParameter("@order_code", orderInformationViewModel.codeOrder));
                            oCommand4.Parameters.Add(new SqlParameter("@check_in", checkIn));
                            oCommand4.Parameters.Add(new SqlParameter("@check_out", checkOut));
                            if (orderInformationViewModel.codeOrder.Substring(0, 3) == "RPD")
                            {
                                oCommand4.Parameters.Add(new SqlParameter("@price", (orderInformationViewModel.price * 100) / 30));
                                oCommand4.Parameters.Add(new SqlParameter("@type_order", '2'));
                            }
                            else
                            {
                                oCommand4.Parameters.Add(new SqlParameter("@type_order", '1'));
                                oCommand4.Parameters.Add(new SqlParameter("@price", orderInformationViewModel.price));
                            }
                            oCommand4.Parameters.Add(new SqlParameter("@customer_id", customerID));
                            oCommand4.Parameters.Add(new SqlParameter("@content_payment", orderInformationViewModel.contentPayment + "(" + roomName + ")"));
                            oCommand4.Parameters.Add(new SqlParameter("@status_payment", orderInformationViewModel.resultPayment));
                            oCommand4.Parameters.Add(new SqlParameter("@content_result_payment", orderInformationViewModel.contentResultPayment));
                            oCommand4.Parameters.Add(new SqlParameter("@bank_transaction_number", orderInformationViewModel.bankTransactionNumber));
                            oCommand4.Parameters.Add(new SqlParameter("@vnp_transaction_number", orderInformationViewModel.vnpTransactionNumber));
                            oCommand4.Parameters.Add(new SqlParameter("@type_payment", orderInformationViewModel.typePayment));
                            oCommand4.Parameters.Add(new SqlParameter("@bank_code", orderInformationViewModel.bankCode));
                            oCommand4.Parameters.Add(new SqlParameter("@date_time", orderInformationViewModel.timePayment));
                            oCommand4.Parameters.Add(new SqlParameter("@notification_order_receptionist_id", notificationID));
                            oCommand4.Parameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                            oCommand4.Transaction = oTransaction;
                            decimal orderID = (decimal)oCommand4.ExecuteScalar();

                            for (int i = 0; i < roomIDList.Count; i++)
                            {
                                oCommand5.Parameters.Add(new SqlParameter($"@order_id{i}", orderID));
                                oCommand5.Parameters.Add(new SqlParameter($"@room_id{i}", roomIDList[i].roomID));
                                oCommand5.Parameters.Add(new SqlParameter($"@status_room{i}", IConstants.STATUS_ROOM.OrderRoom));
                            }
                            oCommand5.Transaction = oTransaction;
                            oCommand5.ExecuteNonQuery();
                            oTransaction.Commit();
                            _fluentEmail.To(orderInformationViewModel.email).Subject("Tài Khoảng Westlake Của Bạn").Body("Tên Tài Khoản : " + userName + "\n Mật Khẩu : " + randomPassword).SendAsync();
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
                            oCommand3.Dispose();
                            oCommand4.Dispose();
                            oCommand5.Dispose();
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
        public int createOrderRoom(OrderRoomInformationViewModel orderInformationViewModel)
        {
            try
            {
                string _mutiSqlCreateOrderRoomDetail = "";
                string roomName = "";
                List<RoomInformation> roomIDList = new List<RoomInformation>();
                string checkIn = orderInformationViewModel.checkIn.ToString("yyyy-MM-dd") + " 12:00:00.000";
                string checkOut = orderInformationViewModel.checkOut.ToString("yyyy-MM-dd") + " 12:00:00.000";
                roomIDList = getTheRoomID(orderInformationViewModel.typeRoomID, checkIn, checkOut, orderInformationViewModel.numberOfRoom);
                if (roomIDList.Count > 1)
                {
                    for (int j = 1; j < roomIDList.Count; j++)
                    {
                        _mutiSqlCreateOrderRoomDetail = _sqlCreateOrderRoomDetail + $", (@order_id{j},@room_id{j},@status_room{j})";
                        if (j == 1)
                        {
                            roomName = roomName + roomIDList[j].roomName;
                        }
                        else
                        {
                            roomName = roomName + "," + roomIDList[j].roomName;
                        }
                    }
                }
                else
                {
                    _mutiSqlCreateOrderRoomDetail = _sqlCreateOrderRoomDetail;
                    roomName = roomIDList[0].roomName;
                }
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString(IConstants.SqlString.SqlServerString)))
                {
                    string stringQueryCreateGuestInformation = _sqlCreateGuest;
                    string stringQueryCreateOrderRoom = _sqlCreateOderForGuest;
                    string stringQueryCreateOrderRoomDetail = _mutiSqlCreateOrderRoomDetail;

                    SqlCommand oCommand1 = new SqlCommand(stringQueryCreateGuestInformation, oConnection);
                    SqlCommand oCommand2 = new SqlCommand(stringQueryCreateOrderRoom, oConnection);
                    SqlCommand oCommand3 = new SqlCommand(stringQueryCreateOrderRoomDetail, oConnection);

                    oCommand1.CommandType = CommandType.Text;
                    oCommand2.CommandType = CommandType.Text;
                    oCommand3.CommandType = CommandType.Text;

                    oCommand1.Parameters.Add(new SqlParameter("@full_name", orderInformationViewModel.fullName));
                    oCommand1.Parameters.Add(new SqlParameter("@identity_number", orderInformationViewModel.identifyNumber));
                    oCommand1.Parameters.Add(new SqlParameter("@phone_number", orderInformationViewModel.phoneNumber));
                    oCommand1.Parameters.Add(new SqlParameter("@email", orderInformationViewModel.email));
                    oCommand1.Parameters.Add(new SqlParameter("@day_of_birth", orderInformationViewModel.dateOfBirth.ToString("yyyy-MM-dd")));

                    oConnection.Open();
                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {
                            oCommand1.Transaction = oTransaction;
                            decimal guestID = (decimal)oCommand1.ExecuteScalar();

                            oCommand2.Parameters.Add(new SqlParameter("@order_code", orderInformationViewModel.codeOrder));
                            oCommand2.Parameters.Add(new SqlParameter("@check_in", checkIn));
                            oCommand2.Parameters.Add(new SqlParameter("@check_out", checkOut));
                            if (orderInformationViewModel.codeOrder.Substring(0, 3) == "RPD")
                            {
                                oCommand2.Parameters.Add(new SqlParameter("@price", (orderInformationViewModel.price * 100) / 30));
                                oCommand2.Parameters.Add(new SqlParameter("@type_order", '2'));
                            }
                            else
                            {
                                oCommand2.Parameters.Add(new SqlParameter("@type_order", '1'));
                                oCommand2.Parameters.Add(new SqlParameter("@price", orderInformationViewModel.price));
                            }
                            oCommand2.Parameters.Add(new SqlParameter("@guest_id", guestID));
                            oCommand2.Parameters.Add(new SqlParameter("@content_payment", orderInformationViewModel.contentPayment + "(" + roomName + ")"));
                            oCommand2.Parameters.Add(new SqlParameter("@status_payment", orderInformationViewModel.resultPayment));
                            oCommand2.Parameters.Add(new SqlParameter("@content_result_payment", orderInformationViewModel.contentResultPayment));
                            oCommand2.Parameters.Add(new SqlParameter("@bank_transaction_number", orderInformationViewModel.bankTransactionNumber));
                            oCommand2.Parameters.Add(new SqlParameter("@vnp_transaction_number", orderInformationViewModel.vnpTransactionNumber));
                            oCommand2.Parameters.Add(new SqlParameter("@type_payment", orderInformationViewModel.typePayment));
                            oCommand2.Parameters.Add(new SqlParameter("@bank_code", orderInformationViewModel.bankCode));
                            oCommand2.Parameters.Add(new SqlParameter("@date_time", orderInformationViewModel.timePayment));
                            oCommand2.Parameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                            oCommand2.Transaction = oTransaction;
                            decimal orderID = (decimal)oCommand2.ExecuteScalar();

                            for (int i = 0; i < roomIDList.Count; i++)
                            {
                                oCommand3.Parameters.Add(new SqlParameter($"@order_id{i}", orderID));
                                oCommand3.Parameters.Add(new SqlParameter($"@room_id{i}", roomIDList[i].roomID));
                                oCommand3.Parameters.Add(new SqlParameter($"@status_room{i}", IConstants.STATUS_ROOM.OrderRoom));
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
                            oCommand1.Dispose();
                            oCommand2.Dispose();
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
        public int createOrderRoomUnpayment(OrderRoomUnpayment orderRoomUnpayment)
        {
            try
            {
                string _mutiSqlCreateOrderRoomDetail = "";
                string roomName = "";
                string randomCodeOrder = Common.RandomString();
                List<RoomInformation> roomIDList = new List<RoomInformation>();
                string checkInConvert = orderRoomUnpayment.checkIn.ToString("yyyy-MM-dd") + " 12:00:00.000";
                string checkOutConvert = orderRoomUnpayment.checkOut.ToString("yyyy-MM-dd") + " 12:00:00.000";
                roomIDList = getTheRoomID(orderRoomUnpayment.typeRoomID, checkInConvert, checkOutConvert, orderRoomUnpayment.numberOfRoom);
                if (roomIDList.Count > 1)
                {
                    for (int j = 1; j < roomIDList.Count; j++)
                    {
                        _mutiSqlCreateOrderRoomDetail = _sqlCreateOrderRoomDetail + $", (@order_id{j},@room_id{j},@status_room{j})";
                        if (j == 1)
                        {
                            roomName = roomName + roomIDList[j].roomName;
                        }
                        else
                        {
                            roomName = roomName + "," + roomIDList[j].roomName;
                        }
                    }
                }
                else
                {
                    _mutiSqlCreateOrderRoomDetail = _sqlCreateOrderRoomDetail;
                    roomName = roomIDList[0].roomName;
                }
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString(IConstants.SqlString.SqlServerString)))
                {
                    string stringQueryCreateGuestInformation = _sqlCreateGuest;
                    string stringQueryCreateOrderRoom = _sqlCreateOderUnpaymentForGuest;
                    string stringQueryCreateOrderRoomDetail = _mutiSqlCreateOrderRoomDetail;
                    SqlCommand oCommand1 = new SqlCommand(stringQueryCreateGuestInformation, oConnection);
                    SqlCommand oCommand2 = new SqlCommand(stringQueryCreateOrderRoom, oConnection);
                    SqlCommand oCommand3 = new SqlCommand(stringQueryCreateOrderRoomDetail, oConnection);
                    oCommand1.CommandType = CommandType.Text;
                    oCommand2.CommandType = CommandType.Text;
                    oCommand3.CommandType = CommandType.Text;
                    oCommand1.Parameters.Add(new SqlParameter("@full_name", orderRoomUnpayment.fullName));
                    oCommand1.Parameters.Add(new SqlParameter("@identity_number", orderRoomUnpayment.identifyNumber));
                    oCommand1.Parameters.Add(new SqlParameter("@phone_number", orderRoomUnpayment.phoneNumber));
                    oCommand1.Parameters.Add(new SqlParameter("@email", orderRoomUnpayment.email));
                    oCommand1.Parameters.Add(new SqlParameter("@day_of_birth", orderRoomUnpayment.dateOfBirth.ToString("yyyy-MM-dd")));

                    oConnection.Open();
                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {
                            oCommand1.Transaction = oTransaction;
                            decimal guestID = (decimal)oCommand1.ExecuteScalar();

                            oCommand2.Parameters.Add(new SqlParameter("@order_code", randomCodeOrder));
                            oCommand2.Parameters.Add(new SqlParameter("@check_in", checkInConvert));
                            oCommand2.Parameters.Add(new SqlParameter("@check_out", checkOutConvert));
                            oCommand2.Parameters.Add(new SqlParameter("@price", orderRoomUnpayment.price));
                            oCommand2.Parameters.Add(new SqlParameter("@guest_id", guestID));
                            oCommand2.Parameters.Add(new SqlParameter("@content_payment", "Trả tiền " + orderRoomUnpayment.numberOfRoom + " phòng " + orderRoomUnpayment.typeRoomName + " " + orderRoomUnpayment.numberOfDay + " Đêm(" + roomName + ")"));
                            oCommand2.Parameters.Add(new SqlParameter("@content_result_payment", "Chưa thanh toán"));
                            oCommand2.Parameters.Add(new SqlParameter("@date_time", Common.ConvertUTCDateTime().ToString("yyyyMMddHHmmss")));
                            oCommand2.Parameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                            oCommand2.Transaction = oTransaction;
                            decimal orderID = (decimal)oCommand2.ExecuteScalar();

                            for (int i = 0; i < roomIDList.Count; i++)
                            {
                                oCommand3.Parameters.Add(new SqlParameter($"@order_id{i}", orderID));
                                oCommand3.Parameters.Add(new SqlParameter($"@room_id{i}", roomIDList[i].roomID));
                                oCommand3.Parameters.Add(new SqlParameter($"@status_room{i}", IConstants.STATUS_ROOM.OrderRoom));
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
                            oCommand1.Dispose();
                            oCommand2.Dispose();
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
        public int createOrderRoomUnpaymentForCustomer(OrderRoomUnpayment orderRoomUnpayment)
        {
            try
            {
                string _mutiSqlCreateOrderRoomDetail = "";
                string roomName = "";
                string randomCodeOrder = Common.RandomString();
                List<RoomInformation> roomIDList = new List<RoomInformation>();
                string checkInConvert = orderRoomUnpayment.checkIn.ToString("yyyy-MM-dd") + " 12:00:00.000";
                string checkOutConvert = orderRoomUnpayment.checkOut.ToString("yyyy-MM-dd") + " 12:00:00.000";
                roomIDList = getTheRoomID(orderRoomUnpayment.typeRoomID, checkInConvert, checkOutConvert, orderRoomUnpayment.numberOfRoom);
                if (roomIDList.Count > 1)
                {
                    for (int j = 1; j < roomIDList.Count; j++)
                    {
                        _mutiSqlCreateOrderRoomDetail = _sqlCreateOrderRoomDetail + $", (@order_id{j},@room_id{j},@status_room{j})";
                        if (j == 1)
                        {
                            roomName = roomName + roomIDList[j].roomName;
                        }
                        else
                        {
                            roomName = roomName + "," + roomIDList[j].roomName;
                        }
                    }
                }
                else
                {
                    _mutiSqlCreateOrderRoomDetail = _sqlCreateOrderRoomDetail;
                    roomName = roomIDList[0].roomName;
                }
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString(IConstants.SqlString.SqlServerString)))
                {

                    string stringQueryCreateOrderRoom = _sqlCreateOderUnpaymentForCustomer;
                    string stringQueryCreateOrderRoomDetail = _mutiSqlCreateOrderRoomDetail;

                    SqlCommand oCommand1 = new SqlCommand(stringQueryCreateOrderRoom, oConnection);
                    SqlCommand oCommand2 = new SqlCommand(stringQueryCreateOrderRoomDetail, oConnection);

                    oCommand1.CommandType = CommandType.Text;
                    oCommand2.CommandType = CommandType.Text;
                    oConnection.Open();
                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {

                            oCommand1.Parameters.Add(new SqlParameter("@order_code", randomCodeOrder));
                            oCommand1.Parameters.Add(new SqlParameter("@check_in", checkInConvert));
                            oCommand1.Parameters.Add(new SqlParameter("@check_out", checkOutConvert));
                            oCommand1.Parameters.Add(new SqlParameter("@price", orderRoomUnpayment.price));
                            oCommand1.Parameters.Add(new SqlParameter("@customer_id", orderRoomUnpayment.customerID));
                            oCommand1.Parameters.Add(new SqlParameter("@content_payment", "Trả tiền mặt " + orderRoomUnpayment.numberOfRoom + " phòng " + orderRoomUnpayment.typeRoomName + " " + orderRoomUnpayment.numberOfDay + " Đêm(" + roomName + ")"));
                            oCommand1.Parameters.Add(new SqlParameter("@content_result_payment", "Chưa thanh toán"));
                            oCommand1.Parameters.Add(new SqlParameter("@date_time", Common.ConvertUTCDateTime().ToString("yyyyMMddHHmmss")));
                            oCommand1.Parameters.Add(new SqlParameter("@bank_transaction_number", "VNP00000000"));
                            oCommand1.Parameters.Add(new SqlParameter("@status_payment", "00"));
                            oCommand1.Parameters.Add(new SqlParameter("@type_order", '3'));
                            oCommand1.Parameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                            oCommand1.Transaction = oTransaction;
                            decimal orderID = (decimal)oCommand1.ExecuteScalar();

                            for (int i = 0; i < roomIDList.Count; i++)
                            {
                                oCommand2.Parameters.Add(new SqlParameter($"@order_id{i}", orderID));
                                oCommand2.Parameters.Add(new SqlParameter($"@room_id{i}", roomIDList[i].roomID));
                                oCommand2.Parameters.Add(new SqlParameter($"@status_room{i}", IConstants.STATUS_ROOM.OrderRoom));
                            }
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
        public int createOrderRoomForCustomer(OrderRoomInformationViewModel orderInformationViewModel)
        {
            try
            {
                string _mutiSqlCreateOrderRoomDetail = "";
                string roomName = "";
                List<RoomInformation> roomIDList = new List<RoomInformation>();
                string checkIn = orderInformationViewModel.checkIn.ToString("yyyy-MM-dd") + " 12:00:00.000";
                string checkOut = orderInformationViewModel.checkOut.ToString("yyyy-MM-dd") + " 12:00:00.000";
                roomIDList = getTheRoomID(orderInformationViewModel.typeRoomID, checkIn, checkOut, orderInformationViewModel.numberOfRoom);
                if (roomIDList.Count > 1)
                {
                    for (int j = 1; j < roomIDList.Count; j++)
                    {
                        _mutiSqlCreateOrderRoomDetail = _sqlCreateOrderRoomDetail + $", (@order_id{j},@room_id{j},@status_room{j})";
                        if (j == 1)
                        {
                            roomName = roomName + roomIDList[j].roomName;
                        }
                        else
                        {
                            roomName = roomName + "," + roomIDList[j].roomName;
                        }
                    }
                }
                else
                {
                    _mutiSqlCreateOrderRoomDetail = _sqlCreateOrderRoomDetail;
                    roomName = roomIDList[0].roomName;
                }
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString(IConstants.SqlString.SqlServerString)))
                {
                    string stringQueryNotificationReceptionist = _sqlInsertNotificationReceptionist;
                    string stringQueryCreateOrderRoom = _sqlCreateOderForCustomer;
                    string stringQueryCreateOrderRoomDetail = _mutiSqlCreateOrderRoomDetail;

                    SqlCommand oCommand1 = new SqlCommand(stringQueryNotificationReceptionist, oConnection);
                    SqlCommand oCommand2 = new SqlCommand(stringQueryCreateOrderRoom, oConnection);
                    SqlCommand oCommand3 = new SqlCommand(_mutiSqlCreateOrderRoomDetail, oConnection);

                    oCommand1.CommandType = CommandType.Text;
                    oCommand2.CommandType = CommandType.Text;
                    oCommand3.CommandType = CommandType.Text;

                    oConnection.Open();
                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {
                            oCommand1.Parameters.Add(new SqlParameter("@content_notification", "Có đơn đặt phòng " + orderInformationViewModel.codeOrder + " mới từ khách hàng"));
                            oCommand1.Parameters.Add(new SqlParameter("@date_time", Common.ConvertUTCDateTime().ToString("yyyy-MM-dd HH:mm") + ":00.000"));
                            oCommand1.Parameters.Add(new SqlParameter("@status_notification", true));
                            oCommand1.Parameters.Add(new SqlParameter("@type_notification", '1'));
                            oCommand1.Transaction = oTransaction;
                            decimal notificationID = (decimal)oCommand1.ExecuteScalar();


                            oCommand2.Parameters.Add(new SqlParameter("@order_code", orderInformationViewModel.codeOrder));
                            oCommand2.Parameters.Add(new SqlParameter("@check_in", checkIn));
                            oCommand2.Parameters.Add(new SqlParameter("@check_out", checkOut));
                            if (orderInformationViewModel.codeOrder.Substring(0, 3) == "RPD")
                            {
                                oCommand2.Parameters.Add(new SqlParameter("@price", (orderInformationViewModel.price * 100) / 30));
                                oCommand2.Parameters.Add(new SqlParameter("@type_order", '2'));
                            }
                            else
                            {
                                oCommand2.Parameters.Add(new SqlParameter("@type_order", '1'));
                                oCommand2.Parameters.Add(new SqlParameter("@price", orderInformationViewModel.price));
                            }
                            oCommand2.Parameters.Add(new SqlParameter("@customer_id", orderInformationViewModel.customerID));
                            oCommand2.Parameters.Add(new SqlParameter("@content_payment", orderInformationViewModel.contentPayment + "(" + roomName + ")"));
                            oCommand2.Parameters.Add(new SqlParameter("@status_payment", orderInformationViewModel.resultPayment));
                            oCommand2.Parameters.Add(new SqlParameter("@content_result_payment", orderInformationViewModel.contentResultPayment));
                            oCommand2.Parameters.Add(new SqlParameter("@bank_transaction_number", orderInformationViewModel.bankTransactionNumber));
                            oCommand2.Parameters.Add(new SqlParameter("@vnp_transaction_number", orderInformationViewModel.vnpTransactionNumber));
                            oCommand2.Parameters.Add(new SqlParameter("@type_payment", orderInformationViewModel.typePayment));
                            oCommand2.Parameters.Add(new SqlParameter("@bank_code", orderInformationViewModel.bankCode));
                            oCommand2.Parameters.Add(new SqlParameter("@date_time", orderInformationViewModel.timePayment));
                            oCommand2.Parameters.Add(new SqlParameter("@notification_order_receptionist_id", notificationID));
                            oCommand2.Parameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                            oCommand2.Transaction = oTransaction;
                            decimal orderID = (decimal)oCommand2.ExecuteScalar();

                            for (int i = 0; i < roomIDList.Count; i++)
                            {
                                oCommand3.Parameters.Add(new SqlParameter($"@order_id{i}", orderID));
                                oCommand3.Parameters.Add(new SqlParameter($"@room_id{i}", roomIDList[i].roomID));
                                oCommand3.Parameters.Add(new SqlParameter($"@status_room{i}", IConstants.STATUS_ROOM.OrderRoom));
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
