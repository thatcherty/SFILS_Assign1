using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SFILS.Pages
{
    public class CreateModel : PageModel
    {
        private readonly SFILS.Pages.SFILS_Context _context;

        public CreateModel(SFILS.Pages.SFILS_Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Patron Patron { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Patron.Add(Patron);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
