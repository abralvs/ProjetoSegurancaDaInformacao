-- MySQL dump 10.13  Distrib 5.6.26, for Win64 (x86_64)
--
-- Host: localhost    Database: db_a62123_italabs
-- ------------------------------------------------------
-- Server version	5.6.26-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Dumping routines for database 'db_a62123_italabs'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-05-27 20:41:50

-- -----------------------------------------------------
-- Table `db_a62123_italabs`.`Usuario`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `db_a62123_italabs`.`Usuario` (
  `idUsuario` INT NOT NULL AUTO_INCREMENT,
  `nome` VARCHAR(100) NOT NULL,
  `usuario` VARCHAR(100) NOT NULL,
  `senha` VARCHAR(16) NOT NULL,
  `perfil` VARCHAR(200) NOT NULL,
  PRIMARY KEY (`idUsuario`),
  UNIQUE INDEX `idUsuario_UNIQUE` (`idUsuario` ASC) VISIBLE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `db_a62123_italabs`.`Tematica`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `db_a62123_italabs`.`Tematica` (
  `idTematica` INT NOT NULL AUTO_INCREMENT,
  `titulo` VARCHAR(100) NOT NULL,
  `descricao` VARCHAR(300) NULL,
  PRIMARY KEY (`idTematica`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `db_a62123_italabs`.`Pergunta`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `db_a62123_italabs`.`Pergunta` (
  `idPergunta` INT NOT NULL AUTO_INCREMENT,
  `questao` VARCHAR(500) NOT NULL,
  `alternativa1` VARCHAR(150) NOT NULL,
  `alternativa2` VARCHAR(150) NOT NULL,
  `alternativa3` VARCHAR(150) NULL,
  `alternativa4` VARCHAR(150) NULL,
  `opcaoCorreta` INT NOT NULL,
  `pontuacao` ENUM('5', '10', '15') NOT NULL,
  `tempo` ENUM('15', '25', '35') NOT NULL,
  `dificuldade` ENUM('FACIL', 'MEDIA', 'DIFICIL') NOT NULL,
  `Tematica_idTematica` INT NOT NULL,
  PRIMARY KEY (`idPergunta`),
  UNIQUE INDEX `idPergunta_UNIQUE` (`idPergunta` ASC) VISIBLE,
  INDEX `fk_Pergunta_Tematica1_idx` (`Tematica_idTematica` ASC) VISIBLE,
  CONSTRAINT `fk_Pergunta_Tematica1`
    FOREIGN KEY (`Tematica_idTematica`)
    REFERENCES `db_a62123_italabs`.`Tematica` (`idTematica`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `db_a62123_italabs`.`Usuario_has_Pergunta`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `db_a62123_italabs`.`Usuario_has_Pergunta` (
  `idUsuario` INT NOT NULL,
  `idPergunta` INT NOT NULL,
  `acertou` TINYINT NOT NULL,
  PRIMARY KEY (`idUsuario`, `idPergunta`),
  INDEX `fk_Usuario_has_Pergunta_Pergunta1_idx` (`idPergunta` ASC) VISIBLE,
  INDEX `fk_Usuario_has_Pergunta_Usuario_idx` (`idUsuario` ASC) VISIBLE,
  CONSTRAINT `fk_Usuario_has_Pergunta_Usuario`
    FOREIGN KEY (`idUsuario`)
    REFERENCES `db_a62123_italabs`.`Usuario` (`idUsuario`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_Usuario_has_Pergunta_Pergunta1`
    FOREIGN KEY (`idPergunta`)
    REFERENCES `db_a62123_italabs`.`Pergunta` (`idPergunta`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
