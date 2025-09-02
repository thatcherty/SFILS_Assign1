insert into sfils.home_libraries
select distinct home_library_code, home_library from sfils.master as m
order by m.home_library

-- deleted null home_library_code after insert