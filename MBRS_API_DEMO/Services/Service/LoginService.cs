using log4net.Core;
using MBRS_API.Models;
using MBRS_API.Models.ViewModels;
using MBRS_API_DEMO.Models;
using MBRS_API_DEMO.Repositories.IRepository;
using MBRS_API_DEMO.Response;
using MBRS_API_DEMO.Services.IService;
using MBRS_API_DEMO.Utils;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static MBRS_API_DEMO.Utils.IConstants;

namespace MBRS_API_DEMO.Services.Service
{
    public class LoginService : ILoginService
    {
        private readonly IConfiguration _configuration;
        private readonly ILoginRepository _loginRepository;

        public LoginService(IConfiguration configuration, ILoginRepository loginRepository)
        {
            this._configuration = configuration;
            _loginRepository = loginRepository;
        }
        public int ForgetPassword(User user)
        {
            return _loginRepository.ForgetPassword(user);
        }

        public int ChangePassword(ChangePassword changePassword)
        {
           return _loginRepository.ChangePassword(changePassword);
        }

        public int CheckNotActive(string UserName)
        {
            return _loginRepository.CheckNotActive(UserName);
        }

        public int CheckPasswordCorrect(ChangePassword changePassword)
        {
            return _loginRepository.CheckPasswordCorrect(changePassword);
        }
        public List<CustomerViewModel> getCustomerInformationByID(int accountID)
        {
            return _loginRepository.getCustomerInformationByID(accountID);
        }
        public BaseResponse<string> Login(User user)
        {
            User userModel = _loginRepository.CheckLoginByUser(user);
            if (userModel == null)
            {
                return new BaseResponse<String>(IErrorCodeApi.NOT_FOUND,IConstants.Data.DataNull,ErrorCodeResponse.WRONG_USERNAME_OR_PASSWORD);
            }
            var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim("UserName", user.UserName),
            new Claim("DepartmentCode", userModel.DepartmentCode),
            new Claim("DepartmentName", userModel.DepartmentName.ConvertToString()),
            new Claim(ClaimTypes.Role, userModel.DepartmentCode)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: signIn
                );
            return new BaseResponse<String>(IErrorCodeApi.OK,new JwtSecurityTokenHandler().WriteToken(token),userModel, ErrorCodeResponse.LOGIN_SUCCESSFULLY);
        }

        public BaseResponse<string> Logout(User user)
        {
            try
            {
                if (Guid.TryParse(user.Token.Trim(), out Guid userId))
                {
                    return new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST, ErrorCodeResponse.NULL);
                }
                var stream = "[encoded jwt]";
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(stream);
                var tokenS = jsonToken as JwtSecurityToken;
                return new BaseResponse<String>(IErrorCodeApi.OK);
            }
            catch (Exception ex)
            {
                LogUtil.Error(ex.Message, ex);
                return new BaseResponse<String>(IErrorCodeApi.BAD_REQUEST);
            }
        }

       
    }
}
