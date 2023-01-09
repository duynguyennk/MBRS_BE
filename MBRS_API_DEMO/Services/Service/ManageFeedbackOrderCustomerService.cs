using MBRS_API.Models;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.IService;

namespace MBRS_API.Services.Service
{
    public class ManageFeedbackOrderCustomerService : IManageFeedbackOrderCustomerService
    {
        private readonly IConfiguration _configuration;
        private readonly IManageFeedbackOrderCustomerRepository _manageFeedbackCustomerRepository;

        public ManageFeedbackOrderCustomerService(IConfiguration configuration, IManageFeedbackOrderCustomerRepository manageFeedbackCustomerRepository)
        {
            this._configuration = configuration;
            _manageFeedbackCustomerRepository = manageFeedbackCustomerRepository;
        }

        public int createRoomFeedback(FeedbackRoom feedback)
        {
            return _manageFeedbackCustomerRepository.createRoomFeedback(feedback);
        }

        public int createServiceFeedback(FeedbackService feedbackService)
        {
            if (feedbackService.selectedOption == 2)
            {
                return _manageFeedbackCustomerRepository.createFoodFeedback(feedbackService);
            }
            else
            {
                return _manageFeedbackCustomerRepository.createBikeFeedback(feedbackService);
            }
        }

        public List<FeedbackRoom> getRatingRoomByRatingID(int ratingID)
        {
            return _manageFeedbackCustomerRepository.getRatingRoomByRatingID(ratingID);
        }

        public List<FeedbackService> getRatingServiceByRatingID(int ratingID, int selectedOption)
        {
            if (selectedOption == 2)
            {
                return _manageFeedbackCustomerRepository.getRatingFoodByRatingID(ratingID);
            }
            else
            {
                return _manageFeedbackCustomerRepository.getRatingBikeByRatingID(ratingID);
            }
        }
    }
}
