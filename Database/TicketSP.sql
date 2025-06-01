print '' print 'Using Imitation Database'
USE Imitation
GO


print '' print 'Creating sp_select_all_tickets'
GO
CREATE PROCEDURE sp_select_all_tickets
AS 
BEGIN
    SELECT  
        [ID],
        [Title],
        [Issue],
        [Priority],
        [Resolved],
        [Submitter],
        [DateSubmitted]
    FROM Tickets
END
GO

print '' print 'Creating sp_select_users_tickets'
GO
CREATE PROCEDURE sp_select_users_tickets(
    @User   NVARCHAR(64)
)
AS 
BEGIN
    SELECT  
        [ID],
        [Title],
        [Issue],
        [Priority],
        [Resolved],
        [Submitter],
        [DateSubmitted]
    FROM Tickets
    WHERE [Submitter] = @User
END
GO


print '' print 'Creating sp_select_ticket'
GO
CREATE PROCEDURE sp_select_ticket(
    @TicketID  UNIQUEIDENTIFIER
)
AS 
BEGIN
    SELECT  
        [ID],
        [Title],
        [Issue],
        [Priority],
        [Resolved],
        [Submitter],
        [DateSubmitted]
    FROM Tickets
    WHERE [ID] = @TicketID
END
GO

print '' print 'Creating sp_select_ticket_messages'
GO
CREATE PROCEDURE sp_select_ticket_messages(
    @TicketID  UNIQUEIDENTIFIER
)
AS 
BEGIN
    SELECT  
        [TicketID],
        [Message],
        [Sender],
        [SentTime],
        [IsStaff]
    FROM TicketMessages
    WHERE [TicketID] = @TicketID
END
GO

print '' print 'Creating sp_create_ticket'
GO
CREATE PROCEDURE sp_create_ticket(
    @ID         UNIQUEIDENTIFIER,
    @Title      NVARCHAR(64),
    @Issue      TEXT,
    @Priority   INT,
    @Submitter  NVARCHAR(64)
)
AS 
BEGIN
    INSERT INTO Tickets (
        [ID], [Title], [Issue],
        [Priority], [Submitter]
    ) VALUES (
        @ID, @Title, @Issue,
        @Priority, @Submitter
    )
END
GO

print '' print 'Creating sp_add_ticket_message'
GO
CREATE PROCEDURE sp_add_ticket_message(
    @TicketID      UNIQUEIDENTIFIER,
    @Message       TEXT,
    @Sender        NVARCHAR(64),
    @IsStaff       BIT
)
AS 
BEGIN
    INSERT INTO TicketMessages (
        [TicketID], [Message], 
        [Sender], [IsStaff]
    ) VALUES (
        @TicketID, @Message,
        @Sender, @IsStaff
    )
END
GO

print '' print 'Creating sp_update_ticket'
GO
CREATE PROCEDURE sp_update_ticket(
    @TicketID   UNIQUEIDENTIFIER,
    @Title      NVARCHAR(64),
    @Priority   INT,
    @Resolved   BIT
)
AS 
BEGIN
    UPDATE Tickets
    SET 
        [Title] = @Title,
        [Priority] = @Priority,
        [Resolved] = @Resolved
    WHERE [ID] = @TicketID
END
GO