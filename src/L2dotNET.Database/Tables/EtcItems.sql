CREATE TABLE [dbo].[EtcItems]
(
	[EtcItemId] INT NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(70) NOT NULL, 
    [ItemType] TINYINT NOT NULL, 
    [Weight] INT NOT NULL,
    [CrystalType] TINYINT NOT NULL, 
    [Duration] INT NOT NULL, 
    [Price] INT NOT NULL, 
    [CrystalCount] INT NOT NULL,
    [Crystallizable] BIT NOT NULL, 
    [Sellable] BIT NOT NULL, 
    [Dropable] BIT NOT NULL, 
    [Destroyable] BIT NOT NULL, 
    [Tradeable] BIT NOT NULL, 
    [Stackable] BIT NOT NULL
)
