/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50143
Source Host           : localhost:3306
Source Database       : rabbit_cgame

Target Server Type    : MYSQL
Target Server Version : 50143
File Encoding         : 65001

Date: 2010-10-09 04:15:27
*/

SET FOREIGN_KEY_CHECKS=0;
-- ----------------------------
-- Table structure for `user_skills`
-- ----------------------------
DROP TABLE IF EXISTS `user_skills`;
CREATE TABLE `user_skills` (
  `ownerId` int(11) NOT NULL,
  `id` int(11) NOT NULL DEFAULT '0',
  `lvl` int(11) DEFAULT NULL,
  `iclass` int(3) NOT NULL,
  PRIMARY KEY (`ownerId`,`id`,`iclass`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

