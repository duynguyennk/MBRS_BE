using MBRS_API.Models;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.IService;

namespace MBRS_API.Services.Service
{
    public class ManageTypeRoomService : IManageTypeRoomService
    {
        private readonly IConfiguration _configuration;
        private readonly IManageTypeRoomRepository _manageTypeRoomRepository;

        public ManageTypeRoomService(IConfiguration configuration, IManageTypeRoomRepository manageTypeRoomRepository)
        {
            this._configuration = configuration;
            _manageTypeRoomRepository = manageTypeRoomRepository;
        }
        public int createTypeRoom(TypeRoom typeRoom)
        {
            if(_manageTypeRoomRepository.checkDuplicateTypeRoomCode(typeRoom.typeRoomCode, typeRoom.typeRoomID) == 0)
            {
                if(_manageTypeRoomRepository.checkDuplicateTypeRoomName(typeRoom.typeRoomName, typeRoom.typeRoomID)==0)
                {

                    return _manageTypeRoomRepository.createTypeRoom(typeRoom);
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

        public int deleteImageTypeRoom(int position, int typeRoomID)
        {
            return _manageTypeRoomRepository.deleteImageTypeRoom(position, typeRoomID);
        }

        public int deleteTypeRoom(int typeRoomID, int listUtilitiesID)
        {
            return _manageTypeRoomRepository.deleteTypeRoom(typeRoomID, listUtilitiesID);
        }

        public List<TypeRoom> getAllTypeRoom()
        {
            return _manageTypeRoomRepository.getAllTypeRoom();
        }

        public List<TypeRoom> getAllTypeRoomWithFilter(string filterName, string filterValue)
        {
            return _manageTypeRoomRepository.getAllTypeRoomWithFilter(filterName, filterValue);
        }

        public List<TypeRoom> getTypeRoomInformation(int typeRoomID)
        {
            return _manageTypeRoomRepository.getTypeRoomInformation(typeRoomID);
        }

        public int updateImageTypeRoom(List<ItemImage> itemImage)
        {

            return _manageTypeRoomRepository.updateImageTypeRoom(itemImage);
        }

        public int updateAImageTypeRoom(ImageTypeRoom imageTypeRoom)
        {
            return _manageTypeRoomRepository.updateAImageTypeRoom(imageTypeRoom);
        }

        public int updateTheTypeRoom(TypeRoom typeRoom)
        {
            if (_manageTypeRoomRepository.checkDuplicateTypeRoomCode(typeRoom.typeRoomCode, typeRoom.typeRoomID) == 0)
            {
                if (_manageTypeRoomRepository.checkDuplicateTypeRoomName(typeRoom.typeRoomName, typeRoom.typeRoomID) == 0)
                {

                    return _manageTypeRoomRepository.updateTheTypeRoom(typeRoom);
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
