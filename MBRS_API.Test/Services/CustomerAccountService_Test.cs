using FakeItEasy;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.Service;
using MBRS_API_DEMO.Models;
using Microsoft.Extensions.Configuration;


namespace MBRS_API.Tests.Services
{
    public class CustomerAccountService_Test
    {
        private IConfiguration _configuration;
        private ICustomerAccountRepository _customerAccountRepository;
        private CustomerAccountService _customerAccountService;
        public CustomerAccountService_Test()
        {
            _configuration = A.Fake<IConfiguration>();
            _customerAccountRepository = A.Fake<ICustomerAccountRepository>();
            _customerAccountService = new CustomerAccountService( _configuration, _customerAccountRepository);
        }

        [Fact]
        public void CustomerAccountService_registerCustomerAccount_Success()
        {
            var user = A.Fake<CustomerViewModel>();
            user = new CustomerViewModel
            {
                accountID = 1,
                customerID = 1,
                dateOfBirth = DateTime.Now,
                departmentID = 1,
                email = "nhatdv15@gmail.com",
                fullName = "Dam Nhat",
                identifyNumber = "23432",
                password = "88DFA2EA2E9EF240F45935D257FEA20A",
                phoneNumber = "0981732817",
                role = "2",
                Total = 5,
                userName = "nhatdv15"
            };
            A.CallTo(() => _customerAccountRepository.registerCustomerAccount(user)).Returns(1);
            var result = _customerAccountService.registerCustomerAccount(user);
            Assert.Equal(1, result);
        }

        [Fact]
        public void CustomerAccountService_getCustomerInformationByID_ReturntotalCount()
        {
            var user = A.Fake<CustomerViewModel>();
            user = new CustomerViewModel
            {
                accountID = 1,
                customerID = 1,
                dateOfBirth = DateTime.Now,
                departmentID = 1,
                email = "nhatdv15@gmail.com",
                fullName = "Dam Nhat",
                identifyNumber = "23432",
                password = "88DFA2EA2E9EF240F45935D257FEA20A",
                phoneNumber = "0981732817",
                role = "2",
                Total = 5,
                userName = "nhatdv15"
            };
            var listUser = A.Fake<List<CustomerViewModel>>();
            listUser.Add(user);
            A.CallTo(() => _customerAccountRepository.getCustomerInformationByID(user.accountID)).Returns(listUser);
            var result = _customerAccountService.getCustomerInformationByID(user.accountID);
            Assert.Equal(listUser.Count, result.Count);
        }

        [Fact]
        public void CustomerAccountService_updateCustomerAccount_Success()
        {
            var user = A.Fake<CustomerViewModel>();
            user = new CustomerViewModel
            {
                accountID = 1,
                customerID = 1,
                dateOfBirth = DateTime.Now,
                departmentID = 1,
                email = "nhatdv15@gmail.com",
                fullName = "Dam Nhat",
                identifyNumber = "23432",
                password = "88DFA2EA2E9EF240F45935D257FEA20A",
                phoneNumber = "0981732817",
                role = "2",
                Total = 5,
                userName = "nhatdv15"
            };
            A.CallTo(() => _customerAccountRepository.updateCustomerAccount(user,user.customerID,user.accountID)).Returns(1);
            var result = _customerAccountService.updateCustomerAccount(user, user.customerID, user.accountID);
            Assert.Equal(1, result);
        }
    }
}
