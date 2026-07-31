using BackAlmancen.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BackAlmancen.Persistence.Context
{
    public partial class ContextDB : DbContext
    {

        public ContextDB() { }

        public ContextDB(DbContextOptions<ContextDB> options) : base(options) { }

        //INICIA LA DECLARACION DE TABLAS
        public DbSet<Producto> Producto { get; set; } = null!;

        //FINALIZA LA DECLARACION DE TABLAS

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ModelConfig(modelBuilder);
        }

        private void ModelConfig(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producto>().HasData(
                new Producto
                {
                    Id = 1,
                    Name = "Barbie Developer",
                    Description = "Edición especial de colección",
                    AgeRestriction = 12,
                    Company = "Mattel",
                    Price = 25.99m,
                    ImageUrl = null
                },
                new Producto
                {
                    Id = 2,
                    Name = "Spider-man",
                    Description = "Figura articulada de acción",
                    AgeRestriction = 4,
                    Company = "Marvel",
                    Price = 75.50m,
                    ImageUrl = null
                },
                new Producto
                {
                    Id = 3,
                    Name = "Xbox Series S",
                    Description = "Consola portátil retro",
                    AgeRestriction = 18,
                    Company = "Microsoft",
                    Price = 99.99m,
                    ImageUrl = null
                }
            );

        }
    }
}
