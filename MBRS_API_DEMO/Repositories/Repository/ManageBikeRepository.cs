using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API_DEMO.DataBase;
using MBRS_API_DEMO.Models.StoreProcedure;
using MBRS_API_DEMO.Utils;
using Microsoft.Data.SqlClient;

namespace MBRS_API.Repositories.Repository
{
    public class ManageBikeRepository : IManageBikeRepository
    {
        private readonly string _sqlGetAllBike = @"SELECT bike_id,bike_code,bike_name,price,color,number_of_seat,type_bike_name
                                                   FROM bike
                                                   INNER JOIN type_bike ON bike.type_bike_id = type_bike.type_bike_id
                                                   WHERE bike.delete_flag=@delete_flag";
        private readonly string _sqlGetTheBike = @"SELECT * FROM bike WHERE delete_flag=@delete_flag AND bike_id = @bike_id";
        private readonly string _sqlUpdateTheBike = @"UPDATE bike SET bike_code=@bike_code,bike_name=@bike_name,type_bike_id=@type_bike_id WHERE bike_id = @bike_id AND delete_flag = @delete_flag";
        private readonly string _sqlCreateBike = @"INSERT INTO bike (bike_code,bike_name,type_bike_id,delete_flag) VALUES (@bike_code,@bike_name,@type_bike_id,@delete_flag)";
        private readonly string _sqlDeleteBike = @"UPDATE bike SET delete_flag = @delete_flag WHERE bike_id = @bike_id";

        private readonly string _sqlGetAllTypeBike = @"SELECT type_bike_id,type_bike_name FROM type_bike WHERE delete_flag = @delete_flag";

        private readonly string _sqlGetDetailInformationBike = @"SELECT * FROM type_bike WHERE delete_flag = @delete_flag AND type_bike_id = @type_bike_id";

        private readonly SqlServerDBContext _sqlServerDbContext;
        private IConfiguration _configuration { get; }
        private readonly DBWorker dBWorker;
        public ManageBikeRepository(SqlServerDBContext sqlServer, IConfiguration configuration)
        {
            _sqlServerDbContext = sqlServer;
            _configuration = configuration;
            dBWorker = new DBWorker(configuration.GetConnectionString(IConstants.SqlString.SqlServerString));
        }

        public List<BikeViewModel> getAllBike()
        {
            List<BikeViewModel> bikeList = new List<BikeViewModel>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllBike;
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                bikeList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<BikeViewModel>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return bikeList;
        }

        public int deleteBike(int bikeID)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlDeleteBike;
                sqlParameters.Add(new SqlParameter("@bike_id", bikeID));
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

        public List<BikeViewModel> getAllBikeWithFilter(string filterName, string filterValue)
        {
            List<BikeViewModel> bikeList = new List<BikeViewModel>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllBike;
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                if (filterName != "")
                {
                    commandText = _sqlGetAllBike + " AND " + filterName + " LIKE @filterValue";
                    sqlParameters.Add(new SqlParameter("@filterValue", "%" + filterValue.Trim() + "%"));
                }
                bikeList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<BikeViewModel>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return bikeList;
        }
        public int updateTheBike(Bike bike)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlUpdateTheBike;
                sqlParameters.Add(new SqlParameter("@bike_id", bike.bikeID));
                sqlParameters.Add(new SqlParameter("@bike_code", bike.bikeCode));
                sqlParameters.Add(new SqlParameter("@bike_name", bike.bikeName));
                sqlParameters.Add(new SqlParameter("@type_bike_id", bike.typeBikeID));
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
        public List<TypeBike> getAllTypeBike()
        {
            List<TypeBike> typeBikeList = new List<TypeBike>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllTypeBike;
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                typeBikeList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<TypeBike>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return typeBikeList;
        }
        public List<TypeBike> getDetailInformationBike(int typeBikeID)
        {
            List<TypeBike> bikeList = new List<TypeBike>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetDetailInformationBike;
                sqlParameters.Add(new SqlParameter("@type_bike_id", typeBikeID));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                bikeList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<TypeBike>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return bikeList;
        }

        public int createBike(Bike bike)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlCreateBike;
                sqlParameters.Add(new SqlParameter("@bike_code", bike.bikeCode));
                sqlParameters.Add(new SqlParameter("@bike_name", bike.bikeName));
                sqlParameters.Add(new SqlParameter("@type_bike_id", bike.typeBikeID));
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

        public List<Bike> getTheBikeInformation(int bikeID)
        {
            List<Bike> bikeList = new List<Bike>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetTheBike;
                sqlParameters.Add(new SqlParameter("@bike_id", bikeID));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                bikeList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<Bike>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return bikeList;
        }
    }
}
