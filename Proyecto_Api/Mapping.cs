using AutoMapper;
using Proyecto_Api.Models;
using Proyecto_Api.Models.Dtos;

namespace Proyecto_Api
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            //Forma antigua de mapear
            //CreateMap<Persona, PersonaDto>();
            //CreateMap<PersonaDto, Persona>();

            CreateMap<Persona,PersonaDto>().ReverseMap();

            CreateMap<Persona, CreatePersonaDto>().ReverseMap();

            CreateMap<Persona,UpdatePersonaDto>().ReverseMap();
        }
    }
}
