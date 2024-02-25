using Microsoft.EntityFrameworkCore;
using Proyecto_Api.Models;

namespace Proyecto_Api.Crud
{
    public class DataBaseContext : DbContext
    {

        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { 
        
        }

        public DbSet<Persona> Persona { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            {
                //Agregando HasData despues del parentesis, se pueden agregar datos de forma analoga a la BD. Pero como yo quiero agregar los datos directaente desde el Swagger, no voy a rellenar este metodo.
                modelBuilder.Entity<Persona>();
            }
        }
    }
}
