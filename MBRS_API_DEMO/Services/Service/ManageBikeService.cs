using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.IService;

namespace MBRS_API.Services.Service
{
    public class ManageBikeService : IManageBikeService
    {
        private readonly IConfiguration _configuration;
        private readonly IManageBikeRepository _manageBikeRepository;

        public ManageBikeService(IConfiguration configuration, IManageBikeRepository manageBikeRepository)
        {
            this._configuration = configuration;
            _manageBikeRepository = manageBikeRepository;
        }
        public List<BikeViewModel> getAllBike()
        {
            return _manageBikeRepository.getAllBike();
        }
        public int deleteBike(int bikeID)
        {
            return _manageBikeRepository.deleteBike(bikeID);
        }

        public List<BikeViewModel> getAllBikeWithFilter(string filterName, string filterValue)
        {
            return _manageBikeRepository.getAllBikeWithFilter(filterName, filterValue);
        }

        public List<TypeBike> getAllTypeBike()
        {
            return _manageBikeRepository.getAllTypeBike();
        }
        public List<TypeBike> getDetailInformationBike(int typeBikeID)
        {
            return _manageBikeRepository.getDetailInformationBike(typeBikeID);
        }
        public int createBike(Bike bike)
        {
            return _manageBikeRepository.createBike(bike);
        }

        public List<Bike> getTheBikeInformation(int bikeID)
        {
            return _manageBikeRepository.getTheBikeInformation(bikeID);
        }
        public int updateTheBike(Bike bike)
        {
            return _manageBikeRepository.updateTheBike(bike);
        }
    }
}
