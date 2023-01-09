using MBRS_API.Models;
using MBRS_API.Repositories.IRepository;
using MBRS_API.Services.IService;

namespace MBRS_API.Services.Service
{
    public class ManageActivityEmployeeService : IManageActivityEmployeeService
    {
        private readonly IConfiguration _configuration;
        private readonly IManageActivityEmployeeRepository _manageActivityEmployeeRepository;

        public ManageActivityEmployeeService(IConfiguration configuration, IManageActivityEmployeeRepository manageActivityEmployeeRepository)
        {
            this._configuration = configuration;
            _manageActivityEmployeeRepository = manageActivityEmployeeRepository;
        }

        public int createActivityEmployee(ActivityEmployee activityEmployee)
        {
            return _manageActivityEmployeeRepository.createActivityEmployee(activityEmployee);
        }

        public List<ActivityEmployee> getAllActivityEmployee()
        {
            return _manageActivityEmployeeRepository.getAllActivityEmployee();
        }
        public List<ActivityEmployee> getAllActivityEmployeeWithFilter(string filterName, string filterValue)
        {
            return _manageActivityEmployeeRepository.getAllActivityEmployeeWithFilter(filterName, filterValue);
        }
    }
}
