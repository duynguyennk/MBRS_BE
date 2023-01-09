namespace MBRS_API_DEMO.Utils
{
    public static class ErrorCodeResponse
    {
        public const string NULL = "Null";
        public const int SUCCESS_CODE = 1;
        public const int FAIL_CODE = -1;

        public const string GET_DATA_SUCCESS = "Lấy dữ liệu thành công";
        public const string UPDATE_SUCCESS = "Cập nhập dữ liệu thành công";
        public const string UPDATE_FAIL = "Cập nhập dữ liệu thành công";
        public const string DATA_EMPTY = "Không có dữ liệu";
        public const string DATA_DELETE_SUCCESS = "Dữ liệu xóa thành công";

        public const string CHANGE_PASSWORD_SUCCESS = "Thay đổi mật khẩu thành công";
        public const string CHANGE_PASSWORD_FAILED = "Thay đổi mật khẩu không thành công";
        public const string SEND_NEW_PASSWORD_SUCCESS = "Mật khẩu mới đã được gửi đến mail thành công";
        public const string SEND_NEW_PASSWORD_FAILED = "Mật khẩu mới đã được gửi đến mail không thành công";

        public const string WRONG_USERNAME_OR_PASSWORD = "Mật khẩu hoặc tên tài khoản bị sai";
        public const string LOGIN_SUCCESSFULLY = "Đăng nhập thành công";
        public const string CHECK_ACCOUNT_STATUS = "Tài Khoản không tồn tại";

        public const string GET_LIST_EMPLOYEE_SUCCESS = "Lấy danh sách nhân viên thành công";
        public const string LIST_EMPLOYEE_EMPTY = "Danh sách nhân viên trống";
        public const string CREATE_EMPLOYEE_SUCCESS = "Tạo dữ liệu nhân viên thành công";
        public const string CREATE_EMPLOYEE_FAILED = "Tạo dữ liệu nhân viên không thành công";
        public const string UPDATE_EMPLOYEE_FAILED = "Cập nhật dữ liệu nhân viên không thành công";
        public const string UPDATE_EMPLOYEE_SUCCESS = "Cập nhật dữ liệu nhân viên thành công";
        public const string DELETE_EMPLOYEE_FAILED = "Xóa dữ liệu nhân viên không thành công";
        public const string DELETE_EMPLOYEE_SUCCESS = "Xóa dữ liệu nhân viên thành công";

        public const string GET_LIST_DEPARTMENT_SUCCESS = "Lấy danh sách phòng ban thành công";
        public const string GET_LIST_DEPARTMENT_FAILED = "Lấy danh sách phòng ban không thành công";
        public const string LIST_DEPARTMENT_EMPTY = "Danh sách phòng ban trống";

        public const string UPDATE_CUSTOMER_FAILED = "Cập nhật dữ liệu Khách hàng không thành công";
        public const string UPDATE_CUSTOMER_SUCCESS = "Cập nhật dữ liệu khách hàng thành công";
        public const string GET_LIST_CUSTOMER_SUCCESS = "Lấy danh sách khách hàng thành công";
        public const string GET_LIST_CUSTOMER_FAILED = "Lấy danh sách khách hàng không thành công";
        public const string LIST_CUSTOMER_EMPTY = "Danh sách khách hàng trống";
        public const string CREATE_CUSTOMER_SUCCESS = "Tạo dữ liệu khách hàng thành công";
        public const string CREATE_CUSTOMER_FAILED = "Tạo dữ liệu khách hàng không thành công";
        public const string DELETE_CUSTOMER_FAILED = "Xóa dữ liệu khách hàng không thành công";
        public const string DELETE_CUSTOMER_SUCCESS = "Xóa dữ liệu khách hàng thành công";

        public const string USER_NAME_DUPLICATE = "Tên tài khoản đã tồn tại";
        public const string EMAIL_DUPLICATE = "Email đã tồn tại";
        public const string IDENTITY_NUMBER_DUPLICATE = "Căn cước công dân hoặc chứng minh nhân dân đã tồn tại";

        public const string DELETE_ROOM_FAILED = "Xóa dữ liệu phòng không thành công";
        public const string DELETE_ROOM_SUCCESS = "Xóa dữ liệu phòng thành công";
        public const string GET_LIST_ROOM_SUCCESS = "Lấy danh sách phòng thành công";
        public const string GET_LIST_ROOM_FAILED = "Lấy danh sách phòng không thành công";
        public const string LIST_ROOM_EMPTY = "Danh sách Phòng trống";

        public const string LIST_RATING_EMPTY = "Danh sách đánh giá trống";
        public const string GET_LIST_RATING_SUCCESS = "Lấy danh sách đánh giá thành công";
        public const string GET_LIST_RATING_FAILED = "Lấy danh sách đánh giá không thành công";

        public const string LIST_ID_ROOM_EMPTY = "Danh sách ID phòng trống";
        public const string GET_LIST_ID_ROOM_SUCCESS = "Lấy danh sách ID phòng thành công";

        public const string DUPLICATE_TYPE_ROOM_CODE = "Mã loại phòng đã tồn tại";
        public const string DUPLICATE_TYPE_ROOM_NAME = "Tên loại phòng đã tồn tại";

        public const string LIST_TYPE_ROOM_EMPTY = "Danh sách loại phòng trống";
        public const string GET_LIST_TYPE_ROOM_SUCCESS = "Lấy danh sách loại phòng thành công";
        public const string GET_LIST_TYPE_ROOM_FAILED = "Lấy danh sách loại phòng không thành công";
        public const string DELETE_TYPE_ROOM_FAILED = "Xóa dữ liệu loại phòng không thành công";
        public const string DELETE_TYPE_ROOM_SUCCESS = "Xóa dữ liệu loại phòng thành công";
        public const string CREATE_TYPE_ROOM_SUCCESS = "Tạo dữ liệu loại phòng thành công";
        public const string CREATE_TYPE_ROOM_FAILED = "Tạo dữ liệu loại phòng không thành công";
        public const string UPDATE_TYPE_ROOM_FAILED = "Cập nhật dữ liệu loại phòng không thành công";
        public const string UPDATE_TYPE_ROOM_SUCCESS = "Cập nhật dữ liệu loại phòng thành công";
        public const string DELETE_IMAGE_TYPE_ROOM_FAILED = "Xóa ảnh loại phòng không thành công";
        public const string DELETE_IMAGE_TYPE_ROOM_SUCCESS = "Xóa ảnh loại phòng thành công";
        public const string UPDATE_IMAGE_TYPE_ROOM_FAILED = "Cập nhật ảnh loại phòng không thành công";
        public const string UPDATE_IMAGE_TYPE_ROOM_SUCCESS = "Cập nhật ảnh loại phòng thành công";

        public const string CREATE_FEEDBACK_SUCCESS = "Tạo dữ liệu phản hồi thành công";
        public const string CREATE_FEEDBACK_FAILED = "Tạo dữ liệu phản hồi không thành công";
        public const string GET_FEEDBACK_SUCCESS= "Lấy danh sách phản hồi thành công";
        public const string GET_FEEDBACK_FAILED = "Lấy danh sách phản hồi không thành công";

        public const string GET_LIST_FLOOR_SUCCESS = "Lấy danh sách tầng thành công";
        public const string GET_LIST_FLOOR_FAILED = "Lấy danh sách tầng không thành công";
        public const string LIST_FLOOR_EMPTY = "Danh sách tầng trồng";
        public const string DELETE_FLOOR_FAILED = "Xóa dữ liệu tầng không thành công";
        public const string DELETE_FLOOR_SUCCESS = "Xóa dữ liệu tầng thành công";
        public const string CREATE_FLOOR_SUCCESS = "Tạo dữ liệu tầng thành công";
        public const string CREATE_FLOOR_FAILED = "Tạo dữ liệu tầng không thành công";
        public const string UPDATE_FLOOR_FAILED = "Cập nhật dữ tầng phòng không thành công";
        public const string UPDATE_FLOOR_SUCCESS = "Cập nhật dữ tầng phòng thành công";

        public const string DUPLICATE_ROOM_CODE = "Mã phòng đã tồn tại";
        public const string DUPLICATE_ROOM_NAME = "Tên phòng đã tồn tại";

        public const string CREATE_ROOM_FAILED = "Tạo dữ liệu phòng không thành công";
        public const string CREATE_ROOM_SUCCESS = "Tạo dữ liệu phòng thành công";
        public const string UPDATE_ROOM_FAILED = "Cập nhật dữ liệu phòng không thành công";
        public const string UPDATE_ROOM_SUCCESS = "Cập nhật dữ liệu phòng thành công";

        public const string FILTER_ROOM_FAILED = "Kiểm tra nghiệp vụ vào service thành công";
        public const string FILTER_ROOM_SUCCESS = "Kiểm tra nghiệp vụ vào service thành công";

        public const string CREATE_ACTIVITY_EMPLOYEE_FAILED = "Tạo dữ liệu hoạt động không thành công";
        public const string CREATE_ACTIVITY_EMPLOYEE_SUCCESS = "Tạo dữ liệu hoạt động thành công";
        public const string GET_LIST_ACTIVITY_EMPLOYEE_SUCCESS = "Lấy danh sách hoạt động thành công";
        public const string GET_LIST_ACTIVITY_EMPLOYEE_FAILED = "Lấy danh sách hoạt động không thành công";
        public const string LIST_ACTIVITY_EMPLOYEE_FAILED = "Danh sách hoạt động trống";

        public const string UPDATE_BIKE_FAILED = "Cập nhật dữ liệu xe đạp không thành công";
        public const string UPDATE_BIKE_SUCCESS = "Cập nhật dữ liệu xe đạp thành công";
        public const string CREATE_BIKE_FAILED = "Tạo dữ liệu xe đạp không thành công";
        public const string CREATE_BIKE_SUCCESS = "Tạo dữ liệu xe đạp thành công";
        public const string GET_LIST_BIKE_SUCCESS = "Lấy danh sách xe đạp thành công";
        public const string GET_LIST_BIKE_FAILED = "Lấy danh sách xe đạp không thành công";
        public const string DELETE_BIKE_FAILED = "Xóa dữ liệu xe đạp không thành công";
        public const string DELETE_BIKE_SUCCESS = "Xóa dữ liệu xe đạp thành công";
        public const string LIST_BIKE_EMPTY = "Danh sách xe đạp trống";

        public const string UPDATE_CAR_FAILED = "Cập nhật dữ liệu xe không thành công";
        public const string UPDATE_CAR_SUCCESS = "Cập nhật dữ liệu xe thành công";
        public const string CREATE_CAR_FAILED = "Tạo dữ liệu xe không thành công";
        public const string CREATE_CAR_SUCCESS = "Tạo dữ liệu xe thành công";
        public const string GET_LIST_CAR_SUCCESS = "Lấy danh sách xe thành công";
        public const string GET_LIST_CAR_FAILED = "Lấy danh sách xe không thành công";
        public const string DELETE_CAR_FAILED = "Xóa dữ liệu xe không thành công";
        public const string DELETE_CAR_SUCCESS = "Xóa dữ liệu xe thành công";
        public const string LIST_CAR_EMPTY = "Danh sách xe trống";

        public const string UPDATE_IMAGE_TYPE_BIKE_FAILED = "Cập nhật dữ liệu loại xe đạp không thành công";
        public const string UPDATE_IMAGE_TYPE_BIKE_SUCCESS = "Cập nhật dữ liệu loại xe đạp thành công";
        public const string UPDATE_TYPE_BIKE_FAILED = "Cập nhật dữ liệu loại xe đạp không thành công";
        public const string UPDATE_TYPE_BIKE_SUCCESS = "Cập nhật dữ liệu loại xe đạp thành công";
        public const string CREATE_TYPE_BIKE_FAILED = "Tạo dữ liệu loại xe đạp không thành công";
        public const string CREATE_TYPE_BIKE_SUCCESS = "Tạo dữ liệu loại xe đạp thành công";
        public const string GET_LIST_TYPE_BIKE_SUCCESS = "Lấy danh sách loại xe đạp thành công";
        public const string GET_LIST_TYPE_BIKE_FAILED = "Lấy danh sách loại xe đạp không thành công";
        public const string DELETE_TYPE_BIKE_FAILED = "Xóa dữ liệu loại xe đạp không thành công";
        public const string DELETE_TYPE_BIKE_SUCCESS = "Xóa dữ liệu loại xe đạp thành công";
        public const string LIST_TYPE_BIKE_EMPTY = "Danh sách loại xe đạp trống";

        public const string UPDATE_TYPE_CAR_AIRPORT_FAILED = "Cập nhật dữ liệu loại xe không thành công";
        public const string UPDATE_TYPE_CAR_AIRPORT_SUCCESS = "Cập nhật dữ liệu loại xe thành công";
        public const string CREATE_TYPE_CAR_AIRPORT_FAILED = "Tạo dữ liệu loại xe không thành công";
        public const string CREATE_TYPE_CAR_AIRPORT_SUCCESS = "Tạo dữ liệu loại xe thành công";
        public const string GET_LIST_TYPE_CAR_AIRPORT_SUCCESS = "Lấy danh sách loại xe thành công";
        public const string GET_LIST_TYPE_CAR_AIRPORT_FAILED = "Lấy danh sách loại xe không thành công";
        public const string DELETE_TYPE_CAR_AIRPORT_FAILED = "Xóa dữ liệu loại xe không thành công";
        public const string DELETE_TYPE_CAR_AIRPORT_SUCCESS = "Xóa dữ liệu loại xe thành công";
        public const string LIST_TYPE_CAR_AIRPORT_EMPTY = "Danh sách loại xe trống";

        public const string UPDATE_TYPE_FOOD_FAILED = "Cập nhật dữ liệu loại thức ăn không thành công";
        public const string UPDATE_TYPE_FOOD_SUCCESS = "Cập nhật dữ liệu loại thức ăn thành công";
        public const string CREATE_TYPE_FOOD_FAILED = "Tạo dữ liệu loại thức ăn không thành công";
        public const string CREATE_TYPE_FOOD_SUCCESS = "Tạo dữ liệu loại thức ăn thành công";
        public const string GET_LIST_TYPE_FOOD_SUCCESS = "Lấy danh sách loại thức ăn thành công";
        public const string GET_LIST_TYPE_FOOD_FAILED = "Lấy danh sách loại thức ăn không thành công";
        public const string DELETE_TYPE_FOOD_FAILED = "Xóa dữ liệu loại thức ăn không thành công";
        public const string DELETE_TYPE_FOOD_SUCCESS = "Xóa dữ liệu loại thức ăn thành công";
        public const string LIST_TYPE_FOOD_EMPTY = "Danh sách loại thức ăn trống";

        public const string DUPLICATE_FOOD_CODE = "Mã thức ăn đã tồn tại";
        public const string UPDATE_FOOD_FAILED = "Cập nhật dữ liệu thức ăn không thành công";
        public const string UPDATE_FOOD_SUCCESS = "Cập nhật dữ liệu thức ăn thành công";
        public const string CREATE_FOOD_FAILED = "Tạo dữ liệu thức ăn không thành công";
        public const string CREATE_FOOD_SUCCESS = "Tạo dữ liệu thức ăn thành công";
        public const string GET_LIST_FOOD_SUCCESS = "Lấy danh sách thức ăn thành công";
        public const string GET_LIST_FOOD_FAILED = "Lấy danh sách thức ăn không thành công";
        public const string DELETE_FOOD_FAILED = "Xóa dữ liệu thức ăn không thành công";
        public const string DELETE_FOOD_SUCCESS = "Xóa dữ liệu thức ăn thành công";
        public const string LIST_FOOD_EMPTY = "Danh sách thức ăn trống";

        public const string UPDATE_ORDER_ROOM_FAILED = "Cập nhật dữ liệu hóa đơn không thành công";
        public const string UPDATE_ORDER_ROOM_SUCCESS = "Cập nhật dữ liệu hóa đơn thành công";
        public const string CREATE_ORDER_ROOM_FAILED = "Tạo dữ liệu hóa đơn không thành công";
        public const string CREATE_ORDER_ROOM_SUCCESS = "Tạo dữ liệu hóa đơn thành công";
        public const string GET_LIST_ORDER_ROOM_SUCCESS = "Lấy danh sách hóa đơn thành công";
        public const string GET_LIST_ORDER_ROOM_FAILED = "Lấy danh sách hóa đơn không thành công";
        public const string DELETE_ORDER_ROOM_FAILED = "Xóa dữ liệu hóa đơn không thành công";
        public const string DELETE_ORDER_ROOM_SUCCESS = "Xóa dữ liệu hóa đơn thành công";
        public const string LIST_ORDER_ROOM_EMPTY = "Danh sách hóa đơn trống";

        public const string GET_LIST_NOTIFICATION_SUCCESS = "Lấy danh sách thông báo thành công";
        public const string GET_LIST_NOTIFICATION_FAILED = "Lấy danh sách thông báo không thành công";
        public const string LIST_NOTIFICATION_EMPTY = "Danh sách thông báo trống";
        public const string UPDATE_NOTIFICATION_FAILED = "Cập nhật dữ liệu thông báo không thành công";
        public const string UPDATE_NOTIFICATION_SUCCESS = "Cập nhật dữ liệu thông báo thành công";

        public const string GET_LIST_ORDER_BIKE_SUCCESS = "Lấy danh sách hóa đơn thành công";
        public const string GET_LIST_ORDER_BIKE_FAILED = "Lấy danh sách hóa đơn không thành công";
        public const string LIST_ORDER_BIKE_EMPTY = "Danh sách hóa đơn trống";
        public const string DELETE_ORDER_BIKE_FAILED = "Xóa dữ liệu hóa đơn không thành công";
        public const string DELETE_ORDER_BIKE_SUCCESS = "Xóa dữ liệu hóa đơn thành công";

        public const string GET_LIST_ORDER_FOOD_SUCCESS = "Lấy danh sách hóa đơn thành công";
        public const string GET_LIST_ORDER_FOOD_FAILED = "Lấy danh sách hóa đơn không thành công";
        public const string LIST_ORDER_FOOD_EMPTY = "Danh sách hóa đơn trống";
        public const string DELETE_ORDER_FOOD_FAILED = "Xóa dữ liệu hóa đơn không thành công";
        public const string DELETE_ORDER_FOOD_SUCCESS = "Xóa dữ liệu hóa đơn thành công";
        public const string UPDATE_ORDER_FOOD_FAILED = "Cập nhật dữ liệu hóa đơn không thành công";
        public const string UPDATE_ORDER_FOOD_SUCCESS = "Cập nhật dữ liệu hóa đơn thành công";

        public const string GET_LIST_TYPE_CAR_SUCCESS = "Lấy danh sách loại xe thành công";
        public const string GET_LIST_TYPE_CAR_FAILED = "Lấy danh sách loại xe không thành công";
        public const string LIST_TYPE_CAR_EMPTY = "Danh sách thức loại xe trống";


    }
}
