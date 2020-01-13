CREATE TABLE Tournament (
	Id INT PRIMARY KEY AUTO_INCREMENT,
	Title VARCHAR(50) NOT NULL,
	StartDate DATETIME NOT NULL
);

Insert INTO Tournament (Title, StartDate) VALUES ("Erstes Turnier", "2020-02-12");
Insert INTO Tournament (Title, StartDate) VALUES ("Zweites Turnier", "2020-02-29");