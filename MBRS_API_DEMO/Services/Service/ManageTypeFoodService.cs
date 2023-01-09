using MBRS_API.Models;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.IService;

namespace MBRS_API.Services.Service
{
    public class ManageTypeFoodService : IManageTypeFoodService
    {
        private readonly IConfiguration _configuration;
        private readonly IManageTypeFoodRepository _manageTypeFoodRepository;

        public ManageTypeFoodService(IConfiguration configuration, IManageTypeFoodRepository manageTypeFoodRepository)
        {
            this._configuration = configuration;
            _manageTypeFoodRepository = manageTypeFoodRepository;
        }
        public int createTypeFood(TypeFood typeFood)
        {
            return _manageTypeFoodRepository.createTypeFood(typeFood);
        }

        public int deleteTypeFood(int typeFoodID)
        {
            return _manageTypeFoodRepository.deleteTypeFood(typeFoodID);
        }

        public List<TypeFood> getAllTypeFood()
        {
            return _manageTypeFoodRepository.getAllTypeFood();
        }

        public List<TypeFood> getAllTypeFoodWithFilter(string filterName, string filterValue)
        {
            return _manageTypeFoodRepository.getAllTypeFoodWithFilter(filterName, filterValue);
        }

        public List<TypeFood> getTypeFoodInformation(int typeFoodID)
        {
            return _manageTypeFoodRepository.getTypeFoodInformation(typeFoodID);
        }

        public int updateTheTypeFood(TypeFood typeFood)
        {
            return _manageTypeFoodRepository.updateTheTypeFood(typeFood);
        }
    }
}
