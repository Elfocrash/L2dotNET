/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50143
Source Host           : localhost:3306
Source Database       : rabbit_cgame

Target Server Type    : MYSQL
Target Server Version : 50143
File Encoding         : 65001

Date: 2010-10-07 17:10:11
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for `minigame_rank`
-- ----------------------------
DROP TABLE IF EXISTS `minigame_rank`;
CREATE TABLE `minigame_rank` (
  `playerId` int(11) NOT NULL,
  `score` int(10) DEFAULT NULL,
  `achived` varchar(70) COLLATE utf8_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`playerId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Records of minigame_rank
-- ----------------------------
