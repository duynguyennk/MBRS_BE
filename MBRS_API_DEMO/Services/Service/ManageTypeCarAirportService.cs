using MBRS_API.Models;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.IService;

namespace MBRS_API.Services.Service
{
    public class ManageTypeCarAirportService : IManageTypeCarAirportService
    {
        private readonly IConfiguration _configuration;
        private readonly IManageTypeCarAirportRepository _manageTypeCarAirportRepository;

        public ManageTypeCarAirportService(IConfiguration configuration, IManageTypeCarAirportRepository manageTypeCarAirportRepository)
        {
            this._configuration = configuration;
            _manageTypeCarAirportRepository = manageTypeCarAirportRepository;
        }
        public int createTypeCarAirport(TypeCarAirport typeCarAirport)
        {
            return _manageTypeCarAirportRepository.createTypeCarAirport(typeCarAirport);
        }

        public int deleteTypeCarAirport(int typeCarAirportID)
        {
            return _manageTypeCarAirportRepository.deleteTypeCarAirport(typeCarAirportID);
        }

        public List<TypeCarAirport> getAllTypeCarAirport()
        {
            return _manageTypeCarAirportRepository.getAllTypeCarAirport();
        }

        public List<TypeCarAirport> getAllTypeCarAirportWithFilter(string filterName, string filterValue)
        {
            return _manageTypeCarAirportRepository.getAllTypeCarAirportWithFilter(filterName, filterValue);
        }

        public List<TypeCarAirport> getTypeCarAirportInformation(int typeCarAirportID)
        {
            return _manageTypeCarAirportRepository.getTypeCarAirportInformation(typeCarAirportID);
        }

        public int updateTheTypeCarAirport(TypeCarAirport typeCarAirport)
        {
            return _manageTypeCarAirportRepository.updateTheTypeCarAirport(typeCarAirport);
        }
    }
}
