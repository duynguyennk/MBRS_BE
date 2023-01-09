using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.IService;

namespace MBRS_API.Services.Service
{
    public class ManageFoodService : IManageFoodService
    {
        private readonly IConfiguration _configuration;
        private readonly IManageFoodRepository _manageFoodRepository;

        public ManageFoodService(IConfiguration configuration, IManageFoodRepository manageFoodRepository)
        {
            this._configuration = configuration;
            _manageFoodRepository = manageFoodRepository;
        }

        public int createFood(Food food)
        {
            if(_manageFoodRepository.checkDuplicateFoodCode(food.foodCode,food.foodID)==0)
            {
                return _manageFoodRepository.createFood(food);
            }
            else
            {
                return -2;
            }    
        }

        public int deleteFood(int foodID)
        {
            return _manageFoodRepository.deleteFood(foodID);
        }

        public List<FoodViewModel> getAllFood()
        {
            return _manageFoodRepository.getAllFood();
        }

        public List<FoodViewModel> getAllFoodWithFilter(string filterName, string filterValue)
        {
            return _manageFoodRepository.getAllFoodWithFilter(filterName, filterValue);
        }

        public List<TypeFood> getAllTypeFood()
        {
            return _manageFoodRepository.getAllTypeFood();
        }

        public List<Food> getFoodInformation(int foodID)
        {
            return _manageFoodRepository.getFoodInformation(foodID);
        }

        public int updateTheFood(Food food)
        {
            if (_manageFoodRepository.checkDuplicateFoodCode(food.foodCode, food.foodID) == 0)
            {
                return _manageFoodRepository.updateTheFood(food);
            }
            else
            {
                return -2;
            }
        }
        public int updateImageFood(ItemImage itemImage)
        {
            return _manageFoodRepository.updateImageFood(itemImage);
        }
    }
}
