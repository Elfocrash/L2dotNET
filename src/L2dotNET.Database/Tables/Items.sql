CREATE TABLE [dbo].[Items]
(
    [ItemId] INT NOT NULL PRIMARY KEY,
	[CharacterId] INT NOT NULL, 
    [ObjectId] INT NOT NULL, 
    [Count] INT NOT NULL, 
    [Enchant] INT NOT NULL, 
    [Location] TINYINT NOT NULL, 
    [LocationData] INT NOT NULL, 
    [TimeOfUse] INT NULL, 
    [CustomType1] INT NOT NULL, 
    [CustomType2] INT NOT NULL, 
    [ManaLeft] INT NOT NULL, 
    [Time] INT NOT NULL, 
    CONSTRAINT [FK_Items_Characters] FOREIGN KEY ([CharacterId]) REFERENCES [Characters]([CharacterId])
)

GO

CREATE INDEX [IX_Item_CharacterId] ON [dbo].[Items]([CharacterId])

GO

CREATE INDEX [IX_Item_ObjectId] ON [dbo].[Items] ([ObjectId])
