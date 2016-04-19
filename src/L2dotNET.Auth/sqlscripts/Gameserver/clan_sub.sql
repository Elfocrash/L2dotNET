/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50143
Source Host           : localhost:3306
Source Database       : rabbit_cgame

Target Server Type    : MYSQL
Target Server Version : 50143
File Encoding         : 65001

Date: 2010-10-15 07:23:03
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for `clan_sub`
-- ----------------------------
DROP TABLE IF EXISTS `clan_sub`;
CREATE TABLE `clan_sub` (
  `id` int(11) NOT NULL,
  `subId` int(11) NOT NULL,
  `name` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  `leaderId` int(11) NOT NULL DEFAULT '0',
  `leaderName` varchar(50) COLLATE utf8_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`id`,`subId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

