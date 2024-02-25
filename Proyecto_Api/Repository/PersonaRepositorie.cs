using Proyecto_Api.Crud;
using Proyecto_Api.Models;
using Proyecto_Api.Repository.IRepository;
using System.Linq.Expressions;

namespace Proyecto_Api.Repository
{
    public class PersonaRepositorie : Repositorie<Persona>, IPersonaRepositorie
    {

        private readonly DataBaseContext _context;

        public PersonaRepositorie(DataBaseContext baseContext) : base(baseContext) 
        {
              _context = baseContext;
        }

        public async Task<Persona> UpdatePersona(Persona entity)
        {
           _context.Persona.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
