﻿use master;
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
Name varchar (30) not null,
IsActive bit
);

create table Member(
Id int primary key identity(1,1),
UniqueId uniqueidentifier not null,
FirstName varchar(30) not null,
LastName varchar(30) not null,
DateOfBirth datetime not null,
Bio varchar(1200),
IsActive bit not null,
DateCreated datetime not null,
DateUpdated datetime,
Username varchar(50) not null,
Password varchar(255) not null,
RoleId int not null,
PictureId int

);

create table Category (
Id int primary key identity(1,1),
Name varchar(50) not null,
Description varchar(500),
IsActive bit
);

create table Picture(
Id int primary key identity (1,1),
Name varchar (100) not null,
Location varchar(255) not null,
IsActive bit
);

create table Recipe(
Id int primary key identity(1,1),
Title varchar(200) not null,
Subtitle varchar(200) not null,
Text nvarchar(MAX) not null,
IsActive bit not null,
DateCreated datetime not null, 
DateUpdated datetime not null,
CategoryId int not null,
PictureId int,
Rating decimal
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
Rating int,
IsActive bit,
DateCreated datetime,
DateUpdated datetime
);

create table Tag(
Id int primary key identity(1,1),
Text varchar(20)
);

create table RecipeTags(
Id int primary key identity(1,1),
TagId int,
RecipeId int,
);


create table Banner(
Id int primary key identity (1,1),
Location varchar(255),
Destination varchar(255),
Name varchar(30),
DateCreated datetime,
DateUpdated datetime,
IsActive bit, 
BannerType int,
);

create table BannerPosition(
Id int primary key identity(1,1),
Position int,
BannerId int
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

alter table RecipeTags add foreign key (TagId) references Tag(Id);
alter table RecipeTags add foreign key (RecipeId) references Recipe(Id);

alter table BannerPosition add foreign key(BannerId) references Banner(Id);

----------------------------- CONSTRAINTS ------------------------------

ALTER table Recipe add constraint Rating check (Rating >= 0.0 And Rating <= 5.0);

ALTER table Comment add constraint Comment_Rating check (Rating >= 0 and Rating <= 5);

------------------------------ INSERTS ----------------------------------

insert into Role (Name, IsActive) values ('Member', 1), ('Contributor', 1), ('Moderator', 1), ('Admin', 1);

insert into Category(Name, Description, IsActive) values
('Comfort food', 'Food that provides a nostalgic or sentimental value to someone and may be characterized by its high caloric nature associated with childhood or home cooking.',1),
('Soups', 'A liquid dish, typically savoury and made by boiling meat, fish, or vegetables etc. in stock or water.', 1),
('BBQ', 'A meal or gathering at which meat, fish, or other food is cooked out of doors on a rack over an open fire or on a special appliance.', 1);

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
values
('d7b19efe-39aa-4d25-b802-21ac8fa0b0f4',
'Izmisljeni',
'Korisnik',
'10-05-1999',
'Temp bio',
'1',
'08-07-2024',
'08-07-2024',
'samsung',
'$2a$12$LfIczJbu.Akfa0/Pkt1ANe42gSguBgvcoqVIFJ0UUJ5P6WphzQv1a',
'4');

insert into Tag(Text) VALUES 
('riba'), 
('janjetina'), 
('tikvice'), 
('bundeva'), 
('domaće'), 
('fish'),
('šaran'); 

insert into BannerPosition(Position) Values (1), (2);


select * distinct Name from Picture;

WITH DistinctPictures AS
(
    SELECT *,
           ROW_NUMBER() OVER (PARTITION BY Name ORDER BY Name) AS RowNum
    FROM Picture
)
SELECT *
FROM DistinctPictures
WHERE RowNum = 1;

SELECT *
FROM Picture p
WHERE p.Id IN (
    SELECT MIN(p2.Id)
    FROM Picture p2
    GROUP BY p2.Name
) Order by Name;



EXEC sp_rename 'dbo.Banner.DestinationName', 'Name', 'COLUMN';


ALTER TABLE Banner ALTER COLUMN BannerType int;  

ALTER TABLE Banner
ADD CONSTRAINT chk_BANNER_TYPE CHECK (BannerType > 0);

select * from BannerPosition;

select * from Category;

insert into BannerPosition (Position) values (2);
 
insert into BannerPosition (BannerId, Position) values (11,1);

select top 1 b.* from BannerPosition a join Banner b on b.Id = a.BannerId 
where a.Position = 1 order by b.DateCreated desc;

select top 1 a.* from Banner a join BannerPosition b on a.Id = b.BannerId
where b.Position = 1 ORDER BY a.DateCreated DESC;

select COUNT(DISTINCT a.Id) FROM Banner a where 1 = 1 and a.Name like '%web%';

select * from Role WHERE 1 = 1;

select a.*, b.Position from Banner a LEFT JOIN BannerPosition b ON a.Id = b.BannerId;

	
