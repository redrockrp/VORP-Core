-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Versión del servidor:         10.4.13-MariaDB - mariadb.org binary distribution
-- SO del servidor:              Win64
-- HeidiSQL Versión:             11.0.0.5919
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Volcando estructura de base de datos para vorp
CREATE DATABASE IF NOT EXISTS `vorp` /*!40100 DEFAULT CHARACTER SET utf8mb4 */;
USE `vorp`;

-- Volcando estructura para tabla vorp.characters
CREATE TABLE IF NOT EXISTS `characters` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `identifier` varchar(50) COLLATE utf8mb4_bin NOT NULL DEFAULT '',
  `charidentifier` int(11) DEFAULT -1,
  `group` varchar(10) COLLATE utf8mb4_bin DEFAULT 'user',
  `money` double(11,2) DEFAULT 0.00,
  `gold` double(11,2) DEFAULT 0.00,
  `rol` double(11,2) NOT NULL DEFAULT 0.00,
  `xp` int(11) DEFAULT 0,
  `inventory` longtext COLLATE utf8mb4_bin DEFAULT '',
  `job` varchar(50) COLLATE utf8mb4_bin DEFAULT 'unemployed',
  `status` varchar(140) COLLATE utf8mb4_bin DEFAULT '{}',
  `firstname` varchar(50) COLLATE utf8mb4_bin DEFAULT ' ',
  `lastname` varchar(50) COLLATE utf8mb4_bin DEFAULT ' ',
  `skinPlayer` longtext COLLATE utf8mb4_bin DEFAULT '',
  `compPlayer` longtext COLLATE utf8mb4_bin DEFAULT '',
  `jobgrade` int(11) DEFAULT 0,
  `coords` varchar(75) COLLATE utf8mb4_bin DEFAULT '{}',
  `isdead` tinyint(1) DEFAULT 0,
  PRIMARY KEY (`id`) USING BTREE,
  UNIQUE KEY `identifier_charidentifier` (`identifier`,`charidentifier`),
  CONSTRAINT `FK_characters_users` FOREIGN KEY (`identifier`) REFERENCES `users` (`identifier`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin ROW_FORMAT=DYNAMIC;

-- La exportación de datos fue deseleccionada.

-- Volcando estructura para tabla vorp.users
CREATE TABLE IF NOT EXISTS `users` (
  `identifier` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_bin NOT NULL,
  `group` varchar(50) DEFAULT 'user',
  `warnings` int(11) DEFAULT 0,
  `banned` tinyint(4) DEFAULT 0,
  PRIMARY KEY (`identifier`),
  UNIQUE KEY `identifier` (`identifier`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- La exportación de datos fue deseleccionada.

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
