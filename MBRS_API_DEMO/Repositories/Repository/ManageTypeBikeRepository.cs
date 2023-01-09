using MBRS_API.Models;
using MBRS_API.Repositories.IRepository;
using MBRS_API_DEMO.DataBase;
using MBRS_API_DEMO.Models.StoreProcedure;
using MBRS_API_DEMO.Utils;
using Microsoft.Data.SqlClient;

namespace MBRS_API.Repositories.Repository
{
    public class ManageTypeBikeRepository : IManageTypeBikeRepository
    {
        private readonly string _sqlGetInformationTypeBike = @"SELECT type_bike_id,type_bike_code,type_bike_name,price,color,number_of_seat,image_base64
                                                               FROM type_bike
                                                               WHERE delete_flag=@delete_flag AND type_bike_id=@type_bike_id";
        private readonly string _sqlGetAllTypeBike = @"SELECT type_bike_id,type_bike_code,type_bike_name,price,color,number_of_seat
                                                       FROM type_bike
                                                       WHERE delete_flag=@delete_flag";
        private readonly string _sqlCreateTypeBike = @"INSERT INTO type_bike (type_bike_code,type_bike_name,price,color,number_of_seat,delete_flag) VALUES (@type_bike_code,@type_bike_name,@price,@color,@number_of_seat,@delete_flag); SELECT SCOPE_IDENTITY()";
        private readonly string _sqlUpdateTypeBike = @"UPDATE type_bike SET type_bike_code=@type_bike_code,type_bike_name=@type_bike_name,price=@price,color=@color,number_of_seat=@number_of_seat WHERE type_bike_id = @type_bike_id";
        private readonly string _sqlDeleteTypeBike = @"UPDATE type_bike SET delete_flag = @delete_flag WHERE type_bike_id = @type_bike_id";
        private readonly string _sqlUpdateTypeBikeImage = @"UPDATE type_bike SET image_base64=@image_base64 WHERE type_bike_id=@type_bike_id";
        private readonly SqlServerDBContext _sqlServerDbContext;
        private IConfiguration _configuration { get; }

        private readonly DBWorker dBWorker;
        public ManageTypeBikeRepository(SqlServerDBContext sqlServer, IConfiguration configuration)
        {
            _sqlServerDbContext = sqlServer;
            _configuration = configuration;
            dBWorker = new DBWorker(configuration.GetConnectionString(IConstants.SqlString.SqlServerString));
        }
        public int updateImageTypeBike(ItemImage itemImage)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlUpdateTypeBikeImage;
                sqlParameters.Add(new SqlParameter("@image_base64", itemImage.Base64));
                sqlParameters.Add(new SqlParameter("@type_bike_id", itemImage.idObject));
                dBWorker.ExecuteNonQuery(commandText, sqlParameters.ToArray());
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

        public List<TypeBike> getAllTypeBikeWithFilter(string filterName, string filterValue)
        {
            List<TypeBike> typeBikeList = new List<TypeBike>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllTypeBike;
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                if (filterName != "")
                {
                    commandText = _sqlGetAllTypeBike + " AND " + filterName + " LIKE @filterValue";
                    sqlParameters.Add(new SqlParameter("@filterValue", "%" + filterValue.Trim() + "%"));
                }
                typeBikeList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<TypeBike>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return typeBikeList;
        }

        public int deleteTypeBike(int typeBikeID)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlDeleteTypeBike;
                sqlParameters.Add(new SqlParameter("@type_bike_id", typeBikeID));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.Deleted));
                dBWorker.ExecuteNonQuery(commandText, sqlParameters.ToArray());
                return ErrorCodeResponse.SUCCESS_CODE;
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return ErrorCodeResponse.FAIL_CODE;
            }
        }

        public int updateTheTypeBike(TypeBike typeBike)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlUpdateTypeBike;
                sqlParameters.Add(new SqlParameter("@type_bike_code", typeBike.typeBikeCode));
                sqlParameters.Add(new SqlParameter("@type_bike_name", typeBike.typeBikeName));
                sqlParameters.Add(new SqlParameter("@price", typeBike.price));
                sqlParameters.Add(new SqlParameter("@color", typeBike.color));
                sqlParameters.Add(new SqlParameter("@number_of_seat", typeBike.numberOfSeat));
                sqlParameters.Add(new SqlParameter("@type_bike_id", typeBike.typeBikeID));
                dBWorker.ExecuteNonQuery(commandText, sqlParameters.ToArray());
                return ErrorCodeResponse.SUCCESS_CODE;
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return ErrorCodeResponse.FAIL_CODE;
            }
        }

        public List<TypeBike> getTypeBikeInformation(int typeBikeID)
        {
            List<TypeBike> typeBikeList = new List<TypeBike>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetInformationTypeBike;
                sqlParameters.Add(new SqlParameter("@type_bike_id", typeBikeID));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                typeBikeList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<TypeBike>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return typeBikeList;
        }

        public int createTypeBike(TypeBike typeBike)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlCreateTypeBike;
                sqlParameters.Add(new SqlParameter("@type_bike_code", typeBike.typeBikeCode));
                sqlParameters.Add(new SqlParameter("@type_bike_name", typeBike.typeBikeName));
                sqlParameters.Add(new SqlParameter("@price", typeBike.price));
                sqlParameters.Add(new SqlParameter("@color", typeBike.color));
                sqlParameters.Add(new SqlParameter("@number_of_seat", typeBike.numberOfSeat));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                int typeBikeID = Convert.ToInt32(dBWorker.ExecuteScalar(commandText, sqlParameters.ToArray()));
                return typeBikeID;
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return ErrorCodeResponse.FAIL_CODE;
            }
        }
    }
}
