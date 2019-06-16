-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Versión del servidor:         8.0.11 - MySQL Community Server - GPL
-- SO del servidor:              Win64
-- HeidiSQL Versión:             9.5.0.5280
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Volcando estructura de base de datos para gtc
DROP DATABASE IF EXISTS `gtc`;
CREATE DATABASE IF NOT EXISTS `gtc` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `gtc`;

-- Volcando estructura para tabla gtc.accounts
DROP TABLE IF EXISTS `accounts`;
CREATE TABLE IF NOT EXISTS `accounts` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(32) NOT NULL,
  `email` varchar(64) NOT NULL,
  `password` varchar(64) NOT NULL,
  `socialName` varchar(32) NOT NULL,
  `state` tinyint(4) NOT NULL DEFAULT '1',
  `lastLogged` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `socialName` (`socialName`,`username`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COMMENT='User accounts';

-- La exportación de datos fue deseleccionada.
-- Volcando estructura para tabla gtc.characters
DROP TABLE IF EXISTS `characters`;
CREATE TABLE IF NOT EXISTS `characters` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `account` int(11) DEFAULT NULL,
  `name` varchar(32) NOT NULL,
  `played` int(11) NOT NULL DEFAULT '0',
  `experience` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `name` (`name`),
  KEY `Accounts_Characters` (`account`),
  CONSTRAINT `Accounts_Characters` FOREIGN KEY (`account`) REFERENCES `accounts` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Characters bound to an account';

-- La exportación de datos fue deseleccionada.
-- Volcando estructura para tabla gtc.games
DROP TABLE IF EXISTS `games`;
CREATE TABLE IF NOT EXISTS `games` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(64) NOT NULL DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='List of the games from Grand Theft Challenge';

-- La exportación de datos fue deseleccionada.
-- Volcando estructura para tabla gtc.maps
DROP TABLE IF EXISTS `maps`;
CREATE TABLE IF NOT EXISTS `maps` (
  `trackId` int(11) NOT NULL,
  `object` int(11) NOT NULL,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  `rotX` float NOT NULL,
  `rotY` float NOT NULL,
  `rotZ` float NOT NULL,
  KEY `Tracks_Maps` (`trackId`),
  CONSTRAINT `Tracks_Maps` FOREIGN KEY (`trackId`) REFERENCES `tracks` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='List of objects for each one of the tracks';

-- La exportación de datos fue deseleccionada.
-- Volcando estructura para tabla gtc.punishments
DROP TABLE IF EXISTS `punishments`;
CREATE TABLE IF NOT EXISTS `punishments` (
  `player` int(11) NOT NULL,
  `staff` int(11) NOT NULL,
  `type` tinyint(4) NOT NULL DEFAULT '0',
  `reason` varchar(128) NOT NULL DEFAULT '',
  `time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`player`,`staff`,`time`),
  KEY `Staff_Characters` (`staff`),
  CONSTRAINT `Player_Characters` FOREIGN KEY (`player`) REFERENCES `characters` (`id`),
  CONSTRAINT `Staff_Characters` FOREIGN KEY (`staff`) REFERENCES `characters` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Stores all the punishments from the admins';

-- La exportación de datos fue deseleccionada.
-- Volcando estructura para tabla gtc.spawns
DROP TABLE IF EXISTS `spawns`;
CREATE TABLE IF NOT EXISTS `spawns` (
  `trackId` int(11) DEFAULT NULL,
  `posX` float NOT NULL,
  `posY` float NOT NULL,
  `posZ` float NOT NULL,
  `rotX` float NOT NULL,
  `rotY` float NOT NULL,
  `rotZ` float NOT NULL,
  KEY `Tracks_Spawns` (`trackId`),
  CONSTRAINT `Tracks_Spawns` FOREIGN KEY (`trackId`) REFERENCES `tracks` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='List of the spawn points for the players on a track';

-- La exportación de datos fue deseleccionada.
-- Volcando estructura para tabla gtc.tracks
DROP TABLE IF EXISTS `tracks`;
CREATE TABLE IF NOT EXISTS `tracks` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `game` int(11) DEFAULT NULL,
  `name` varchar(64) NOT NULL DEFAULT '',
  PRIMARY KEY (`id`),
  KEY `Games_Tracks` (`game`),
  CONSTRAINT `Games_Tracks` FOREIGN KEY (`game`) REFERENCES `games` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='List of the tracks for each game type';

-- La exportación de datos fue deseleccionada.
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
