using MBRS_API.Models.ViewModels;

namespace MBRS_API.Repositories.IRepository
{
    public interface ICustomerAccountRepository
    {
        public int registerCustomerAccount(CustomerViewModel customerViewModel);
        public List<CustomerViewModel> getCustomerInformationByID(int accountID);

        public int updateCustomerAccount(CustomerViewModel customerViewModel, int customerID, int accountID);
        public int checkDuplicateUserName(string userName);
        public int checkDuplicateEmail(string email);
        public int checkDuplicateIdentityNumber(string identityNumber);
    }
}
