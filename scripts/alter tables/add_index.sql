use sfils;
alter table patrons
add index Patron_Type (patron_type_code);

alter table patrons
add index Notif_Type (notif_pref_code);

alter table patrons
add index Year_Reg (year_reg);