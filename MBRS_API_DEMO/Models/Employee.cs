using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API_DEMO.Models
{
    public class Employee
    {
        [Column("employee_id")]
        public int employeeID { get; set; }

        [Column("full_name")]
        public string fullName { get; set; }

        [Column("phone_number")]
        public string phoneNumber { get; set; }

        [Column("day_of_birth")]
        public string dateOfBirth { get; set; }

        [Column("cccd")]
        public string identifyNumber { get; set; }

        [Column("account_id")]
        public int accountID { get; set; }

        [Column("user_name")]
        public string userName { get; set; }

        [Column("department_name")]
        public string departmentName { get; set; }

    }
}
