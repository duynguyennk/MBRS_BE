using FakeItEasy;
using MBRS_API.Models;
using MBRS_API_DEMO.Models;
using MBRS_API_DEMO.Models.ViewModels;
using MBRS_API_DEMO.Repositories.IRepository;
using MBRS_API_DEMO.Services.IService;
using MBRS_API_DEMO.Services.Service;
using Microsoft.Extensions.Configuration;

using System.Globalization;


namespace MBRS_API.Tests.Services
{
    public class ManageEmployeeAccountService_Test
    {
        private IConfiguration _configuration;
        private IManageEmployeeAccountRepository _manageEmployeeAccountRepository;
        private ManageEmployeeAccountService _manageEmployeeAccountService;

        public ManageEmployeeAccountService_Test()
        {
            _configuration = A.Fake<IConfiguration>();
            _manageEmployeeAccountRepository = A.Fake<IManageEmployeeAccountRepository>();
            _manageEmployeeAccountService = new ManageEmployeeAccountService(_configuration, _manageEmployeeAccountRepository);
        }

        [Fact]
        public void ManageEmployeeAccountService_getAllEmployeeAccount_ReturnEqualCount()
        {
            var cus = A.Fake<Employee>();
            cus = new Employee
            {
                accountID = 1,
                dateOfBirth = "27/09/1998",
                fullName = "Dam Nhat",
                identifyNumber = "56456456546",
                phoneNumber = "0912381273",
                userName = "nhatdv15",
                departmentName = "Admin",
                employeeID = 1
            };
            var listCus = A.Fake<List<Employee>>();
            listCus.Add(cus);
            A.CallTo(() => _manageEmployeeAccountRepository.getAllEmployeeAccount()).Returns(listCus);
            var result = _manageEmployeeAccountService.getAllEmployeeAccount();
            Assert.Equal(listCus.Count, result.Count);
        }

        [Fact]
        public void ManageEmployeeAccountService_getAllEmployeeAccountWithFilter_ReturnEqualCount()
        {
            var cus = A.Fake<Employee>();
            cus = new Employee
            {
                accountID = 1,
                dateOfBirth = "27/09/1998",
                fullName = "Dam Nhat",
                identifyNumber = "56456456546",
                phoneNumber = "0912381273",
                userName = "nhatdv15",
                departmentName = "Admin",
                employeeID = 1
            };
            var listCus = A.Fake<List<Employee>>();
            listCus.Add(cus);
            A.CallTo(() => _manageEmployeeAccountRepository.getAllEmployeeAccountWithFilter(cus.phoneNumber,"Số điện thoại")).Returns(listCus);
            var result = _manageEmployeeAccountService.getAllEmployeeAccountWithFilter(cus.phoneNumber, "Số điện thoại");
            Assert.Equal(listCus.Count, result.Count);
        }

        [Fact]
        public void ManageEmployeeAccountService_createEmployee_ReturnSuccess()
        {
            var cus = A.Fake<EmployeeViewModel>();
            cus = new EmployeeViewModel
            {
                accountID = 1,
                dateOfBirth = DateTime.ParseExact("27/09/1998", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                fullName = "Dam Nhat",
                identifyNumber = "56456456546",
                phoneNumber = "0912381273",
                userName = "nhatdv15",
                employeeID = 1,
                departmentID = 3,
                email = "nhatdv15@gmail.com",
                role = "Admin"
            };
            A.CallTo(() => _manageEmployeeAccountRepository.createEmployee(cus)).Returns(1);
            var result = _manageEmployeeAccountService.createEmployee(cus);
            Assert.Equal(1, result);
        }

        [Fact]
        public void ManageEmployeeAccountService_deleteEmloyeeByID_ReturnSuccess()
        {
            var cus = A.Fake<EmployeeViewModel>();
            cus = new EmployeeViewModel
            {
                accountID = 1,
                employeeID = 1
            };
            A.CallTo(() => _manageEmployeeAccountRepository.deleteEmloyeeByID(cus.employeeID,cus.accountID)).Returns(1);
            var result = _manageEmployeeAccountService.deleteEmloyeeByID(cus.employeeID, cus.accountID);
            Assert.Equal(1, result);
        }

        [Fact]
        public void ManageEmployeeAccountService_updateEmployeeAccount_ReturnSuccess()
        {
            var cus = A.Fake<EmployeeViewModel>();
            cus = new EmployeeViewModel
            {
                accountID = 1,
                dateOfBirth = DateTime.ParseExact("27/09/1998", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                fullName = "Dam Nhat",
                identifyNumber = "56456456546",
                phoneNumber = "0912381273",
                userName = "nhatdv15",
                employeeID = 1,
                departmentID = 3,
                email = "nhatdv15@gmail.com",
                role = "Admin",
                Total = 5
            };
            A.CallTo(() => _manageEmployeeAccountRepository.updateEmployeeAccount(cus,cus.employeeID, cus.accountID)).Returns(1);
            var result = _manageEmployeeAccountService.updateEmployeeAccount(cus,cus.employeeID, cus.accountID);
            Assert.Equal(1, result);
        }

        [Fact]
        public void ManageEmployeeAccountService_getEmployeeInformationToUpdateByID_ReturnEqualCount()
        {
            var cus = A.Fake<EmployeeViewModel>();
            cus = new EmployeeViewModel
            {
                accountID = 1,
                dateOfBirth = DateTime.ParseExact("27/09/1998", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                fullName = "Dam Nhat",
                identifyNumber = "56456456546",
                phoneNumber = "0912381273",
                userName = "nhatdv15",
                employeeID = 1,
                departmentID = 3,
                email = "nhatdv15@gmail.com",
                role = "Admin",
                Total = 5
            };
            var listCus = A.Fake<List<EmployeeViewModel>>();
            listCus.Add(cus);
            A.CallTo(() => _manageEmployeeAccountRepository.getEmployeeInformationToUpdateByID(cus.employeeID)).Returns(listCus);
            var result = _manageEmployeeAccountService.getEmployeeInformationToUpdateByID(cus.employeeID);
            Assert.Equal(listCus.Count, result.Count);
        }

        [Fact]
        public void ManageEmployeeAccountService_getListDepartment_ReturnEqualCount()
        {
            var cus = A.Fake<Department>();
            cus = new Department
            {
                departmentCode = "1",
                departmentID = 2,
                departmentName = "Admin"
            };
            var listCus = A.Fake<List<Department>>();
            listCus.Add(cus);
            A.CallTo(() => _manageEmployeeAccountRepository.getListDepartment()).Returns(listCus);
            var result = _manageEmployeeAccountService.getListDepartment();
            Assert.Equal(listCus.Count, result.Count);
        }
    }
}
