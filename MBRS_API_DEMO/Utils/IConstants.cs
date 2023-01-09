namespace MBRS_API_DEMO.Utils
{
    public interface IConstants
    {
        interface IRoutes
        {
            public const string API = "v1/api/";
            public const string LOGIN = API + "login/";
            public const string MANAGE_ACCOUNT = API + "manage_account/";
            public const string CUSTOMER_ACCOUNT = API + "customer_account/";
            public const string MANAGE_ROOM = API + "manage_room/";
            public const string MANAGE_BIKE = API + "manage_bike/";
            public const string MANAGE_ACTIVITY_EMPLOYEE = API + "manage_activity_employee/";
            public const string MANAGE_CAR = API + "manage_car/";
            public const string MANAGE_FOOD = API + "manage_food/";
            public const string MANAGE_ORDER_ROOM = API + "manage_order_room/";
            public const string MANAGE_ORDER_FOOD = API + "manage_order_food/";
            public const string VIEW_STATUS_ROOM = API + "view_status_room/";
            public const string VIEW_ORDER_CUSTOMER = API + "view_order_customer/";
            public const string NOTIFICATION = API + "notification/";
            public const string MANAGE_TYPE_BIKE = API + "manage_type_bike/";
            public const string MANAGE_TYPE_CAR_AIRPORT = API + "manage_type_car_airport/";
            public const string MANAGE_TYPE_FOOD = API + "manage_type_food/";
            public const string MANAGE_FLOOR = API + "manage_floor/";
            public const string MANAGE_FEEDBACK_ORDER_ROOM_CUSTOMER = API + "manage_feedback_order_room_customer/";
            public const string FILTER_USING_SERVICE_CUSTOMER = API + "filter_using_service_customer/";
            public const string MANAGE_TYPE_ROOM = API + "manage_type_room/";
            public const string ORDER_BIKE = API + "order_bike/";
            public const string ORDER_FOOD = API + "order_food/";
            public const string ORDER_ROOM = API + "order_room/";
            public const string PAYMENT = API + "payment/";
            public const string SERVICE_HOTEL = API + "service_hotel/";
        }
        interface Data
        {
            public const string DataNull = null;
        }

        interface SqlString
        {
            public const string SqlServerString = "SqlServerConnection";
        }

        interface FormatDate
        {
            public const string normalDate = "MM/dd/yyyy";
        }
        public enum CHECKING_STATUS_DELETE_FLAG
        {
            NoDelete = 0,
            Deleted = 1
        }
        public enum STATUS_NOTIFICATION
        {
            Active = 1,
            NotActive = 0
        }
        public enum STATUS_ROOM
        {
            OrderRoom = 1,
            CheckIn = 2,
            CheckOut = 3
        }
        public enum CHECK_ROLE
        {
            MN = 3,
            CS = 2,
            LT = 1,
            AM = 4
        }
        interface IErrorCodeApi
        {
            public const string OK = "200";
            public const string NOK = "201";
            public const string BAD_REQUEST = "400";
            public const string UNAUTHORIZED = "401";
            public const string FORBIDDEN = "403";
            public const string NOT_FOUND = "404";
            public const string ERROR_EXTEND = "406";
            public const string InternalServerError = "500";
        }
    }
}
