CREATE TABLE [dbo].[Announcements]
(
	[AnnouncementId] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [Text] NVARCHAR(200) NOT NULL, 
    [Type] TINYINT NOT NULL
)
