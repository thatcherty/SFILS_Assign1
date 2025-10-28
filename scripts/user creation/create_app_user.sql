CREATE USER IF NOT EXISTS 'sfils_reader'@'%' IDENTIFIED BY 'ReadOnlyPass!';
GRANT SELECT ON sfils.* TO 'sfils_reader'@'%';
FLUSH PRIVILEGES;