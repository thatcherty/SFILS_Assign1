# Returning customers
- Finding: 297893
```SQL
select count(patron_id) as Returning_Customer from patrons as p
where p.total_checkouts > 0;
```
# Customers who renewed
- Finding: 248821
```SQL
select count(patron_id) as Customer_Renewals from patrons as p
where p.total_renewals > 0;
```
# Customers in the county
- Finding: 369259
```SQL
select count(patron_id) as Customers_in_County from patrons as p
where p.in_county = 1;
```
# Customers who prefer print notifications
- Finding: 7242
```SQL
select count(patron_id) as Print_Pref from patrons as p
inner join notification_pref as n on p.notif_pref_code = n.notif_pref_code
where n.notif_pref = 'Print';
```
# Count of each activation year
- Findings: 2022 had the most at 140032
```SQL
select circ_active_yr as Active_Year, count(patron_id) as Patron_count from patrons as p
group by p.circ_active_yr
order by circ_active_yr;
```
# Count of each activation month and year
- Findings: January 2023 had the most at 129621
```SQL
select circ_active_mo as Active_Month, circ_active_yr as Active_Year, count(patron_id) as Patron_count from patrons as p
group by p.circ_active_mo, p.circ_active_yr
order by Patron_count desc, circ_active_yr;
```
# Count of each age range
- Findings: 25 to 34 years have the most at 92790
```SQL
select  a.age_range as Age_Range, count(patron_id) as Patron_count from patrons as p
inner join age_ranges as a on p.age_range_code = a.age_range_code
group by a.age_range
order by Patron_Count desc;
```
# Count of each home library
- Findings: Main had the most at 143471
```SQL
select  h.home_library as Home_Library, count(patron_id) as Patron_count from patrons as p
inner join home_libraries as h on p.home_library_code = h.home_library_code
group by h.home_library
order by Patron_Count desc;
```
# Count of each patron type
- Findings: Adult has the most at 271588
```SQL
select  pa.patron_type as Patron_Type, count(patron_id) as Patron_count from patrons as p
inner join patron_types as pa on p.patron_type_code = pa.patron_type_code
group by pa.patron_type
order by Patron_count desc;
```
# Count of patron types who want print notifications
- Findings: Adults have the highest count of print preference at 3020 (they may have more Adults in general)
```SQL
select pa.patron_type as Patron_Type, count(patron_id) as Print_Pref_Count from patrons as p
inner join notification_pref as n on p.notif_pref_code = n.notif_pref_code
inner join patron_types as pa on pa.patron_type_code = p.patron_type_code
where n.notif_pref = 'Print'
group by pa.patron_type
order by Print_Pref_Count desc;
```
# Count of patron types by notification preference
- Findings: 
```SQL
select 
	pa.patron_type as Patron_Type, 
    n.notif_pref as Notification_Pref,
    count(*) as Notification_Type_Pref
from patrons as p
inner join notification_pref as n on p.notif_pref_code = n.notif_pref_code
inner join patron_types as pa on pa.patron_type_code = p.patron_type_code
group by pa.patron_type, n.notif_pref
order by Notification_Type_Pref desc;
```
# Count of patron types by age range
```SQL
select 
	pa.patron_type as Patron_Type, 
    a.age_range as Age_Range,
    count(*) as Age_Range_Count
from patrons as p
inner join age_ranges as a on p.age_range_code = a.age_range_code
inner join patron_types as pa on pa.patron_type_code = p.patron_type_code
group by pa.patron_type, a.age_range
order by Age_Range_Count desc;
```
# Patron type by age ranges in county
```SQL
select 
	pa.patron_type as Patron_Type, 
    a.age_range as Age_Range,
    count(*) as Age_Range_Count
from patrons as p
inner join age_ranges as a on p.age_range_code = a.age_range_code
inner join patron_types as pa on pa.patron_type_code = p.patron_type_code
where p.in_county = 0
group by pa.patron_type, a.age_range
order by Age_Range_Count desc;
```
