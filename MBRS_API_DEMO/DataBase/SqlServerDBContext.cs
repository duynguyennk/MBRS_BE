using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace MBRS_API_DEMO.DataBase
{
    public partial class SqlServerDBContext : DbContext
    {
        public SqlServerDBContext()
        {

        }
        public SqlServerDBContext(DbContextOptions<SqlServerDBContext> options) : base(options)
        {

        }
    }
}
