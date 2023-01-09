using MBRS_API.Models;
using MBRS_API_DEMO.Models;
using MBRS_API_DEMO.Models.ViewModels;

namespace MBRS_API_DEMO.Repositories.IRepository
{
    public interface IManageEmployeeAccountRepository
    {
        public List<Employee> getAllEmployeeAccount();
        public int deleteEmloyeeByID(int employeeID, int accountID);

        public int createEmployee(EmployeeViewModel employeeViewModel);

        public List<Employee> getAllEmployeeAccountWithFilter(string filterName, string filterValue);

        public int updateEmployeeAccount(EmployeeViewModel employeeViewModel, int employeeID, int accountID);
        public List<EmployeeViewModel> getEmployeeInformationToUpdateByID(int employeeID);

        public List<Department> getListDepartment();
        public int checkDuplicateUserName(string userName);
        public int checkDuplicateEmail(string email);
        public int checkDuplicateIdentityNumber(string cccd);
    }
}



