using MBRS_API.Models;
using MBRS_API.Models.ViewModels;

namespace MBRS_API.Repositories.IRepository
{
    public interface IManageCarAirportRepository
    {
        public List<CarAirportViewModel> getAllCar();
        public int deleteCar(int carID);
        public List<CarAirportViewModel> getAllCarWithFilter(string filterName, string filterValue);
        public int updateTheCar(CarAirport carAirport);
        public List<TypeCarAirport> getAllTypeCar();

        public List<TypeCarAirport> getDetailInformationCar(int typeCarID);

        public int createCar(CarAirport carAirport);

        public List<CarAirport> getTheCarInformation(int carID);
    }
}
