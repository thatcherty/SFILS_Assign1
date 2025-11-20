using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFILS.Pages
{
    [Table("patrons")]
    public class Patron
    {
        [Key]
        [Column("patron_id")]
        [Display(Name = "ID")]
        public int Patron_Id { get; set; }

        [Column("patron_type_code")]
        [Display(Name = "Patron Type Code")]
        public int Patron_Type_Code { get; set; }

        [Column("age_range_code")]
        [Display(Name = "Age Range Code")]
        public int Age_Range_Code { get; set; }

        [Column("home_library_code")]
        [Display(Name = "Home Library Code")]
        public string Home_Library_Code { get; set; } = null!;

        [Column("notif_pref_code")]
        [Display(Name = "Notification Pref Code")]
        public string Notif_Pref_Code { get; set; } = null!;

        [Column("prov_email")]
        [Display(Name = "Provided Email")]
        public bool Provided_Email { get; set; }

        [Column("in_county")]
        [Display(Name = "Within County")]
        public bool Within_County { get; set; } 

        [Column("year_reg")]
        [Display(Name = "Year Registered")]
        public string Year_Reg { get; set; } = null!;

        [Column("total_checkouts")]
        [Display(Name = "Total Checkouts")]
        public int Total_Checkouts { get; set; }

        [Column("total_renewals")]
        [Display(Name = "Total Renewals")]
        public int Total_Renewals { get; set; }

        [Column("circ_active_mo")]
        [Display(Name = "Circulation Active Month")]
        public string? Circ_Active_Mo { get; set; }

        [Column("circ_active_yr")]
        [Display(Name = "Circulation Active Year")]
        public string? Circ_Active_Yr { get; set; }

        
        [ForeignKey(nameof(Patron_Type_Code))]
        [ValidateNever]                      
        public PatronTypes? Patron_Type { get; set; }

        [ForeignKey(nameof(Age_Range_Code))]
        [ValidateNever]
        public AgeRanges? Age_Range { get; set; }

        [ForeignKey(nameof(Home_Library_Code))]
        [ValidateNever]
        public HomeLibraries? Home_Library { get; set; }

        [ForeignKey(nameof(Notif_Pref_Code))]
        [ValidateNever]
        public Notification_Pref? Notification_Pref { get; set; }

    }

    [Table("patron_types")]
    public class PatronTypes
    {
        [Key]
        [Column("patron_type_code")]
        public int Patron_Type_Code { get; set; }

        [Column("patron_type")]
        public string Patron_Type { get; set; } = null!;

        public ICollection<Patron> Patrons { get; set; } = new List<Patron>();
    }

    [Table("age_ranges")]
    public class AgeRanges
    {
        [Key]
        [Column("age_range_code")]
        public int Age_Range_Code { get; set; }

        [Column("age_range")]
        public string Age_Range { get; set; } = null!;

        public ICollection<Patron> Patrons { get; set; } = new List<Patron>();
    }

    [Table("home_libraries")]
    public class HomeLibraries
    {
        [Key]
        [Column("home_library_code")]
        public string Home_Library_Code { get; set; } = null!;

        [Column("home_library")]
        public string Home_Library { get; set; } = null!;

        public ICollection<Patron> Patrons { get; set; } = new List<Patron>();
    }

    [Table("notification_pref")]
    public class Notification_Pref
    {
        [Key]
        [Column("notif_pref_code")]
        public string Notif_Pref_Code { get; set; } = null!;

        [Column("notif_pref")]
        public string Notif_Pref { get; set; } = null!;

        public ICollection<Patron> Patrons { get; set; } = new List<Patron>();
    }
}
