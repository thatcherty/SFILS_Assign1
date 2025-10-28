create table sfils.master (
	patron_type_code int,
    patron_type varchar(25),
    total_checkouts int,
    total_renewals int,
    age_range varchar(25),
    home_library_code varchar(25),
    home_library varchar(25),
    circ_active_mo varchar(25),
    circ_active_yr int,
    notif_pref_code varchar(25),
    notif_pref varchar(25),
    provided_email varchar(25),
    within_county varchar(25),
    year_reg int
)