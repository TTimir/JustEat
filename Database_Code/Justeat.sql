CREATE DATABASE JustEatdb
use justeatdb 

Create table Users
(
	UserId int primary key identity(1,1) not null,
	Name varchar(50) null,
	Username varchar(50) null Unique,
	[Password] varchar(50) null,
	Mobile varchar(50) null,
	Email varchar(50) null unique,
	[Address] varchar(max) null,
	PostCode varchar(50) null,
	ImageUrl varchar(max) null,
	CreatedDate datetime null
);

Create table Contact(
	ContactId int primary key identity(1,1) not null,
	Name varchar(50) null,
	Email varchar(50) null,
	[Subject] varchar(200) null,
	[Message] varchar(max) null,
	CreatedDate datetime null
);

Create table Categories(
	CategoryId int primary key identity(1,1) not null,
	Name varchar(50) null,
	ImageUrl varchar(max) null,
	IsActive bit null,
	CreatedDate datetime null
);

Create table Products(
	ProductId int primary key identity(1,1) not null,
	Name varchar(50) null,
	[Description] varchar(max) null,
	Price decimal(18,2) null,
	Quantity int null,
	ImageUrl varchar(max) null,
	CategoryId int null, --FK
	IsActive bit null,
	CreatedDate datetime null
);

Create table Carts(
	CartId int primary key identity(1,1) not null,
	ProductId int null, --FK,
	Quantity int null,
	UserId int null, --FK
);

Create table Orders(
	OrderDetailsId int primary key identity(1,1) not null,
	OrderNo varchar(max) null,
	ProductId int null, --FK
	Quantity int null,
	UserId int null, --FK
	[Status] varchar(50) null,
	PaymentId int null, --FK
	OrderDate datetime null
);

Create table Payment(
	PaymentId int primary key identity(1,1) not null,
	Name varchar(50) null,
	CardNo varchar(50) null,
	ExpiryDate varchar(50) null,
	CvNo int null,
	[Address] varchar(max) null,
	PaymentMode varchar(50)  null
);