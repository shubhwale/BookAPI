CREATE TABLE [dbo].[RefreshToken] (
    [token_id]    INT          IDENTITY (1, 1) NOT NULL,
    [user_id]    INT          NOT NULL,
    [token]  VARCHAR (200) NOT NULL,
    [expiry_date] DATETIME     NOT NULL
    
    CONSTRAINT [PK_RefreshToken] PRIMARY KEY CLUSTERED ([token_id] ASC),
    FOREIGN KEY ([user_id]) REFERENCES [dbo].[Customers] ([CustId]) ON DELETE CASCADE ON UPDATE CASCADE  
);
GO

CREATE PROCEDURE SP_GetBooksByCategoryId
(
	@CategoryName VARCHAR(20)
) AS
BEGIN
	SELECT *
	FROM Books_Categories bc
	INNER JOIN Books b ON bc.Bid = b.BookId
	INNER JOIN Categories c ON bc.Cid=c.CategoryId
	WHERE c.CategoryId = bc.Cid
END;

EXECUTE SP_GetBooksByCategory 'programming'
GO