print '' print 'Using Imitation Database - APISP.sql'
USE Imitation
GO

print '' print 'Creating sp_select_apis'
GO
CREATE PROCEDURE sp_select_apis
AS
BEGIN
    SELECT 
        [Link], [ParameterPlaceHolder], [Enabled]
    FROM APIs
END
GO

print '' print 'Creating sp_insert_api'
GO
CREATE PROCEDURE sp_insert_api
(
    @Link                   NVARCHAR(255),
    @ParameterPlaceHolder   NVARCHAR(32),
    @Enabled                BIT
)
AS
BEGIN
    INSERT INTO APIs
        ([Link], [ParameterPlaceHolder], [Enabled])
    VALUES
        (@Link, @ParameterPlaceHolder, @Enabled)
END
GO

print '' print 'Creating sp_update_api'
GO
CREATE PROCEDURE sp_update_api
(
    @Link                   NVARCHAR(255),
    @ParameterPlaceHolder   NVARCHAR(32),
    @Enabled                BIT
)
AS
BEGIN
    UPDATE APIs
    SET
        [ParameterPlaceHolder] = @ParameterPlaceHolder,
        [Enabled] = @Enabled
    WHERE 
        [Link] = @Link
END
GO