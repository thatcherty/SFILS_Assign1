using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFILS.Pages
{
    public class CreateModel(SFILS_Context db) : PageModel
    {
        [BindProperty] public Patron Patron { get; set; } = new();

        public SelectList PatronTypeOptions { get; private set; } = null!;
        public SelectList AgeRangeOptions { get; private set; } = null!;
        public SelectList HomeLibraryOptions { get; private set; } = null!;
        public SelectList NotificationPrefOptions { get; private set; } = null!;

        public async Task OnGetAsync() => await LoadLookupsAsync();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadLookupsAsync();
                return Page();
            }

            db.Patron.Add(Patron);
            try
            {
                await db.SaveChangesAsync();
                return RedirectToPage("Index");
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, $"Create failed: {ex.Message}");
                await LoadLookupsAsync();
                return Page();
            }
        }

        private async Task LoadLookupsAsync()
        {
            PatronTypeOptions = new SelectList(
                await db.PatronTypes.AsNoTracking().OrderBy(x => x.Patron_Type).ToListAsync(),
                nameof(SFILS.Pages.PatronTypes.Patron_Type_Code),
                nameof(SFILS.Pages.PatronTypes.Patron_Type));

            AgeRangeOptions = new SelectList(
                await db.AgeRanges.AsNoTracking().OrderBy(x => x.Age_Range).ToListAsync(),
                nameof(SFILS.Pages.AgeRanges.Age_Range_Code),
                nameof(SFILS.Pages.AgeRanges.Age_Range));

            HomeLibraryOptions = new SelectList(
                await db.HomeLibraries.AsNoTracking().OrderBy(x => x.Home_Library_Code).ToListAsync(),
                nameof(SFILS.Pages.HomeLibraries.Home_Library_Code),
                nameof(SFILS.Pages.HomeLibraries.Home_Library_Code));

            NotificationPrefOptions = new SelectList(
                await db.Notification_Pref.AsNoTracking().OrderBy(x => x.Notif_Pref).ToListAsync(),
                nameof(SFILS.Pages.Notification_Pref.Notif_Pref_Code),
                nameof(SFILS.Pages.Notification_Pref.Notif_Pref));
        }
    }
}