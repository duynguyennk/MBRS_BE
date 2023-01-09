using MBRS_API.Models;
using MBRS_API.Models.ViewModels;

namespace MBRS_API.Services.IService
{
    public interface IManageFoodService
    {
        public List<FoodViewModel> getAllFood();
        public List<FoodViewModel> getAllFoodWithFilter(string filterName, string filterValue);
        public int deleteFood(int foodID);
        public int updateTheFood(Food food);
        public List<Food> getFoodInformation(int foodID);
        public List<TypeFood> getAllTypeFood();
        public int createFood(Food food);
        public int updateImageFood(ItemImage itemImage);
    }
}
