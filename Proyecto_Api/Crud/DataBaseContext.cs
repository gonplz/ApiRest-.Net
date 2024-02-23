using Microsoft.EntityFrameworkCore;
using Proyecto_Api.Controllers.Models;

namespace Proyecto_Api.Crud
{
    public class DataBaseContext : DbContext
    {

        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { 
        
        }

        public DbSet<Persona> Persona { get; set; }
    }
}
