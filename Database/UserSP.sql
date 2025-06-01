print '' print 'Using Imitation Database'
USE Imitation
GO

/*
    Authenticate User
    Select user info
    Select level info
    Insert new user
    Registered user count
*/


print '' print '-----' print '' print 'STORED PROCEDURE SECTION' print '' print '-----'
GO

print '' print 'Creating sp_authenticate_user'
GO
CREATE PROCEDURE sp_authenticate_user -- WORKS
(
	@Username		    NVARCHAR(64),
	@PasswordHash	    CHAR(64)
)
AS
    IF((SELECT Count([Username]) FROM Users 
            WHERE Username = @Username AND PasswordHash = @PasswordHash
		    AND [Enabled] = 1 and Expiration >= GETDATE()) = 0)
	BEGIN

        IF((SELECT Count([Username]) FROM Users WHERE @Username = Username) = 0)
        BEGIN
            SELECT 2 -- Incorrect Username
        END
        ELSE
        BEGIN
            DECLARE @Result INT
            SET @Result = 0
            SELECT @Result = 
                CASE
                    WHEN PasswordHash != @PasswordHash THEN 3 -- Invalid Password
                    WHEN [Enabled] = 0 THEN 4 -- Disabled
                    WHEN Expiration < GETDATE() THEN 5 -- Expired account
                END
            FROM Users
            WHERE Username = @Username;

            select @Result
        END
	END
    ELSE
    BEGIN
        SELECT 1 -- authenticated
    END
GO

print '' print 'Creating sp_select_user_information'
GO
CREATE PROCEDURE sp_select_user_information
(
    @Username  NVARCHAR(64)
)
AS
BEGIN
    SELECT 
        [Username], [Level], [Expiration], [RegisterToken], [Enabled]
    FROM Users
    WHERE @Username = Username
END
GO

print '' print 'Creating sp_select_all_users'
GO
CREATE PROCEDURE sp_select_all_users
AS
BEGIN
    SELECT 
        [Username], [Level], [Expiration], [RegisterToken], [Enabled]
    FROM Users
END
GO

print '' print 'Creating sp_select_level_information'
GO
CREATE PROCEDURE sp_select_level_information
(
    @Level  TINYINT
)
AS
BEGIN
    SELECT 
        [Level], [HandleCooldown]
    FROM Levels
    WHERE @Level = [Level]
END
GO

print '' print 'Creating sp_insert_new_user'
GO
CREATE PROCEDURE sp_insert_new_user
(
    @Username       NVARCHAR(64),
    @PasswordHash   CHAR(64),
    @Level          TINYINT,
    @Expiration     DATETIME,
    @Token  UNIQUEIDENTIFIER
)
AS
BEGIN
    INSERT INTO Users
        ([Username], [PasswordHash], [Level],
        [Expiration], [RegisterToken])
    VALUES
        (@Username, @PasswordHash, @Level,
        @Expiration, @Token)
END
GO

print '' print 'Creating sp_select_registered_user_count'
GO
CREATE PROCEDURE sp_select_registered_user_count
AS
BEGIN
    SELECT 
        COUNT([Username])
    FROM Users
END
GO

print '' print 'Creating sp_select_user_settings'
GO
CREATE PROCEDURE sp_select_user_settings
(
    @Username       NVARCHAR(64)
)
AS
BEGIN
    SELECT 
        [EnableMOTD]
    FROM UserSettings
    WHERE @Username = [Username]
END
GO

print '' print 'Creating sp_update_user_settings'
GO
CREATE PROCEDURE sp_update_user_settings
(
    @Username       NVARCHAR(64),
    @EnableMOTD     BIT
)
AS
BEGIN
    UPDATE UserSettings
    SET
        [EnableMOTD] = @EnableMOTD
    WHERE @Username = [Username]
END
GO

print '' print 'Creating sp_update_user'
GO
CREATE PROCEDURE sp_update_user
(
    @Username       NVARCHAR(64),
    @Level          TINYINT,
    @Expiration     DATETIME,
    @Enabled        BIT
)
AS
BEGIN
    UPDATE Users
    SET
        [Level] = @Level
        , [Expiration] = @Expiration
        , [Enabled] = @Enabled
    WHERE @Username = [Username]
END
GO

print '' print 'Creating sp_insert_user_settings'
GO
CREATE PROCEDURE sp_insert_user_settings
(
    @Username       NVARCHAR(64)
)
AS
BEGIN
    INSERT INTO UserSettings
        ([Username])
    VALUES
        (@Username)
END
GO

