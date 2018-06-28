CREATE TABLE [dbo].[Accounts]
(
	[AccountId] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [Login] NVARCHAR(45) NOT NULL, 
    [Password] BINARY(32) NOT NULL, 
    [LastActive] DATETIME2 NOT NULL, 
    [AccessLevel] INT NOT NULL, 
    [LastServer] TINYINT NOT NULL
)

GO

CREATE INDEX [IX_Accounts_Login] ON [dbo].[Accounts] ([Login])
