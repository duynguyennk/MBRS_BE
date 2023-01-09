using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API_DEMO.DataBase;
using MBRS_API_DEMO.Models.StoreProcedure;
using MBRS_API_DEMO.Utils;
using Microsoft.Data.SqlClient;

namespace MBRS_API.Repositories.Repository
{
    public class ManageCarAirportRepository : IManageCarAirportRepository
    {
        private readonly string _sqlGetAllCar = @"SELECT car_airport_id,car_airport_name,car_airport_code,identify_car_number,car_airport.[image],type_car_airport_name,price,number_of_seat,color
                                                  FROM car_airport
                                                  INNER JOIN type_car_airport ON type_car_airport.type_car_airport_id = car_airport.type_car_airport_id
                                                  WHERE car_airport.delete_flag = @delete_flag1 AND type_car_airport.delete_flag = @delete_flag2";

        private readonly string _sqlDeleteCar = @"UPDATE car_airport SET delete_flag = @delete_flag WHERE car_airport_id = @car_airport_id";

        private readonly string _sqlUpdateTheCar = @"UPDATE car_airport SET car_airport_name=@car_airport_name,car_airport_code=@car_airport_code,identify_car_number=@identify_car_number,type_car_airport_id=@type_car_airport_id WHERE car_airport_id=@car_airport_id AND delete_flag=@delete_flag";

        private readonly string _sqlGetAllTypeCar = @"SELECT type_car_airport_id,type_car_airport_name FROM type_car_airport WHERE delete_flag = @delete_flag";

        private readonly string _sqlGetInformationDetailTypeCar = @"SELECT * FROM type_car_airport WHERE type_car_airport_id=@type_car_airport_id AND delete_flag = @delete_flag";

        private readonly string _sqlCreateCar = @"INSERT INTO car_airport (car_airport_name,car_airport_code,identify_car_number,type_car_airport_id,delete_flag) VALUES (@car_airport_name,@car_airport_code,@identify_car_number,@type_car_airport_id,@delete_flag)";

        private readonly string _sqlGetTheCar = @"SELECT * FROM car_airport WHERE  car_airport_id=@car_airport_id AND delete_flag=@delete_flag";
        private readonly SqlServerDBContext _sqlServerDbContext;
        private IConfiguration _configuration { get; }
        private readonly DBWorker dBWorker;
        public ManageCarAirportRepository(SqlServerDBContext sqlServer, IConfiguration configuration)
        {
            _sqlServerDbContext = sqlServer;
            _configuration = configuration;
            dBWorker = new DBWorker(configuration.GetConnectionString(IConstants.SqlString.SqlServerString));
        }

        public List<CarAirportViewModel> getAllCar()
        {
            List<CarAirportViewModel> carList = new List<CarAirportViewModel>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllCar;
                sqlParameters.Add(new SqlParameter("@delete_flag1", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                sqlParameters.Add(new SqlParameter("@delete_flag2", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                carList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<CarAirportViewModel>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return carList;
        }

        public int deleteCar(int carID)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlDeleteCar;
                sqlParameters.Add(new SqlParameter("@car_airport_id", carID));
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

        public List<CarAirportViewModel> getAllCarWithFilter(string filterName, string filterValue)
        {
            List<CarAirportViewModel> carList = new List<CarAirportViewModel>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllCar;
                sqlParameters.Add(new SqlParameter("@delete_flag1", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                sqlParameters.Add(new SqlParameter("@delete_flag2", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                if (filterName != "")
                {
                    commandText = _sqlGetAllCar + " AND " + filterName + " LIKE @filterValue";
                    sqlParameters.Add(new SqlParameter("@filterValue", "%" + filterValue.Trim() + "%"));
                }
                carList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<CarAirportViewModel>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return carList;
        }
        public int updateTheCar(CarAirport carAirport)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlUpdateTheCar;
                sqlParameters.Add(new SqlParameter("@car_airport_name", carAirport.carAirportName));
                sqlParameters.Add(new SqlParameter("@car_airport_code", carAirport.carAirportCode));
                sqlParameters.Add(new SqlParameter("@identify_car_number", carAirport.identifyCarNumber));
                sqlParameters.Add(new SqlParameter("@type_car_airport_id", carAirport.typeCarAirportID));
                sqlParameters.Add(new SqlParameter("@car_airport_id", carAirport.carAirportID));
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
        public List<TypeCarAirport> getAllTypeCar()
        {
            List<TypeCarAirport> typeCarList = new List<TypeCarAirport>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllTypeCar;
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                typeCarList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<TypeCarAirport>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return typeCarList;
        }
        public List<TypeCarAirport> getDetailInformationCar(int typeCarID)
        {
            List<TypeCarAirport> detailTypeCarList = new List<TypeCarAirport>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetInformationDetailTypeCar;
                sqlParameters.Add(new SqlParameter("@type_car_airport_id", typeCarID));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                detailTypeCarList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<TypeCarAirport>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return detailTypeCarList;
        }

        public int createCar(CarAirport carAirport)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlCreateCar;
                sqlParameters.Add(new SqlParameter("@car_airport_name", carAirport.carAirportName));
                sqlParameters.Add(new SqlParameter("@car_airport_code", carAirport.carAirportCode));
                sqlParameters.Add(new SqlParameter("@identify_car_number", carAirport.identifyCarNumber));
                sqlParameters.Add(new SqlParameter("@type_car_airport_id", carAirport.typeCarAirportID));
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

        public List<CarAirport> getTheCarInformation(int carID)
        {
            List<CarAirport> bikeList = new List<CarAirport>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetTheCar;
                sqlParameters.Add(new SqlParameter("@car_airport_id", carID));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                bikeList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<CarAirport>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return bikeList;
        }
    }
}
