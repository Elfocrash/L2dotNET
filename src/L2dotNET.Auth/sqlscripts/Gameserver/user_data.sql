/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50143
Source Host           : localhost:3306
Source Database       : rabbit_cgame

Target Server Type    : MYSQL
Target Server Version : 50143
File Encoding         : 65001

Date: 2010-10-15 07:22:57
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for `user_data`
-- ----------------------------
DROP TABLE IF EXISTS `user_data`;
CREATE TABLE `user_data` (
  `account` varchar(200) COLLATE utf8_unicode_ci NOT NULL,
  `objId` int(11) NOT NULL,
  `pname` varchar(200) COLLATE utf8_unicode_ci NOT NULL DEFAULT '',
  `ptitle` varchar(200) COLLATE utf8_unicode_ci NOT NULL DEFAULT '',
  `plevel` int(2) NOT NULL DEFAULT '1',
  `pCurHp` decimal(20,0) DEFAULT '0',
  `pMaxHp` decimal(20,0) DEFAULT '0',
  `pCurMp` decimal(20,0) DEFAULT '0',
  `pMaxMp` decimal(20,0) DEFAULT '0',
  `pCurCp` decimal(20,0) DEFAULT '0',
  `pMaxCp` decimal(20,0) DEFAULT '0',
  `pface` int(2) DEFAULT '0',
  `pHairStyle` int(2) DEFAULT '0',
  `pHairColor` int(2) DEFAULT '0',
  `pSex` int(1) DEFAULT '0',
  `locx` int(11) DEFAULT '0',
  `locy` int(11) DEFAULT '0',
  `locz` int(11) DEFAULT '0',
  `loch` int(11) DEFAULT '0',
  `pExp` decimal(20,0) DEFAULT '0',
  `pExpDeath` decimal(20,0) DEFAULT '0',
  `sp` int(11) DEFAULT '0',
  `karma` int(11) DEFAULT '0',
  `kill_pvp` int(11) DEFAULT '0',
  `kill_pk` int(11) DEFAULT '0',
  `pClanId` int(11) NOT NULL DEFAULT '0',
  `pBaseClass` int(3) DEFAULT '0',
  `pActiveClass` int(3) DEFAULT '0',
  `pRecCur` int(11) DEFAULT '0',
  `pRecEval` int(11) DEFAULT '0',
  `slotId` int(1) DEFAULT '0',
  `penalty_death` int(1) DEFAULT '0',
  `clanId` int(11) NOT NULL DEFAULT '0',
  `clanType` int(6) NOT NULL DEFAULT '0',
  `clanPrivs` int(11) NOT NULL DEFAULT '0',
  `penalty_clancreate` varchar(75) COLLATE utf8_unicode_ci NOT NULL DEFAULT '0',
  `penalty_clanjoin` varchar(75) COLLATE utf8_unicode_ci NOT NULL DEFAULT '0',
  PRIMARY KEY (`objId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Records of user_data
-- ----------------------------
