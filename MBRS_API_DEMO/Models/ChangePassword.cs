using System.ComponentModel.DataAnnotations.Schema;

namespace MBRS_API.Models
{
    public class ChangePassword
    {
        public String userName { get; set; }

        public String oldPassword { get; set; }

        public String newPassword { get; set; }
    }
}
