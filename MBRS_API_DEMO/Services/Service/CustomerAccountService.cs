using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.IService;

namespace MBRS_API.Services.Service
{
    public class CustomerAccountService : ICustomerAccountService
    {
        private readonly IConfiguration _configuration;
        private readonly ICustomerAccountRepository _customerAccountRepository;

        public CustomerAccountService(IConfiguration configuration, ICustomerAccountRepository customerAccountRepository)
        {
            this._configuration = configuration;
            _customerAccountRepository = customerAccountRepository;
        }
        public int registerCustomerAccount(CustomerViewModel customerViewModel)
        {
            if(_customerAccountRepository.checkDuplicateUserName(customerViewModel.userName) == 0)
            {
                if(_customerAccountRepository.checkDuplicateEmail(customerViewModel.email) == 0)
                {
                    if(_customerAccountRepository.checkDuplicateIdentityNumber(customerViewModel.identifyNumber)==0)
                    {
                        return _customerAccountRepository.registerCustomerAccount(customerViewModel);
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
        public List<CustomerViewModel> getCustomerInformationByID(int accountID)
        {
            return _customerAccountRepository.getCustomerInformationByID(accountID);
        }
        public int updateCustomerAccount(CustomerViewModel customerViewModel, int customerID, int accountID)
        {
            return _customerAccountRepository.updateCustomerAccount(customerViewModel, customerID, accountID);
        }
    }
}
