using Microsoft.EntityFrameworkCore;

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
    }
}
 