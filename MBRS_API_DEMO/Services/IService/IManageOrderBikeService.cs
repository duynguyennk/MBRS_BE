using MBRS_API.Models;

namespace MBRS_API.Services.IService
{
    public interface IManageOrderBikeService
    {
        public List<OrderBike> getAllOrderBike();
        public List<OrderBike> getAllOrderBikeFilter(string filterName, string filterValue);

        public int deleteOrderBike(int orderBikeID);
        public int updateStatusBike(StatusBike statusBike);
    }
}
