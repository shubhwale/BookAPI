SP_HELP Books
GO

ALTER TABLE Books ADD ImageUrl VARCHAR(200)
GO

INSERT INTO Books(BookId,ImageUrl,Title,Author,Publisher,NoOfPages,Rating,Edition,Price,ReleaseDate) VALUES(101,'assets/images/java8_in_action.jpeg','Java 8 in Action','Mario Fusco','Wiley',424,4,2,618,'2018/5/23')
GO

SELECT BookId,ImageUrl,Title,Author,Publisher,NoOfPages,Rating,Edition,Price,ReleaseDate FROM Books
GO

INSERT INTO Books(BookId,ImageUrl,Title,Author,Publisher,NoOfPages,Rating,Edition,Price,ReleaseDate) VALUES
(102,'assets/images/head_first_servlet_&_jsp.jpg','Head First Servlet & JSP','Bert Bates','O Reilly',928,5,2,1140,'2014/10/28'),
(103,'assets/images/angular_2_cookbook.jpeg','Angular 2 Cookbook','Matt Frisbie','Packt',464,3,1,958,'2017/4/25'),
(104,'assets/images/its_my_life.jpeg','Its My Life','Baig Mirza Yawar','Prakash Book Depot',230,3,1,255,'2018/5/12'),
(105,'assets/images/a_monks_memoir.jpeg','A Monk''s Memoir','Om Swami','Harper',230,3,1,311,'2013/7/22'),
(106,'assets/images/alexander_the_great.jpeg','Alexander : The Great','Abbott Jacob','Prakash Book Depot',320,4,1,135,'2012/2/7'),
(107,'assets/images/rich_dad_poor_dad.jpeg','Rich Dad Poor Dad','Robert Kiyosaki','MANJUL PUBLISHING HOUSE PVT. LTD',215,4,1,150,'2012/6/17'),
(108,'assets/images/start_with_why.jpeg','Start With Why','Simon Simek','Penguin',215,5,1,264,'2015/11/17'),
(109,'assets/images/one_up_on_wall_street.jpeg','One Up On Wall Street','Lynch Peter','Simon & Schuster',304,4,1,343,'2013/12/27')
GO

SP_HELP Cities
GO

SP_HELP States
GO

INSERT INTO Cities VALUES
('Bhopal'),
('Mumbai'),
('Bengaluru'),
('Kolkata'),
('Chennai')
GO

INSERT INTO States VALUES
('Madhya Pradesh'),
('Maharashtra'),
('Karnataka'),
('West Bengal'),
('Tamil Nadu')
GO

SELECT * FROM Cities
GO

SELECT * FROM States
GO

SP_HELP Categories
GO

INSERT INTO Categories VALUES
('Programming'),
('Biographies'),
('Business'),
('Technology'),
('History'),
('Self Help')
GO

SELECT * FROM Categories
GO

SP_HELP Books_Categories
GO

INSERT INTO Books_Categories VALUES
(101,1),
(101,4),
(102,1),
(102,4),
(103,1),
(103,4),
(104,2),
(104,6),
(105,2),
(106,2),
(106,5),
(107,3),
(107,6),
(108,3),
(108,6),
(109,3)
GO

SELECT * FROM Books_Categories
GO

SP_HELP Customers
GO

SELECT * FROM Cities
GO

SELECT * FROM States
GO

INSERT INTO Customers(Name,Email,Password,Contact,Gender,AddressLine,CityID,StateID) VALUES
('James Margold','james@email.com','james123',7894561230,'Male','Borivali',2,2)
GO

SELECT * FROM Customers
GO