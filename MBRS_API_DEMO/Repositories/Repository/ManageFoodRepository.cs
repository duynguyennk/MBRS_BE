using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API_DEMO.DataBase;
using MBRS_API_DEMO.Models.StoreProcedure;
using MBRS_API_DEMO.Utils;
using Microsoft.Data.SqlClient;

namespace MBRS_API.Repositories.Repository
{
    public class ManageFoodRepository : IManageFoodRepository
    {
        private readonly string _sqlGetAllFood = @"SELECT food_id,food_name,quantity,type_food.type_food_name,price,food_code
                                                   FROM food
                                                   INNER JOIN type_food ON type_food.type_food_id = food.type_food_id
                                                   WHERE food.delete_flag =@delete_flag1 AND type_food.delete_flag = @delete_flag2";

        private readonly string _sqlDeleteFood = @"UPDATE food SET delete_flag = @delete_flag WHERE food_id = @food_id";

        private readonly string _sqlUpdateFood = @"UPDATE food SET food_code=@food_code,food_name=@food_name,quantity=@quantity,type_food_id=@type_food_id,price=@price WHERE food_id=@food_id";

        private readonly string _sqlGetInformationFood = @"SELECT food_id,food_name,food_code,price,type_food_id,quantity,image_base64
                                                           FROM food
                                                           WHERE delete_flag = @delete_flag AND food_id = @food_id";

        private readonly string _sqlCheckFoodCode = @"SELECT COUNT(food_id) FROM food WHERE food_code = @food_code AND delete_flag = 0 AND food_id !=@food_id";

        private readonly string _sqlUpdateFoodImage = @"UPDATE food SET image_base64=@image_base64 WHERE food_id=@food_id";

        private readonly string _sqlGetTypeFood = @"SELECT type_food_id,type_food_name
                                                    FROM type_food
                                                    WHERE delete_flag = @delete_flag";

        private readonly string _sqlCreateFood = @"INSERT INTO food (food_name,food_code,price,type_food_id,quantity,delete_flag) VALUES (@food_name,@food_code,@price,@type_food_id,@quantity,@delete_flag); SELECT SCOPE_IDENTITY()";

        private readonly SqlServerDBContext _sqlServerDbContext;
        private IConfiguration _configuration { get; }

        private readonly DBWorker dBWorker;
        public ManageFoodRepository(SqlServerDBContext sqlServer, IConfiguration configuration)
        {
            _sqlServerDbContext = sqlServer;
            _configuration = configuration;
            dBWorker = new DBWorker(configuration.GetConnectionString(IConstants.SqlString.SqlServerString));
        }

        public List<FoodViewModel> getAllFood()
        {
            List<FoodViewModel> foodList = new List<FoodViewModel>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllFood + " ORDER BY food_id DESC";
                sqlParameters.Add(new SqlParameter("@delete_flag1", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                sqlParameters.Add(new SqlParameter("@delete_flag2", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                foodList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<FoodViewModel>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return foodList;
        }

        public int checkDuplicateFoodCode(string foodCode,int foodID)
        {
            int checkDuplicate = 0;
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlCheckFoodCode;
                sqlParameters.Add(new SqlParameter("@food_code", foodCode));
                sqlParameters.Add(new SqlParameter("@food_id", foodID));
                checkDuplicate = Convert.ToInt32(dBWorker.ExecuteScalar(commandText, sqlParameters.ToArray()));
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return checkDuplicate;
        }

        public List<FoodViewModel> getAllFoodWithFilter(string filterName, string filterValue)
        {
            List<FoodViewModel> foodList = new List<FoodViewModel>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllFood;
                sqlParameters.Add(new SqlParameter("@delete_flag1", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                sqlParameters.Add(new SqlParameter("@delete_flag2", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                if (filterName != "")
                {
                    commandText = _sqlGetAllFood + " AND " + filterName + " LIKE @filterValue  ORDER BY food_id DESC";
                    sqlParameters.Add(new SqlParameter("@filterValue", "%" + filterValue.Trim() + "%"));
                }
                foodList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<FoodViewModel>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return foodList;
        }

        public int deleteFood(int foodID)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlDeleteFood;
                sqlParameters.Add(new SqlParameter("@food_id", foodID));
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

        public int updateTheFood(Food food)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlUpdateFood;
                sqlParameters.Add(new SqlParameter("@food_code", food.foodCode));
                sqlParameters.Add(new SqlParameter("@food_name", food.foodName));
                sqlParameters.Add(new SqlParameter("@quantity", food.quantity));
                sqlParameters.Add(new SqlParameter("@type_food_id", food.typeFoodID));
                sqlParameters.Add(new SqlParameter("@price", food.price));
                sqlParameters.Add(new SqlParameter("@food_id", food.foodID));
                dBWorker.GetDataTable(commandText, sqlParameters.ToArray());
                return ErrorCodeResponse.SUCCESS_CODE;
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return ErrorCodeResponse.FAIL_CODE;
            }
        }

        public List<Food> getFoodInformation(int foodID)
        {
            List<Food> foodList = new List<Food>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetInformationFood;
                sqlParameters.Add(new SqlParameter("@food_id", foodID));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                foodList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<Food>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return foodList;
        }

        public List<TypeFood> getAllTypeFood()
        {
            List<TypeFood> typeFoodList = new List<TypeFood>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetTypeFood;
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                typeFoodList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<TypeFood>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return typeFoodList;
        }
        public int updateImageFood(ItemImage itemImage)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlUpdateFoodImage;
                sqlParameters.Add(new SqlParameter("@image_base64", itemImage.Base64));
                sqlParameters.Add(new SqlParameter("@food_id", itemImage.idObject));
                dBWorker.ExecuteNonQuery(commandText, sqlParameters.ToArray());
                return ErrorCodeResponse.SUCCESS_CODE;
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return ErrorCodeResponse.FAIL_CODE;
            }
        }
        public int createFood(Food food)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlCreateFood;
                sqlParameters.Add(new SqlParameter("@food_name", food.foodName));
                sqlParameters.Add(new SqlParameter("@food_code", food.foodCode));
                sqlParameters.Add(new SqlParameter("@price", food.price));
                sqlParameters.Add(new SqlParameter("@type_food_id", food.typeFoodID));
                sqlParameters.Add(new SqlParameter("@quantity", food.quantity));
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                int foodID = Convert.ToInt32(dBWorker.ExecuteScalar(commandText, sqlParameters.ToArray()));
                return foodID;
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return ErrorCodeResponse.FAIL_CODE;
            }
        }


    }
}
