print '' print 'Using Imitation Database - PlatformSP.sql'
USE Imitation
GO

print '' print 'Creating sp_select_platforms'
GO
CREATE PROCEDURE sp_select_platforms
AS
BEGIN
    SELECT 
        [PlatformName], [MinimumLevel], [Enabled]
    FROM Platforms
END
GO

print '' print 'Creating sp_select_platform_constraints'
GO
CREATE PROCEDURE sp_select_platform_constraints
AS
BEGIN
    SELECT 
        [PlatformName], [RequestCount], [RequestType]
    FROM PlatformConstraints
END
GO
