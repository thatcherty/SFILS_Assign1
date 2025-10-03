using Microsoft.EntityFrameworkCore;
using SFILS.Pages;

namespace SFILS.Pages
{
    public class SFILS_Context : DbContext
    {
        public SFILS_Context(DbContextOptions<SFILS_Context> options) : base(options)
        {
        }

        protected SFILS_Context()
        {
        }
        public DbSet<SFILS.Pages.Patron> Patron { get; set; } = default!;
    }
}
 