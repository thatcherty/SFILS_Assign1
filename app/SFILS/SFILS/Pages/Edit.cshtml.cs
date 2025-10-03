using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace SFILS.Pages
{
    public class EditModel : PageModel
    {
        private readonly SFILS.Pages.SFILS_Context _context;

        public EditModel(SFILS.Pages.SFILS_Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Patron Patron { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patron =  await _context.Patron.FirstOrDefaultAsync(m => m.Patron_Id == id);
            if (patron == null)
            {
                return NotFound();
            }
            Patron = patron;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Patron).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatronExists(Patron.Patron_Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PatronExists(int id)
        {
            return _context.Patron.Any(e => e.Patron_Id == id);
        }
    }
}
