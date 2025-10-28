# Performance
Based on the average of the following queries:
 - 159 rows/sec

## Adding Indexes
After adding an index on Patron Type:
 - [Count of patron types by age range](#Count-of-patron-types-by-age-range) performance did not improve
 - [Patron type by age ranges in county](#Patron-type-by-age-ranges-in-county) performance did not improve

After adding an index on Notif Type:
 - [Customers who prefer print notifications](#Customers-who-prefer-print-notifications) provided 0sec query compared to 0.015
 - [Count of patron types by notification preference](#Count-of-patron-types-by-notification-preference) provided between 0.015 - 0.032 sec queries compared to 1.047 sec

## Table View
| Query | Before | After | Change | Notes |
|:--|--:|--:|--:|:--|
| **Count of patron types by age range** | 1.000 | 1.000 | 0% | No improvement; only `patron_type_code` indexed — `age_range_code` not indexed or composite. |
| **Patron type by age ranges in county** | 0.172 | 0.172 | 0% | No improvement; could improve with composite `(patron_type_code, age_range_code)` or index on `age_range_code`. |
| **Customers who prefer print notifications** | 0.015 | **0.000** | **−100%** | Major improvement; benefited from index on `notif_pref_code`. |
| **Count of patron types by notification preference** | 1.047 | **0.032** | **−96.9%** | Major improvement; both `patron_type_code` and  `notif_pref_code` index used. |


## Conclusions
In both Patron Type queries, the other columns were not indexed; however, in the second Notif Type query, both patron types and notif type were indexed. This may have been the cause of the improvement. For the first notif type query, it is simply getting a count and only looking for notif pref.

# Initial Query Results

## Returning customers
- Performance: 0.125 sec
- Rows returned: 1
```SQL
select count(patron_id) as Returning_Customer from patrons as p
where p.total_checkouts > 0;
```
## Customers who renewed
- Performance: 0.141 sec
- Rows returned: 1
```SQL
select count(patron_id) as Customer_Renewals from patrons as p
where p.total_renewals > 0;
```
## Customers in the county
- Performance: 0.094 sec
- Rows returned: 1
```SQL
select count(patron_id) as Customers_in_County from patrons as p
where p.in_county = 1;
```
## Customers who prefer print notifications
- Performance: 0.015
- Rows returned: 1
```SQL
select count(patron_id) as Print_Pref from patrons as p
inner join notification_pref as n on p.notif_pref_code = n.notif_pref_code
where n.notif_pref = 'Print';
```
## Count of each activation year
- Performance: 0.203 sec
- Rows returned: 21
```SQL
select circ_active_yr as Active_Year, count(patron_id) as Patron_count from patrons as p
group by p.circ_active_yr
order by circ_active_yr;
```
## Count of each activation month and year
- Performance: 0.313
- Rows returned: 217
```SQL
select circ_active_mo as Active_Month, circ_active_yr as Active_Year, count(patron_id) as Patron_count from patrons as p
group by p.circ_active_mo, p.circ_active_yr
order by Patron_count desc, circ_active_yr;
```
## Count of each age range
- Performance: 0.219 sec
- Rows returned: 11
```SQL
select  a.age_range as Age_Range, count(patron_id) as Patron_count from patrons as p
inner join age_ranges as a on p.age_range_code = a.age_range_code
group by a.age_range
order by Patron_Count desc;
```
## Count of each home library
- Performance: 0.297 sec
- Rows returned: 30
```SQL
select  h.home_library as Home_Library, count(patron_id) as Patron_count from patrons as p
inner join home_libraries as h on p.home_library_code = h.home_library_code
group by h.home_library
order by Patron_Count desc;
```
## Count of each patron type
- Performance: 0.140 sec
- Rows returned: 18
```SQL
select  pa.patron_type as Patron_Type, count(patron_id) as Patron_count from patrons as p
inner join patron_types as pa on p.patron_type_code = pa.patron_type_code
group by pa.patron_type
order by Patron_count desc;
```
## Count of patron types who want print notifications
- Time: 0.032 sec
- Rows returned: 14
```SQL
select pa.patron_type as Patron_Type, count(patron_id) as Print_Pref_Count from patrons as p
inner join notification_pref as n on p.notif_pref_code = n.notif_pref_code
inner join patron_types as pa on pa.patron_type_code = p.patron_type_code
where n.notif_pref = 'Print'
group by pa.patron_type
order by Print_Pref_Count desc;
```
## Count of patron types by notification preference
- Time: 1.047 sec
- Rows returned: 57
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
## Count of patron types by age range
- Time: 1 sec
- Rows returned: 134 
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
## Patron type by age ranges in county
- Time: 0.172 seconds
- Rows returned: 97
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

