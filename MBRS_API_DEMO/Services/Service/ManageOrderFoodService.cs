using MBRS_API.Models;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.IService;

namespace MBRS_API.Services.Service
{
    public class ManageOrderFoodService : IManageOrderFoodService
    {
        private readonly IConfiguration _configuration;
        private readonly IManageOrderFoodRepository _manageOrderFoodRepository;

        public ManageOrderFoodService(IConfiguration configuration, IManageOrderFoodRepository manageOrderFoodRepository)
        {
            this._configuration = configuration;
            _manageOrderFoodRepository = manageOrderFoodRepository;
        }
        public int deleteOrderFood(int orderFoodID)
        {
            return _manageOrderFoodRepository.deleteOrderFood(orderFoodID);
        }

        public List<OrderFood> getAllOrderFood()
        {
            return _manageOrderFoodRepository.getAllOrderFood();
        }

        public List<OrderFood> getAllOrderFoodFilter(string filterName, string filterValue)
        {
            return _manageOrderFoodRepository.getAllOrderFoodFilter(filterName, filterValue);
        }

        public int updateStatusFood(StatusFood statusFood)
        {
            return _manageOrderFoodRepository.updateStatusFood(statusFood);
        }
    }
}