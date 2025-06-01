/*

    CREATE DATABASE

*/
print '' print '-----' print '' print 'DATABASE CREATION SECTION' print '' print '-----'
GO

print 'Dropping Imitation Database'
DROP DATABASE IF EXISTS Imitation
GO

print '' print 'Creating Imitation Database'
CREATE DATABASE Imitation
GO

:r Tables.sql
:r Insert.sql
:r UserSP.sql
:r TokenSP.sql
:r LogSP.sql
:r PlatformSP.sql
:r ApplicationSP.sql
:r APISP.sql
:r TicketSP.sql
