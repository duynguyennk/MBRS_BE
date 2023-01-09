using MBRS_API.Models;

namespace MBRS_API.Services.IService
{
    public interface IManageOrderFoodService
    {
        public int updateStatusFood(StatusFood statusFood);
        public List<OrderFood> getAllOrderFood();
        public List<OrderFood> getAllOrderFoodFilter(string filterName, string filterValue);

        public int deleteOrderFood(int orderFoodID);
    }
}
