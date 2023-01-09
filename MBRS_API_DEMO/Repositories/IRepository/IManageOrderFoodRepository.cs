using MBRS_API.Models;

namespace MBRS_API.Repositories.IRepository
{
    public interface IManageOrderFoodRepository
    {
        public List<OrderFood> getAllOrderFood();
        public List<OrderFood> getAllOrderFoodFilter(string filterName, string filterValue);
        public int updateStatusFood(StatusFood statusFood);
        public int deleteOrderFood(int orderFoodID);
    }
}
