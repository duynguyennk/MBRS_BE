using FakeItEasy;
using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.Service;
using Microsoft.Extensions.Configuration;

namespace MBRS_API.Tests.Services
{
    public class ManageFoodService_Test
    {
        private readonly IManageFoodRepository _manageFoodRepository;
        private readonly IConfiguration _configuration;
        private readonly ManageFoodService _manageFoodService;

        public ManageFoodService_Test()
        {
            _configuration = A.Fake<IConfiguration>();
            _manageFoodRepository = A.Fake<IManageFoodRepository>();
            _manageFoodService = new ManageFoodService(_configuration,_manageFoodRepository);
        }

        [Fact]
        public void ManageFoodService_GetAllFood_ReturnTotalRecord()
        {
            var food = A.Fake<List<FoodViewModel>>();
            food.Add(new FoodViewModel{ foodCode = "B03", foodID = 1, foodName = "Bim bim", price = 5000, quantity = 5, typeFoodID = 3, typeFoodName = "Đồ ăn nhanh" });
            A.CallTo(()=> _manageFoodRepository.getAllFood() ).Returns(food);
            var result = _manageFoodService.getAllFood();
            Assert.Equal(food.Count, result.Count);
        }

        [Fact]
        public void ManageFoodService_UpdateTheFoodByID_ReturnSuccess()
        {
            var food = A.Fake<Food>();
            food = new Food { foodCode = "B03", foodID = 1, foodName = "Bim bim",  price = 5000, quantity = 5, typeFoodID = 3 };
            A.CallTo(() => _manageFoodRepository.updateTheFood(food)).Returns(1);
            var result = _manageFoodService.updateTheFood(food);
            Assert.Equal(1,result);
        }

        [Fact]
        public void ManageFoodService_DeleteFoodByID_ReturnSuccess()
        {
            var food = A.Fake<Food>();
            food = new Food { foodCode = "B03", foodID = 1, foodName = "Bim bim", price = 5000, quantity = 5, typeFoodID = 3 };
            A.CallTo(() => _manageFoodRepository.deleteFood(food.foodID)).Returns(1);
            var result = _manageFoodService.deleteFood(food.foodID);
            Assert.Equal(1, result);
        }

        [Fact]
        public void ManageFoodService_CreateFood_ReturnSuccess()
        {
            var food = A.Fake<Food>();
            food = new Food { foodCode = "B03", foodID = 1, foodName = "Bim bim", price = 5000, quantity = 5, typeFoodID = 3 };
            A.CallTo(() => _manageFoodRepository.createFood(food)).Returns(1);
            var result = _manageFoodService.createFood(food);
            Assert.Equal(1, result);
        }

        [Fact]
        public void ManageFoodService_GetAllFoodWithFilter_ReturntotalRecordWithFilter()
        {
            var food = A.Fake<List<FoodViewModel>>();
            var FoodViewModel = new FoodViewModel { foodCode = "B03", foodID = 1, foodName = "Bim bim", price = 5000, quantity = 5, typeFoodID = 3, typeFoodName = "Đồ ăn nhanh" };
            food.Add(FoodViewModel);
            A.CallTo(() => _manageFoodRepository.getAllFoodWithFilter(FoodViewModel.foodCode,"B03")).Returns(food);
            var result = _manageFoodService.getAllFoodWithFilter(FoodViewModel.foodCode, "B03");
            Assert.Equal(food.Count, result.Count);
        }

        [Fact]
        public void ManageFoodService_GetAllTypeFood_ReturntotalRecord()
        {
            var food = A.Fake<List<TypeFood>>();
            var FoodViewModel = new TypeFood { typeFoodID = 1,typeFoodCode = "F01",typeFoodName = "Đồ ăn nhanh"};
            food.Add(FoodViewModel);
            A.CallTo(() => _manageFoodRepository.getAllTypeFood()).Returns(food);
            var result = _manageFoodService.getAllTypeFood();
            Assert.Equal(food.Count, result.Count);
        }

        [Fact]
        public void ManageFoodService_GetFoodInformationByID_ReturnSuccess()
        {
            var listFood = A.Fake<List<Food>>();
            var food = new Food { foodCode = "B03", foodID = 1, foodName = "Bim bim", price = 5000, quantity = 5, typeFoodID = 3 };
            listFood.Add(food);
            A.CallTo(() => _manageFoodRepository.getFoodInformation(food.foodID)).Returns(listFood);
            var result = _manageFoodService.getFoodInformation(food.foodID);
            Assert.Equal(listFood.Count, result.Count);
        }
    }
}
