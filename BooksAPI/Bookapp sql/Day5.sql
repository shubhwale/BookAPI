SP_HELP Books_Categories
GO

ALTER TABLE Books_Categories ADD CONSTRAINT PK_BookId_CategoryId PRIMARY KEY (BookId,CategoryId)
GO

SELECT * FROM Books_Categories
GO

SELECT * FROM Categories
GO

SELECT b.Title,c.CategoryName
FROM Books_Categories bc
INNER JOIN Books b ON bc.BookId = b.BookId
INNER JOIN Categories c ON bc.CategoryId=c.CategoryId
WHERE c.CategoryName='Technology'