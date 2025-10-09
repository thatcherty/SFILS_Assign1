using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFILS.Pages
{
    [Table("patrons")]
    public class Patron
    {
        [Key]
        [Column("patron_id")]
        public int Patron_Id { get; set; }

        [Column("patron_type_code")]
        public int Patron_Type_Code { get; set; }

        [Column("age_range_code")]
        public int Age_Range_Code { get; set; }

        [Column("home_library_code")]
        public string Home_Library_Code { get; set; } = null!;

        [Column("notif_pref_code")]
        public string Notif_Pref_Code { get; set; } = null!;

        [Column("prov_email")]
        public bool Provided_Email { get; set; }

        [Column("in_county")]
        public bool Within_County { get; set; } 

        [Column("year_reg")]
        public string Year_Reg { get; set; } = null!;

        [Column("total_checkouts")]
        public int Total_Checkouts { get; set; }

        [Column("total_renewals")]
        public int Total_Renewals { get; set; }

        [Column("circ_active_mo")]
        public string? Circ_Active_Mo { get; set; }

        [Column("circ_active_yr")]
        public string? Circ_Active_Yr { get; set; }

        [ForeignKey(nameof(Patron_Type_Code))]
        public PatronTypes Patron_Type { get; set; } = null!;

        [ForeignKey(nameof(Age_Range_Code))]
        public AgeRanges Age_Range { get; set; } = null!;

        [ForeignKey(nameof(Home_Library_Code))]
        public HomeLibraries Home_Library { get; set; } = null!;

        [ForeignKey(nameof(Notif_Pref_Code))]
        public Notification_Pref Notification_Pref { get; set; } = null!;
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
