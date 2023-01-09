using MBRS_API.Models;
using MBRS_API.Repositories.IRepository;
using MBRS_API_DEMO.DataBase;
using MBRS_API_DEMO.Models.StoreProcedure;
using MBRS_API_DEMO.Utils;
using Microsoft.Data.SqlClient;

namespace MBRS_API.Repositories.Repository
{
    public class ManageFeedbackOrderCustomerRepository : IManageFeedbackOrderCustomerRepository
    {
        private readonly string _sqlCreateFeedbackRoom = @"INSERT INTO rating_room (number_rating_conveniences,number_rating_Interior,number_rating_employee,number_rating_service,number_rating_hygiene,number_rating_view,content_rating,date_time,order_id) VALUES (@number_rating_conveniences,@number_rating_Interior,@number_rating_employee,@number_rating_service,@number_rating_hygiene,@number_rating_view,@content_rating,@date_time,@order_id)";
        
        private readonly string _sqlCreateFeedbackBike = @"INSERT INTO rating_bike (rating_number,order_id,content_rating) VALUES (@rating_number,@order_id,@content_rating)";

        private readonly string _sqlCreateFeedbackFood = @"INSERT INTO rating_food (rating_number,order_id,content_rating) VALUES (@rating_number,@order_id,@content_rating)";

        private readonly string _sqlGetFeedbackRoomByRatingID = @"SELECT number_rating_conveniences,number_rating_Interior,number_rating_employee,number_rating_service,number_rating_hygiene,number_rating_view,content_rating " +
                                                             "FROM rating_room " +
                                                             "WHERE rating_id = @rating_id";

        private readonly string _sqlGetFeedbackFoodByRatingID = @"SELECT rating_number,content_rating " +
                                                                 "FROM rating_food " +
                                                                 "WHERE rating_id = @rating_id";

        private readonly string _sqlGetFeedbackBikeByRatingID = @"SELECT rating_number,content_rating " +
                                                                "FROM rating_bike " +
                                                                "WHERE rating_id = @rating_id";

        private readonly SqlServerDBContext _sqlServerDbContext;
        private IConfiguration _configuration { get; }

        private readonly DBWorker dBWorker;
        public ManageFeedbackOrderCustomerRepository(SqlServerDBContext sqlServer, IConfiguration configuration)
        {
            _sqlServerDbContext = sqlServer;
            _configuration = configuration;
            dBWorker = new DBWorker(configuration.GetConnectionString(IConstants.SqlString.SqlServerString));
        }
        public int createRoomFeedback(FeedbackRoom feedback)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlCreateFeedbackRoom;
                sqlParameters.Add(new SqlParameter("@number_rating_conveniences", feedback.numberRatingConveniences));
                sqlParameters.Add(new SqlParameter("@number_rating_Interior", feedback.numberRatingInterior));
                sqlParameters.Add(new SqlParameter("@number_rating_employee", feedback.numberRatingEmployee));
                sqlParameters.Add(new SqlParameter("@number_rating_service", feedback.numberRatingService));
                sqlParameters.Add(new SqlParameter("@number_rating_hygiene", feedback.numberRatingHygiene));
                sqlParameters.Add(new SqlParameter("@number_rating_view", feedback.numberRatingView));
                sqlParameters.Add(new SqlParameter("@content_rating", feedback.contentRating));
                sqlParameters.Add(new SqlParameter("@date_time", Common.ConvertUTCDateTime().ToString("yyyy-MM-dd")));
                sqlParameters.Add(new SqlParameter("@order_id", feedback.orderID));
                dBWorker.ExecuteNonQuery(commandText, sqlParameters.ToArray());
                return ErrorCodeResponse.SUCCESS_CODE;
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return ErrorCodeResponse.FAIL_CODE;
            }
        }
        public int createBikeFeedback(FeedbackService feedbackService)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlCreateFeedbackBike;
                sqlParameters.Add(new SqlParameter("@rating_number", feedbackService.ratingNumber));
                sqlParameters.Add(new SqlParameter("@content_rating", feedbackService.contentRating));
                sqlParameters.Add(new SqlParameter("@order_id", feedbackService.orderID));
                dBWorker.ExecuteNonQuery(commandText, sqlParameters.ToArray());
                return ErrorCodeResponse.SUCCESS_CODE;
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return ErrorCodeResponse.FAIL_CODE;
            }
        }

        public int createFoodFeedback(FeedbackService feedbackService)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlCreateFeedbackFood;
                sqlParameters.Add(new SqlParameter("@rating_number", feedbackService.ratingNumber));
                sqlParameters.Add(new SqlParameter("@content_rating", feedbackService.contentRating));
                sqlParameters.Add(new SqlParameter("@order_id", feedbackService.orderID));
                dBWorker.ExecuteNonQuery(commandText, sqlParameters.ToArray());
                return ErrorCodeResponse.SUCCESS_CODE;
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return ErrorCodeResponse.FAIL_CODE;
            }
        }
        public List<FeedbackRoom> getRatingRoomByRatingID(int ratingID)
        {
            List<FeedbackRoom> feedbackList = new List<FeedbackRoom>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetFeedbackRoomByRatingID;
                sqlParameters.Add(new SqlParameter("@rating_id", ratingID));
                feedbackList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<FeedbackRoom>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return feedbackList;
        }

        public List<FeedbackService> getRatingFoodByRatingID(int ratingID)
        {
            List<FeedbackService> feedbackList = new List<FeedbackService>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetFeedbackFoodByRatingID;
                sqlParameters.Add(new SqlParameter("@rating_id", ratingID));
                feedbackList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<FeedbackService>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return feedbackList;
        }

        public List<FeedbackService> getRatingBikeByRatingID(int ratingID)
        {
            List<FeedbackService> feedbackList = new List<FeedbackService>();
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                string commandText = _sqlGetFeedbackBikeByRatingID;
                sqlParameters.Add(new SqlParameter("@rating_id", ratingID));
                feedbackList = dBWorker.GetDataTable(commandText, sqlParameters.ToArray()).ToList<FeedbackService>();
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
            }
            return feedbackList;
        }

    }
}
