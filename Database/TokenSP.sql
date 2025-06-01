print '' print 'Using Imitation Database - TokenSP.sql'
USE Imitation
GO

/*
    Get token
    Claim Token
    Insert Token
*/

print '' print 'Creating sp_get_register_token'
GO
CREATE PROCEDURE sp_get_register_token
(
    @Token       UNIQUEIDENTIFIER
)
AS
BEGIN
    SELECT
        Token, [Level], [Days], UsedBy, UsedDate
    FROM RegisterTokens
    WHERE Token = @Token
END
GO

print '' print 'Creating sp_claim_register_token'
GO
CREATE PROCEDURE sp_claim_register_token
(
    @Token       UNIQUEIDENTIFIER,
    @UsedBy      NVARCHAR(64)
)
AS
BEGIN
    UPDATE RegisterTokens
    SET UsedBy = @UsedBy, UsedDate = GETDATE()
    WHERE Token = @Token
END
GO

print '' print 'Creating sp_insert_register_token'
GO
CREATE PROCEDURE sp_insert_register_token
(
    @Token      UNIQUEIDENTIFIER,
    @Level      TINYINT,
    @Days       SMALLINT
)
AS
BEGIN
    INSERT INTO RegisterTokens
        ([Token], [Level], [Days])
    VALUES
        (@Token, @Level, @Days)
END
GO
