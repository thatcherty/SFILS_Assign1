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