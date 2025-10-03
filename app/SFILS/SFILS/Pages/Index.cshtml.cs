using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace SFILS.Pages
{
    public class IndexModel : PageModel
    {
        private readonly SFILS.Pages.SFILS_Context _context;

        public IndexModel(SFILS.Pages.SFILS_Context context)
        {
            _context = context;
        }

        public IList<Patron> Patron { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Patron = await _context.Patron.AsNoTracking()
                                .OrderBy(p => p.Patron_Id)
                                .Take(50)
                                .ToListAsync();
        }
    }
}
