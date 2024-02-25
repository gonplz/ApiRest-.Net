using AutoMapper;
using Proyecto_Api.Models;
using Proyecto_Api.Models.Dtos;

namespace Proyecto_Api
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Persona,PersonaDto>().ReverseMap();
            CreateMap<Persona, CreatePersonaDto>().ReverseMap();
            CreateMap<Persona,UpdatePersonaDto>().ReverseMap();
        }
    }
}
