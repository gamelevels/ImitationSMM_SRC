color 0a
@ECHO OFF
ECHO Creating Database

sqlcmd -S localhost -E -i Database.sql


rem server is localhost

ECHO Database created if no errors occured
PAUSE
