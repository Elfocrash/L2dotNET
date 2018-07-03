CREATE TABLE [dbo].[Servers]
(
	[ServerId] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY, 
    [Name] VARCHAR(50) NOT NULL, 
    [Wan] VARCHAR(15) NOT NULL, 
    [Port] SMALLINT NOT NULL, 
    [Key] CHAR(64) NOT NULL, 
    CONSTRAINT [AK_Servers_Key] UNIQUE ([Key])
)
