/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50143
Source Host           : localhost:3306
Source Database       : rabbit_cauth

Target Server Type    : MYSQL
Target Server Version : 50143
File Encoding         : 65001

Date: 2010-08-30 04:21:53
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for `accounts`
-- ----------------------------
DROP TABLE IF EXISTS `accounts`;
CREATE TABLE `accounts` (
  `id` int(20) NOT NULL AUTO_INCREMENT,
  `account` varchar(40) COLLATE utf8_unicode_ci NOT NULL,
  `password` varchar(100) COLLATE utf8_unicode_ci DEFAULT NULL,
  `serverId` int(3) NOT NULL DEFAULT '0',
  `builder` int(1) NOT NULL DEFAULT '0',
  `type` enum('limited','trial','free') COLLATE utf8_unicode_ci NOT NULL DEFAULT 'free',
  `timeEnd` varchar(255) COLLATE utf8_unicode_ci NOT NULL DEFAULT '-',
  `lastlogin` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `lastAddress` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`id`,`account`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Records of accounts
-- ----------------------------
INSERT INTO `accounts` VALUES ('1', 'test', 't', '0', '0', 'free', '-', '29.08.2010 20:34:22', '192.168.1.2:59677');
INSERT INTO `accounts` VALUES ('2', 'test2', 't', '0', '0', 'limited', '2009/02/26 18:00:00', '29.08.2010 20:34:26', '192.168.1.2:59678');
INSERT INTO `accounts` VALUES ('3', 'test3', 't', '0', '0', 'limited', '2011/02/26 18:00:00', '29.08.2010 20:34:57', '192.168.1.2:59680');
