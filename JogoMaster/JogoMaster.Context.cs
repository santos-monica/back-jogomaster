﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace JogoMaster
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class JogoMasterEntities : DbContext
    {
        public JogoMasterEntities()
            : base("name=JogoMasterEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Administrador> Administradores { get; set; }
        public virtual DbSet<Classificacao> Classificacoes { get; set; }
        public virtual DbSet<Nivel> Niveis { get; set; }
        public virtual DbSet<Pergunta> Perguntas { get; set; }
        public virtual DbSet<Resposta> Respostas { get; set; }
        public virtual DbSet<Sala> Salas { get; set; }
        public virtual DbSet<SalaTemas> SalasTemas { get; set; }
        public virtual DbSet<SalaUsuarios> SalasUsuarios { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Tema> Temas { get; set; }
        public virtual DbSet<Usuario> Usuarios { get; set; }
    }
}
