using MBRS_API.Models;

namespace MBRS_API.Repositories.IRepository
{
    public interface ISendNotificationRepository
    {
        public List<NotificationBell> getAllOrderNotificationReceptionist();
        public int UpdateOrderNotificationReceptionist(int notificationID);

    }
}
