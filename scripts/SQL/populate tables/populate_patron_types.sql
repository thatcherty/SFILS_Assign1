insert into sfils.patron_types
select distinct patron_type_code, patron_type from sfils.master as m
order by m.patron_type_code