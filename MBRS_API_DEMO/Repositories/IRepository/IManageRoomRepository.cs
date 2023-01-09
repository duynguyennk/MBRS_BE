using MBRS_API.Models;
using MBRS_API.Models.ViewModels;

namespace MBRS_API.Repositories.IRepository
{
    public interface IManageRoomRepository
    {
        public List<RoomViewModel> getAllRoom();
        public int deleteRoom(int roomID);

        public List<TypeRoom> getAllTypeRoom();

        public List<Floor> getAllFloor();

        public int createRoom(RoomInformation roomInformation);

        public List<RoomViewModel> getAllRoomWithFilter(string filterName, string filterValue);

        public List<TypeRoom> getDetailInformationRoom(int typeRoomID);

        public List<RoomViewModel> getTheRoomInformation(int roomID);

        public int updateTheRoom(RoomInformation roomInformation);
        public int checkDuplicateRoomCode(string roomCode, int roomID);
        public int checkDuplicateRoomName(string roomName, int roomID);
    }
}



