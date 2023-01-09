using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API_DEMO.Models;
using MBRS_API_DEMO.Response;

namespace MBRS_API_DEMO.Services.IService
{
    public interface ILoginService
    {
        BaseResponse<String> Login(User user);
        BaseResponse<String> Logout(User user);
        int CheckNotActive(string UserName);
        int ChangePassword(ChangePassword changePassword);

        public List<CustomerViewModel> getCustomerInformationByID(int accountID);

        int CheckPasswordCorrect(ChangePassword changePassword);
        int ForgetPassword(User user);
    }
}
