using MBRS_API.Models.ViewModels;
using MBRS_API.Models;

namespace MBRS_API.Repositories.IRepository
{
    public interface IManageFloorRepository
    {
        public List<Floor> getAllFloor();
        public List<Floor> getAllFloorWithFilter(string filterName, string filterValue);
        public int deleteFloor(int floorID);
        public int updateTheFloor(Floor floor);
        public List<Floor> getFloorInformation(int floorID);
        public int createFloor(Floor floor);
    }
}
