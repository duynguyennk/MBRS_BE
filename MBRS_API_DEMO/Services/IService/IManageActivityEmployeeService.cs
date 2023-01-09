using MBRS_API.Models;

namespace MBRS_API.Services.IService
{
    public interface IManageActivityEmployeeService
    {
        public List<ActivityEmployee> getAllActivityEmployee();
        public int createActivityEmployee(ActivityEmployee activityEmployee);
        public List<ActivityEmployee> getAllActivityEmployeeWithFilter(string filterName, string filterValue);
    }
}
