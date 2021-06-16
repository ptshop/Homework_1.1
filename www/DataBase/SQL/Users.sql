CREATE TABLE `Users` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Login` varchar(100) NOT NULL,
  `PasswordHash` varchar(100) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `Surname` varchar(100) NOT NULL,
  `Age` int NOT NULL,
  `Gender` int NOT NULL,
  `Interest` varchar(50) DEFAULT NULL,
  `City` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
