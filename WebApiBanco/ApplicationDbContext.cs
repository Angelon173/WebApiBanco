using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApiBanco.Entidades;


namespace WebApiBanco
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Persona> Personas { get; set; }
        public DbSet<Sala> Salas { get; set; }

        
    }
}