using MBRS_API.Models;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.IService;

namespace MBRS_API.Services.Service
{
    public class ManageTypeBikeService : IManageTypeBikeService
    {
        private readonly IConfiguration _configuration;
        private readonly IManageTypeBikeRepository _manageTypeBikeRepository;

        public ManageTypeBikeService(IConfiguration configuration, IManageTypeBikeRepository manageTypeBikeRepository)
        {
            this._configuration = configuration;
            _manageTypeBikeRepository = manageTypeBikeRepository;
        }
        public int createTypeBike(TypeBike typeBike)
        {
            return _manageTypeBikeRepository.createTypeBike(typeBike);
        }

        public int deleteTypeBike(int typeBikeID)
        {
            return _manageTypeBikeRepository.deleteTypeBike(typeBikeID);
        }

        public List<TypeBike> getAllTypeBike()
        {
            return _manageTypeBikeRepository.getAllTypeBike();
        }

        public List<TypeBike> getAllTypeBikeWithFilter(string filterName, string filterValue)
        {
            return _manageTypeBikeRepository.getAllTypeBikeWithFilter(filterName, filterValue);
        }

        public List<TypeBike> getTypeBikeInformation(int typeBikeID)
        {
            return _manageTypeBikeRepository.getTypeBikeInformation(typeBikeID);
        }

        public int updateImageTypeBike(ItemImage itemImage)
        {
            return _manageTypeBikeRepository.updateImageTypeBike(itemImage);
        }

        public int updateTheTypeBike(TypeBike typeBike)
        {
            return _manageTypeBikeRepository.updateTheTypeBike(typeBike);
        }
    }
}
