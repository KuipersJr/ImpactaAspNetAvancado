﻿using Empresa.Dominio;
using Microsoft.EntityFrameworkCore;

namespace Empresa.Repositorios.SqlServer
{
    public class EmpresaDbContext : DbContext
    {
        public EmpresaDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Contato> Contatos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Contato>(entity =>
            //{
            //    entity.Property(e => e.Nome).IsRequired();
            //});
        }
    }
}