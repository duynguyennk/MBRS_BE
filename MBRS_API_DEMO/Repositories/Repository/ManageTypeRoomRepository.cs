using MBRS_API.Models;
using MBRS_API.Repositories.IRepository;
using MBRS_API_DEMO.DataBase;
using MBRS_API_DEMO.Models.StoreProcedure;
using MBRS_API_DEMO.Utils;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MBRS_API.Repositories.Repository
{
    public class ManageTypeRoomRepository : IManageTypeRoomRepository
    {
        private readonly string _sqlGetInformationTypeRoom = @"SELECT type_room_id,type_room_name,number_of_child,number_of_bed,number_of_adult,number_of_bedroom,number_of_view,number_of_adding_bed,number_of_bath_room,price,type_room_code,content_Introduce_Room,room_square,safety_box,dryer,wifi,iron,tivi,fridge,heater_bath,bathtub,air_condition,heater_room,dinner_table,type_room.list_utilities_id,first_image_base64,second_image_base64,third_image_base64,fourth_image_base64,fifth_image_base64
                                                    FROM type_room
                                                    INNER JOIN list_utilities ON list_utilities.list_utilities_id = type_room.list_utilities_id
                                                    WHERE type_room.delete_flag = @delete_flag and type_room_id = @type_room_id";

        private readonly string _sqlGetAllTypeRoom = @"SELECT type_room_id,type_room_code,type_room_name,number_of_child,number_of_bed,number_of_adult,number_of_bedroom,number_of_view,number_of_adding_bed,number_of_bath_room,price,content_Introduce_Room,room_square,list_utilities_id
                                                    FROM type_room
                                                    WHERE delete_flag = @delete_flag";

        private readonly string _sqlUpdateImage = @"UPDATE type_room SET ";

        private readonly string _sqlCheckDuplicateTypeRoomCode = @"SELECT COUNT(type_room_id) FROM type_room WHERE type_room_code = @type_room_code AND delete_flag = 0 AND type_room_id != @type_room_id";

        private readonly string _sqlCheckDuplicateTypeRoomName = @"SELECT COUNT(type_room_id) FROM type_room WHERE type_room_name = @type_room_name AND delete_flag = 0 AND type_room_id != @type_room_id";

        private readonly string _sqlCreateTypeRoom = @"INSERT INTO type_room (type_room_name,type_room_code,number_of_child,number_of_bed,number_of_adult,number_of_bedroom,number_of_view,number_of_adding_bed,number_of_bath_room,price,content_Introduce_Room,room_square,list_utilities_id,delete_flag) VALUES (@type_room_name,@type_room_code,@number_of_child,@number_of_bed,@number_of_adult,@number_of_bedroom,@number_of_view,@number_of_adding_bed,@number_of_bath_room,@price,@content_Introduce_Room,@room_square,@list_utilities_id,@delete_flag); SELECT SCOPE_IDENTITY()";

        private readonly string _sqlCreateUtilities = @"INSERT INTO list_utilities (safety_box,dryer,wifi,iron,tivi,fridge,heater_bath,bathtub,air_condition,heater_room,dinner_table,delete_flag) VALUES (@safety_box,@dryer,@wifi,@iron,@tivi,@fridge,@heater_bath,@bathtub,@air_condition,@heater_room,@dinner_table,@delete_flag); SELECT SCOPE_IDENTITY()";

        private readonly string _sqlUpdateTypeRoom = @"UPDATE type_room SET type_room_name = @type_room_name,type_room_code = @type_room_code,number_of_child = @number_of_child,number_of_bed = @number_of_bed,number_of_adult = @number_of_adult,number_of_bedroom = @number_of_bedroom,number_of_view = @number_of_view,number_of_adding_bed =@number_of_adding_bed,number_of_bath_room = @number_of_bath_room,price = @price,content_Introduce_Room = @content_Introduce_Room,room_square = @room_square WHERE type_room_id = @type_room_id";

        private readonly string _sqlUpdateUtilities = @"UPDATE list_utilities SET safety_box = @safety_box,dryer = @dryer,wifi = @wifi,iron = @iron,tivi = @tivi,fridge = @fridge,heater_bath = @heater_bath,bathtub = @bathtub,air_condition = @air_condition,heater_room = @heater_room,dinner_table = @dinner_table WHERE list_utilities_id = @list_utilities_id";

        private readonly string _sqlDeleteTypeRoom = @"UPDATE type_room SET delete_flag = @delete_flag WHERE type_room_id = @type_room_id";

        private readonly string _sqlDeteleUtilities = @"UPDATE list_utilities SET delete_flag = @delete_flag WHERE list_utilities_id = @list_utilities_id";

        private readonly string _sqlUpdateImageTypeRoom = @"UPDATE type_room SET first_image_base64=@first_image_base64,second_image_base64=@second_image_base64,third_image_base64=@third_image_base64,fourth_image_base64=@fourth_image_base64,fifth_image_base64=@fifth_image_base64 WHERE type_room_id=@type_room_id";

        private readonly SqlServerDBContext _sqlServerDbContext;
        private IConfiguration _configuration { get; }

        private readonly DBWorker dBWorker;
        public ManageTypeRoomRepository(SqlServerDBContext sqlServer, IConfiguration configuration)
        {
            _sqlServerDbContext = sqlServer;
            _configuration = configuration;
            dBWorker = new DBWorker(configuration.GetConnectionString(IConstants.SqlString.SqlServerString));
        }
        public int checkDuplicateTypeRoomCode(string typeRoomCode, int typeRoomID)
        {
            int checkDuplicate = 0;
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlCheckDuplicateTypeRoomCode;
                sqlParameters.Add(new SqlParameter("@type_room_code", typeRoomCode));
                sqlParameters.Add(new SqlParameter("@type_room_id", typeRoomID));
                checkDuplicate = Convert.ToInt32(dBWorker.ExecuteScalar(commandText, sqlParameters.ToArray()));
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return checkDuplicate;
        }
        public int checkDuplicateTypeRoomName(string typeRoomName, int typeRoomID)
        {
            int checkDuplicate = 0;
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlCheckDuplicateTypeRoomName;
                sqlParameters.Add(new SqlParameter("@type_room_name", typeRoomName));
                sqlParameters.Add(new SqlParameter("@type_room_id", typeRoomID));
                checkDuplicate = Convert.ToInt32(dBWorker.ExecuteScalar(commandText, sqlParameters.ToArray()));
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return checkDuplicate;
        }
        public int createTypeRoom(TypeRoom typeRoom)
        {
            try
            {
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString(IConstants.SqlString.SqlServerString)))
                {
                    string stringQueryDeleteEmployeeByID = _sqlCreateUtilities;
                    string stringQueryDeleteAccountByID = _sqlCreateTypeRoom;
                    SqlCommand oCommand1 = new SqlCommand(stringQueryDeleteEmployeeByID, oConnection);
                    SqlCommand oCommand2 = new SqlCommand(stringQueryDeleteAccountByID, oConnection);
                    oCommand1.CommandType = CommandType.Text;
                    oCommand2.CommandType = CommandType.Text;
                    oCommand1.Parameters.Add(new SqlParameter("@safety_box", typeRoom.safetyBox == null ? false : typeRoom.safetyBox));
                    oCommand1.Parameters.Add(new SqlParameter("@dryer", typeRoom.dryer == null ? false : typeRoom.dryer));
                    oCommand1.Parameters.Add(new SqlParameter("@wifi", typeRoom.wifi == null ? false : typeRoom.wifi));
                    oCommand1.Parameters.Add(new SqlParameter("@iron", typeRoom.iron == null ? false : typeRoom.iron));
                    oCommand1.Parameters.Add(new SqlParameter("@tivi", typeRoom.tivi == null ? false : typeRoom.tivi));
                    oCommand1.Parameters.Add(new SqlParameter("@fridge", typeRoom.fridge == null ? false : typeRoom.fridge));
                    oCommand1.Parameters.Add(new SqlParameter("@heater_bath", typeRoom.heaterBath == null ? false : typeRoom.heaterBath));
                    oCommand1.Parameters.Add(new SqlParameter("@bathtub", typeRoom.bathTub == null ? false : typeRoom.bathTub));
                    oCommand1.Parameters.Add(new SqlParameter("@air_condition", typeRoom.airCondition == null ? false : typeRoom.airCondition));
                    oCommand1.Parameters.Add(new SqlParameter("@heater_room", typeRoom.heaterRoom == null ? false : typeRoom.heaterRoom));
                    oCommand1.Parameters.Add(new SqlParameter("@dinner_table", typeRoom.dinnerTable == null ? false : typeRoom.dinnerTable));
                    oCommand1.Parameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));

                    oConnection.Open();
                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {
                            oCommand1.Transaction = oTransaction;
                            decimal listUtilitiesID = (decimal)oCommand1.ExecuteScalar();
                            oCommand2.Parameters.Add(new SqlParameter("@type_room_code", typeRoom.typeRoomCode));
                            oCommand2.Parameters.Add(new SqlParameter("@type_room_name", typeRoom.typeRoomName));
                            oCommand2.Parameters.Add(new SqlParameter("@number_of_child", typeRoom.numberOfChild));
                            oCommand2.Parameters.Add(new SqlParameter("@number_of_bed", typeRoom.numberOfBed));
                            oCommand2.Parameters.Add(new SqlParameter("@number_of_adult", typeRoom.numberOfAdult));
                            oCommand2.Parameters.Add(new SqlParameter("@number_of_bedroom", typeRoom.numberOfBedroom));
                            oCommand2.Parameters.Add(new SqlParameter("@number_of_adding_bed", typeRoom.numberOfAddingBed));
                            oCommand2.Parameters.Add(new SqlParameter("@number_of_bath_room", typeRoom.numberOfBathRoom));
                            oCommand2.Parameters.Add(new SqlParameter("@number_of_view", typeRoom.numberOfView));
                            oCommand2.Parameters.Add(new SqlParameter("@price", typeRoom.price));
                            oCommand2.Parameters.Add(new SqlParameter("@content_Introduce_Room", typeRoom.contentIntroduceRoom));
                            oCommand2.Parameters.Add(new SqlParameter("@room_square", typeRoom.roomSquare));
                            oCommand2.Parameters.Add(new SqlParameter("@list_utilities_id", listUtilitiesID));
                            oCommand2.Parameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));

                            oCommand2.Transaction = oTransaction;
                            int typeRoomID = Convert.ToInt32(oCommand2.ExecuteScalar());
                            oTransaction.Commit();
                            return typeRoomID;
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

        public int deleteTypeRoom(int typeRoomID, int listUtilitiesID)
        {
            try
            {
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString(IConstants.SqlString.SqlServerString)))
                {
                    string stringQueryDeleteEmployeeByID = _sqlDeleteTypeRoom;
                    string stringQueryDeleteAccountByID = _sqlDeteleUtilities;
                    SqlCommand oCommand1 = new SqlCommand(stringQueryDeleteEmployeeByID, oConnection);
                    SqlCommand oCommand2 = new SqlCommand(stringQueryDeleteAccountByID, oConnection);
                    oCommand1.CommandType = CommandType.Text;
                    oCommand2.CommandType = CommandType.Text;
                    oCommand1.Parameters.Add(new SqlParameter("@type_room_id", typeRoomID));
                    oCommand1.Parameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.Deleted));
                    oCommand2.Parameters.Add(new SqlParameter("@list_utilities_id", listUtilitiesID));
                    oCommand2.Parameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.Deleted));

                    oConnection.Open();
                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {
                            oCommand1.Transaction = oTransaction;
                            oCommand1.ExecuteNonQuery();
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

        public List<TypeRoom> getAllTypeRoom()
        {
            List<TypeRoom> typeRoomList = new List<TypeRoom>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllTypeRoom;
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                typeRoomList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<TypeRoom>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return typeRoomList;
        }

        public int updateAImageTypeRoom(ImageTypeRoom imageTypeRoom)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = string.Empty;
                if (imageTypeRoom.position == 1)
                {
                    commandText = _sqlUpdateImage + "first_image_base64=@first_image_base64";
                    sqlParameters.Add(new SqlParameter("@first_image_base64", imageTypeRoom.Base64));
                }
                else if (imageTypeRoom.position == 2)
                {
                    commandText = _sqlUpdateImage + "second_image_base64=@second_image_base64";
                    sqlParameters.Add(new SqlParameter("@second_image_base64", imageTypeRoom.Base64));
                }
                else if (imageTypeRoom.position == 3)
                {
                    commandText = _sqlUpdateImage + "third_image_base64=@third_image_base64";
                    sqlParameters.Add(new SqlParameter("@third_image_base64", imageTypeRoom.Base64));
                }
                else if (imageTypeRoom.position == 4)
                {
                    commandText = _sqlUpdateImage + "fourth_image_base64=@fourth_image_base64";
                    sqlParameters.Add(new SqlParameter("@fourth_image_base64", imageTypeRoom.Base64));
                }
                else
                {
                    commandText = _sqlUpdateImage + "fifth_image_base64=@fifth_image_base64";
                    sqlParameters.Add(new SqlParameter("@fifth_image_base64", imageTypeRoom.Base64));
                }
                commandText = commandText + " WHERE type_room_id=@type_room_id";
                sqlParameters.Add(new SqlParameter("@type_room_id", imageTypeRoom.typeRoomID));
                dBWorker.ExecuteNonQuery(commandText, sqlParameters.ToArray());
                return ErrorCodeResponse.SUCCESS_CODE;
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return ErrorCodeResponse.FAIL_CODE;
            }
        }

        public int deleteImageTypeRoom(int position, int typeRoomID)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = string.Empty;
                if (position == 1)
                {
                    commandText = _sqlUpdateImage + "first_image_base64=@first_image_base64";
                    sqlParameters.Add(new SqlParameter("@first_image_base64", DBNull.Value));
                }
                else if (position == 2)
                {
                    commandText = _sqlUpdateImage + "second_image_base64=@second_image_base64";
                    sqlParameters.Add(new SqlParameter("@second_image_base64", DBNull.Value));
                }
                else if (position == 3)
                {
                    commandText = _sqlUpdateImage + "third_image_base64=@third_image_base64";
                    sqlParameters.Add(new SqlParameter("@third_image_base64", DBNull.Value));
                }
                else if (position == 4)
                {
                    commandText = _sqlUpdateImage + "fourth_image_base64=@fourth_image_base64";
                    sqlParameters.Add(new SqlParameter("@fourth_image_base64", DBNull.Value));
                }
                else
                {
                    commandText = _sqlUpdateImage + "fifth_image_base64=@fifth_image_base64";
                    sqlParameters.Add(new SqlParameter("@fifth_image_base64", DBNull.Value));
                }
                commandText = commandText + " WHERE type_room_id=@type_room_id";
                sqlParameters.Add(new SqlParameter("@type_room_id", typeRoomID));
                dBWorker.ExecuteNonQuery(commandText, sqlParameters.ToArray());
                return ErrorCodeResponse.SUCCESS_CODE;
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return ErrorCodeResponse.FAIL_CODE;
            }
        }

        public List<TypeRoom> getAllTypeRoomWithFilter(string filterName, string filterValue)
        {
            List<TypeRoom> floorList = new List<TypeRoom>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllTypeRoom;
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                if (filterName != "")
                {
                    commandText = _sqlGetAllTypeRoom + " AND " + filterName + " LIKE @filterValue ORDER BY type_room_id DESC";
                    sqlParameters.Add(new SqlParameter("@filterValue", "%" + filterValue.Trim() + "%"));
                }
                floorList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<TypeRoom>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return floorList;
        }
        public int updateImageTypeRoom(List<ItemImage> itemImage)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlUpdateImageTypeRoom;
                sqlParameters.Add(new SqlParameter("@first_image_base64", itemImage[0].Base64));
                sqlParameters.Add(new SqlParameter("@second_image_base64", itemImage[1].Base64));
                sqlParameters.Add(new SqlParameter("@third_image_base64", itemImage[2].Base64));
                sqlParameters.Add(new SqlParameter("@fourth_image_base64", itemImage[3].Base64));
                sqlParameters.Add(new SqlParameter("@fifth_image_base64", itemImage[4].Base64));
                sqlParameters.Add(new SqlParameter("@type_room_id", itemImage[0].idObject));
                dBWorker.ExecuteNonQuery(commandText, sqlParameters.ToArray());
                return ErrorCodeResponse.SUCCESS_CODE;
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return ErrorCodeResponse.FAIL_CODE;
            }
        }

        public List<TypeRoom> getTypeRoomInformation(int typeRoomID)
        {
            List<TypeRoom> floorList = new List<TypeRoom>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetInformationTypeRoom + " ORDER BY type_room_id DESC";
                sqlParameters.Add(new SqlParameter("@type_room_id", typeRoomID));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                floorList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<TypeRoom>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return floorList;
        }

        public int updateTheTypeRoom(TypeRoom typeRoom)
        {
            try
            {
                using (SqlConnection oConnection = new SqlConnection(_configuration.GetConnectionString(IConstants.SqlString.SqlServerString)))
                {
                    string stringQueryDeleteEmployeeByID = _sqlUpdateUtilities;
                    string stringQueryDeleteAccountByID = _sqlUpdateTypeRoom;
                    SqlCommand oCommand1 = new SqlCommand(stringQueryDeleteEmployeeByID, oConnection);
                    SqlCommand oCommand2 = new SqlCommand(stringQueryDeleteAccountByID, oConnection);
                    oCommand1.CommandType = CommandType.Text;
                    oCommand2.CommandType = CommandType.Text;

                    oCommand1.Parameters.Add(new SqlParameter("@safety_box", typeRoom.safetyBox));
                    oCommand1.Parameters.Add(new SqlParameter("@dryer", typeRoom.dryer));
                    oCommand1.Parameters.Add(new SqlParameter("@wifi", typeRoom.wifi));
                    oCommand1.Parameters.Add(new SqlParameter("@iron", typeRoom.iron));
                    oCommand1.Parameters.Add(new SqlParameter("@tivi", typeRoom.tivi));
                    oCommand1.Parameters.Add(new SqlParameter("@fridge", typeRoom.fridge));
                    oCommand1.Parameters.Add(new SqlParameter("@heater_bath", typeRoom.heaterBath));
                    oCommand1.Parameters.Add(new SqlParameter("@bathtub", typeRoom.bathTub));
                    oCommand1.Parameters.Add(new SqlParameter("@air_condition", typeRoom.airCondition));
                    oCommand1.Parameters.Add(new SqlParameter("@heater_room", typeRoom.heaterRoom));
                    oCommand1.Parameters.Add(new SqlParameter("@dinner_table", typeRoom.dinnerTable));
                    oCommand1.Parameters.Add(new SqlParameter("@list_utilities_id", typeRoom.listUtilitiesID));

                    oCommand2.Parameters.Add(new SqlParameter("@type_room_code", typeRoom.typeRoomCode));
                    oCommand2.Parameters.Add(new SqlParameter("@type_room_name", typeRoom.typeRoomName));
                    oCommand2.Parameters.Add(new SqlParameter("@number_of_child", typeRoom.numberOfChild));
                    oCommand2.Parameters.Add(new SqlParameter("@number_of_bed", typeRoom.numberOfBed));
                    oCommand2.Parameters.Add(new SqlParameter("@number_of_adult", typeRoom.numberOfAdult));
                    oCommand2.Parameters.Add(new SqlParameter("@number_of_bedroom", typeRoom.numberOfBedroom));
                    oCommand2.Parameters.Add(new SqlParameter("@number_of_adding_bed", typeRoom.numberOfAddingBed));
                    oCommand2.Parameters.Add(new SqlParameter("@number_of_bath_room", typeRoom.numberOfBathRoom));
                    oCommand2.Parameters.Add(new SqlParameter("@number_of_view", typeRoom.numberOfView));
                    oCommand2.Parameters.Add(new SqlParameter("@price", typeRoom.price));
                    oCommand2.Parameters.Add(new SqlParameter("@content_Introduce_Room", typeRoom.contentIntroduceRoom));
                    oCommand2.Parameters.Add(new SqlParameter("@room_square", typeRoom.roomSquare));
                    oCommand2.Parameters.Add(new SqlParameter("@type_room_id", typeRoom.typeRoomID));

                    oConnection.Open();
                    using (SqlTransaction oTransaction = oConnection.BeginTransaction())
                    {
                        try
                        {
                            oCommand1.Transaction = oTransaction;
                            oCommand1.ExecuteNonQuery();
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
