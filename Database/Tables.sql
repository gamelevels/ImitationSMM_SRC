print '' print 'Using Imitation Database - TABLES.sql'
USE Imitation
GO


/*

    CREATE TABLES

*/
print '' print '-----' print '' print 'CREATE TABLE SECTION' print '' print '-----'
GO

/* Level Table */
print '' print 'Creating Level Table'
CREATE TABLE Levels (
    [Level]              TINYINT        NOT NULL,
    [HandleCooldown]     TINYINT        NOT NULL,
    CONSTRAINT [pk_Levels] PRIMARY KEY ([Level])
)
GO

/* RegisterTokens Table */
print '' print 'Creating RegisterTokens Table'
CREATE TABLE RegisterTokens (
    [Token]         UNIQUEIDENTIFIER        NOT NULL,
    [Level]         SMALLINT                NOT NULL,
    [Days]          SMALLINT                NOT NULL,
    [UsedBy]        NVARCHAR(64)            ,
    [UsedDate]      DATETIME                ,
    CONSTRAINT [pk_RegisterTokens] PRIMARY KEY ([Token])
)
GO

/* User Table */
print '' print 'Creating User Table'
CREATE TABLE Users (
    [Username]          NVARCHAR(64)            NOT NULL,
    [PasswordHash]      CHAR(64)                NOT NULL,
    [Level]             TINYINT                 NOT NULL,
    [Expiration]        DATETIME                NOT NULL,  
    [RegisterToken]     UNIQUEIDENTIFIER        NOT NULL,
    [Enabled]           BIT             DEFAULT 1, 
    CONSTRAINT [pk_Users] PRIMARY KEY ([Username]),
    CONSTRAINT [fk_Users_Level] FOREIGN KEY([Level])
        REFERENCES Levels([Level]),
    CONSTRAINT [fk_Users_RegisterToken] FOREIGN KEY([RegisterToken])
        REFERENCES RegisterTokens([Token])    
)
GO

print '' print 'Creating Ticket Table'
GO
CREATE TABLE Tickets (
    [ID]        UNIQUEIDENTIFIER    DEFAULT NEWID(),
    [Title]     NVARCHAR(64)        NOT NULL,
    [Issue]     TEXT                NOT NULL,
    [Priority]  INT                 NOT NULL,
    [Resolved]  BIT                 DEFAULT 0,
    [Submitter] NVARCHAR(64)        NOT NULL,
    [DateSubmitted] DATETIME        DEFAULT GETDATE(),
    CONSTRAINT [pk_Tickets] PRIMARY KEY ([ID]),
    CONSTRAINT [pk_Tickets_Username] FOREIGN KEY ([Submitter])
        REFERENCES Users([Username])
)
GO

print '' print 'Creating TicketMessages Table'
GO
CREATE TABLE TicketMessages (
    [TicketID]      UNIQUEIDENTIFIER    NOT NULL,
    [Message]       TEXT                NOT NULL,
    [Sender]        NVARCHAR(64)        NOT NULL,
    [SentTime]      DATETIME            DEFAULT GETDATE(),
    [IsStaff]       BIT                 DEFAULT 0,
    CONSTRAINT [pk_TicketMessages_TicketID] FOREIGN KEY ([TicketID])
        REFERENCES Tickets([ID]),
    CONSTRAINT [pk_TicketMessages_Username] FOREIGN KEY ([Sender])
        REFERENCES Users([Username])
)
GO

print '' print 'Creating UserSettings Table'
CREATE TABLE UserSettings (
    [Username]          NVARCHAR(64)            NOT NULL,
    [EnableMOTD]        BIT                     DEFAULT 1,
    CONSTRAINT [fk_UserSettings_Username] FOREIGN KEY([Username])
        REFERENCES Users([Username])
)
GO

/* Platforms Table */
print '' print 'Creating Platforms Table'
CREATE TABLE Platforms (
    [PlatformName]          NVARCHAR(32)        NOT NULL,
    [MinimumLevel]          TINYINT             NOT NULL,
    [Enabled]               BIT                 DEFAULT 1,
    CONSTRAINT [pk_Platforms] PRIMARY KEY ([PlatformName])
)
GO

/* PlatformConstraints Table */
-- For each platform constraint, list of request max and type
print '' print 'Creating PlatformConstraints Table'
CREATE TABLE PlatformConstraints (
    [PlatformName]    NVARCHAR(32)   NOT NULL,
    [RequestCount]    SMALLINT       NOT NULL, -- Max request
    [RequestType]     NVARCHAR(32)   NOT NULL, -- Followers, Likes, ETC
    CONSTRAINT [fk_PlatformConstraints_PlatformName] FOREIGN KEY([PlatformName])
        REFERENCES Platforms([PlatformName])    
)
GO

/* UserHistory Table */
print '' print 'Creating UserHistory Table'
CREATE TABLE UserHistory (
    [Username]          NVARCHAR(64)      NOT NULL,
    [RequestType]       NVARCHAR(32)      NOT NULL,
    [RequestHandle]     NVARCHAR(32)      NOT NULL,
    [RequestDetails]    NVARCHAR(128)     NOT NULL,
    [Date]              DATETIME          NOT NULL,
    CONSTRAINT [fk_UserHistory_Username] FOREIGN KEY([Username])
        REFERENCES Users([Username])
)
GO

/* UserLogs Table */
print '' print 'Creating UserLogs Table'
CREATE TABLE UserLogs (
    [Username]       NVARCHAR(64)      NOT NULL,
    [LogType]        NVARCHAR(32)      NOT NULL,
    [LogDetails]     NVARCHAR(128)     NOT NULL,
    [Date]           DATETIME          NOT NULL,
    CONSTRAINT [fk_UserLogs_Username] FOREIGN KEY([Username])
        REFERENCES Users([Username])
)
GO

/* Webhooks Table */
print '' print 'Creating Webhooks Table'
CREATE TABLE Webhooks (
    [Link]       NVARCHAR(255)      NOT NULL,
    [Title]      NVARCHAR(64)       NOT NULL,
    [Enabled]    BIT                DEFAULT 1,
    CONSTRAINT [pk_Webhooks] PRIMARY KEY([Link])
)
GO

/* WebhookLogs Table */
print '' print 'Creating WebhookLogs Table'
CREATE TABLE WebhookLogs (
    [WebhookLogID]    INT IDENTITY(1000,1)    NOT NULL,
    [RequestLink]     NVARCHAR(255)           NOT NULL,
    [Title]           NVARCHAR(128)           NOT NULL,
    [Message]         NVARCHAR(128)           NOT NULL,
    [Date]            DATETIME                NOT NULL,
    CONSTRAINT [pk_WebhookLogs] PRIMARY KEY([WebhookLogID]),
    CONSTRAINT [fk_WebhookLogs_Link] FOREIGN KEY([RequestLink])
        REFERENCES Webhooks([Link])
)
GO

/* APIs Table */
print '' print 'Creating APIs Table'
CREATE TABLE APIs (
    [Link]                      NVARCHAR(255)     NOT NULL,
    [ParameterPlaceHolder]      NVARCHAR(32)      NOT NULL,
    [Enabled]                   BIT               DEFAULT 1,
    CONSTRAINT [pk_APIs] PRIMARY KEY([Link])
)
GO

/* APILogs Table */
print '' print 'Creating APILogs Table'
CREATE TABLE APILogs (
    [Link]          NVARCHAR(255)     NOT NULL,
    [UsedBy]        NVARCHAR(64)      NOT NULL,
    [Response]      NVARCHAR(255)     NOT NULL,
    [Date]          DATETIME          NOT NULL,
    CONSTRAINT [fk_APILogs_Link] FOREIGN KEY([Link])
        REFERENCES APIs([Link])
)
GO

/* Reviews Table */
print '' print 'Creating Reviews Table'
CREATE TABLE Reviews (
    [ReviewID]      INT IDENTITY(1000,1)    NOT NULL,
    [Submitter]     NVARCHAR(64)            NOT NULL,
    [Message]       NVARCHAR(255)           NOT NULL,
    [Rating]        INT                     NOT NULL,
    [Date]          DATETIME                NOT NULL,
    CONSTRAINT [pk_Reviews] PRIMARY KEY([ReviewID]),
    CONSTRAINT [fk_Reviews_Username] FOREIGN KEY([Submitter])
        REFERENCES Users([Username])
)
GO

/* Application Table */
print '' print 'Creating ApplicationInfo Table'
CREATE TABLE ApplicationInfo (
    [MOTD]          NVARCHAR(255)              ,
    [AppVersion]    NVARCHAR(15)       NOT NULL,
    [TOS]           NVARCHAR(255)      NOT NULL,
    [Help]          NVARCHAR(255)      NOT NULL
)
GO
