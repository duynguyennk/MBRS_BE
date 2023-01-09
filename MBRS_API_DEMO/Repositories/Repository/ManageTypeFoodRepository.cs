using MBRS_API.Models;
using MBRS_API.Repositories.IRepository;
using MBRS_API_DEMO.DataBase;
using MBRS_API_DEMO.Models.StoreProcedure;
using MBRS_API_DEMO.Utils;
using Microsoft.Data.SqlClient;

namespace MBRS_API.Repositories.Repository
{
    public class ManageTypeFoodRepository : IManageTypeFoodRepository
    {
        private readonly string _sqlGetInformationTypeBike = @"SELECT type_food_id,type_food_name,type_food_code
                                                       FROM type_food
                                                       WHERE delete_flag=@delete_flag AND type_food_id=@type_food_id";
        private readonly string _sqlGetAllTypeFood = @"SELECT type_food_id,type_food_name,type_food_code
                                                       FROM type_food
                                                       WHERE delete_flag=@delete_flag";
        private readonly string _sqlCreateTypeFood = @"INSERT INTO type_food (type_food_name,type_food_code,delete_flag) VALUES (@type_food_name,@type_food_code,@delete_flag)";
        private readonly string _sqlUpdateTypeFood = @"UPDATE type_food SET type_food_name=@type_food_name,type_food_code=@type_food_code WHERE type_food_id = @type_food_id";
        private readonly string _sqlDeleteTypeFood = @"UPDATE type_food SET delete_flag = @delete_flag WHERE type_food_id = @type_food_id";
        private readonly SqlServerDBContext _sqlServerDbContext;
        private IConfiguration _configuration { get; }

        private readonly DBWorker dBWorker;
        public ManageTypeFoodRepository(SqlServerDBContext sqlServer, IConfiguration configuration)
        {
            _sqlServerDbContext = sqlServer;
            _configuration = configuration;
            dBWorker = new DBWorker(configuration.GetConnectionString(IConstants.SqlString.SqlServerString));
        }
        public int createTypeFood(TypeFood typeFood)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlCreateTypeFood;
                sqlParameters.Add(new SqlParameter("@type_food_name", typeFood.typeFoodName));
                sqlParameters.Add(new SqlParameter("@type_food_code", typeFood.typeFoodCode));
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

        public int deleteTypeFood(int typeFoodID)
        {

            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlDeleteTypeFood;
                sqlParameters.Add(new SqlParameter("@type_food_id", typeFoodID));
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

        public List<TypeFood> getAllTypeFood()
        {
            List<TypeFood> typeFoodList = new List<TypeFood>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllTypeFood;
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                typeFoodList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<TypeFood>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return typeFoodList;
        }

        public List<TypeFood> getAllTypeFoodWithFilter(string filterName, string filterValue)
        {
            List<TypeFood> typeFoodList = new List<TypeFood>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllTypeFood;
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                if (filterName != "")
                {
                    commandText = _sqlGetAllTypeFood + " AND " + filterName + " LIKE @filterValue";
                    sqlParameters.Add(new SqlParameter("@filterValue", "%" + filterValue.Trim() + "%"));
                }
                typeFoodList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<TypeFood>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return typeFoodList;
        }

        public List<TypeFood> getTypeFoodInformation(int typeFoodID)
        {
            List<TypeFood> typefoodList = new List<TypeFood>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetInformationTypeBike;
                sqlParameters.Add(new SqlParameter("@type_food_id", typeFoodID));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                typefoodList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<TypeFood>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return typefoodList;
        }

        public int updateTheTypeFood(TypeFood typeFood)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlUpdateTypeFood;
                sqlParameters.Add(new SqlParameter("@type_food_name", typeFood.typeFoodName));
                sqlParameters.Add(new SqlParameter("@type_food_code", typeFood.typeFoodCode));
                sqlParameters.Add(new SqlParameter("@type_food_id", typeFood.typeFoodID));
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
