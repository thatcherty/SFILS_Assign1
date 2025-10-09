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
            await db.SaveChangesAsync();
            return RedirectToPage("Index");
        }

        private async Task LoadLookupsAsync()
        {
            PatronTypeOptions = new SelectList(
                await db.PatronTypes.AsNoTracking()
                    .OrderBy(x => x.Patron_Type).ToListAsync(),
                nameof(PatronTypes.Patron_Type_Code),
                nameof(PatronTypes.Patron_Type));

            AgeRangeOptions = new SelectList(
                await db.AgeRanges.AsNoTracking()
                    .OrderBy(x => x.Age_Range).ToListAsync(),
                nameof(AgeRanges.Age_Range_Code),
                nameof(AgeRanges.Age_Range));

            HomeLibraryOptions = new SelectList(
                await db.HomeLibraries.AsNoTracking()
                    .OrderBy(x => x.Home_Library).ToListAsync(),
                nameof(HomeLibraries.Home_Library_Code),
                nameof(HomeLibraries.Home_Library));

            NotificationPrefOptions = new SelectList(
                await db.Notification_Pref.AsNoTracking()
                    .OrderBy(x => x.Notif_Pref).ToListAsync(),
                nameof(Notification_Pref.Notif_Pref_Code),
                nameof(Notification_Pref.Notif_Pref));
        }
    }
}
