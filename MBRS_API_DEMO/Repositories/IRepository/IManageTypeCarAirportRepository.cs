using MBRS_API.Models;

namespace MBRS_API.Repositories.IRepository
{
    public interface IManageTypeCarAirportRepository
    {
        public List<TypeCarAirport> getAllTypeCarAirport();
        public List<TypeCarAirport> getAllTypeCarAirportWithFilter(string filterName, string filterValue);
        public int deleteTypeCarAirport(int typeCarAirportID);
        public int updateTheTypeCarAirport(TypeCarAirport typeCarAirport);
        public List<TypeCarAirport> getTypeCarAirportInformation(int typeCarAirportID);
        public int createTypeCarAirport(TypeCarAirport typeCarAirport);
    }
}
