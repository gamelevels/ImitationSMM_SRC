print '' print 'Using Imitation Database'
USE Imitation
GO

print '' print 'Creating sp_select_application_information'
GO
CREATE PROCEDURE sp_select_application_information
AS
BEGIN
    SELECT
        [MOTD], [AppVersion], [TOS], [Help]
    FROM ApplicationInfo
END
GO