using MBRS_API.Models;
using MBRS_API.Models.ViewModels;

namespace MBRS_API.Services.IService
{
    public interface IViewStatusRoomService
    {
        public List<StatusRoomViewModel> getAllRoom(DateTime selectDate);
        public List<Floor> getAllFloor();

        public NumberRoomStatusViewModel getNumberOfRoomStatus(DateTime selectDate);
        public int updateStatusRoom(int valueStatus, int orderID);
    }
}
