/*
Navicat MySQL Data Transfer

Source Server         : localhost
Source Server Version : 50710
Source Host           : localhost:3306
Source Database       : l2dotnet

Target Server Type    : MYSQL
Target Server Version : 50710
File Encoding         : 65001

Date: 2016-04-30 12:53:13
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `accounts`
-- ----------------------------
DROP TABLE IF EXISTS `accounts`;
CREATE TABLE `accounts` (
  `login` varchar(45) NOT NULL DEFAULT '',
  `password` varchar(45) DEFAULT NULL,
  `lastactive` decimal(20,0) DEFAULT NULL,
  `access_level` int(3) NOT NULL DEFAULT '0',
  `lastServer` int(4) DEFAULT '1',
  PRIMARY KEY (`login`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of accounts
-- ----------------------------
INSERT INTO `accounts` VALUES ('test', 'qUqP5cyxm6YcTAhz05Hph5gvu9M=', '1459206239208', '0', '1');

-- ----------------------------
-- Table structure for `announcements`
-- ----------------------------
DROP TABLE IF EXISTS `announcements`;
CREATE TABLE `announcements` (
  `Id` int(5) NOT NULL AUTO_INCREMENT,
  `Text` varchar(200) DEFAULT NULL,
  `Type` int(1) DEFAULT '0',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of announcements
-- ----------------------------
INSERT INTO `announcements` VALUES ('1', 'Thanks for using L2dotNET', '0');

-- ----------------------------
-- Table structure for `characters`
-- ----------------------------
DROP TABLE IF EXISTS `characters`;
CREATE TABLE `characters` (
  `account_name` varchar(45) DEFAULT NULL,
  `obj_Id` int(10) unsigned NOT NULL DEFAULT '0',
  `char_name` varchar(35) NOT NULL,
  `level` tinyint(3) unsigned DEFAULT NULL,
  `maxHp` mediumint(8) unsigned DEFAULT NULL,
  `curHp` mediumint(8) unsigned DEFAULT NULL,
  `maxCp` mediumint(8) unsigned DEFAULT NULL,
  `curCp` mediumint(8) unsigned DEFAULT NULL,
  `maxMp` mediumint(8) unsigned DEFAULT NULL,
  `curMp` mediumint(8) unsigned DEFAULT NULL,
  `face` tinyint(3) unsigned DEFAULT NULL,
  `hairStyle` tinyint(3) unsigned DEFAULT NULL,
  `hairColor` tinyint(3) unsigned DEFAULT NULL,
  `sex` tinyint(3) unsigned DEFAULT NULL,
  `heading` mediumint(9) DEFAULT NULL,
  `x` mediumint(9) DEFAULT NULL,
  `y` mediumint(9) DEFAULT NULL,
  `z` mediumint(9) DEFAULT NULL,
  `exp` bigint(20) unsigned DEFAULT '0',
  `expBeforeDeath` bigint(20) unsigned DEFAULT '0',
  `sp` int(10) unsigned NOT NULL DEFAULT '0',
  `karma` int(10) unsigned DEFAULT NULL,
  `pvpkills` smallint(5) unsigned DEFAULT NULL,
  `pkkills` smallint(5) unsigned DEFAULT NULL,
  `clanid` int(10) unsigned DEFAULT NULL,
  `race` tinyint(3) unsigned DEFAULT NULL,
  `classid` tinyint(3) unsigned DEFAULT NULL,
  `base_class` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `deletetime` bigint(20) DEFAULT NULL,
  `cancraft` tinyint(3) unsigned DEFAULT NULL,
  `title` varchar(16) DEFAULT NULL,
  `rec_have` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `rec_left` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `accesslevel` mediumint(9) DEFAULT '0',
  `online` tinyint(3) unsigned DEFAULT NULL,
  `onlinetime` int(11) DEFAULT NULL,
  `char_slot` tinyint(3) unsigned DEFAULT NULL,
  `lastAccess` bigint(20) unsigned DEFAULT NULL,
  `clan_privs` mediumint(8) unsigned DEFAULT '0',
  `wantspeace` tinyint(3) unsigned DEFAULT '0',
  `isin7sdungeon` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `punish_level` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `punish_timer` int(10) unsigned NOT NULL DEFAULT '0',
  `power_grade` tinyint(3) unsigned DEFAULT NULL,
  `nobless` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `hero` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `subpledge` smallint(6) NOT NULL DEFAULT '0',
  `last_recom_date` bigint(20) unsigned NOT NULL DEFAULT '0',
  `lvl_joined_academy` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `apprentice` int(10) unsigned NOT NULL DEFAULT '0',
  `sponsor` int(10) unsigned NOT NULL DEFAULT '0',
  `varka_ketra_ally` tinyint(4) NOT NULL DEFAULT '0',
  `clan_join_expiry_time` bigint(20) unsigned NOT NULL DEFAULT '0',
  `clan_create_expiry_time` bigint(20) unsigned NOT NULL DEFAULT '0',
  `death_penalty_level` smallint(5) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`obj_Id`),
  KEY `clanid` (`clanid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for `servers`
-- ----------------------------
DROP TABLE IF EXISTS `servers`;
CREATE TABLE `servers` (
  `Id` int(11) NOT NULL DEFAULT '0',
  `Name` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `Wan` varchar(100) COLLATE utf8_unicode_ci DEFAULT NULL,
  `Port` int(4) NOT NULL DEFAULT '7777',
  `Code` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Records of servers
-- ----------------------------
INSERT INTO `servers` VALUES ('1', 'bartz', '192.168.0.3', '7777', 'elfo');
