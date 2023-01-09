using FakeItEasy;
using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.Service;
using MBRS_API_DEMO.Models;
using MBRS_API_DEMO.Repositories.IRepository;
using MBRS_API_DEMO.Services.Service;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MBRS_API.Tests.Services
{

    public class LoginService_Test
    {
        private IConfiguration _configuration;
        private ILoginRepository _loginRepository;
        private LoginService _loginService;

        public LoginService_Test()
        {
            _configuration = A.Fake<IConfiguration>();
            _loginRepository = A.Fake<ILoginRepository>();
            _loginService = new LoginService(_configuration, _loginRepository);
        }

        [Fact]
        public void LoginService_getCustomerInformationByID_ReturnTotalRecord()
        {
            var cus = A.Fake<CustomerViewModel>();
            cus = new CustomerViewModel
            {
                accountID = 1,
                customerID = 5,
                dateOfBirth = DateTime.ParseExact("27/09/1998", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                departmentID = 3,
                email = "nhatdv15@gmail.com",
                fullName = "Dam Nhat"
                ,
                identifyNumber = "56456456546",
                phoneNumber = "0912381273",
                role = "3",
                Total = 5,
                userName = "nhatdv1",
                password = "88DFA2EA2E9EF240F45935D257FEA20A"
            };
            var listCus = A.Fake<List<CustomerViewModel>>();
            listCus.Add(cus);
            A.CallTo(() => _loginRepository.getCustomerInformationByID(cus.accountID)).Returns(listCus);
            var result = _loginService.getCustomerInformationByID(cus.accountID);
            Assert.Equal(listCus.Count, result.Count);
        }

        [Fact]
        public void LoginService_Login_ReturnTotalRecord()
        {
            var cus = A.Fake<User>();
            cus = new User
            {
                UserName = "nhatdv15",
                DepartmentCode = "1003",
                DepartmentName = "Admin",
                Password = "Abcd1234"
            };
            var listCus = A.Fake<List<User>>();
            listCus.Add(cus);
            A.CallTo(() => _loginRepository.CheckLoginByUser(cus)).Returns(cus);
            var result = _loginService.Login(cus);
            Console.WriteLine(result);
            Assert.Equal(listCus.Count,1);
        }

        [Fact]
        public void LoginService_Logout_ReturnTotalRecord()
        {
            var cus = A.Fake<User>();
            cus = new User
            {
                UserName = "nhatdv15",
                AccountID = 1,
                DepartmentCode = "1003",
                DepartmentName = "Admin",
                Email = "nhatdv15@gmail.com",
                FullName = "Dam Nhat",
                Password = "88DFA2EA2E9EF240F45935D257FEA20A",
                Role = "1",
                Token = "SDCSDVA2EA23R2E9E2E3VF240F23FF45935D257CƯEFEA20A"
            };
            var listCus = A.Fake<List<User>>();
            listCus.Add(cus);
            A.CallTo(() => _loginRepository.CheckNotActive(cus.UserName)).Returns(1);
            var result = _loginService.CheckNotActive(cus.UserName);
            Assert.Equal(1, result);
        }

        [Fact]
        public void LoginService_ForgetPassword_ReturnTotalRecord()
        {
            var cus = A.Fake<User>();
            cus = new User
            {
                UserName = "nhatdv15",
                AccountID = 1,
                DepartmentCode = "1003",
                DepartmentName = "Admin",
                Email = "nhatdv15@gmail.com",
                FullName = "Dam Nhat",
                Password = "88DFA2EA2E9EF240F45935D257FEA20A",
                Role = "1",
                Token = "SDCSDVA2EA23R2E9E2E3VF240F23FF45935D257CƯEFEA20A"
            };
            var listCus = A.Fake<List<User>>();
            listCus.Add(cus);
            A.CallTo(() => _loginRepository.ForgetPassword(cus)).Returns(1);
            var result = _loginService.ForgetPassword(cus);
            Assert.Equal(1, result);
        }

        [Fact]
        public void LoginService_ChangePassword_ReturnTotalRecord()
        {
            var cus = A.Fake<ChangePassword>();
            cus = new ChangePassword
            {
                userName = "nhatdv15",
                newPassword = "88DFA2EA2E9EF240F45935D257FEA20A",
                oldPassword = "A23R2E9E2E3VF240F23FF45935D25ZXZ"
            };
            var listCus = A.Fake<List<ChangePassword>>();
            listCus.Add(cus);
            A.CallTo(() => _loginRepository.ChangePassword(cus)).Returns(1);
            var result = _loginService.ChangePassword(cus);
            Assert.Equal(1, result);
        }

        [Fact]
        public void LoginService_CheckPasswordCorrect_ReturnTotalRecord()
        {
            var cus = A.Fake<ChangePassword>();
            cus = new ChangePassword
            {
                userName = "nhatdv15",
                newPassword = "88DFA2EA2E9EF240F45935D257FEA20A",
                oldPassword = "A23R2E9E2E3VF240F23FF45935D25ZXZ"
            };
            var listCus = A.Fake<List<ChangePassword>>();
            listCus.Add(cus);
            A.CallTo(() => _loginRepository.CheckPasswordCorrect(cus)).Returns(1);
            var result = _loginService.CheckPasswordCorrect(cus);
            Assert.Equal(1, result);
        }
    }
}
