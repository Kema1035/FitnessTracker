-- MySQL dump 10.13  Distrib 9.3.0, for macos15 (arm64)
--
-- Host: localhost    Database: fitnesstracker
-- ------------------------------------------------------
-- Server version	9.3.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `Gewicht`
--

DROP TABLE IF EXISTS `Gewicht`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Gewicht` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Datum` date NOT NULL,
  `Wert` decimal(5,2) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=35 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Gewicht`
--

LOCK TABLES `Gewicht` WRITE;
/*!40000 ALTER TABLE `Gewicht` DISABLE KEYS */;
INSERT INTO `Gewicht` VALUES (33,'2026-06-11',90.00),(34,'2026-06-11',88.00);
/*!40000 ALTER TABLE `Gewicht` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Mahlzeit`
--

DROP TABLE IF EXISTS `Mahlzeit`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Mahlzeit` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Datum` date NOT NULL,
  `Name` varchar(200) NOT NULL,
  `Kalorien` int DEFAULT NULL,
  `Protein` decimal(6,1) DEFAULT NULL,
  `Kohlenhydrate` decimal(6,1) DEFAULT NULL,
  `Fett` decimal(6,1) DEFAULT NULL,
  `Typ` varchar(50) DEFAULT 'Snack',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Mahlzeit`
--

LOCK TABLES `Mahlzeit` WRITE;
/*!40000 ALTER TABLE `Mahlzeit` DISABLE KEYS */;
INSERT INTO `Mahlzeit` VALUES (21,'2026-06-11','Avocados',95,1.0,1.0,9.8,'Frühstück'),(22,'2026-06-11','Baguette',124,2.2,23.0,1.9,'Frühstück'),(23,'2026-06-11','Philadelphia Original',113,2.7,2.2,10.5,'Frühstück'),(24,'2026-06-11','Original',401,9.0,62.0,1.0,'Mittagessen');
/*!40000 ALTER TABLE `Mahlzeit` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Profil`
--

DROP TABLE IF EXISTS `Profil`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Profil` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(100) DEFAULT NULL,
  `Alter_Jahre` int DEFAULT NULL,
  `Groesse` int DEFAULT NULL,
  `Gewicht` decimal(5,2) DEFAULT NULL,
  `Ziel` varchar(50) DEFAULT NULL,
  `TaeglicheKalorien` int DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Profil`
--

LOCK TABLES `Profil` WRITE;
/*!40000 ALTER TABLE `Profil` DISABLE KEYS */;
INSERT INTO `Profil` VALUES (1,'Misha Kemkin',22,185,85.00,'Gewicht halten',400);
/*!40000 ALTER TABLE `Profil` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Uebung`
--

DROP TABLE IF EXISTS `Uebung`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Uebung` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `WorkoutId` int NOT NULL,
  `Name` varchar(100) NOT NULL,
  `Gewicht` decimal(5,2) DEFAULT '0.00',
  `Wiederholungen` int DEFAULT '0',
  `Saetze` int DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `WorkoutId` (`WorkoutId`),
  CONSTRAINT `uebung_ibfk_1` FOREIGN KEY (`WorkoutId`) REFERENCES `Workout` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Uebung`
--

LOCK TABLES `Uebung` WRITE;
/*!40000 ALTER TABLE `Uebung` DISABLE KEYS */;
INSERT INTO `Uebung` VALUES (5,7,'Hantelen',20.00,12,4),(6,7,'bankdruecken',100.00,12,3),(7,7,'Biceps',12.00,12,4);
/*!40000 ALTER TABLE `Uebung` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Workout`
--

DROP TABLE IF EXISTS `Workout`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Workout` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Datum` date NOT NULL,
  `Typ` varchar(100) NOT NULL,
  `Dauer` int DEFAULT NULL,
  `Kalorien` int DEFAULT NULL,
  `Notizen` text,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Workout`
--

LOCK TABLES `Workout` WRITE;
/*!40000 ALTER TABLE `Workout` DISABLE KEYS */;
INSERT INTO `Workout` VALUES (7,'2026-06-11','Gym',120,900,'');
/*!40000 ALTER TABLE `Workout` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `Ziel`
--

DROP TABLE IF EXISTS `Ziel`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `Ziel` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Beschreibung` varchar(200) DEFAULT NULL,
  `Zielgewicht` decimal(5,2) DEFAULT NULL,
  `TaeglicheKalorien` int DEFAULT NULL,
  `Startdatum` date DEFAULT NULL,
  `Enddatum` date DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `Ziel`
--

LOCK TABLES `Ziel` WRITE;
/*!40000 ALTER TABLE `Ziel` DISABLE KEYS */;
INSERT INTO `Ziel` VALUES (3,'abnehmen',78.00,3000,'2026-06-09','2026-09-09');
/*!40000 ALTER TABLE `Ziel` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-06-12 13:36:07
