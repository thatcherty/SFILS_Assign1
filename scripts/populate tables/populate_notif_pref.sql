insert into sfils.notification_pref
select distinct notif_pref_code, notif_pref from sfils.master as m
order by m.notif_pref_code