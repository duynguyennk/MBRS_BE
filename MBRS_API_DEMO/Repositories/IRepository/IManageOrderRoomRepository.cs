using MBRS_API.Models;
using MBRS_API.Models.ViewModels;

namespace MBRS_API.Repositories.IRepository
{
    public interface IManageOrderRoomRepository
    {
        public List<OrderRoom> getAllOrderRoom();
        public List<OrderRoom> getAllOrderRoomFilter(string filterName, string filterValue);
        public int deleteOrderRoom(int orderID);
        public int updateStatusPayment(int orderID);

        public int completedBackPayment(int orderID);
    }
}
