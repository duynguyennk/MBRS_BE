using MBRS_API.Models;
using MBRS_API.Models.ViewModels;

namespace MBRS_API.Repositories.IRepository
{
    public interface IViewOrderForCustomerRepository
    {
        public List<CustomerOrderRoom> getAllOrderRoom(int customerID);
        public List<CustomerViewModel> getCustomerInformationByID(int accountID);

        public List<CustomerOrderFood> getAllOrderFood(int customerID);

        public List<CustomerOrderBike> getAllOrderBike(int customerID);

        public int cancelOrderRoom(CancelOrderRoomCustomer cancelOrderRoomCustomer);
    }
}
