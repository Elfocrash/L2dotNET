/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50143
Source Host           : localhost:3306
Source Database       : rabbit_cgame

Target Server Type    : MYSQL
Target Server Version : 50143
File Encoding         : 65001

Date: 2010-10-09 04:15:47
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for `clan_data`
-- ----------------------------
DROP TABLE IF EXISTS `clan_data`;
CREATE TABLE `clan_data` (
  `id` int(11) NOT NULL,
  `name` varchar(200) COLLATE utf8_unicode_ci NOT NULL,
  `level` int(3) DEFAULT NULL,
  `leaderId` int(11) DEFAULT NULL,
  `leaderName` varchar(30) COLLATE utf8_unicode_ci DEFAULT NULL,
  `crestId` int(11) DEFAULT NULL,
  `crestBigId` int(11) DEFAULT NULL,
  `castleId` int(3) DEFAULT NULL,
  `agitId` int(3) DEFAULT NULL,
  `fortId` int(3) DEFAULT NULL,
  `dominionId` int(11) DEFAULT NULL,
  `allyId` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`,`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

