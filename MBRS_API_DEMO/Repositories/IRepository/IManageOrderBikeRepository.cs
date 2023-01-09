using MBRS_API.Models;

namespace MBRS_API.Repositories.IRepository
{
    public interface IManageOrderBikeRepository
    {
        public List<OrderBike> getAllOrderBike();
        public List<OrderBike> getAllOrderBikeFilter(string filterName, string filterValue);

        public int deleteOrderBike(int orderBikeID);

        public int updateStatusBike(StatusBike statusBike);
    }
}
