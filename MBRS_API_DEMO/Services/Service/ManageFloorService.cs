using MBRS_API.Models;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.IService;

namespace MBRS_API.Services.Service
{
    public class ManageFloorService : IManageFloorService
    {
        private readonly IConfiguration _configuration;
        private readonly IManageFloorRepository _manageFloorRepository;

        public ManageFloorService(IConfiguration configuration, IManageFloorRepository manageFloorRepository)
        {
            this._configuration = configuration;
            _manageFloorRepository = manageFloorRepository;
        }
        public int createFloor(Floor floor)
        {
            return _manageFloorRepository.createFloor(floor);
        }

        public int deleteFloor(int floorID)
        {
            return _manageFloorRepository.deleteFloor(floorID);
        }

        public List<Floor> getAllFloor()
        {
            return _manageFloorRepository.getAllFloor();
        }

        public List<Floor> getAllFloorWithFilter(string filterName, string filterValue)
        {
            return _manageFloorRepository.getAllFloorWithFilter(filterName, filterValue);
        }

        public List<Floor> getFloorInformation(int floorID)
        {
            return _manageFloorRepository.getFloorInformation(floorID);
        }

        public int updateTheFloor(Floor floor)
        {
            return _manageFloorRepository.updateTheFloor(floor);
        }
    }
}
