CREATE TABLE [dbo].[Armors]
(
    [ArmorId] INT NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(70) NOT NULL, 
    [BodyPart] INT NOT NULL, 
    [Crystallizable] BIT NOT NULL, 
    [ArmorType] TINYINT NOT NULL, 
    [Weight] INT NOT NULL, 
    [AvoidModify] BIT NOT NULL, 
    [Duration] INT NOT NULL, 
    [Pdef] INT NOT NULL, 
    [Mdef] INT NOT NULL, 
    [MpBonus] INT NOT NULL, 
    [Price] INT NOT NULL, 
    [CrystalCount] INT NOT NULL, 
    [Sellable] BIT NOT NULL, 
    [Dropable] BIT NOT NULL, 
    [Destroyable] BIT NOT NULL, 
    [Tradeable] BIT NOT NULL, 
    [ItemSkillId] INT NOT NULL, 
    [ItemSkillLvl] TINYINT NOT NULL
)

GO