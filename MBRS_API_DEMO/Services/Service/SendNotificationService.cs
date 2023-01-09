using MBRS_API.Models;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.IService;

namespace MBRS_API.Services.Service
{
    public class SendNotificationService : ISendNotificationService
    {
        private readonly IConfiguration _configuration;
        private readonly ISendNotificationRepository _sendNotificationRepository;

        public SendNotificationService(IConfiguration configuration, ISendNotificationRepository sendNotificationRepository)
        {
            this._configuration = configuration;
            _sendNotificationRepository = sendNotificationRepository;
        }

        public List<NotificationBell> getAllOrderNotificationReceptionist()
        {
            return _sendNotificationRepository.getAllOrderNotificationReceptionist();
        }

        public int UpdateOrderNotificationReceptionist(int notificationID)
        {
            return _sendNotificationRepository.UpdateOrderNotificationReceptionist(notificationID);
        }
    }
}
