using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace SFILS.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly Mongo_Context _context;
        public DetailsModel(Mongo_Context context) => _context = context;

        public Patron? Patron { get; private set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            if (!ObjectId.TryParse(id, out var oid)) return NotFound();

            // 1) Load the base entity (no Include)
            var p = await _context.Patron
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x._id == oid);

            if (p == null) return NotFound();

            // 2) Load related docs by their codes and hydrate nav-like properties
            p.Patron_Type = await _context.PatronTypes.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Patron_Type_Code == p.Patron_Type_Code);

            p.Age_Range = await _context.AgeRanges.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Age_Range_Code == p.Age_Range_Code);

            p.Home_Library = await _context.HomeLibraries.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Home_Library_Code == p.Home_Library_Code);

            p.Notification_Pref = await _context.Notification_Pref.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Notif_Pref_Code == p.Notif_Pref_Code);

            Patron = p;
            return Page();
        }
    }
}
