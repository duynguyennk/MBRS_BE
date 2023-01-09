namespace MBRS_API_DEMO.Models
{
    public class User
    {
        public int? AccountID { get; set; }
        public String? UserName { get; set; }
        public String? Password { get; set; }
        public String? Email { get; set; }
        public String? DepartmentCode { get; set; }
        public String? FullName { get; set; }
        public String? DepartmentName { get; set; }
        public String? Role { get; set; }
        public string? Token { get; set; }
    }
}
