load data local infile "C:/ProgramData/MySQL/MySQL Server 8.0/Uploads/SFILS_comma_delim.csv"
	into table sfils.master
    fields terminated by ',' enclosed by '"'
    lines terminated by '\r\n'
    ignore 1 lines;