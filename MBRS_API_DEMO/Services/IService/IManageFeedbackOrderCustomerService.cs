using MBRS_API.Models;

namespace MBRS_API.Services.IService
{
    public interface IManageFeedbackOrderCustomerService
    {
        public int createRoomFeedback(FeedbackRoom feedback);
        public List<FeedbackRoom> getRatingRoomByRatingID(int ratingID);

        public int createServiceFeedback(FeedbackService feedbackService);

        public List<FeedbackService> getRatingServiceByRatingID(int ratingID , int selectedOption);

    }
}
