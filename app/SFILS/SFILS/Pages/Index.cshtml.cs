using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SFILS.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Mongo_Context _db;
        public IndexModel(Mongo_Context db) => _db = db;

       
        [BindProperty(SupportsGet = true)] public int pageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)] public int pageSize { get; set; } = 20;
        
        [BindProperty(SupportsGet = true)] public string? sort { get; set; }

        
        [BindProperty(SupportsGet = true)] public string? PatronIdFilter { get; set; }
        [BindProperty(SupportsGet = true)] public string? ProvidedFilter { get; set; } 
        [BindProperty(SupportsGet = true)] public int? TypeCodeFilter { get; set; }   
        [BindProperty(SupportsGet = true)] public int? AgeCodeFilter { get; set; }   
        [BindProperty(SupportsGet = true)] public string? LibraryCodeFilter { get; set; }   
        [BindProperty(SupportsGet = true)] public string? NotifCodeFilter { get; set; }   

        
        [BindProperty(SupportsGet = true)] public string? WithinCountyFilter { get; set; } 
        [BindProperty(SupportsGet = true)] public string? YearRegFilter { get; set; } 
        [BindProperty(SupportsGet = true)] public int? TotalCheckoutsFilter { get; set; } 
        [BindProperty(SupportsGet = true)] public int? TotalRenewalsFilter { get; set; } 
        [BindProperty(SupportsGet = true)] public string? CircActiveMoFilter { get; set; } 
        [BindProperty(SupportsGet = true)] public string? CircActiveYrFilter { get; set; } 

        
        public int TotalCount { get; set; }
        public int TotalPages => Math.Max(1, (int)Math.Ceiling((double)TotalCount / Math.Max(1, pageSize)));
        public IList<Patron> Patron { get; set; } = new List<Patron>();

        
        public List<(int Code, string Name)> TypeOptions { get; set; } = new();
        public List<(int Code, string Name)> AgeOptions { get; set; } = new();
        public List<(string Code, string Name)> LibraryOptions { get; set; } = new();
        public List<(string Code, string Name)> NotifOptions { get; set; } = new();

        public async Task OnGetAsync()
        {
            if (pageIndex < 1) pageIndex = 1;
            if (pageSize <= 0) pageSize = 20;

            
            var types = await _db.PatronTypes.AsNoTracking().OrderBy(t => t.Patron_Type).ToListAsync();
            var ages = await _db.AgeRanges.AsNoTracking().OrderBy(a => a.Age_Range).ToListAsync();
            var libs = await _db.HomeLibraries.AsNoTracking().OrderBy(h => h.Home_Library).ToListAsync();
            var prefs = await _db.Notification_Pref.AsNoTracking().OrderBy(n => n.Notif_Pref).ToListAsync();

            TypeOptions = types.Select(t => (t.Patron_Type_Code, t.Patron_Type)).ToList();
            AgeOptions = ages.Select(a => (a.Age_Range_Code, a.Age_Range)).ToList();
            LibraryOptions = libs.Select(h => (h.Home_Library_Code, h.Home_Library)).ToList();
            NotifOptions = prefs.Select(n => (n.Notif_Pref_Code, n.Notif_Pref)).ToList();

            
            IQueryable<Patron> baseQuery = _db.Patron.AsNoTracking();

            
            if (!string.IsNullOrWhiteSpace(PatronIdFilter) &&
                int.TryParse(PatronIdFilter, out var pid))
            {
                baseQuery = baseQuery.Where(p => p.Patron_Id == pid);
            }

            if (!string.IsNullOrWhiteSpace(ProvidedFilter))
            {
                var val = ProvidedFilter.Trim().ToLowerInvariant();
                if (val == "yes") baseQuery = baseQuery.Where(p => p.Provided_Email == 1);
                else if (val == "no") baseQuery = baseQuery.Where(p => p.Provided_Email == 0);
            }

            if (TypeCodeFilter.HasValue)
                baseQuery = baseQuery.Where(p => p.Patron_Type_Code == TypeCodeFilter.Value);

            if (AgeCodeFilter.HasValue)
                baseQuery = baseQuery.Where(p => p.Age_Range_Code == AgeCodeFilter.Value);

            if (!string.IsNullOrWhiteSpace(LibraryCodeFilter))
                baseQuery = baseQuery.Where(p => p.Home_Library_Code == LibraryCodeFilter);

            if (!string.IsNullOrWhiteSpace(NotifCodeFilter))
                baseQuery = baseQuery.Where(p => p.Notif_Pref_Code == NotifCodeFilter);

            if (!string.IsNullOrWhiteSpace(WithinCountyFilter))
            {
                var v = WithinCountyFilter.Trim().ToLowerInvariant();
                if (v == "yes") baseQuery = baseQuery.Where(p => p.Within_County == 1);
                else if (v == "no") baseQuery = baseQuery.Where(p => p.Within_County == 0);
            }

            if (!string.IsNullOrWhiteSpace(YearRegFilter))
                baseQuery = baseQuery.Where(p => p.Year_Reg != null && p.Year_Reg.Contains(YearRegFilter));

            if (TotalCheckoutsFilter.HasValue)
                baseQuery = baseQuery.Where(p => p.Total_Checkouts >= TotalCheckoutsFilter.Value);

            if (TotalRenewalsFilter.HasValue)
                baseQuery = baseQuery.Where(p => p.Total_Renewals >= TotalRenewalsFilter.Value);

            if (!string.IsNullOrWhiteSpace(CircActiveMoFilter))
                baseQuery = baseQuery.Where(p => p.Circ_Active_Mo != null && p.Circ_Active_Mo.Contains(CircActiveMoFilter));

            if (!string.IsNullOrWhiteSpace(CircActiveYrFilter))
                baseQuery = baseQuery.Where(p => p.Circ_Active_Yr != null && p.Circ_Active_Yr.Contains(CircActiveYrFilter));

            
            TotalCount = await baseQuery.CountAsync();

            // Sorting
            baseQuery = (sort?.ToLowerInvariant()) switch
            {
                "provided" => baseQuery.OrderByDescending(p => p.Provided_Email).ThenBy(p => p.Patron_Id),
                "patron_id" => baseQuery.OrderBy(p => p.Patron_Id),
                _ => baseQuery.OrderBy(p => p.Patron_Id)
            };

            // Page patrons FIRST
            var rows = await baseQuery
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (rows.Count == 0)
            {
                Patron = rows;
                return;
            }

            // Hydrate lookups just for this page
            var typeCodesPage = rows.Select(p => p.Patron_Type_Code).Distinct().ToList();
            var ageCodesPage = rows.Select(p => p.Age_Range_Code).Distinct().ToList();
            var libCodesPage = rows.Select(p => p.Home_Library_Code).Distinct().ToList();
            var prefCodesPage = rows.Select(p => p.Notif_Pref_Code).Distinct().ToList();

            var typesMap = types.Where(t => typeCodesPage.Contains(t.Patron_Type_Code))
                                .ToDictionary(t => t.Patron_Type_Code);
            var agesMap = ages.Where(a => ageCodesPage.Contains(a.Age_Range_Code))
                               .ToDictionary(a => a.Age_Range_Code);
            var libsMap = libs.Where(h => libCodesPage.Contains(h.Home_Library_Code))
                               .ToDictionary(h => h.Home_Library_Code);
            var prefsMap = prefs.Where(n => prefCodesPage.Contains(n.Notif_Pref_Code))
                                .ToDictionary(n => n.Notif_Pref_Code);

            foreach (var p in rows)
            {
                if (typesMap.TryGetValue(p.Patron_Type_Code, out var t)) p.Patron_Type = t;
                if (agesMap.TryGetValue(p.Age_Range_Code, out var a)) p.Age_Range = a;
                if (libsMap.TryGetValue(p.Home_Library_Code, out var h)) p.Home_Library = h;
                if (prefsMap.TryGetValue(p.Notif_Pref_Code, out var n)) p.Notification_Pref = n;
            }

            Patron = rows;
        }
    }
}
