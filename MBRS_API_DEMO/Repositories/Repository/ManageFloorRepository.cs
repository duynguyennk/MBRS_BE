using MBRS_API.Models;
using MBRS_API.Repositories.IRepository;
using MBRS_API_DEMO.DataBase;
using MBRS_API_DEMO.Models.StoreProcedure;
using MBRS_API_DEMO.Utils;
using Microsoft.Data.SqlClient;

namespace MBRS_API.Repositories.Repository
{
    public class ManageFloorRepository : IManageFloorRepository
    {
        private readonly string _sqlCreateFloor = @"INSERT INTO floor (floor_code,number_floor,floor_name,delete_flag) VALUES (@floor_code,@number_floor,@floor_name,@delete_flag)";
        private readonly string _sqlDeleteFloor = @"UPDATE floor SET delete_flag = @delete_flag WHERE floor_id = @floor_id";
        private readonly string _sqlGetAllFloor = @"SELECT floor_id,floor_code,number_floor,floor_name
                                                    FROM floor
                                                    WHERE delete_flag = @delete_flag";
        private readonly string _sqlGetInformationFloor = @"SELECT floor_id,floor_code,number_floor,floor_name
                                                           FROM floor
                                                           WHERE delete_flag = @delete_flag AND floor_id = @floor_id";
        private readonly string _sqlUpdateFloor = @"UPDATE floor SET floor_code=@floor_code,number_floor=@number_floor,floor_name=@floor_name WHERE floor_id=@floor_id";
        private readonly SqlServerDBContext _sqlServerDbContext;
        private IConfiguration _configuration { get; }

        private readonly DBWorker dBWorker;
        public ManageFloorRepository(SqlServerDBContext sqlServer, IConfiguration configuration)
        {
            _sqlServerDbContext = sqlServer;
            _configuration = configuration;
            dBWorker = new DBWorker(configuration.GetConnectionString(IConstants.SqlString.SqlServerString));
        }
        public int createFloor(Floor floor)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlCreateFloor;
                sqlParameters.Add(new SqlParameter("@floor_code", floor.floorCode));
                sqlParameters.Add(new SqlParameter("@number_floor", floor.numberFloor));
                sqlParameters.Add(new SqlParameter("@floor_name", floor.floorName));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                dBWorker.ExecuteNonQuery(commandText, sqlParameters.ToArray());
                return ErrorCodeResponse.SUCCESS_CODE;
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return ErrorCodeResponse.FAIL_CODE;
            }
        }

        public int deleteFloor(int floorID)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlDeleteFloor;
                sqlParameters.Add(new SqlParameter("@floor_id", floorID));
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

        public List<Floor> getAllFloorWithFilter(string filterName, string filterValue)
        {
            List<Floor> floorList = new List<Floor>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllFloor;
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                if (filterName != "")
                {
                    commandText = _sqlGetAllFloor + " AND " + filterName + " LIKE @filterValue";
                    sqlParameters.Add(new SqlParameter("@filterValue", "%" + filterValue.Trim() + "%"));
                }
                floorList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<Floor>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return floorList;
        }

        public List<Floor> getFloorInformation(int floorID)
        {

            List<Floor> floorList = new List<Floor>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetInformationFloor;
                sqlParameters.Add(new SqlParameter("@floor_id", floorID));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                floorList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<Floor>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return floorList;
        }

        public int updateTheFloor(Floor floor)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlUpdateFloor;
                sqlParameters.Add(new SqlParameter("@floor_code", floor.floorCode));
                sqlParameters.Add(new SqlParameter("@number_floor", floor.numberFloor));
                sqlParameters.Add(new SqlParameter("@floor_name", floor.floorName));
                sqlParameters.Add(new SqlParameter("@floor_id", floor.floorID));
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
