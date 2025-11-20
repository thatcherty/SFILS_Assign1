using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace SFILS.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly SFILS.Pages.SFILS_Context _context;

        public DetailsModel(SFILS.Pages.SFILS_Context context)
        {
            _context = context;
        }

        public Patron Patron { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patron = await _context.Patron.FirstOrDefaultAsync(m => m.Patron_Id == id);
            if (patron == null)
            {
                return NotFound();
            }
            else
            {
                Patron = patron;
            }
            return Page();
        }
    }
}
