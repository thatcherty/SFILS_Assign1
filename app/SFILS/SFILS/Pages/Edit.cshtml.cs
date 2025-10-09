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
    public class EditModel(SFILS_Context db) : PageModel
    {
        [BindProperty] public Patron Patron { get; set; } = null!;

        public SelectList PatronTypeOptions { get; private set; } = null!;
        public SelectList AgeRangeOptions { get; private set; } = null!;
        public SelectList HomeLibraryOptions { get; private set; } = null!;
        public SelectList NotificationPrefOptions { get; private set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Patron = await db.Patron.FindAsync(id);
            if (Patron is null) return NotFound();

            await LoadLookupsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadLookupsAsync();
                return Page();
            }

            var existing = await db.Patron.FindAsync(Patron.Patron_Id);
            if (existing is null) return NotFound();


            existing.Patron_Type_Code = Patron.Patron_Type_Code;
            existing.Age_Range_Code = Patron.Age_Range_Code;
            existing.Home_Library_Code = Patron.Home_Library_Code;
            existing.Notif_Pref_Code = Patron.Notif_Pref_Code;

            existing.Provided_Email = Patron.Provided_Email; 
            existing.Within_County = Patron.Within_County;  

            existing.Year_Reg = Patron.Year_Reg;
            existing.Total_Checkouts = Patron.Total_Checkouts;
            existing.Total_Renewals = Patron.Total_Renewals;
            existing.Circ_Active_Mo = Patron.Circ_Active_Mo;
            existing.Circ_Active_Yr = Patron.Circ_Active_Yr;

            try
            {
                await db.SaveChangesAsync();
                return RedirectToPage("Index");
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, $"Save failed: {ex.Message}");
                await LoadLookupsAsync();
                return Page();
            }
        }

        private async Task LoadLookupsAsync()
        {
            PatronTypeOptions = new SelectList(
                await db.PatronTypes.AsNoTracking().OrderBy(x => x.Patron_Type).ToListAsync(),
                nameof(SFILS.Pages.PatronTypes.Patron_Type_Code),
                nameof(SFILS.Pages.PatronTypes.Patron_Type),
                Patron?.Patron_Type_Code);

            AgeRangeOptions = new SelectList(
                await db.AgeRanges.AsNoTracking().OrderBy(x => x.Age_Range).ToListAsync(),
                nameof(SFILS.Pages.AgeRanges.Age_Range_Code),
                nameof(SFILS.Pages.AgeRanges.Age_Range),
                Patron?.Age_Range_Code);

            HomeLibraryOptions = new SelectList(
                await db.HomeLibraries.AsNoTracking().OrderBy(x => x.Home_Library_Code).ToListAsync(),
                nameof(SFILS.Pages.HomeLibraries.Home_Library_Code),
                nameof(SFILS.Pages.HomeLibraries.Home_Library_Code),
                Patron?.Home_Library_Code);

            NotificationPrefOptions = new SelectList(
                await db.Notification_Pref.AsNoTracking().OrderBy(x => x.Notif_Pref).ToListAsync(),
                nameof(SFILS.Pages.Notification_Pref.Notif_Pref_Code),
                nameof(SFILS.Pages.Notification_Pref.Notif_Pref),
                Patron?.Notif_Pref_Code);
        }
    }
}
