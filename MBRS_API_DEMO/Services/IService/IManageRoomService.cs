using MBRS_API.Models;
using MBRS_API.Models.ViewModels;

namespace MBRS_API.Services.IService
{
    public interface IManageRoomService
    {
        public int deleteRoom(int roomID);
        public List<RoomViewModel> getAllRoom();

        public List<TypeRoom> getAllTypeRoom();
        public List<Floor> getAllFloor();

        public int createRoom(RoomInformation roomInformation);

        public List<RoomViewModel> getAllRoomWithFilter(string filterName, string filterValue);

        public List<TypeRoom> getDetailInformationRoom(int typeRoomID);

        public List<RoomViewModel> getTheRoomInformation(int roomID);

        public int updateTheRoom(RoomInformation roomInformation);
    }
}



