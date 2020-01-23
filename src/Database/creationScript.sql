CREATE DATABASE kickerapp_db;
USE kickerapp_db;

-- Turniere
CREATE TABLE Tournament (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Title NVARCHAR(50) NOT NULL,
	StartDate DATETIME NOT NULL,
  State INT NOT NULL
);

Insert INTO Tournament (Title, StartDate, STATE) VALUES ('Erstes Turnier', '2020-02-12', 1);
Insert INTO Tournament (Title, StartDate, STATE) VALUES ('Zweites Turnier', '2020-02-29', 1);

CREATE TABLE Teams (
  TournamentId INT NOT NULL FOREIGN KEY REFERENCES Tournament(Id),
  Name NVARCHAR(255) NOT NULL,
  FirstPlayerId INT,
  SecondPlayerId INT
);

-- Spieler
CREATE TABLE Player (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Abbreviation NVARCHAR(3) NOT NULL
);

Insert INTO Player (Abbreviation) VALUES ('EBE');
Insert INTO Player (Abbreviation) VALUES ('HEN');
