using FakeItEasy;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.Service;
using MBRS_API_DEMO.Models;
using Microsoft.Extensions.Configuration;


namespace MBRS_API.Tests.Services
{
    public class FilterServiceCustomerService_Test
    {
        private IConfiguration _configuration;
        private IFilterServiceCustomerRepository _filterServiceCustomerRepository;
        private FilterServiceCustomerService _filterServiceCustomerService;

        public FilterServiceCustomerService_Test()
        {
            _configuration = A.Fake<IConfiguration>();
            _filterServiceCustomerRepository = A.Fake<IFilterServiceCustomerRepository>();
            _filterServiceCustomerService = new FilterServiceCustomerService(_configuration, _filterServiceCustomerRepository);
        }

        [Fact]
        public void ManageBikeService_checkUsingCustomerService_ReturnSuccess()
        {
            var user = A.Fake<User>();
            user = new User { AccountID = 1,UserName = "nhatdv15",FullName = "Dam Nhat",DepartmentCode = "2",DepartmentName = "Le tan",Email = "nhatdv15@gmail.com",
                                Password = "88DFA2EA2E9EF240F45935D257FEA20A",Role = "2",Token = "VuzRpJfR3q3WzNSZsiGAzoCfKoUjqUIznqvOZ1Y"
            };
            A.CallTo(() => _filterServiceCustomerRepository.checkUsingCustomerService((int)user.AccountID)).Returns(1);
            var result = _filterServiceCustomerService.checkUsingCustomerService((int)user.AccountID);
            Assert.Equal(1, result);
        }
    }
}
