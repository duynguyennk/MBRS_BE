using MBRS_API.Models;
using MBRS_API.Repositories.IRepository;
using MBRS_API_DEMO.DataBase;
using MBRS_API_DEMO.Models.StoreProcedure;
using MBRS_API_DEMO.Utils;
using Microsoft.Data.SqlClient;

namespace MBRS_API.Repositories.Repository
{
    public class ManageTypeCarAirportRepository : IManageTypeCarAirportRepository
    {
        private readonly string _sqlGetInformationTypeCarAirport = @"SELECT type_car_airport_id,type_car_airport_code,type_car_airport_name,price,color,number_of_seat
                                                                     FROM type_car_airport
                                                                     WHERE delete_flag=@delete_flag AND type_car_airport_id = @type_car_airport_id";
        private readonly string _sqlGetAllTypeCarAirport = @"SELECT type_car_airport_id,type_car_airport_code,type_car_airport_name,price,color,number_of_seat
                                                             FROM type_car_airport
                                                             WHERE delete_flag=@delete_flag";
        private readonly string _sqlCreateTypeCarAirport = @"INSERT INTO type_car_airport (type_car_airport_code,type_car_airport_name,price,color,number_of_seat,delete_flag) VALUES (@type_car_airport_code,@type_car_airport_name,@price,@color,@number_of_seat,@delete_flag)";
        private readonly string _sqlUpdateTypeCarAirport = @"UPDATE type_car_airport SET type_car_airport_code=@type_car_airport_code,type_car_airport_name=@type_car_airport_name,price=@price,color=@color,number_of_seat=@number_of_seat WHERE type_car_airport_id = @type_car_airport_id";
        private readonly string _sqlDeleteTypeCarAirport = @"UPDATE type_car_airport SET delete_flag = @delete_flag WHERE type_car_airport_id = @type_car_airport_id";
        private readonly SqlServerDBContext _sqlServerDbContext;
        private IConfiguration _configuration { get; }

        private readonly DBWorker dBWorker;
        public ManageTypeCarAirportRepository(SqlServerDBContext sqlServer, IConfiguration configuration)
        {
            _sqlServerDbContext = sqlServer;
            _configuration = configuration;
            dBWorker = new DBWorker(configuration.GetConnectionString(IConstants.SqlString.SqlServerString));
        }
        public int createTypeCarAirport(TypeCarAirport typeCarAirport)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlCreateTypeCarAirport;
                sqlParameters.Add(new SqlParameter("@type_car_airport_code", typeCarAirport.typeCarAirportCode));
                sqlParameters.Add(new SqlParameter("@type_car_airport_name", typeCarAirport.typeCarAirportName));
                sqlParameters.Add(new SqlParameter("@price", typeCarAirport.price));
                sqlParameters.Add(new SqlParameter("@color", typeCarAirport.color));
                sqlParameters.Add(new SqlParameter("@number_of_seat", typeCarAirport.numberOfSeat));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                dBWorker.GetDataTable(commandText, sqlParameters.ToArray());
                return ErrorCodeResponse.SUCCESS_CODE;
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return ErrorCodeResponse.FAIL_CODE;
            }
        }

        public int deleteTypeCarAirport(int typeCarAirportID)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlDeleteTypeCarAirport;
                sqlParameters.Add(new SqlParameter("@type_car_airport_id", typeCarAirportID));
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

        public List<TypeCarAirport> getAllTypeCarAirport()
        {
            List<TypeCarAirport> typeCarAirportList = new List<TypeCarAirport>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllTypeCarAirport;
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                typeCarAirportList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<TypeCarAirport>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return typeCarAirportList;
        }

        public List<TypeCarAirport> getAllTypeCarAirportWithFilter(string filterName, string filterValue)
        {
            List<TypeCarAirport> typeCarAirportList = new List<TypeCarAirport>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllTypeCarAirport;
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                if (filterName != "")
                {
                    commandText = _sqlGetAllTypeCarAirport + " AND " + filterName + " LIKE @filterValue";
                    sqlParameters.Add(new SqlParameter("@filterValue", "%" + filterValue.Trim() + "%"));
                }
                typeCarAirportList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<TypeCarAirport>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return typeCarAirportList;
        }

        public List<TypeCarAirport> getTypeCarAirportInformation(int typeCarAirportID)
        {
            List<TypeCarAirport> typeCarAirportList = new List<TypeCarAirport>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetInformationTypeCarAirport;
                sqlParameters.Add(new SqlParameter("@type_car_airport_id", typeCarAirportID));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                typeCarAirportList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<TypeCarAirport>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return typeCarAirportList;
        }

        public int updateTheTypeCarAirport(TypeCarAirport typeCarAirport)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlUpdateTypeCarAirport;
                sqlParameters.Add(new SqlParameter("@type_car_airport_code", typeCarAirport.typeCarAirportCode));
                sqlParameters.Add(new SqlParameter("@type_car_airport_name", typeCarAirport.typeCarAirportName));
                sqlParameters.Add(new SqlParameter("@price", typeCarAirport.price));
                sqlParameters.Add(new SqlParameter("@color", typeCarAirport.color));
                sqlParameters.Add(new SqlParameter("@number_of_seat", typeCarAirport.numberOfSeat));
                sqlParameters.Add(new SqlParameter("@type_car_airport_id", typeCarAirport.typeCarAirportID));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
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
