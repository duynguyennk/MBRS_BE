using MBRS_API.Models;
using MBRS_API_DEMO.Models;
using MBRS_API_DEMO.Models.ViewModels;
using MBRS_API_DEMO.Repositories.IRepository;
using MBRS_API_DEMO.Services.IService;

namespace MBRS_API_DEMO.Services.Service
{
    public class ManageEmployeeAccountService : IManageEmployeeAccountService
    {
        private readonly IConfiguration _configuration;
        private readonly IManageEmployeeAccountRepository _manageAccountRepository;

        public ManageEmployeeAccountService(IConfiguration configuration, IManageEmployeeAccountRepository manageAccountRepository)
        {
            this._configuration = configuration;
            _manageAccountRepository = manageAccountRepository;
        }

        public int createEmployee(EmployeeViewModel employeeViewModel)
        {
            if (_manageAccountRepository.checkDuplicateUserName(employeeViewModel.userName) == 0)
            {
                if (_manageAccountRepository.checkDuplicateEmail(employeeViewModel.email) == 0)
                {
                    if (_manageAccountRepository.checkDuplicateIdentityNumber(employeeViewModel.identifyNumber) == 0)
                    {
                        return _manageAccountRepository.createEmployee(employeeViewModel);
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

        public int deleteEmloyeeByID(int employeeID, int accountID)
        {
            return _manageAccountRepository.deleteEmloyeeByID(employeeID, accountID);
        }

        public List<Employee> getAllEmployeeAccount()
        {
            return _manageAccountRepository.getAllEmployeeAccount();
        }

        public int updateEmployeeAccount(EmployeeViewModel employeeViewModel, int employeeID, int accountID)
        {
            return _manageAccountRepository.updateEmployeeAccount(employeeViewModel, employeeID, accountID);
        }

        public List<EmployeeViewModel> getEmployeeInformationToUpdateByID(int employeeID)
        {
            return _manageAccountRepository.getEmployeeInformationToUpdateByID(employeeID);
        }

        public List<Department> getListDepartment()
        {
            return _manageAccountRepository.getListDepartment();
        }

        public List<Employee> getAllEmployeeAccountWithFilter(string filterName, string filterValue)
        {
            return _manageAccountRepository.getAllEmployeeAccountWithFilter(filterName, filterValue);
        }
    }
}



