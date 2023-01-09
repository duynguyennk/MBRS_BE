using MBRS_API.Models;
using MBRS_API.Models.ViewModels;

namespace MBRS_API.Repositories.IRepository
{
    public interface IViewStatusRoomRepository
    {
        public List<StatusRoomViewModel> getAllRoom(DateTime selectDate);
        public List<Floor> getAllFloor();

        public int getCountRoomCheckIn(DateTime selectDate);

        public int getCountRoomEmpty(DateTime selectDate);

        public int getCountRoomHaveOrder(DateTime selectDate);

        public int updateStatusRoom(int valueStatus, int orderID);
    }
}
