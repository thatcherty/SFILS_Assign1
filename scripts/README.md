# Script Sequence
## Table Creation
First, I created the master table to hold the data from the main CSV sheet. This table has no constraints since it is only the source of the data.
```SQL
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
```

## Import Files
I used the master_import doc to insert all the rows from the CSV into the DB. 
The path used is likely not correct anymore, it was the location at the time of the inport
```SQL
load data local infile "C:/ProgramData/MySQL/MySQL Server 8.0/Uploads/SFILS_comma_delim.csv"
	into table sfils.master
    fields terminated by ',' enclosed by '"'
    lines terminated by '\r\n'
    ignore 1 lines;
```

## Table Creation
After importing the data, I determined how to split the data into normalized relations.

During my table creation, I opted to add the constraints during creation since I knew what they would be. This meant I needed to add the foreign key tables first.

## Patron Types
I utilized the patron type code as the primary key for this table.
```sql
create table sfils.patron_types (
	patron_type_code int NOT NULL,
    patron_type varchar(25),
    PRIMARY KEY (patron_type_code)
)
```

### Age Ranges Table
I added a primary (numerical) for the age ranges in this table.
```SQL
create table sfils.age_ranges (
	age_range_code int NOT NULL AUTO_INCREMENT,
    age_range varchar(25),
    PRIMARY KEY (age_range_code)
)
```

### Home Libraries Table
I used the library code as the primary code. I did notice that some libraries are associated with more than one code. The primary key attribute still works, but it does mean you get different filtering results if you consider the code.
```SQL
create table sfils.home_libraries (
	home_library_code varchar(25) NOT NULL,
    home_library varchar(25),
    PRIMARY KEY (home_library_code)
)
```

### Notification Preferences
I used the notif pref code as the primary key here.
```SQL
create table sfils.notification_pref (
	notif_pref_code varchar(25) NOT NULL,
    notif_pref varchar(25),
    PRIMARY KEY (notif_pref_code)
)
```

### Patron Table
This is the main table; it holds the primary key for each patron and unique details about each of them.

I added a primary key for each patron row, and also added a few keys to reference the 
```SQL
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
```

## Populate Tables
### Age Ranges
```SQL
insert into sfils.age_ranges (age_range)
select distinct age_range from sfils.master as m
order by m.age_range
```

### Home Library
```SQL
insert into sfils.home_libraries
select distinct home_library_code, home_library from sfils.master as m
order by m.home_library

-- deleted null home_library_code after insert
```

### Notification Preferences
```SQL
insert into sfils.notification_pref
select distinct notif_pref_code, notif_pref from sfils.master as m
order by m.notif_pref_code
```

### Patron Types
```SQL
insert into sfils.patron_types
select distinct patron_type_code, patron_type from sfils.master as m
order by m.patron_type_code
```

### Patrons
```SQL
Insert into sfils.patrons (patron_type_code, age_range_code, home_library_code, notif_pref_code, provided_email, within_county, year_reg, total_checkouts, total_renewals, circ_active_mo, circ_active_yr) 
SELECT m.patron_type_code, a.age_range_code, m.home_library_code, m.notif_pref_code, m.provided_email, m.within_county, m.year_reg, m.total_checkouts, m.total_renewals, m.circ_active_mo, m.circ_active_yr
FROM sfils.master as m
Inner Join age_ranges as a on m.age_range = a.age_range
```

## Clean Tables
### Home Library
There were some empty values in the home libraries column in the master table. I replaced them with 0's initially.
```SQL
-- perform update
update sfils.master
set home_library_code = '0'
where home_library_code = ''

-- check where condition
select home_library_code, home_library
from sfils.master
where home_library_code = '' or home_library_code is null
```

Once I inserted all the values into their destination tables, I changed the 0 to Unknown.

```SQL
insert into sfils.home_libraries
values ('0', 'Unknown');
```

## Alter Tables
### Boolean Values
Originally, I was going to leave the True and False strings that were in the CSV in the provided email and within the county columns. To allow for slightly more intuitive app use and queries, I made them tinyint boolean values.

```SQL
-- To make these columns boolean

ALTER TABLE sfils.patrons
ADD prov_email tinyint(1);

ALTER TABLE sfils.patrons
ADD in_county tinyint(1);

UPDATE sfils.patrons
set prov_email = case
	when provided_email = "TRUE" then 1
    else 0
end;

UPDATE sfils.patrons
set in_county = case
	when within_county = "TRUE" then 1
    else 0
end;

ALTER TABLE sfils.patrons
DROP COLUMN provided_email;

ALTER TABLE sfils.patrons
DROP COLUMN within_county;
```

I considered doing this with the month and year columns, but opted out. This was not impacting the ability to query; however, it would be beneficial to modify when ordering by date.

I decided to add a few indexes to the patrons table for values that may be used to commonly search.

```SQL
use sfils;
alter table patrons
add index Patron_Type (patron_type_code);

alter table patrons
add index Notif_Type (notif_pref_code);

alter table patrons
add index Year_Reg (year_reg);
```

## User Creation
After validating the app connection worked with the root user, I created a new DB user to limit access to read only.

```SQL
CREATE USER IF NOT EXISTS 'sfils_reader'@'%' IDENTIFIED BY 'ReadOnlyPass!';
GRANT SELECT ON sfils_prod.* TO 'sfils_reader'@'%';
FLUSH PRIVILEGES;
```
