use master;
go
drop database if exists "FamilyCookbook";
go
create database "FamilyCookbook";
go
alter database "FamilyCookbook" collate CROATIAN_CI_AS;
go
use "FamilyCookbook";


------------------------- TABLES --------------------------

create table Role (
Id int not null primary key identity(1,1),
Name varchar (30) not null
);

create table Member(
Id int primary key identity(1,1),
UniqueId uniqueidentifier,
FirstName varchar(30),
LastName varchar(30),
DateOfBirth date,
Bio varchar(1200),
IsActive bit,
DateCreated date,
DateUpdated date,
RoleId int,
PictureId int

);

create table Category (
Id int primary key identity(1,1),
Name varchar(50),
Description varchar(500)
);

create table Picture(
Id int primary key identity (1,1),
Name varchar (100),
Location varchar(255)
);

create table Recipe(
Id int primary key identity(1,1),
Title varchar(200) not null,
Subtitle varchar(200) not null,
Text varchar(MAX) not null,
IsActive bit not null,
DateCreated date not null, 
DateUpdated date not null,
CategoryId int not null,
PictureId int
);

create table MemberRecipe(
Id int primary key identity(1,1),
MemberId int,
RecipeId int

);

create table Comment(
Id int primary key identity(1,1),
MemberId int,
RecipeId int,
Text varchar(1200),
Rating int
);

----------------------------------- ALTERS --------------------------

alter table Member add foreign key (RoleId) references Role(Id);
alter table Member add foreign key (PictureId) references Picture(Id);
alter table Recipe add foreign key (CategoryId) references Category(Id);
alter table Recipe add foreign key (PictureId) references Picture(Id);
alter table MemberRecipe add foreign key (MemberId) references Member(Id);
alter table MemberRecipe add foreign key (RecipeId) references Recipe(Id);
alter table Comment add foreign key(MemberId) references Member(Id);
alter table Comment add foreign key(RecipeId) references Recipe(Id);