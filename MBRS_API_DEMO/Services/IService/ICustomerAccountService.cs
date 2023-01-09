using MBRS_API.Models.ViewModels;

namespace MBRS_API.Services.IService
{
    public interface ICustomerAccountService
    {
        public int registerCustomerAccount(CustomerViewModel customerViewModel);
        public List<CustomerViewModel> getCustomerInformationByID(int accountID);
        public int updateCustomerAccount(CustomerViewModel customerViewModel, int customerID, int accountID);
    }
}
