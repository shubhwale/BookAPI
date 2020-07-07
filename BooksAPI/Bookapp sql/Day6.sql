CREATE PROCEDURE SP_GetBooksByCategory
(
	@CategoryName VARCHAR(20)
) AS
BEGIN
	SELECT b.Title,c.CategoryName
	FROM Books_Categories bc
	INNER JOIN Books b ON bc.BookId = b.BookId
	INNER JOIN Categories c ON bc.CategoryId=c.CategoryId
	WHERE c.CategoryName=@CategoryName;
END;

EXECUTE SP_GetBooksByCategory 'programming'
GO