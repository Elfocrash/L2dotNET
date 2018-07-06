INSERT INTO [dbo].[Servers] ([Name], [Wan], [Port], [Key]) VALUES (N'default', N'127.0.0.1', N'7777', CONVERT(CHAR(64), HASHBYTES('SHA2_512', 'default+salt'), 2))
GO