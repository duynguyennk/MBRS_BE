using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.IService;

namespace MBRS_API.Services.Service
{
    public class FilterServiceCustomerService : IFilterServiceCustomerService
    {
        private readonly IConfiguration _configuration;
        private readonly IFilterServiceCustomerRepository _filterServiceCustomerRepository;

        public FilterServiceCustomerService(IConfiguration configuration, IFilterServiceCustomerRepository filterServiceCustomerRepository)
        {
            this._configuration = configuration;
            _filterServiceCustomerRepository = filterServiceCustomerRepository;
        }
        public int checkUsingCustomerService(int accountID)
        {
            return _filterServiceCustomerRepository.checkUsingCustomerService(accountID);
        }
    }
}
