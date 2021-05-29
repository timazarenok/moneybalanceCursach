create database [Money];

use [Money];

create table Users(
id int Identity(1,1) primary key,
[email] varchar(30)
)

create table Wallets (
id int Identity(1,1) primary key,
[name] varchar(30)
)

create table Settings (
id int Identity(1,1) primary key,
[user_id] int references Users(id) on delete cascade,
[wallet_id] int references Wallets(id) on delete cascade
)

create table Operations (
id int Identity(1,1) primary key,
[name] varchar(30)
)

insert into Operations values('Доход'),('Расход')

create table Categories (
id int Identity(1,1) primary key,
operation int references Operations(id) on delete cascade,
[name] varchar(30)
)

create table Categories_Money(
id int Identity(1,1) primary key,
[user_id] int references Users(id) on delete cascade,
category_id int references Categories(id) on delete cascade,
[value] decimal(5,2)
)

select * from Settings
insert into Wallets values ('BY'), ('EU'), ('US')
select Categories.name as category, Categories_Money.value as value, Operations.name as operation from Categories_Money 
join Categories on Categories_Money.category_id = Categories.id
join Operations on Categories.operation = Operations.id