using MBRS_API.Models;
using MBRS_API.Models.ViewModels;

namespace MBRS_API.Repositories.IRepository
{
    public interface IManageCustomerAccountRepository
    {
        public List<Customer> getAllCustomerAccount();
        public int deleteCustomerByID(int customerID, int accountID);

        public int createCustomer(CustomerViewModel customerViewModel);

        public int updateCustomerAccount(CustomerViewModel customerViewModel, int customerID, int accountID);
        public List<CustomerViewModel> getCustomerInformationToUpdateByID(int customerID);
        public List<Customer> getAllCustomerAccountWithFilter(string filterName, string filterValue);
        public int checkDuplicateUserName(string userName);
        public int checkDuplicateEmail(string email);
        public int checkDuplicateIdentityNumber(string identityNumber);
    }
}
