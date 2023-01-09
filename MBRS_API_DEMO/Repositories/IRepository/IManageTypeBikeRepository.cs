using MBRS_API.Models;

namespace MBRS_API.Repositories.IRepository
{
    public interface IManageTypeBikeRepository
    {
        public List<TypeBike> getAllTypeBike();
        public List<TypeBike> getAllTypeBikeWithFilter(string filterName, string filterValue);
        public int deleteTypeBike(int typeBikeID);
        public int updateTheTypeBike(TypeBike typeBike);
        public List<TypeBike> getTypeBikeInformation(int typeBikeID);
        public int createTypeBike(TypeBike typeBike);

        public int updateImageTypeBike(ItemImage itemImage);
    }
}
