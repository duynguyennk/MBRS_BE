using FakeItEasy;
using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.Service;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAct;

namespace MBRS_API.Tests.Services
{
    public class ManageCustomerAccountService_Test
    {
        private IConfiguration _configuration;
        private IManageCustomerAccountRepository _manageCustomerAccountRepository;
        private ManageCustomerAccountService _manageCustomerAccountService;

        public ManageCustomerAccountService_Test()
        {
            _configuration = A.Fake<IConfiguration>();
            _manageCustomerAccountRepository = A.Fake<IManageCustomerAccountRepository>();
            _manageCustomerAccountService = new ManageCustomerAccountService(_configuration, _manageCustomerAccountRepository);
        }

        [Fact]
        public void ManageCustomerAccountService_getCustomerInformationToUpdateByID_ReturnTotalRecord()
        {
            var cus = A.Fake<CustomerViewModel>();
            cus = new CustomerViewModel { accountID = 1,customerID = 5,dateOfBirth = DateTime.ParseExact("27/09/1998", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                departmentID = 3,email = "nhatdv15@gmail.com",fullName = "Dam Nhat"
                ,identifyNumber = "56456456546" ,phoneNumber = "0912381273",role= "3",Total = 5,userName = "nhatdv1",password = "88DFA2EA2E9EF240F45935D257FEA20A"
            };
            var listCus = A.Fake<List<CustomerViewModel>>();
            listCus.Add(cus);
            A.CallTo(() => _manageCustomerAccountRepository.getCustomerInformationToUpdateByID(cus.customerID)).Returns(listCus);
            var result = _manageCustomerAccountService.getCustomerInformationToUpdateByID(cus.customerID);
            Assert.Equal(listCus.Count, result.Count);
        }

        [Fact]
        public void ManageCustomerAccountService_deleteCustomerByID_ReturnSuccess()
        {
            var cus = A.Fake<CustomerViewModel>();
            cus = new CustomerViewModel
            {
                accountID = 1,
                customerID = 5,
                dateOfBirth = DateTime.ParseExact("27/09/1998", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                departmentID = 3,
                email = "nhatdv15@gmail.com",
                fullName = "Dam Nhat",
                identifyNumber = "56456456546",
                phoneNumber = "0912381273",
                role = "3",
                Total = 5,
                userName = "nhatdv1",
                password = "88DFA2EA2E9EF240F45935D257FEA20A"
            };
            A.CallTo(() => _manageCustomerAccountRepository.deleteCustomerByID(cus.customerID,cus.accountID)).Returns(1);
            var result = _manageCustomerAccountService.deleteCustomerByID(cus.customerID, cus.accountID);
            Assert.Equal(1, result);
        }

        [Fact]
        public void ManageCustomerAccountService_getAllCustomerAccountWithFilter_ReturnEqualtotalrecord()
        {
            var cus = A.Fake<Customer>();
            cus = new Customer
            {
                accountID = 1,
                customerID = 5,
                email = "nhatdv15@gmail.com",
                fullName = "Dam Nhat",
                departmentName = "Le Tan",
                identifyNumber = "56456456546",
                phoneNumber = "0912381273",
                userName = "nhatdv1",
                dateOfBirth = "27/09/1998"
            };
            var listCus = A.Fake<List<Customer>>();
            listCus.Add(cus);
            A.CallTo(() => _manageCustomerAccountRepository.getAllCustomerAccountWithFilter(cus.fullName,"Dam Nhat")).Returns(listCus);
            var result = _manageCustomerAccountService.getAllCustomerAccountWithFilter(cus.fullName, "Dam Nhat");
            Assert.Equal(listCus.Count, result.Count);
        }

        [Fact]
        public void ManageCustomerAccountService_getAllCustomerAccount_ReturnEqualtotalrecord()
        {
            var cus = A.Fake<Customer>();
            cus = new Customer
            {
                accountID = 1,
                customerID = 5,
                email = "nhatdv15@gmail.com",
                fullName = "Dam Nhat",
                departmentName = "Le Tan",
                identifyNumber = "56456456546",
                phoneNumber = "0912381273",
                userName = "nhatdv1",
                dateOfBirth = "27/09/1998"
            };
            var listCus = A.Fake<List<Customer>>();
            listCus.Add(cus);
            A.CallTo(() => _manageCustomerAccountRepository.getAllCustomerAccount()).Returns(listCus);
            var result = _manageCustomerAccountService.getAllCustomerAccount();
            Assert.Equal(listCus.Count, result.Count);
        }

        [Fact]
        public void ManageCustomerAccountService_updateCustomerAccount_ReturnSuccess()
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
            A.CallTo(() => _manageCustomerAccountRepository.updateCustomerAccount(cus,cus.customerID,cus.accountID)).Returns(1);
            var result = _manageCustomerAccountService.updateCustomerAccount(cus, cus.customerID, cus.accountID);
            Assert.Equal(1, result);
        }
        [Fact]
        public void ManageCustomerAccountService_createCustomer_ReturnSuccess()
        {
            var cus = A.Fake<CustomerViewModel>();
            cus = new CustomerViewModel
            {
                accountID = 1,
                customerID = 5,
                dateOfBirth = DateTime.ParseExact("27/09/1998", "dd/MM/yyyy", CultureInfo.InvariantCulture),
                departmentID = 3,
                email = "nhatdv15@gmail.com",
                fullName = "Dam Nhat",
                identifyNumber = "56456456546",
                phoneNumber = "0912381273",
                role = "3",
                Total = 5,
                userName = "nhatdv1",
                password = "88DFA2EA2E9EF240F45935D257FEA20A"
            };
            var listCus = A.Fake<List<CustomerViewModel>>();
            listCus.Add(cus);
            A.CallTo(() => _manageCustomerAccountRepository.createCustomer(cus)).Returns(1);
            var result = _manageCustomerAccountService.createCustomer(cus);
            Assert.Equal(1, result);
        }
    }
}
