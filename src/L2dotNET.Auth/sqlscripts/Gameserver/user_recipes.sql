/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50143
Source Host           : localhost:3306
Source Database       : rabbit_cgame

Target Server Type    : MYSQL
Target Server Version : 50143
File Encoding         : 65001

Date: 2010-09-04 07:00:00
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for `user_recipes`
-- ----------------------------
DROP TABLE IF EXISTS `user_recipes`;
CREATE TABLE `user_recipes` (
  `ownerId` int(11) DEFAULT NULL,
  `recid` int(11) DEFAULT NULL,
  `iclass` int(3) DEFAULT NULL,
  `tact` bigint(20) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`tact`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Records of user_recipes
-- ----------------------------
INSERT INTO `user_recipes` VALUES ('10', '1', '0', '2');
INSERT INTO `user_recipes` VALUES ('10', '856', '0', '3');
