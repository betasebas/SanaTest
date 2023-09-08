CREATE DATABASE SanaTest
GO 
USE SanaTest
GO

CREATE TABLE Products (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Code VARCHAR(250) NOT NULL,
    [Description] VARCHAR(100) NULL,
    Stock INT NOT NULL,
    [Image] VARCHAR(600),
    [Value] DECIMAL not null

)
GO

CREATE TABLE Categories (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    [Name] VARCHAR(50) NOT NULL,
    [Description] VARCHAR(100) NULL
)
GO

CREATE TABLE ProductCategories(
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    IdProduct UNIQUEIDENTIFIER,
    IdCategory UNIQUEIDENTIFIER
)
GO

ALTER TABLE ProductCategories ADD CONSTRAINT FK_PRODUCTS
FOREIGN KEY (IdProduct) REFERENCES Products
GO

ALTER TABLE ProductCategories ADD CONSTRAINT FK_CATEGORIES
FOREIGN KEY (IdCategory) REFERENCES Categories
GO

CREATE TABLE Customers(
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    FirstName VARCHAR(64) NOT NULL,
    LastName VARCHAR(64) NOT NULL,
    CellPhone INT NULL,
    Mail VARCHAR(120) NULL,
    Identification varchar(30) not null
)
GO

CREATE TABLE Orders(
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    [Date] DATE not null,
    [Value] DECIMAL not null,
    IdCustomer UNIQUEIDENTIFIER not null,
)
GO


ALTER TABLE Orders ADD CONSTRAINT FK_CUSTOMER
FOREIGN KEY (IdCustomer) REFERENCES Customers
GO

CREATE TABLE OrderProducts(
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    IdOrder UNIQUEIDENTIFIER not null,
    IdProduct UNIQUEIDENTIFIER not null,
    [SubValue] DECIMAL not null,
    [Quantity] INT not null
)
GO

ALTER TABLE OrderProducts ADD CONSTRAINT FK_ORDER
FOREIGN KEY (IdOrder) REFERENCES Orders
GO

ALTER TABLE OrderProducts ADD CONSTRAINT FK_PRODUCTORDER
FOREIGN KEY (IdProduct) REFERENCES Products



