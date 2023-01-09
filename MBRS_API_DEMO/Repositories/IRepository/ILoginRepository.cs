using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API_DEMO.Models;

namespace MBRS_API_DEMO.Repositories.IRepository
{
    public interface ILoginRepository
    {
        User CheckLoginByUser(User user);
        int CheckNotActive(string UserName);

        int ChangePassword(ChangePassword changePassword);

        int CheckPasswordCorrect(ChangePassword changePassword);

        public List<CustomerViewModel> getCustomerInformationByID(int accountID);

        int ForgetPassword(User user);
    }
}
