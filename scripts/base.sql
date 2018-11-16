use master
go
create database JogoMaster
go
use JogoMaster
go
create table Nivel (
	Id int not null primary key identity(1, 1),
	Nivel varchar(10) not null
)

create table Tema (
	Id int not null primary key identity(1, 1),
	Tema varchar(20) not null,
	Icone varchar(20) not null,
	Cor varchar(20) not null
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
	Username varchar(200) not null,
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

create table Sala (
	Id int primary key identity(1,1),
	Nivel int foreign key references Nivel(id),
	Criador int foreign key references Usuario(Id)
)

create table SalaTemas (
	SalaId int foreign key references Sala(Id),
	TemaId int foreign key references Tema(Id),
)

create table SalaUsuarios (
	SalaId int foreign key references Sala(Id),
	UsuarioId int foreign key references Usuario(Id),
)
