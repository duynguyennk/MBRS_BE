using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.IService;

namespace MBRS_API.Services.Service
{
    public class ManageRoomService : IManageRoomService
    {
        private readonly IConfiguration _configuration;
        private readonly IManageRoomRepository _manageRoomRepository;

        public ManageRoomService(IConfiguration configuration, IManageRoomRepository manageRoomRepository)
        {
            this._configuration = configuration;
            _manageRoomRepository = manageRoomRepository;
        }
        public List<RoomViewModel> getAllRoom()
        {
            return _manageRoomRepository.getAllRoom();
        }

        public int deleteRoom(int typeRoomID)
        {
            return _manageRoomRepository.deleteRoom(typeRoomID);
        }

        public List<TypeRoom> getAllTypeRoom()
        {
            return _manageRoomRepository.getAllTypeRoom();
        }

        public List<Floor> getAllFloor()
        {
            return _manageRoomRepository.getAllFloor();
        }

        public int createRoom(RoomInformation roomInformation)
        {
            if(_manageRoomRepository.checkDuplicateRoomCode(roomInformation.roomCode, roomInformation.roomID) == 0)
            {
                if(_manageRoomRepository.checkDuplicateRoomName(roomInformation.roomName, roomInformation.roomID) == 0)
                {
                    return _manageRoomRepository.createRoom(roomInformation);
                }
                else
                {
                    return -3;
                }    
            }
            else
            {
                return -2;
            }    
        }

        public List<RoomViewModel> getAllRoomWithFilter(string filterName, string filterValue)
        {
            return _manageRoomRepository.getAllRoomWithFilter(filterName, filterValue);
        }

        public List<TypeRoom> getDetailInformationRoom(int typeRoomID)
        {
            return _manageRoomRepository.getDetailInformationRoom(typeRoomID);
        }

        public List<RoomViewModel> getTheRoomInformation(int roomID)
        {
            return _manageRoomRepository.getTheRoomInformation(roomID);
        }
        public int updateTheRoom(RoomInformation roomInformation)
        {
            if (_manageRoomRepository.checkDuplicateRoomCode(roomInformation.roomCode, roomInformation.roomID) == 0)
            {
                if (_manageRoomRepository.checkDuplicateRoomName(roomInformation.roomName,roomInformation.roomID) == 0)
                {
                    return _manageRoomRepository.updateTheRoom(roomInformation);
                }
                else
                {
                    return -3;
                }
            }
            else
            {
                return -2;
            }
        }
    }
}



