using MBRS_API.Models;
using MBRS_API.Repositories.IRepository;
using MBRS_API_DEMO.DataBase;
using MBRS_API_DEMO.Models.StoreProcedure;
using MBRS_API_DEMO.Utils;
using Microsoft.Data.SqlClient;
using System.Globalization;

namespace MBRS_API.Repositories.Repository
{
    public class ManageOrderFoodRepository : IManageOrderFoodRepository
    {
        private readonly string _sqlGetAllOrderFood = @"SELECT order_food.order_id,order_food.order_code,order_food.price,order_food.content_payment,order_food.status_payment,order_food.vnp_transaction_number,order_food.date_time,full_name,phone_number,status_food
                                                        FROM order_food
                                                        INNER JOIN customer ON customer.customer_id = order_food.customer_id
                                                        WHERE order_food.delete_flag=@delete_flag";

        private readonly string _sqlDeleteOrderFood = @"UPDATE order_food SET delete_flag = @delete_flag WHERE order_id = @order_id";

        private readonly string _sqlUpdateStatusOrderFood = @"UPDATE order_food SET status_food = @status_food WHERE order_id = @order_id";

        private readonly SqlServerDBContext _sqlServerDbContext;
        private IConfiguration _configuration { get; }

        private readonly DBWorker dBWorker;
        public ManageOrderFoodRepository(SqlServerDBContext sqlServer, IConfiguration configuration)
        {
            _sqlServerDbContext = sqlServer;
            _configuration = configuration;
            dBWorker = new DBWorker(configuration.GetConnectionString(IConstants.SqlString.SqlServerString));
        }

        public int updateStatusFood(StatusFood statusFood)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlUpdateStatusOrderFood;
                sqlParameters.Add(new SqlParameter("@status_food", statusFood.status));
                sqlParameters.Add(new SqlParameter("@order_id", statusFood.orderID));
                dBWorker.ExecuteNonQuery(commandText, sqlParameters.ToArray());
                return ErrorCodeResponse.SUCCESS_CODE;
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return ErrorCodeResponse.FAIL_CODE;
            }
        }
        public List<OrderFood> getAllOrderFood()
        {
            List<OrderFood> orderFoodList = new List<OrderFood>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllOrderFood;
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                orderFoodList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<OrderFood>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return orderFoodList;
        }

        public List<OrderFood> getAllOrderFoodFilter(string filterName, string filterValue)
        {
            List<OrderFood> orderBikeList = new List<OrderFood>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllOrderFood;
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                if (filterName != "")
                {
                    commandText = _sqlGetAllOrderFood + " AND " + filterName + " LIKE @filterValue";
                    if (filterName == "date_time_get_bike" || filterName == "date_time_back_bike")
                    {
                        commandText = _sqlGetAllOrderFood + " AND " + "CONVERT(VARCHAR(10)," + filterName + ",120)" + " LIKE @filterValue";
                        if (filterValue != null)
                        {
                            string format = "dd/MM/yyyy";
                            filterValue = (DateTime.ParseExact(filterValue, format, CultureInfo.InvariantCulture)).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            filterValue = "";
                        }
                    }
                    else if (filterName == "date_time")
                    {
                        if (filterValue != null)
                        {
                            string format = "dd/MM/yyyy";
                            filterValue = (DateTime.ParseExact(filterValue, format, CultureInfo.InvariantCulture)).ToString("yyyyMMdd");
                        }
                        else
                        {
                            filterValue = "";
                        }
                    }
                    sqlParameters.Add(new SqlParameter("@filterValue", "%" + filterValue.Trim() + "%"));
                }
                orderBikeList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<OrderFood>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return orderBikeList;
        }

        public int deleteOrderFood(int orderFoodID)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlDeleteOrderFood;
                sqlParameters.Add(new SqlParameter("@order_id", orderFoodID));
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
    }
}
