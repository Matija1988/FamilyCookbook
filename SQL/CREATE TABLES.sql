--use master;
--go
--drop database if exists "FamilyCookbook";
--go
--create database "FamilyCookbook";
--go
--alter database "FamilyCookbook" collate CROATIAN_CI_AS;
--go
--use "FamilyCookbook";


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
DestinationName varchar(30),
DateCreated datetime,
DateUpdated datetime,
IsActive bit, 
BannerType varchar(20)
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

alter table RecipeTags add foreign key (TagId) references Tags(Id);
alter table RecipeTags add foreign key (RecipeId) references Recipe(Id);

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
--('d7b19efe-39aa-4d25-b802-21ac8fa0b0f4',
--'Izmisljeni',
--'Korisnik',
--'10-05-1999',
--'Temp bio',
--'1',
--'08-07-2024',
--'08-07-2024',
--'samsung',
--'$2a$12$LfIczJbu.Akfa0/Pkt1ANe42gSguBgvcoqVIFJ0UUJ5P6WphzQv1a',
--'4'),
('a86823ad-f120-4a8c-94ff-d3d37330653a',
'Test',
'Tester',
'10-07-2001',
'Temp bio',
'1',
'12-07-2024',
'12-07-2024',
'apple',
'iPhone14',
'1');


insert into Picture(Name, Location,IsActive) VALUES 
('Test', 'Test', 1);


insert into MemberRecipe(MemberId, RecipeId) values (1,1),(1,2);

insert into Comment(MemberId, RecipeId, Text, DateCreated, DateUpdated, Rating,IsActive) 
Values
(1,1, 'Not bad, not terrible', '11-09-2023', '11-09-2023', 3, 1),
(2,1, 'Can pass', '11-10-2023','11-10-2023', 2, 1),
(1,2, 'Childhood memories', '11-11-2023','11-11-2023', 5,1),
(2,2, 'Zer gutt', '11-11-2023','11-11-2023', 4,1);

insert into Tag(Text) VALUES 
('riba'), 
('janjetina'), 
('tikvice'), 
('bundeva'), 
('domaće'), 
('fish'),
('šaran'); 

insert into RecipeTags(TagId, RecipeId) VALUES (1,8), (6,8), (4, 7);

select a.*, c.Id, c.Text from Recipe a 
join RecipeTags b on b.RecipeId = a.Id
join Tag c on c.Id = b.TagId
where c.Text = 'riba';

select * from Tag;
select * from RecipeTags;

sp_rename 'Tags', 'Tag'; 

ALTER TABLE Tags RENAME Tag;

select * from Comment;