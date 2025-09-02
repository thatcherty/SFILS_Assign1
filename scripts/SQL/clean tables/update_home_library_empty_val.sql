-- perform update
update sfils.master
set home_library_code = '0'
where home_library_code = ''

-- check where condition
select home_library_code, home_library
from sfils.master
where home_library_code = '' or home_library_code is null