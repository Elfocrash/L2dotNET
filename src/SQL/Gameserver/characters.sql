-- -------------------------------
-- characters (account_name, obj_Id, char_name, Level, MaxHp, CurHp, MaxCp, CurCp,MaxMp,CurMp,
--                             Face,HairStyle,HairColor,Sex,Heading,X,Y,Z,Exp,ExpBeforeDeath,Sp,Karma,PvpKills,PkKills,ClanId,Race,classid,base_class, DeleteTime,CanCraft,Title,
--                             rec_have,rec_left,AccessLevel,char_slot,clan_privs, WantsPeace,IsIn7sDungeon,punish_level,punish_timer, power_grade,Nobless,Hero,Subpledge,
--                             last_recom_date,lvl_joined_academy, Apprentice, Sponsor,varka_ketra_ally,clan_join_expiry_time,clan_create_expiry_time, death_penalty_level) 
-- -------------------------------
							 
-- ---------------------------
-- Table structure for characters
-- ---------------------------
DROP TABLE IF EXISTS `characters`;
CREATE TABLE IF NOT EXISTS `characters` (
  `account_name` varchar(200) COLLATE utf8_unicode_ci NOT NULL,
  `obj_Id` decimal(20,0) NOT NULL DEFAULT '0',
  `char_name` varchar(200) COLLATE utf8_unicode_ci NOT NULL,
  `Level` int(2) NOT NULL DEFAULT '1',
  `maxHp` decimal(20,0) DEFAULT '0',
  `curHp` decimal(20,0) DEFAULT '0',
  `maxCp` decimal(20,0) DEFAULT '0',
  `curCp` decimal(20,0) DEFAULT '0',
  `maxMp` decimal(20,0) DEFAULT '0',
  `curMp` decimal(20,0) DEFAULT '0',
  `Face` int(2) DEFAULT '0',
  `HairStyle` int(2) DEFAULT '0',
  `HairColor` decimal(20,0) DEFAULT '0',
  `Sex` int(1) DEFAULT '0',
  `Heading` int(11) DEFAULT '0',
  `X` int(11) DEFAULT '0',
  `Y` int(11) DEFAULT '0',
  `Z` int(11) DEFAULT '0', 
  `Exp` decimal(20,0) DEFAULT '0',
  `ExpBeforeDeath` decimal(20,0) DEFAULT '0',
  `Sp` int(11) DEFAULT '0',
  `Karma` int(11) DEFAULT '0',
  `Pvpkills` int(11) DEFAULT '0',
  `Pkkills` int(11) DEFAULT '0',
  `Race` int(11) DEFAULT '0',
  `ClanId` int(11) DEFAULT '0',
  `classid` int(11) DEFAULT '0',
  `base_class` int(2) NOT NULL DEFAULT '0',
  `DeleteTime` decimal(20,0) DEFAULT '0',
  `CanCraft` int(11) DEFAULT '0',
  `Title` varchar(16) COLLATE utf8_unicode_ci NOT NULL,
  `rec_have` int(3) NOT NULL DEFAULT '0',
  `rec_left` int(3) NOT NULL DEFAULT '0',
  `AccessLevel` decimal(4,0) DEFAULT '0',
  `char_slot` int(1) DEFAULT '0',
  `clan_privs` INT DEFAULT '0',
  `WantsPeace` int(1) DEFAULT '0',
  `Isin7sDungeon` int(1) NOT NULL DEFAULT '0',
  `punish_level` TINYINT UNSIGNED NOT NULL DEFAULT '0',
  `punish_timer` INT UNSIGNED NOT NULL DEFAULT '0',
  `power_grade` int(11) DEFAULT '0',
  `Nobless` int(1) NOT NULL DEFAULT '0',
  `Hero` int(1) NOT NULL DEFAULT '0',
  `Subpledge` int(1) NOT NULL DEFAULT '0',
  `last_recom_date` decimal(20,0) NOT NULL DEFAULT '0',
  `lvl_joined_academy` int(2) NOT NULL DEFAULT '0',
  `Apprentice` int(1) NOT NULL DEFAULT '0',
  `Sponsor` int(1) NOT NULL DEFAULT '0',
  `varka_ketra_ally` int(1) NOT NULL DEFAULT '0',
  `clan_join_expiry_time` DECIMAL(20,0) NOT NULL DEFAULT '0',
  `clan_create_expiry_time` DECIMAL(20,0) NOT NULL DEFAULT '0',
  `death_penalty_level` int(2) NOT NULL DEFAULT '0',
  PRIMARY KEY  (`obj_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;
							 
							 
							 