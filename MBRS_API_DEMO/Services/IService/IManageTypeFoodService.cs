using MBRS_API.Models;

namespace MBRS_API.Services.IService
{
    public interface IManageTypeFoodService
    {
        public List<TypeFood> getAllTypeFood();
        public List<TypeFood> getAllTypeFoodWithFilter(string filterName, string filterValue);
        public int deleteTypeFood(int typeFoodID);
        public int updateTheTypeFood(TypeFood typeFood);
        public List<TypeFood> getTypeFoodInformation(int typeFoodID);
        public int createTypeFood(TypeFood typeFood);
    }
}
