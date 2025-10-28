Insert into sfils.patrons (patron_type_code, age_range_code, home_library_code, notif_pref_code, provided_email, within_county, year_reg, total_checkouts, total_renewals, circ_active_mo, circ_active_yr) 
SELECT m.patron_type_code, a.age_range_code, m.home_library_code, m.notif_pref_code, m.provided_email, m.within_county, m.year_reg, m.total_checkouts, m.total_renewals, m.circ_active_mo, m.circ_active_yr
FROM sfils.master as m
Inner Join age_ranges as a on m.age_range = a.age_range
