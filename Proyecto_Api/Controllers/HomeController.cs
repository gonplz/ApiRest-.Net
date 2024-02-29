using Microsoft.AspNetCore.Mvc;
using Proyecto_Api.Models;
using Proyecto_Api.Data;
using AutoMapper;
using Proyecto_Api.Repository.IRepository;
using System.Threading.Tasks;
using System.Collections.Generic;
using Proyecto_Api.Models.Dtos;

namespace Proyecto_Api.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<ProyectController> _logger;
        private readonly IPersonaRepositorie _personaRepositorie;
        private readonly IMapper _mapper;

        public HomeController(ILogger<ProyectController> logger, IPersonaRepositorie personaRepositorie, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _personaRepositorie = personaRepositorie;
        }


        Persona _persona = new Persona();
        private int personaList;

        public IActionResult Listar()
        {
            //Mostrar una lista de contactos
            var olist = _personaRepositorie.findAll();

            return View(olist);
        }

        public IActionResult Guardar() { 
        
            //Devolver solo la vista
         return View();

        }
        [HttpPost]
        public IActionResult Guardar(CreatePersonaDto personaDTO)
        {
            //Recibir un objeto y guradarlo en la BD

            var respt = _personaRepositorie.Guardar(personaDTO);
            return View();

        }

    }
}
