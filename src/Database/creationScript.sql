CREATE DATABASE kickerapp_db;
USE kickerapp_db;

-- Turniere
CREATE TABLE Tournament (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Title NVARCHAR(50) NOT NULL,
	StartDate DATETIME NOT NULL
);

Insert INTO Tournament (Title, StartDate) VALUES ('Erstes Turnier', '2020-02-12');
Insert INTO Tournament (Title, StartDate) VALUES ('Zweites Turnier', '2020-02-29');

-- Spieler
CREATE TABLE Player (
	Id INT PRIMARY KEY IDENTITY(1,1),
	Abbreviation NVARCHAR(3) NOT NULL
);

Insert INTO Player (Abbreviation) VALUES ('ABC');
Insert INTO Player (Abbreviation) VALUES ('DEF');
