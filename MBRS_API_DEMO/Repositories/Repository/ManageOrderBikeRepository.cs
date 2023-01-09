using MBRS_API.Models;
using MBRS_API.Repositories.IRepository;
using MBRS_API_DEMO.DataBase;
using MBRS_API_DEMO.Models.StoreProcedure;
using MBRS_API_DEMO.Utils;
using Microsoft.Data.SqlClient;
using System.Globalization;

namespace MBRS_API.Repositories.Repository
{
    public class ManageOrderBikeRepository : IManageOrderBikeRepository
    {
        private readonly string _sqlGetAllOrderBike = @"SELECT full_name,identify_number,phone_number,email,order_id,order_code,price,content_payment,status_payment,content_result_payment,bank_transaction_number,vnp_transaction_number,type_payment,bank_code,date_time,date_time_get_bike,date_time_back_bike,number_hours_rent,order_bike.customer_id,status_bike
                                                        FROM order_bike
                                                        LEFT JOIN customer ON customer.customer_id = order_bike.customer_id
                                                        LEFT JOIN user_account ON customer.account_id = user_account.account_id
                                                        WHERE order_bike.delete_flag = @delete_flag";

        private readonly string _sqlDeleteOrderBike = @"UPDATE order_bike SET delete_flag = @delete_flag WHERE order_id = @order_id";

        private readonly string _sqlUpdateStatusOrderBike = @"UPDATE order_bike SET status_bike = @status_bike WHERE order_id = @order_id";

        private readonly SqlServerDBContext _sqlServerDbContext;
        private IConfiguration _configuration { get; }

        private readonly DBWorker dBWorker;
        public ManageOrderBikeRepository(SqlServerDBContext sqlServer, IConfiguration configuration)
        {
            _sqlServerDbContext = sqlServer;
            _configuration = configuration;
            dBWorker = new DBWorker(configuration.GetConnectionString(IConstants.SqlString.SqlServerString));
        }

        public int updateStatusBike(StatusBike statusBike)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlUpdateStatusOrderBike;
                sqlParameters.Add(new SqlParameter("@status_bike", statusBike.status));
                sqlParameters.Add(new SqlParameter("@order_id", statusBike.orderID));
                dBWorker.ExecuteNonQuery(commandText, sqlParameters.ToArray());
                return ErrorCodeResponse.SUCCESS_CODE;
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return ErrorCodeResponse.FAIL_CODE;
            }
        }
        public List<OrderBike> getAllOrderBike()
        {
            List<OrderBike> orderBikeList = new List<OrderBike>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllOrderBike;
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                orderBikeList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<OrderBike>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return orderBikeList;
        }

        public List<OrderBike> getAllOrderBikeFilter(string filterName, string filterValue)
        {
            List<OrderBike> orderBikeList = new List<OrderBike>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetAllOrderBike;
                sqlParameters.Add(new SqlParameter("@delete_flag", IConstants.CHECKING_STATUS_DELETE_FLAG.NoDelete));
                if (filterName != "")
                {
                    commandText = _sqlGetAllOrderBike + " AND " + filterName + " LIKE @filterValue";
                    if (filterName == "date_time_get_bike" || filterName == "date_time_back_bike")
                    {
                        commandText = _sqlGetAllOrderBike + " AND " + "CONVERT(VARCHAR(10)," + filterName + ",120)" + " LIKE @filterValue";
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
                orderBikeList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<OrderBike>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return orderBikeList;
        }

        public int deleteOrderBike(int orderBikeID)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlDeleteOrderBike;
                sqlParameters.Add(new SqlParameter("@order_id", orderBikeID));
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
