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
Username varchar(50),
Password varchar(255),
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
Text nvarchar(MAX) not null,
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

------------------------------ INSERTS ----------------------------------

insert into Role (Name) values ('Member'), ('Contributor'), ('Moderator'), ('Admin');

insert into Category(Name, Description) values
('Comfort food', 'Food that provides a nostalgic or sentimental value to someone and may be characterized by its high caloric nature associated with childhood or home cooking.'),
('Soups', 'A liquid dish, typically savoury and made by boiling meat, fish, or vegetables etc. in stock or water.'),
('BBQ', 'A meal or gathering at which meat, fish, or other food is cooked out of doors on a rack over an open fire or on a special appliance.');

insert into Member(
					UniqueId, 
					FirstName,
					LastName, 
					DateOfBirth, 
					Bio, 
					IsActive, 
					DateCreated, 
					DateUpdated, 
					Username, 
					Password, 
					RoleId) 
values('d7b19efe-39aa-4d25-b802-21ac8fa0b0f4',
'Izmisljeni',
'Korisnik',
'10-05-1999',
'Temp bio',
'1',
'08-07-2024',
'08-07-2024',
'samsung',
'galaxyA52',
'4');

