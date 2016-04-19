/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50143
Source Host           : localhost:3306
Source Database       : rabbit_cgame

Target Server Type    : MYSQL
Target Server Version : 50143
File Encoding         : 65001

Date: 2010-09-02 08:16:18
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for `user_quests`
-- ----------------------------
DROP TABLE IF EXISTS `user_quests`;
CREATE TABLE `user_quests` (
  `ownerId` int(11) DEFAULT NULL,
  `iclass` int(3) DEFAULT NULL,
  `qid` int(11) DEFAULT NULL,
  `qstage` int(11) NOT NULL DEFAULT '1',
  `qfin` int(1) NOT NULL DEFAULT '0',
  `tact` bigint(20) NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`tact`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- ----------------------------
-- Records of user_quests
-- ----------------------------
INSERT INTO `user_quests` VALUES ('10', '0', '1', '1', '0', '2');
