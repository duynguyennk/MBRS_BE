using MBRS_API.Models;

namespace MBRS_API.Services.IService
{
    public interface IManageOrderRoomService
    {
        public List<OrderRoom> getAllOrderRoom();
        public List<OrderRoom> getAllOrderRoomFilter(string filterName, string filterValue);

        public int deleteOrderRoom(int orderID);
        public int updateStatusPayment(int orderID);
        public int completedBackPayment(int orderID);
    }
}
