using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API_DEMO.Models.ViewModels
{
    public class EmployeeViewModel
    {
        [Column("employee_id")]
        public int employeeID { get; set; }

        [Column("full_name")]
        public string fullName { get; set; }

        [Column("phone_number")]
        public string phoneNumber { get; set; }

        [Column("day_of_birth")]
        public DateTime dateOfBirth { get; set; }

        [Column("cccd")]
        public string identifyNumber { get; set; }

        [Column("account_id")]
        public int accountID { get; set; }

        [Column("user_name")]
        public string userName { get; set; }

        [Column("department_id")]
        public int departmentID { get; set; }

        [Column("email")]
        public string email { get; set; }

        [Column("role")]
        public string? role { get; set; }

        [Column("Total")]
        public int Total { get; set; }
    }
}
