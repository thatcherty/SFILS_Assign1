using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace SFILS.Pages
{
    public class IndexModel(SFILS_Context db) : PageModel
    {
        public sealed record Row(
            int PatronId,
            string PatronType,
            string AgeRange,
            string HomeLibrary,
            string NotifPref,
            bool ProvidedEmail,
            bool WithinCounty,
            string YearReg,
            int TotalCheckouts,
            int TotalRenewals);

        public IReadOnlyList<Row> Rows { get; private set; } = [];

        // Paging state exposed to the view
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        public async Task OnGetAsync(int pageNum = 1, int pageSize = 50)
        {
            PageNumber = Math.Max(1, pageNum);
            PageSize = Math.Clamp(pageSize, 10, 200); // keep it reasonable

            var baseQuery = db.Patron
                .AsNoTracking()
                .Include(p => p.Patron_Type)
                .Include(p => p.Age_Range)
                .Include(p => p.Home_Library)
                .Include(p => p.Notification_Pref);

            TotalCount = await baseQuery.CountAsync();

            Rows = await baseQuery
                .OrderBy(p => p.Patron_Id)
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .Select(p => new Row(
                    p.Patron_Id,
                    p.Patron_Type.Patron_Type,
                    p.Age_Range.Age_Range,
                    p.Home_Library.Home_Library,
                    p.Notification_Pref.Notif_Pref,
                    p.Provided_Email,
                    p.Within_County,
                    p.Year_Reg,
                    p.Total_Checkouts,
                    p.Total_Renewals))
                .ToListAsync();
        }
    }
}