using MBRS_API.Models;
using MBRS_API_DEMO.Models;
using MBRS_API_DEMO.Models.ViewModels;

namespace MBRS_API_DEMO.Services.IService
{
    public interface IManageEmployeeAccountService
    {
        public List<Employee> getAllEmployeeAccount();
        public int deleteEmloyeeByID(int employeeID, int accountID);
        public int createEmployee(EmployeeViewModel employeeViewModel);
        public int updateEmployeeAccount(EmployeeViewModel employeeViewModel, int employeeID, int accountID);
        public List<EmployeeViewModel> getEmployeeInformationToUpdateByID(int employeeID);
        public List<Department> getListDepartment();

        public List<Employee> getAllEmployeeAccountWithFilter(string filterName, string filterValue);

    }
}


