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
[value] decimal(5,2),
[date] date
)

Update Categories_Money set [value] = 20 * 0.32 where id = 1
select * from Categories_Money

create table Converter(
id int Identity(1,1) primary key,
[first] int,
[second] int,
[value] decimal(5,2),
foreign key ([first]) references Wallets(id),
foreign key ([second]) references Wallets(id)
)

GO
CREATE TRIGGER Users_insert
ON Users
AFTER INSERT
AS
INSERT INTO Settings values((SELECT id FROM INSERTED), (select id from Wallets where [name] = 'BY'))

select * from Settings
insert into Wallets values ('BY'), ('EU'), ('US')
insert into Converter values(1,2,0.32), (1,3,0.40), (2,1,3.09), (2,3,1.22), (3,1,2.52), (3,2,0.82)