using MBRS_API_DEMO.Models;
using MBRS_API_DEMO.Utils;

namespace MBRS_API_DEMO.Response
{
    public class BaseResponse<T>
    {
        public BaseResponse()
        {
            this.Code = IConstants.IErrorCodeApi.OK;
        }

        public BaseResponse(T data)
        {
            this.Code = IConstants.IErrorCodeApi.OK;
            this.Data = data;
        }
        public BaseResponse(string URL)
        {
            this.Code = IConstants.IErrorCodeApi.OK;
            this.URL = URL;
        }
        public BaseResponse(String code, String errorMessage)
        {
            this.Code = code;
            this.ErrorMessage = errorMessage;
        }

        public BaseResponse(String code,T data,User user, String errorMessage)
        {
            this.Code = code;
            this.Data = data;
            this.User = user;
            this.ErrorMessage = errorMessage;
        }

        public BaseResponse(T data, int total)
        {
            this.Code = IConstants.IErrorCodeApi.OK;
            this.Data = data;
        }

        public BaseResponse(String code, T data)
        {
            this.Code = code;
            this.Data = data;
        }

        public BaseResponse(String code, T data, String errorMessage)
        {
            this.Code = code;
            this.Data = data;
            this.ErrorMessage = errorMessage;
        }


        public String Code { get; set; }
        public String ErrorMessage { get; set; }
        public string DeveloperMessage { get; set; }

        public string URL { get; set; }
        public User User { get; set; }

        public T Data { get; set; }
    }
}
