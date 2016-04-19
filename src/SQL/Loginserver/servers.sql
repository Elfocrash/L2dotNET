/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50143
Source Host           : localhost:3306
Source Database       : rabbit_cauth

Target Server Type    : MYSQL
Target Server Version : 50143
File Encoding         : 65001

Date: 2010-08-29 04:57:45
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for `servers`
-- ----------------------------
DROP TABLE IF EXISTS `servers`;
CREATE TABLE `servers` (
  `id` int(11) NOT NULL DEFAULT '0',
  `info` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `wan` varchar(100) COLLATE utf8_unicode_ci DEFAULT NULL,
  `port` int(4) NOT NULL DEFAULT '7777',
  `code` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Records of servers
-- ----------------------------
INSERT INTO `servers` VALUES ('1', 'lineage2 bartz', '192.168.1.2', '7777', 'jeremy');
INSERT INTO `servers` VALUES ('58', 'l2 ken abigale', '192.168.1.2', '7776', 'botl2');
