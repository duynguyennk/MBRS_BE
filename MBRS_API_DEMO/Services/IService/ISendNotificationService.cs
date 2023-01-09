using MBRS_API.Models;

namespace MBRS_API.Services.IService
{
    public interface ISendNotificationService
    {
        public List<NotificationBell> getAllOrderNotificationReceptionist();
        public int UpdateOrderNotificationReceptionist(int notificationID);
    }
}
