create table sfils.patrons (
	patron_id int NOT NULL AUTO_INCREMENT,
    patron_type_code int, -- foreign key
    age_range_code int, -- foreign key
    home_library_code varchar(25), -- foreign key
    notif_pref_code varchar(25), -- foreign key
    provided_email varchar(25),
    within_county varchar(25),
    year_reg varchar(25),
    total_checkouts int, -- to drop
    total_renewals int, -- to drop
    circ_active_mo varchar(25), -- to drop
    circ_active_yr varchar(25), -- to drop
    primary key (patron_id),
    foreign key (patron_type_code) references patron_types(patron_type_code),
    foreign key (home_library_code) references home_libraries(home_library_code),
    foreign key (notif_pref_code) references notification_pref(notif_pref_code),
    foreign key (age_range_code) references age_ranges(age_range_code)
)