using Microsoft.EntityFrameworkCore;

namespace StudentsApi.Models
{
    public class DBCONTEXT : DbContext
    {
        public DBCONTEXT()
        {

        }
        public DBCONTEXT(DbContextOptions<DBCONTEXT> options) : base(options)
        {
        }
        public DbSet<Students> students { get; set; }

    }
}
   

