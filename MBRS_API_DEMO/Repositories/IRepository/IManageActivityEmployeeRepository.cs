using MBRS_API.Models;

namespace MBRS_API.Repositories.IRepository
{
    public interface IManageActivityEmployeeRepository
    {
        public List<ActivityEmployee> getAllActivityEmployee();
        public int createActivityEmployee(ActivityEmployee activityEmployee);
        public List<ActivityEmployee> getAllActivityEmployeeWithFilter(string filterName, string filterValue);
    }
}
