insert into sfils.age_ranges (age_range)
select distinct age_range from sfils.master as m
order by m.age_range
