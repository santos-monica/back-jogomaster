use master
go
create database JogoMaster
go
use JogoMaster
go
create table Nivel (
	Id int not null primary key identity(1, 1),
	Nivel varchar(50) not null
)
go
create table Tema (
	Id int not null primary key,
	Tema varchar(50) not null,
	Icone varchar(50) not null,
	Cor varchar(50) not null
)
go
create table Pergunta (
	Id int not null primary key identity(1, 1),
	Pergunta varchar(500) not null,
	Patrocinada bit not null,
	IdTema int not null foreign key references Tema(Id),
	IdNivel int not null foreign key references Nivel(Id)
)
go
create table Resposta (
	Id int not null primary key identity(1, 1),
	reta bit not null,
	Resposta varchar(200) not null,
	IdPergunta int not null foreign key references Pergunta(Id)
)
go
create table Classificacao (
	Id int not null primary key identity(1, 1),
	Classificacao varchar(100) not null,
	PontuacaoMinima int not null,
	PontuacaoMaxima int not null
)
go
create table Usuario (
	Id int not null primary key identity(1, 1),
	Nome varchar(200) not null,
	Username varchar(200) not null,
	Email varchar(200) not null,
	Senha varchar(200) not null,
	Skin varchar(100) not null default('default.png'),
	Pontos int not null default(0),
	Cadastrado bit not null default(0),
	IdClassificacao int not null foreign key references Classificacao(Id)
)
go
create table Sala (
	Id int primary key identity(1,1),
	Nivel int not null foreign key references Nivel(id),
	Criador int not null foreign key references Usuario(Id),
	Jogadores int not null,
	Ativa bit not null
)
go
create table SalaTemas (
	Id int primary key identity(1,1),
	SalaId int not null foreign key references Sala(Id),
	TemaId int not null foreign key references Tema(Id),
)
go
create table SalaUsuarios (
	Id int primary key identity(1,1),
	SalaId int not null foreign key references Sala(Id),
	UsuarioId int not null foreign key references Usuario(Id),
)
go

insert into Nivel values 
('Fácil'),
('Intermediário'),
('Difícil'),
('Master')
go

insert into Tema values 
(301,'Culinária','icon', '#000000'),
(401,'Biologia','dna.png', '#00FF00'),
(501,'Harry Potter','harry-potter.png', '#FF7F00'),
(601,'Tecnologia','innovation.png', '#FF0000'),
(701,'Games','game-controller.png', '#FF6EC7'),
(801,'História do Brasil','cristo-redentor.png', '#0000FF'),
(901,'Lógica','icon', '#000000')
go


insert into Classificacao values
('Noob', 0, 999),
('Principiante', 1000, 4999),
('Pequeno Gafanhoto', 5000, 19999),
('Gafanhoto', 20000, 49999),
('Manjador', 50000, 249999),
('Sabixão', 250000, 799999),
('Especialista', 800000, 1499999),
('Mestrão', 1500000, 1000000000)
