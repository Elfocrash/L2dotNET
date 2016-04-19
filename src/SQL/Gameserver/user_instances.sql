/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50143
Source Host           : localhost:3306
Source Database       : rabbit_cgame

Target Server Type    : MYSQL
Target Server Version : 50143
File Encoding         : 65001

Date: 2010-09-27 19:07:11
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for `user_instances`
-- ----------------------------
DROP TABLE IF EXISTS `user_instances`;
CREATE TABLE `user_instances` (
  `ownerId` int(11) NOT NULL,
  `name` varchar(200) COLLATE utf8_unicode_ci DEFAULT NULL,
  `instanceId` int(5) NOT NULL,
  `disabledTo` varchar(200) COLLATE utf8_unicode_ci NOT NULL,
  PRIMARY KEY (`ownerId`,`instanceId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Records of user_instances
-- ----------------------------
