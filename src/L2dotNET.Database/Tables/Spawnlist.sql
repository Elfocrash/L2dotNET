CREATE TABLE [dbo].[Spawnlist]
(
	[SpawnId] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [TemplateId] INT NOT NULL, 
    [LocX] INT NOT NULL, 
    [LocY] INT NOT NULL, 
    [LocZ] INT NOT NULL, 
    [Heading] INT NOT NULL, 
    [RespownDelay] INT NOT NULL, 
    [RespawnRand] INT NOT NULL, 
    [PeriodOfDay] INT NOT NULL
)
