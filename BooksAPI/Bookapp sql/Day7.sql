USE BookApp
GO

SP_HELP Books_Categories
GO

EXEC SP_RENAME 'dbo.Books_Categories.BookId','Bid','COLUMN'
GO

EXEC SP_RENAME 'dbo.Books_Categories.CategoryId','Cid','COLUMN'
GO

ALTER TABLE Books_Categories
DROP CONSTRAINT FK_Books
GO

ALTER TABLE Books_Categories
DROP CONSTRAINT FK_Categories
GO

ALTER TABLE Books_Categories
DROP CONSTRAINT PK_BookId_CategoryId
GO

ALTER TABLE Books_Categories
ADD CONSTRAINT FK_Bid
FOREIGN KEY (Bid) REFERENCES Books(BookId)
GO

ALTER TABLE Books_Categories
ADD CONSTRAINT FK_Cid
FOREIGN KEY (Cid) REFERENCES Categories(CategoryId)
GO

ALTER TABLE Books_Categories
ADD PRIMARY KEY (Bid,Cid)
GO

ALTER TABLE Books_Categories
DROP CONSTRAINT PK__Books_Ca__CACEEC4F86E6646B
GO

ALTER TABLE Books_Categories
ADD CONSTRAINT PK_Bid_Cid
PRIMARY KEY (Bid,Cid)
GO

DROP PROCEDURE SP_GetBooksByCategory  
GO 

SELECT *
FROM Books_Categories bc
INNER JOIN Books b ON bc.Bid = b.BookId
INNER JOIN Categories c ON bc.Cid=c.CategoryId
WHERE c.CategoryName='Technology'
GO

CREATE PROCEDURE SP_GetBooksByCategory
(
	@CategoryName VARCHAR(20)
) AS
BEGIN
	SELECT b.Title,c.CategoryName
	FROM Books_Categories bc
	INNER JOIN Books b ON bc.Bid = b.BookId
	INNER JOIN Categories c ON bc.Cid=c.CategoryId
	WHERE c.CategoryName=@CategoryName;
END;
GO

ALTER PROCEDURE SP_GetBooksByCategory
(
	@CategoryName VARCHAR(20)
) AS
BEGIN
	SELECT *
	FROM Books_Categories bc
	INNER JOIN Books b ON bc.Bid = b.BookId
	INNER JOIN Categories c ON bc.Cid=c.CategoryId
	WHERE c.CategoryName=@CategoryName;
END;

EXECUTE SP_GetBooksByCategory 'programming'
GO

UPDATE Categories
SET CategoryName='Self_Help'
WHERE CategoryId=6
GO