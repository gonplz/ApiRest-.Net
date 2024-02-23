using Proyecto_Api.Controllers.Models.Dtos;

namespace Proyecto_Api.Data
{
    public static class PersonaStore
    {
        public static List<PersonaDto> personaListDtos = new List<PersonaDto>
        {
              new PersonaDto {Id=1, name="Diego",},
              new PersonaDto {Id=2, name="Fran"},
              new PersonaDto {Id=3, name="Gonza"}

        };
    }
}
