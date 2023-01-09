using MBRS_API.Models;
using MBRS_API.Models.ViewModels;

namespace MBRS_API.Repositories.IRepository
{
    public interface IManageBikeRepository
    {
        public List<BikeViewModel> getAllBike();
        public int deleteBike(int bikeID);

        public List<BikeViewModel> getAllBikeWithFilter(string filterName, string filterValue);

        public List<TypeBike> getAllTypeBike();

        public List<TypeBike> getDetailInformationBike(int typeBikeID);

        public int createBike(Bike bike);

        public List<Bike> getTheBikeInformation(int bikeID);

        public int updateTheBike(Bike bike);
    }
}
