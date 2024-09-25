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
Rating int,
IsActive bit
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
'4'),
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

insert into Recipe 
(Title, 
Subtitle,
Text,
IsActive,
DateCreated,
DateUpdated,
CategoryId) 
values 
('Cheesy Baked Burritos',
'Quick and delicious',
'Ingredients 
1 Tbsp. extra-virgin olive oil 
1 onion, chopped
2 garlic cloves, minced
2 c. shredded rotisserie chicken
1 c. enchilada sauce
Juice of 1 lime
kosher salt
Freshly ground black pepper
oz. can black beans, drained
2 c. cooked white rice
1 c. cheddar cheese, divided
1 c. Monterey Jack cheese
6 large flour tortillas
Sour cream, for serving (optional)
Hot sauce, for serving (optional)
Chopped cilantro, for serving (opt5ional)
Directions
Step 1
Preheat oven to 350°. In a large skillet over medium heat, heat oil. Add onion and sauté until soft. Stir in garlic and cook until fragrant, about 30 seconds. Add chicken and about 1/2 cup enchilada sauce, or until the chicken is fully coated. Toss until evenly combined. Stir in lime juice, and season with salt and pepper to taste.
Step 2
Working one burrito at a time, lay a tortilla on a cutting board or clean working service. Add a scoopful each of rice and beans to the center. Add the chicken mixture then top with a small handful each of both cheeses. Reserve about ½ cup of cheese total to sprinkle on the burritos before baking. Roll the burrito tightly and place in a large casserole dish. Repeat with remaining tortillas.
Step 3
Pour the remaining enchilada sauce over the burritos then sprinkle the extra cheeses on top. Cover until the cheese is melted, about 15 minutes.
Step 4
Garnish with cilantro and serve with sour cream and hot sauce, if desired.',
1,
'11-07-2024',
'11-07-2024',
1),
('Stuffed Peppers',
'Peppers to lick your fingers',
'Ingredients
1/2 c. uncooked white or brown rice
2 Tbsp. extra-virgin olive oil, plus more for drizzling
1 medium yellow onion, chopped
3 cloves garlic, finely chopped
2 Tbsp. tomato paste
1 lb. ground beef
1 (14.5-oz.) can diced tomatoes
1 1/2 tsp. dried oregano
Kosher salt
Freshly ground black pepper
6 bell peppers, tops and cores removed
1 c. shredded Monterey jack
Chopped fresh parsley, for serving
Directions
Step 1
Preheat oven to 400°. In a small saucepan, prepare rice according to package instructions.
Step 2
Meanwhile, in a large skillet over medium heat, heat oil. Cook onion, stirring occasionally, until softened, about 7 minutes. Stir in garlic and tomato paste and cook, stirring, until fragrant, about 1 minute more. Add ground beef and cook, breaking up meat with a wooden spoon, until no longer pink, about 6 minutes. Drain excess fat.
Step 3
Stir in rice and diced tomatoes; season with oregano, salt, and pepper. Let simmer, stirring occasionally, until liquid has reduced slightly, about 5 minutes.
Step 4
Arrange peppers cut side up in a 13"x9" baking dish and drizzle with oil. Spoon beef mixture into each pepper. Top with cheese, then cover baking dish with foil.
Step 5
Bake peppers until tender, about 35 minutes. Uncover and continue to bake until cheese is bubbly, about 10 minutes more.
Step 6
Top with parsley before serving.
colorful stuffed peppers with ground beef and tomato ricepinterest
PHOTO: ANDREW BUI; FOOD STYLING: BROOKE CAISON
How to Make Stuffed Peppers
Ingredients
Cooked white or brown rice. Either works here, though you’ll definitely want to plan ahead if you’re cooking brown rice the same night as your peppers (it can take up to an hour to cook, whereas white usually takes less than 30 minutes). Not a rice cooking pro yet? Follow food editor Taylor Ann’s guides to cooking both brown and white rice, and you’ll be a master in no time.

Tomato paste. If you’re like me, every time you need tomato paste for a recipe, you open a can, use a few tablespoons, then pop the can back in the fridge to wait to be thrown away next time you do a fridge clean out. Relatable? You’ve got to try my new hack: whenever you open a can, spoon all the tomato paste from it onto a small sheet pan or plate, then freeze it. Once it’s frozen enough to not stick together, store them in an airtight container. Next time you need just one or two tablespoons, you can use your frozen back stock instead of opening a new can—just cook it for a little longer than you normally would!

Ground beef. I usually default to 85/15 (85 percent lean meat and 15 percent fat) or 90/10, but use whatever you like here.

Diced tomatoes. I love the convenience of canned tomatoes, but not all are created equal. My favorite brand is San Marzano because I find it to be the most consistent, but other brands will work. Look for ones that have short ingredient lists (just tomatoes, salt, citric acid, and water or tomato juice). I have on occasion been known to use fire-roasted or tomatoes with green chiles here, but I never want ones that contain any added sugar or additives.

Bell peppers. You can go with any colors here, but keep this in mind: standard red, yellow, and orange are usually a little sweeter than their green or purple counterparts, which lean towards grassier and slightly bitter. Any will work here, but I do recommend trying to buy ones that are similar in size, both height- and width-wise.

Monterey Jack cheese. This is my favorite cheese to use here because it’s a little buttery and nutty, but whatever cheese you like works—try cheddar for a sharper, more robust flavor, pepper Jack for a little kick, or even a Mexican blend for a little variety. Though I’m usually a bit proponent of shredding your own cheese, for this family-friendly meal, I’m okay with saying you can use the pre-shredded kind.

Parsley. This is my preferred garnish, but anything that adds a little pop of green will work here—chives, thyme, even scallions would be great.

Step-By-Step Instructions
Start by preheating your oven to 400°, and making your rice if you need to.

stuffed peppers sbspinterest
PHOTO: CHELSEA LUPKIN
Personally, I like to get my peppers prepped and cut before making the filling so I don’t have to multitask too much. Here’s how I do it: I cut off the top (using a small paring knife to carve a circle around the stem, kind of like when carving a pumpkin), then I pull out the core and the seeds. I suggest turning the peppers upside down over the sink and tapping them to get the excess seeds out. You could even try rinsing the peppers out, if you need some extra help. If a few seeds stay in, don’t panic—it won’t mess up your dish.

stuffed peppers sbspinterest
PHOTO: CHELSEA LUPKIN
Once your peppers are ready, get a large skillet and some oil heating over medium heat. Add your onions, garlic, and tomato paste in stages, letting each one cook a bit before adding the next. You might be tempted to just chuck in your tomato paste with your ground beef, but trust me—letting it cook on its own for a minute or two helps caramelize it and bring out some of its more complex flavors (AND helps get rid of any leftover taste from the can it came in).

stuffed peppers sbspinterest
PHOTO: CHELSEA LUPKIN
Once your aromatics are softened and fragrant, add your ground beef. Cook it, breaking it up with your spoon, until it’s no longer pink (a little under is okay, because it’s going to get baked). Drain the excess oil if needed.

stuffed peppers sbspinterest
PHOTO: CHELSEA LUPKIN

Stir in your cooked rice and diced tomatoes, then add your oregano, salt, and pepper. Taste for seasoning here, then let it cook down for a few minutes until the liquid is reduced a bit.

stuffed peppers sbs
PHOTO: CHELSEA LUPKIN
stuffed peppers sbs
PHOTO: CHELSEA LUPKIN
Then fill your peppers! I like to drizzle my empty peppers with a little oil to keep them nice and supple while baking and then do a little on top to guarantee a flavorful and tasty top, but it’s up to you. Once you’ve got your peppers filled, top them with cheese, then wrap your whole pan in foil and pop it in the oven.

stuffed peppers sbspinterest
PHOTO: CHELSEA LUPKIN
Most of the baking will happen while the peppers are covered, but you do want to give them a little uncovered time in the oven to make sure that cheese gets nice and bubbly. 10 minutes should be enough, but feel free to let it go a little longer (especially if you’re using pre-shredded cheese that can take a bit longer to melt).

Garnish with a little green, then serve!

Full list of ingredients and directions can be found in the recipe above.

Stuffed Pepper Variations
multicolored peppers stuffed with shawarma and drizzled with a cream sauce and mint
Shawarma Stuffed Peppers
PHOTO: ERIK BERNSTEIN; FOOD STYLING: LENA ABRAHAM
baked bell peppers stuffed with chicken, marinara sauce, and cheese
Chicken Parm Stuffed Peppers
PHOTO: JOSEPH DE LEO; FOOD STYLING: MAKINZE GORE
stuffed peppers
Vegetarian Stuffed Peppers With Falafel
Recipe Tips
Do you need to cook peppers before stuffing them? You can, but I prefer not to. The peppers are easier to fill when they’re still raw, and this way they still retain a little bit of texture after they’ve been baked. If you do cook them before stuffing, make sure not to over-bake them—you want them to be tender, but not mushy or soggy.

colorful stuffed peppers with ground beef and tomato ricepinterest
PHOTO: ANDREW BUI; FOOD STYLING: BROOKE CAISON


Make-Ahead & Storage
If you want to prep this a day ahead, cut and core your peppers, then store in an airtight container in the fridge. Prepare your filling as directed, then store it in a separate airtight container in the fridge. When you re ready to serve, simply fill and bake until tender. You can also assemble these through step 4, and then freeze the entire baking tray for up to three months. Hot tip: freeze them in the baking tray, then wrap them up individually—then you can cook just one or two peppers at a time, without having to make the whole tray.

When you’re ready to bake, you can either pull them out to defrost the night before and bake them for the regular time, or bake them from frozen, being mindful that they will take ~30 minutes longer to cook.
',
1,
'11-07-2024',
'11-07-2024',
1);

insert into MemberRecipe(MemberId, RecipeId) values (1,1),(1,2);


select * from Member;

select * from Recipe;

insert into Comment(MemberId, RecipeId, Text, Rating,IsActive) 
Values
(1,1, 'Not bad, not terrible', 3, 1),
(2,2, 'Can pass', 2, 1),
(1,69, 'Childhood memories', 5,1);



