using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API_DEMO.DataBase;
using MBRS_API_DEMO.Models.StoreProcedure;
using MBRS_API_DEMO.Utils;
using Microsoft.Data.SqlClient;

namespace MBRS_API.Repositories.Repository
{
    public class ManageRoomRepository : IManageRoomRepository
    {
        private readonly string _sqlGetAllRoomInfor = @"SELECT room_id,room_code,room_name,type_room_name,price,room_square 
                                                        FROM room_information
                                                        INNER JOIN type_room ON room_information.type_room_id = type_room.type_room_id
                                                        WHERE room_information.delete_flag = @delete_flag1 AND type_room.delete_flag = @delete_flag2";

        private readonly string _sqlSelectAllTypeRoom = @"SELECT * FROM type_room WHERE delete_flag = @delete_flag";

        private readonly string _sqlSelectAllFloor = @"SELECT * FROM floor WHERE delete_flag = @delete_flag";

        private readonly string _sqlCheckDuplicateRoomCode = @"SELECT COUNT(room_id) FROM room_information WHERE room_code = @room_code AND delete_flag = 0 AND room_id != @room_id";

        private readonly string _sqlCheckDuplicateRoomName = @"SELECT COUNT(room_id) FROM room_information WHERE room_name = @room_name AND delete_flag = 0 AND room_id != @room_id";

        private readonly string _sqlSelectDetailInformationRoom = @"SELECT price,room_square,number_of_child,number_of_bed,number_of_adult,number_of_bedroom,number_of_view,content_Introduce_Room,number_of_adding_bed,number_of_bath_room,safety_box,dryer,wifi,iron,tivi,fridge,heater_bath,bathtub,air_condition,heater_room,dinner_table,first_image_base64,second_image_base64,third_image_base64
                                                                       FROM type_room
                                                                       INNER JOIN list_utilities ON type_room.list_utilities_id = list_utilities.list_utilities_id
                                                                       WHERE type_room_id = @typeRoomID AND type_room.delete_flag = @deleteFlag";

        private readonly string _sqlGetTheRoomInformation = @"SELECT room_id,room_code,room_name,type_room_id,floor_id
                                                             FROM room_information
                                                             WHERE room_information.delete_flag = @delete_flag AND room_id = @room_id";

        private readonly string _sqlDeleteRoom = @"UPDATE room_information SET delete_flag = @delete_flag WHERE room_id = @room_id";

        private readonly string _sqlCreateRoom = @"INSERT INTO room_information (room_code,room_name,type_room_id,floor_id,delete_flag) VALUES (@roomCode,@roomName,@typeRoomID,@floorID,@deleteFlag)";

        private readonly string _sqlUpdateRoom = @"UPDATE room_information SET room_code=@room_code,room_name=@room_name,type_room_id=@type_room_id,floor_id=@floor_id WHERE room_id=@room_id";


        private readonly SqlServerDBContext _sqlServerDbContext;
        private IConfiguration _configuration { get; }

        private readonly DBWorker dBWorker;
        public ManageRoomRepository(SqlServerDBContext sqlServer, IConfiguration configuration)
        {
            _sqlServerDbContext = sqlServer;
            _configuration = configuration;
            dBWorker = new DBWorker(configuration.GetConnectionString(IConstants.SqlString.SqlServerString));
        }

        public int deleteRoom(int roomID)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlDeleteRoom;
                sqlParameters.Add(new SqlParameter("@room_id", roomID));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.Deleted));
                dBWorker.GetDataTable(commandText, sqlParameters.ToArray());
                return ErrorCodeResponse.SUCCESS_CODE;
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return ErrorCodeResponse.FAIL_CODE;
            }
        }

        public int checkDuplicateRoomCode(string roomCode,int roomID)
        {
            int checkDuplicate = 0;
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlCheckDuplicateRoomCode;
                sqlParameters.Add(new SqlParameter("@room_code", roomCode));
                sqlParameters.Add(new SqlParameter("@room_id", roomID));
                checkDuplicate = Convert.ToInt32(dBWorker.ExecuteScalar(commandText, sqlParameters.ToArray()));
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return checkDuplicate;
        }

        public int checkDuplicateRoomName(string roomName, int roomID)
        {
            int checkDuplicate = 0;
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlCheckDuplicateRoomName;
                sqlParameters.Add(new SqlParameter("@room_name", roomName));
                sqlParameters.Add(new SqlParameter("@room_id", roomID));
                checkDuplicate = Convert.ToInt32(dBWorker.ExecuteScalar(commandText, sqlParameters.ToArray()));
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return checkDuplicate;
        }
        public int updateTheRoom(RoomInformation roomInformation)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlUpdateRoom;
                sqlParameters.Add(new SqlParameter("@room_id", roomInformation.roomID));
                sqlParameters.Add(new SqlParameter("@room_code", roomInformation.roomCode));
                sqlParameters.Add(new SqlParameter("@room_name", roomInformation.roomName));
                sqlParameters.Add(new SqlParameter("@type_room_id", roomInformation.typeRoomID));
                sqlParameters.Add(new SqlParameter("@floor_id", roomInformation.floorID));
                dBWorker.GetDataTable(commandText, sqlParameters.ToArray());
                return ErrorCodeResponse.SUCCESS_CODE;
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return ErrorCodeResponse.FAIL_CODE;
            }
        }
        public List<RoomViewModel> getAllRoomWithFilter(string filterName, string filterValue)
        {
            List<RoomViewModel> roomList = new List<RoomViewModel>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllRoomInfor;
                sqlParameters.Add(new SqlParameter("@delete_flag1", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                sqlParameters.Add(new SqlParameter("@delete_flag2", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                if (filterName != "")
                {
                    commandText = _sqlGetAllRoomInfor + " AND " + filterName + " LIKE @filterValue ORDER BY room_id DESC";
                    sqlParameters.Add(new SqlParameter("@filterValue", "%" + filterValue.Trim() + "%"));
                }
                roomList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<RoomViewModel>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return roomList;
        }

        public List<RoomViewModel> getAllRoom()
        {
            List<RoomViewModel> roomList = new List<RoomViewModel>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllRoomInfor + " ORDER BY room_id DESC";
                sqlParameters.Add(new SqlParameter("@delete_flag1", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                sqlParameters.Add(new SqlParameter("@delete_flag2", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                roomList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<RoomViewModel>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return roomList;
        }
        public List<RoomViewModel> getTheRoomInformation(int roomID)
        {
            List<RoomViewModel> roomList = new List<RoomViewModel>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetTheRoomInformation;
                sqlParameters.Add(new SqlParameter("@room_id", roomID));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                roomList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<RoomViewModel>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return roomList;
        }
        public List<TypeRoom> getDetailInformationRoom(int typeRoomID)
        {
            List<TypeRoom> typeRoomList = new List<TypeRoom>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlSelectDetailInformationRoom;
                sqlParameters.Add(new SqlParameter("@typeRoomID", typeRoomID));
                sqlParameters.Add(new SqlParameter("@deleteFlag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                typeRoomList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<TypeRoom>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return typeRoomList;
        }

        public int createRoom(RoomInformation roomInformation)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlCreateRoom;
                sqlParameters.Add(new SqlParameter("@roomCode", roomInformation.roomCode));
                sqlParameters.Add(new SqlParameter("@roomName", roomInformation.roomName));
                sqlParameters.Add(new SqlParameter("@typeRoomID", roomInformation.typeRoomID));
                sqlParameters.Add(new SqlParameter("@floorID", roomInformation.floorID));
                sqlParameters.Add(new SqlParameter("@deleteFlag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                dBWorker.GetDataTable(commandText, sqlParameters.ToArray());
                return ErrorCodeResponse.SUCCESS_CODE;
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
                string commandText = _sqlSelectAllTypeRoom;
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                typeRoomList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<TypeRoom>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return typeRoomList;
        }

        public List<Floor> getAllFloor()
        {
            List<Floor> floorList = new List<Floor>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlSelectAllFloor;
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



