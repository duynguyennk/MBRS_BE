using MBRS_API.Models;

namespace MBRS_API.Repositories.IRepository
{
    public interface IManageFeedbackOrderCustomerRepository
    {
        public int createRoomFeedback(FeedbackRoom feedback);
        public List<FeedbackRoom> getRatingRoomByRatingID(int ratingID);

        public int createFoodFeedback(FeedbackService feedbackService);

        public int createBikeFeedback(FeedbackService feedbackService);
        public List<FeedbackService> getRatingBikeByRatingID(int ratingID);
        public List<FeedbackService> getRatingFoodByRatingID(int ratingID);
    }
}
