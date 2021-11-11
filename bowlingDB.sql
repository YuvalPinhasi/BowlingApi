CREATE DATABASE bowlingDB;

--- Create tables

USE bowlingDB
CREATE TABLE Users (
    UserID INT NOT NULL IDENTITY(1, 1),
    UserName varchar(255) NOT NULL,
    PRIMARY KEY (UserID)
);

USE bowlingDB
CREATE TABLE GameData (
    GameID INT NOT NULL IDENTITY(1, 1),
    UserID INT NOT NULL ,
	Shots  varchar(50),
	TotalScore INT,
	PRIMARY KEY (UserID)
);

--- Create view 

	USE bowlingDB
	GO
	CREATE VIEW V_TopScore
	AS
	SELECT GameData.TotalScore, Users.UserName
	FROM GameData,  Users
	Where GameData.UserID=Users.UserID
	GO










