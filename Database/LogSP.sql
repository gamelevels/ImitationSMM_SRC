print '' print 'Using Imitation Database - LogSP.sql'
USE Imitation
GO

/*
    Insert user history
    Insert user log
    Insert webhook log
    Insert api log
    Insert review
*/

print '' print 'Creating sp_insert_user_history'
GO
CREATE PROCEDURE sp_insert_user_history
(
    @Username           NVARCHAR(64),
    @RequestType        NVARCHAR(32),
    @RequestHandle      NVARCHAR(32),
    @RequestDetails     NVARCHAR(128)
)
AS
BEGIN
    INSERT INTO UserHistory
        ([Username], [RequestType], [RequestHandle],
        [RequestDetails], [Date])
    VALUES
        (@Username, @RequestType, @RequestHandle,
        @RequestDetails, GETDATE())
END
GO

print '' print 'Creating sp_insert_user_log'
GO
CREATE PROCEDURE sp_insert_user_log
(
    @Username       NVARCHAR(64),
    @LogType        NVARCHAR(32),
    @LogDetails     NVARCHAR(128)
)
AS
BEGIN
    INSERT INTO UserLogs
        ([Username], [LogType], [LogDetails], [Date])
    VALUES
        (@Username, @LogType, @LogDetails, GETDATE())
END
GO

print '' print 'Creating sp_insert_webhook_log'
GO
CREATE PROCEDURE sp_insert_webhook_log
(
    @RequestLink    NVARCHAR(255),
    @Title          NVARCHAR(64),
    @Message        NVARCHAR(255)
)
AS
BEGIN
    INSERT INTO WebhookLogs
        ([RequestLink], [Title], [Message], [Date])
    VALUES
        (@RequestLink, @Title, @Message, GETDATE())
END
GO

print '' print 'Creating sp_insert_api_log'
GO
CREATE PROCEDURE sp_insert_api_log
(
    @Link       NVARCHAR(255),
    @UsedBy     NVARCHAR(64),
    @Response   NVARCHAR(255)
)
AS
BEGIN
    INSERT INTO APILogs
        ([Link], [UsedBy], [Response], [Date])
    VALUES
        (@Link, @UsedBy, @Response, GETDATE())
END
GO

print '' print 'Creating sp_insert_review'
GO
CREATE PROCEDURE sp_insert_review
(
    @Submitter      NVARCHAR(64),
    @Message        NVARCHAR(255),
    @Rating         INT
)
AS
BEGIN
    INSERT INTO Reviews
        ([Submitter], [Message], [Rating], [Date])
    VALUES 
        (@Submitter, @Message, @Rating, GETDATE())
END
GO

print '' print 'Creating sp_select_webhooks'
GO
CREATE PROCEDURE sp_select_webhooks
AS
BEGIN
    SELECT 
        [Link], [Title], [Enabled]
    FROM Webhooks
END
GO

print '' print 'Creating sp_update_webhook'
GO
CREATE PROCEDURE sp_update_webhook
(
    @Link           NVARCHAR(255),
    @Title          NVARCHAR(64),
    @Enabled        BIT
)
AS
BEGIN
   UPDATE Webhooks
   SET
       [Title] = @Title,
       [Enabled] = @Enabled
    WHERE @Link = [Link]
END
GO

print '' print 'Creating sp_insert_webhook'
GO
CREATE PROCEDURE sp_insert_webhook
(
    @Link           NVARCHAR(255),
    @Title          NVARCHAR(64),
    @Enabled        BIT
)
AS
BEGIN
    INSERT INTO Webhooks
        ([Link], [Title], [Enabled])
    VALUES
        (@Link, @Title, @Enabled)
END
GO

print '' print 'Creating sp_select_user_history'
GO
CREATE PROCEDURE sp_select_user_history
(
    @Username NVARCHAR(64)
)
AS
BEGIN
    SELECT 
        [Username], [RequestType], [RequestHandle], [RequestDetails], [Date]
    FROM UserHistory
    WHERE [Username] = @Username
END
GO

print '' print 'Creating sp_select_user_log'
GO
CREATE PROCEDURE sp_select_user_log
(
    @Username NVARCHAR(64)
)
AS
BEGIN
    SELECT 
        [Username], [LogType], [LogDetails], [Date]
    FROM UserLogs
    WHERE [Username] = @Username
END
GO

print '' print 'Creating sp_select_popular_platform'
GO
CREATE PROCEDURE sp_select_popular_platform
AS
BEGIN
    SELECT -- RequestDetails = platform not changing code
        [RequestDetails], COUNT([RequestDetails])
    FROM UserHistory
    GROUP BY [RequestDetails]
END
GO