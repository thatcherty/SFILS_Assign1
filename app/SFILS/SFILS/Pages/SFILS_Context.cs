using Microsoft.EntityFrameworkCore;
using SFILS.Pages;

namespace SFILS.Pages
{
    public class SFILS_Context : DbContext
    {
        public SFILS_Context(DbContextOptions<SFILS_Context> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder b)
        {
            // ---------- Lookup tables ----------
            b.Entity<PatronTypes>(e =>
            {
                e.ToTable("patron_types");
                e.HasKey(x => x.Patron_Type_Code);
                e.Property(x => x.Patron_Type_Code).HasColumnName("patron_type_code");
                e.Property(x => x.Patron_Type).HasColumnName("patron_type");
            });

            b.Entity<AgeRanges>(e =>
            {
                e.ToTable("age_ranges");
                e.HasKey(x => x.Age_Range_Code);
                e.Property(x => x.Age_Range_Code).HasColumnName("age_range_code");
                e.Property(x => x.Age_Range).HasColumnName("age_range");
            });

            b.Entity<HomeLibraries>(e =>
            {
                e.ToTable("home_libraries");
                e.HasKey(x => x.Home_Library_Code);
                e.Property(x => x.Home_Library_Code).HasColumnName("home_library_code").HasMaxLength(64);
                e.Property(x => x.Home_Library).HasColumnName("home_library");
            });

            b.Entity<Notification_Pref>(e =>
            {
                e.ToTable("notification_pref"); // ensure this matches the actual table name
                e.HasKey(x => x.Notif_Pref_Code);
                e.Property(x => x.Notif_Pref_Code).HasColumnName("notif_pref_code").HasMaxLength(64);
                e.Property(x => x.Notif_Pref).HasColumnName("notif_pref");
            });

            // ---------- Patrons ----------
            b.Entity<Patron>(e =>
            {
                e.ToTable("patrons");
                e.HasKey(x => x.Patron_Id);
                e.Property(x => x.Patron_Id).HasColumnName("patron_id");

                e.Property(x => x.Patron_Type_Code).HasColumnName("patron_type_code");
                e.Property(x => x.Age_Range_Code).HasColumnName("age_range_code");
                e.Property(x => x.Home_Library_Code).HasColumnName("home_library_code");
                e.Property(x => x.Notif_Pref_Code).HasColumnName("notif_pref_code");

                // keep as strings per your model; mark optional if DB allows NULLs
                e.Property(x => x.Provided_Email).HasColumnName("prov_email");
                e.Property(x => x.Within_County).HasColumnName("in_county");

                e.Property(x => x.Year_Reg).HasColumnName("year_reg").IsRequired(false);
                e.Property(x => x.Total_Checkouts).HasColumnName("total_checkouts");
                e.Property(x => x.Total_Renewals).HasColumnName("total_renewals");
                e.Property(x => x.Circ_Active_Mo).HasColumnName("circ_active_mo").IsRequired(false);
                e.Property(x => x.Circ_Active_Yr).HasColumnName("circ_active_yr").IsRequired(false);

                // Relationships (FK-by-code)
                e.HasOne(x => x.Patron_Type)
                 .WithMany(pt => pt.Patrons)
                 .HasForeignKey(x => x.Patron_Type_Code)
                 .HasPrincipalKey(pt => pt.Patron_Type_Code)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Age_Range)
                 .WithMany(ar => ar.Patrons)
                 .HasForeignKey(x => x.Age_Range_Code)
                 .HasPrincipalKey(ar => ar.Age_Range_Code)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Home_Library)
                 .WithMany(h => h.Patrons)
                 .HasForeignKey(x => x.Home_Library_Code)
                 .HasPrincipalKey(h => h.Home_Library_Code)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Notification_Pref)
                 .WithMany(n => n.Patrons)
                 .HasForeignKey(x => x.Notif_Pref_Code)
                 .HasPrincipalKey(n => n.Notif_Pref_Code)
                 .OnDelete(DeleteBehavior.Restrict);
            });
        }
        public DbSet<SFILS.Pages.Patron> Patron { get; set; } = default!;
        public DbSet<PatronTypes> PatronTypes { get; set; } = null!;
        public DbSet<AgeRanges> AgeRanges { get; set; } = null!;
        public DbSet<HomeLibraries> HomeLibraries { get; set; } = null!;
        public DbSet<Notification_Pref> Notification_Pref { get; set; } = null!;
    }
}
 