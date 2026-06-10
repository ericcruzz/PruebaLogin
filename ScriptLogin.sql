
CREATE DATABASE Staging;

USE Staging;

CREATE TABLE Users(
	UserID INT IDENTITY(1000,1) PRIMARY KEY,
	Username VARCHAR(50) NOT NULL,
	RecordDate DATETIME DEFAULT GETDATE(),
);

CREATE TABLE Passwords(
	UserID INT PRIMARY KEY,
	PasswordHash VARCHAR(255) NOT NULL,
	CONSTRAINT FK_Users_Passwords
	FOREIGN KEY (UserId) REFERENCES Users(UserID) ON DELETE CASCADE
);

GO

CREATE PROCEDURE SP_Operations(
	@operationaction VARCHAR(50),
	@recordid INT,
	@user VARCHAR(50),
	@password VARCHAR(255)
	)
AS
BEGIN
	
	IF @operationaction = 'inner'
	BEGIN
		SELECT u.UserID AS ID, u.Username AS Username, p.PasswordHash AS PasswordHash, u.RecordDate
		FROM Users u
		INNER JOIN Passwords p ON u.UserID = p.UserID;
	END
	ELSE IF @operationaction = 'view'
	BEGIN
		SELECT u.UserID AS ID, u.Username , p.PasswordHash AS PasswordHash, u.RecordDate
		FROM Users u
		INNER JOIN Passwords p ON u.UserID = p.UserID
		WHERE u.UserId = @recordid;
	END
	ELSE IF @operationaction = 'login'
	BEGIN
		SELECT 
			CASE 
				WHEN t.PasswordHash = @password THEN 0
			ELSE
				1
			END AS Result
		FROM (
			SELECT PasswordHash
			FROM Users s
			INNER JOIN Passwords p ON s.UserID = p.UserID
			WHERE s.Username = @user) AS t;
	END
END;

GO

INSERT INTO Users (Username) VALUES ('EricCruz');
INSERT INTO Passwords (UserID,PasswordHash) VALUES (1000, 'abcd');

INSERT INTO Users (Username) VALUES ('JuanPerez');
INSERT INTO Passwords (UserID,PasswordHash) VALUES (1001, '1234');

INSERT INTO Users (Username) VALUES ('ValeriaGarcia');
INSERT INTO Passwords (UserID,PasswordHash) VALUES (1002, 'ab12');

INSERT INTO Users (Username) VALUES ('Anonimo2');
INSERT INTO Passwords (UserID,PasswordHash) VALUES (1003, 'acd2');


SELECT * FROM Users;
SELECT * FROM Passwords;
SELECT u.UserID AS ID, u.Username , p.PasswordHash AS PasswordHash, u.RecordDate
FROM Users u
INNER JOIN Passwords p ON u.UserID = p.UserID;

GO

EXEC SP_Operations @operationaction = 'login', @recordid = 1002, @user = 'EricCruz', @password = 'abcd';

GO

DROP PROCEDURE SP_Operations;
 
GO
USE master;
DROP DATABASE Staging;