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
    public class IndexModel(SFILS_Context db) : PageModel
    {
        // records
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


        // pagination
        [BindProperty(SupportsGet = true)] public int pageNumber { get; set; } = 1;
        [BindProperty(SupportsGet = true)] public int pageSize { get; set; } = 50;

        public int TotalCount { get; private set; }
        public int TotalPages => TotalCount == 0 ? 1 : (int)Math.Ceiling(TotalCount / (double)pageSize);

        // filtering
        [BindProperty(SupportsGet = true)] public int? patronType { get; set; }
        [BindProperty(SupportsGet = true)] public int? ageRange { get; set; }
        [BindProperty(SupportsGet = true)] public string? homeLib { get; set; }
        [BindProperty(SupportsGet = true)] public string? notifPref { get; set; }
        [BindProperty(SupportsGet = true)] public bool? provEmail { get; set; }
        [BindProperty(SupportsGet = true)] public bool? inCounty { get; set; }
        [BindProperty(SupportsGet = true)] public int? yearMin { get; set; }
        [BindProperty(SupportsGet = true)] public int? yearMax { get; set; }
        [BindProperty(SupportsGet = true)] public int? patronId { get; set; }
        [BindProperty(SupportsGet = true)] public int? checkouts { get; set; }
        [BindProperty(SupportsGet = true)] public int? renewals { get; set; }

        public SelectList PatronTypeOptions { get; private set; } = null!;
        public SelectList AgeRangeOptions { get; private set; } = null!;
        public SelectList HomeLibraryOptions { get; private set; } = null!;
        public SelectList NotificationPrefOptions { get; private set; } = null!;

        // on load page
        public async Task OnGetAsync(int pageNum = 1, int pageSize = 50)
        {
            pageSize = Math.Clamp(pageSize, 10, 200);
            pageNumber = Math.Max(1, pageNumber);

            var baseQuery = db.Patron
                .AsNoTracking()
                .Include(p => p.Patron_Type)
                .Include(p => p.Age_Range)
                .Include(p => p.Home_Library)
                .Include(p => p.Notification_Pref)
                .AsQueryable();

            if (patronType is not null) baseQuery = baseQuery.Where(p => p.Patron_Type_Code == patronType);
            if (ageRange is not null) baseQuery = baseQuery.Where(p => p.Age_Range_Code == ageRange);
            if (!string.IsNullOrWhiteSpace(homeLib)) baseQuery = baseQuery.Where(p => p.Home_Library_Code == homeLib);
            if (!string.IsNullOrWhiteSpace(notifPref)) baseQuery = baseQuery.Where(p => p.Notif_Pref_Code == notifPref);
            if (provEmail is not null) baseQuery = baseQuery.Where(p => p.Provided_Email == provEmail);
            if (inCounty is not null) baseQuery = baseQuery.Where(p => p.Within_County == inCounty);
            if (patronId is not null) baseQuery = baseQuery.Where(p => p.Patron_Id == patronId);
            if (checkouts is not null) baseQuery = baseQuery.Where(p => p.Total_Checkouts >= checkouts);
            if (renewals is not null) baseQuery = baseQuery.Where(p => p.Total_Renewals >= renewals);
            // if (yearMin is not null) baseQuery = baseQuery.Where(p => p.Year_Reg >= yearMin);
            // if (yearMax is not null) baseQuery = baseQuery.Where(p => p.Year_Reg <= yearMax);

            TotalCount = await baseQuery.CountAsync();
            if (pageNumber > TotalPages) pageNumber = TotalPages;

            Rows = await baseQuery
                .OrderBy(p => p.Patron_Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
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

            // drop downs
            PatronTypeOptions = new SelectList(
                await db.PatronTypes.AsNoTracking().OrderBy(x => x.Patron_Type).ToListAsync(),
                nameof(PatronTypes.Patron_Type_Code), nameof(PatronTypes.Patron_Type), patronType);

            AgeRangeOptions = new SelectList(
                await db.AgeRanges.AsNoTracking().OrderBy(x => x.Age_Range).ToListAsync(),
                nameof(AgeRanges.Age_Range_Code), nameof(AgeRanges.Age_Range), ageRange);

            HomeLibraryOptions = new SelectList(
                await db.HomeLibraries.AsNoTracking().OrderBy(x => x.Home_Library).ToListAsync(),
                nameof(HomeLibraries.Home_Library_Code), nameof(HomeLibraries.Home_Library), homeLib);

            NotificationPrefOptions = new SelectList(
                await db.Notification_Pref.AsNoTracking().OrderBy(x => x.Notif_Pref).ToListAsync(),
                nameof(Notification_Pref.Notif_Pref_Code), nameof(Notification_Pref.Notif_Pref), notifPref);
        }
    }
}