/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50143
Source Host           : localhost:3306
Source Database       : rabbit_cgame

Target Server Type    : MYSQL
Target Server Version : 50143
File Encoding         : 65001

Date: 2010-10-14 12:32:30
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for `user_items`
-- ----------------------------
DROP TABLE IF EXISTS `user_items`;
CREATE TABLE `user_items` (
  `ownerId` int(11) DEFAULT NULL,
  `iobjectId` int(11) NOT NULL,
  `itemId` int(11) NOT NULL,
  `icount` int(11) NOT NULL DEFAULT '1',
  `ienchant` int(11) NOT NULL DEFAULT '0',
  `iaugment` int(11) NOT NULL DEFAULT '0',
  `imana` int(11) NOT NULL DEFAULT '-1',
  `lifetime` varchar(55) COLLATE utf8_unicode_ci NOT NULL DEFAULT '-1',
  `iequipped` tinyint(1) NOT NULL DEFAULT '0',
  `iequip_data` int(11) NOT NULL DEFAULT '0',
  `ilocation` enum('warehouse','paperdoll','pet','inventory','refund') COLLATE utf8_unicode_ci NOT NULL DEFAULT 'inventory',
  `iloc_data` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`iobjectId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

