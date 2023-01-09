using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.IService;

namespace MBRS_API.Services.Service
{
    public class ManageCarAirportService : IManageCarAirportService
    {
        private readonly IConfiguration _configuration;
        private readonly IManageCarAirportRepository _manageCarAirportRepository;

        public ManageCarAirportService(IConfiguration configuration, IManageCarAirportRepository manageCarAirportRepository)
        {
            this._configuration = configuration;
            _manageCarAirportRepository = manageCarAirportRepository;
        }
        public List<CarAirportViewModel> getAllCar()
        {
            return _manageCarAirportRepository.getAllCar();
        }
        public int deleteCar(int carID)
        {
            return _manageCarAirportRepository.deleteCar(carID);
        }
        public List<CarAirportViewModel> getAllCarWithFilter(string filterName, string filterValue)
        {
            return _manageCarAirportRepository.getAllCarWithFilter(filterName, filterValue);
        }
        public int updateTheCar(CarAirport carAirport)
        {
            return _manageCarAirportRepository.updateTheCar(carAirport);
        }
        public List<TypeCarAirport> getAllTypeCar()
        {
            return _manageCarAirportRepository.getAllTypeCar();
        }

        public List<TypeCarAirport> getDetailInformationCar(int typeCarID)
        {
            return _manageCarAirportRepository.getDetailInformationCar(typeCarID);
        }

        public int createCar(CarAirport carAirport)
        {
            return _manageCarAirportRepository.createCar(carAirport);
        }

        public List<CarAirport> getTheCarInformation(int carID)
        {
            return _manageCarAirportRepository.getTheCarInformation(carID);
        }
    }
}
