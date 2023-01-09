using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.IService;

namespace MBRS_API.Services.Service
{
    public class ManageCustomerAccountService : IManageCustomerAccountService
    {
        private readonly IConfiguration _configuration;
        private readonly IManageCustomerAccountRepository _manageCustomerAccountRepository;

        public ManageCustomerAccountService(IConfiguration configuration, IManageCustomerAccountRepository manageCustomerAccountRepository)
        {
            this._configuration = configuration;
            _manageCustomerAccountRepository = manageCustomerAccountRepository;
        }

        public int createCustomer(CustomerViewModel customerViewModel)
        {
            if (_manageCustomerAccountRepository.checkDuplicateUserName(customerViewModel.userName) == 0)
            {
                if (_manageCustomerAccountRepository.checkDuplicateEmail(customerViewModel.email) == 0)
                {
                    if (_manageCustomerAccountRepository.checkDuplicateIdentityNumber(customerViewModel.identifyNumber) == 0)
                    {
                        return _manageCustomerAccountRepository.createCustomer(customerViewModel);
                    }
                    else
                    {
                        return -4;
                    }
                }
                else
                {
                    return -3;
                }
            }
            else
            {
                return -2;
            }
        }

        public int deleteCustomerByID(int customerID, int accountID)
        {
            return _manageCustomerAccountRepository.deleteCustomerByID(customerID, accountID);
        }

        public List<Customer> getAllCustomerAccount()
        {
            return _manageCustomerAccountRepository.getAllCustomerAccount();
        }

        public List<CustomerViewModel> getCustomerInformationToUpdateByID(int customerID)
        {
            return _manageCustomerAccountRepository.getCustomerInformationToUpdateByID(customerID);
        }

        public int updateCustomerAccount(CustomerViewModel customerViewModel, int customerID, int accountID)
        {
            return _manageCustomerAccountRepository.updateCustomerAccount(customerViewModel, customerID, accountID);
        }

        public List<Customer> getAllCustomerAccountWithFilter(string filterName, string filterValue)
        {
            return _manageCustomerAccountRepository.getAllCustomerAccountWithFilter(filterName, filterValue);
        }
    }
}
