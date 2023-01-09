using MBRS_API.Models;

namespace MBRS_API.Repositories.IRepository
{
    public interface IManageTypeRoomRepository
    {
        public List<TypeRoom> getAllTypeRoom();
        public List<TypeRoom> getAllTypeRoomWithFilter(string filterName, string filterValue);
        public int deleteTypeRoom(int typeRoomID, int listUtilitiesID);
        public int updateTheTypeRoom(TypeRoom typeRoom);
        public List<TypeRoom> getTypeRoomInformation(int typeRoomID);
        public int createTypeRoom(TypeRoom typeRoom);

        public int updateImageTypeRoom(List<ItemImage> itemImage);

        public int deleteImageTypeRoom(int position, int typeRoomID);
        public int updateAImageTypeRoom(ImageTypeRoom imageTypeRoom);
        public int checkDuplicateTypeRoomCode(string typeRoomCode, int typeRoomID);
        public int checkDuplicateTypeRoomName(string typeRoomName, int typeRoomID);

    }
}
