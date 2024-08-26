CREATE TABLE users (    
    UserId INT PRIMARY KEY IDENTITY(1,1),	
    Name NVARCHAR(255) NOT NULL,
	Role NVARCHAR(255) NOT NULL,
	Email NVARCHAR(255) NOT NULL,
    UserCreated DATETIME NOT NULL
);



CREATE TABLE SearchHistory (
    SearchId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    Query NVARCHAR(255) NOT NULL,
    Timestamp DATETIME NOT NULL
);

CREATE TABLE SearchResults (
    ResultId INT PRIMARY KEY IDENTITY(1,1),
	UserId INT FOREIGN KEY REFERENCES Users(UserId),
    SearchId INT FOREIGN KEY REFERENCES SearchHistory(SearchId),
    ResultData NVARCHAR(MAX) NOT NULL,
    ResultRank INT NOT NULL,
    RetrievedAt DATETIME NOT NULL
);


INSERT INTO Users (Name, Role, Email, UserCreated)
VALUES ('Shailesh', 'Developer', 'jadhavshailesh00@gmail.com', GETDATE());

INSERT INTO Users (Name, Role, Email, UserCreated)
VALUES ('Ram', 'Admin', 'Ram00@gmail.com', GETDATE());

Select * from users
Select * from  SearchHistory
CREATE TABLE ContentItems (
    ID NVARCHAR(10) PRIMARY KEY,
    Category NVARCHAR(50) NOT NULL,
    Date DATE NOT NULL,
    Description NVARCHAR(100) NOT NULL,
    Title NVARCHAR(100) NOT NULL
);

INSERT INTO ContentItems (ID, Category, Date, Description, Title)
VALUES
  ('1', 'Technology', '2024-08-08', 'New Technology', 'Title Technology'),
  ('2', 'Entertainment', '2024-07-08', 'New Entertainment', 'Title Entertainment'),
  ('3', 'Sports', '2024-06-08', 'New Sports', 'Title Sports'),
  ('4', 'Health', '2024-05-08', 'New Health', 'Title Health'),
  ('5', 'Business', '2024-04-08', 'New Business', 'Title Business'),
  ('6', 'Travel', '2024-03-08', 'New Travel', 'Title Travel'),
  ('7', 'Food', '2024-02-08', 'New Food', 'Title Food'),
  ('8', 'Fashion', '2024-01-08', 'New Fashion', 'Title Fashion'),
  ('9', 'Books', '2024-11-08', 'New Books', 'Title Books'),
  ('10', 'Music', '2024-12-08', 'New album', 'Title album');

  Select * from ContentItems

  SELECT * FROM ContentItems WHERE ID = 9

SELECT * FROM ContentItems WHERE ID = 9 ORDER BY Date DESC


Select * from  users

alter table users add password NVARCHAR(10)
update users set password='password'

Select * from  SearchHistory
Select * from  SearchResults

SELECT * FROM ContentItems WHERE Title like '%b%' or 
Description like '%b%' or Category like '%b%' ORDER BY Date DESC

INSERT INTO SearchResults (UserId, SearchId, ResultData, ResultRank, RetrievedAt)
SELECT 
  NULL AS UserId,  -- Assuming search is not user specific
  NULL AS SearchId, -- Assuming you want to insert this as a generic search
  ci.Title | ' - ' | ci.Description || ' - ' | ci.Category AS ResultData,
  ROW_NUMBER() OVER (ORDER BY ci.Date DESC) AS ResultRank,
  GETDATE() AS RetrievedAt
FROM ContentItems ci
WHERE ci.Title LIKE '%in%' OR ci.Description LIKE '%in%' OR ci.Category LIKE '%in%'
ORDER BY ci.Date DESC;

INSERT INTO SearchResults (UserId, SearchId,ResultData, ResultRank, RetrievedAt)" +
                    " VALUES (1, 1, ,'ResultData',@ResultRank, @RetrievedAt)