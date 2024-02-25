using Proyecto_Api.Models;

namespace Proyecto_Api.Repository.IRepository
{
    public interface IPersonaRepositorie : IRepositorie<Persona>
    {

        Task<Persona> UpdatePersona(Persona persona);

    }
}
