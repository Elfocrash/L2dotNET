CREATE TABLE [dbo].[CharacterSkills]
(
	[CharacterSkillId] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY, 
    [CharacterId] INT NOT NULL, 
    [SkillId] INT NOT NULL, 
    [SkillLvl] TINYINT NOT NULL, 
    [ClassId] INT NOT NULL, 
    CONSTRAINT [FK_CharacterSkills_Characters] FOREIGN KEY ([CharacterId]) REFERENCES [Characters]([CharacterId]) ON DELETE CASCADE
)

GO

CREATE INDEX [IX_CharacterSkills_CharacterId] ON [dbo].[CharacterSkills] ([CharacterId])
