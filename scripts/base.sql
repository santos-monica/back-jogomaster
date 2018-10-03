use master
go
create database JogoMaster
go
use JogoMaster
go
create table Nivel (
	Id int not null primary key identity(1, 1),
	Nivel varchar(10) not null,
	Pontos tinyint not null
)

create table Tema (
	Id int not null primary key identity(1, 1),
	Tema varchar(20) not null
)

create table Pergunta (
	Id int not null primary key identity(1, 1),
	Pergunta varchar(500) not null,
	Patrocinada bit not null,
	IdTema int not null foreign key references Tema(Id),
	IdNivel int not null foreign key references Nivel(Id)
)

create table Resposta (
	Id int not null primary key identity(1, 1),
	Correta bit not null,
	Resposta varchar(200) not null,
	IdPergunta int not null foreign key references Pergunta(Id)
)

create table Classificacao (
	Id int not null primary key identity(1, 1),
	Classificacao varchar(100) not null,
	PontuacaoMinima int not null,
	PontuacaoMaxima int not null
)

create table Usuario (
	Id int not null primary key identity(1, 1),
	Nome varchar(200) not null,
	Email varchar(200) not null,
	Senha varchar(200) not null,
	Pontos int not null default(0),
	IdClassificacao int not null foreign key references Classificacao(Id)
)

create table Administrador (
	Id int not null primary key identity(1, 1),
	Nome varchar(200) not null,
	Email varchar(200) not null,
	Senha varchar(200) not null,
)

insert into Administrador values ('Admin', 'adm@adm.com', 'admin')
insert into Classificacao values
('Iniciante', 0, 1000),
('Intermediário', 1001, 2000),
('Avançado', 2001, 5000),
('Expert', 5001, 100000)
insert into Usuario values
('Jogador1', 'jogador1@gmail.com', 'facil123', 0, 1)
insert into Tema values
('Tecnologia'), ('Ciências'), ('Filmes e Séries'), ('Músicas'),('Curiosidades')
insert into Nivel values
('Fácil', 5), ('Médio', 15), ('Difícil', 30)
insert into Pergunta values
('Qual destes sabores não são da Coca-Cola?', 1, 5, 2)
insert into Resposta values 
(1, 'Grape', 1),(0, 'Vanilla', 1),(0, 'Cherry', 1),(0, 'Lemon', 1)
