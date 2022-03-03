/**
drop database luigiswonderworld;
create database luigiswonderworld collate utf8mb4_general_ci;

use luigiswonderworld;

create table menu(
	menuid int unsigned not null auto_increment primary key,
	preis decimal(4,2) not null,
	name varchar(150) not null unique,
	zutaten varchar(300) not null unique
	);

insert into menu values(null, 6.90, "Pizza Margherita", "Tomaten, Mozarella, Basilikum");
insert into menu values(null, 8.90, "Pizza Prosciuto", "Tomaten, Mozarella, Schinken");
insert into menu values(null, 9.90, "Pizza Prosciuto E Funghi", "Tomaten, Mozarella, Schinken, Champignons");
insert into menu values(null, 9.90, "Pizza Tonno", "Tomaten, Mozarella, Thunfisch, Zwiebeln");
insert into menu values(null, 10.90, "Pizza Rustica", "Tomaten, Mozarella, Oliven, Zwiebeln");
insert into menu values(null, 10.90, "Pizza Hawaii", "Tomaten, Mozarella, Schinken, Ananas");
insert into menu values(null, 9.90, "Pizza Funghi", "Tomaten, Mozarella, Champignons");
insert into menu values(null, 8.90, "Pizza Rucola", "Tomaten, Mozarella, Rucola, Parmesan, Knoblauch");
insert into menu values(null, 10.90, "Pizza Vegetaria", "Tomaten, Mozarella, gegrilltes Gemüse, Champignons, Rucola, Knoblauch");
insert into menu values(null, 11.90, "Pizza Calzone", "Tomaten, Mozarella, Schinken, Champignons, Salami");
insert into menu values(null, 8.90, "Pizza Salami", "Tomaten, Mozarella, Salami");

create table customer(
	customerID int unsigned not null auto_increment primary key,
	vorname varchar(150) not null, 
	nachname varchar(150) not null, 
	passwort varchar(200) not null, 
	geburtstag date not null,
	email varchar(150) not null
);

insert into customer values(null, "pedda", "pedda", sha2("pedda",512), "pedda@pedda.pedda", "2004-12-23");