using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Proyecto_Api.Crud;
using Proyecto_Api.Data;
using Proyecto_Api.Models;
using Proyecto_Api.Models.Dtos;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Proyecto_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProyectController : ControllerBase
    {
        private readonly ILogger<ProyectController> _logger;
        private readonly DataBaseContext _database;

        public ProyectController(ILogger<ProyectController> logger, DataBaseContext dataBase)
        {
            _logger = logger;
            _database = dataBase;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PersonaDto>> GetPersonas()
        {
            _logger.LogInformation("Obtener Personas");
            return Ok(_database.Persona.ToList());
        }

        ///////////////////////////////////////////////////////GET///////////////////////////////////////////////////////////////////////////
        

        [HttpGet("id:int", Name = "GetIdPersona")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PersonaDto> GetIdPersona(int id)
        {
            if (id == 0)
            {
                _logger.LogInformation("Error al traer el Id " + id);
                return BadRequest();
            }
            //Metodo antiguo para pedir por Id
            //var perso = PersonaStore.personaList.FirstOrDefault(v => v.Id == id);

            var perso = _database.Persona.FirstOrDefault(p => p.Id == id);

            if (perso == null)
            {
                return NotFound();
            }

            return Ok(perso);
        }

        ///////////////////////////////////////////////////////POST///////////////////////////////////////////////////////////////////////////

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        //El [FromBody] se utiliza para deserializar y transformar los bytes en Objetos (Funciona con el Post y Put, por el momento) // 
        public ActionResult<PersonaDto> CreatePersona([FromBody] CreatePersonaDto personaDto)
        {
            ////Validacion del Modelo, para verifiar que funcione correctamente el metodo post//
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Validacion del Modelo, Personalizado. En caso de que el nombre ya este utilizado//
            //Reemplazando el PersonaStore.personasList por el _database.Persona
            if (_database.Persona.FirstOrDefault(v => v.name.ToLower() == personaDto.name.ToLower()) != null)
            {
                ModelState.AddModelError("El nombre ya existe", "El nombre se encuentra en uso");

                return BadRequest(ModelState);
            }

            //Haciendo una query para crear un nuevo usuario//
            if (personaDto == null)
            {
                return BadRequest(personaDto);
            }

            //Forma antigua
            //personaDto.Id = PersonaStore.personaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
            //PersonaStore.personaList.Add(personaDto);

            Persona modelo = new()
            {
                name = personaDto.name,
                number = personaDto.number,
            };

            _database.Persona.Add(modelo);
            _database.SaveChanges();

            return CreatedAtRoute("GetIdPersona", new { id = modelo.Id }, modelo);
        }

        ///////////////////////////////////////////////////////DELETE///////////////////////////////////////////////////////////////////////////

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        //El metodo borra permanentemente el id, pero no lo vuelve a uitilar a ese id(Buscar un metodo para volver a buscar el id)//
        public IActionResult DeletePersona(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            //Reemplazando el PersonaStore.personasList por el _database.Persona
            //AssNoTracking... asincrona?(No es Async, es otra cosa)
            var perso = _database.Persona.AsNoTracking().FirstOrDefault(v => v.Id == id);
            if (perso == null)
            {
                return NotFound();
            }

            //PersonaStore.personaList.Remove(perso);
            //return NoContent();
            _database.Persona.Remove(perso);
            _database.SaveChanges();
            return NoContent();
            
        }
        ///////////////////////////////////////////////////////PUT///////////////////////////////////////////////////////////////////////////
        
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        //El Put me modifica todo, pero tienen que coincidir los id, porque sino me tira en Error status 400.
        public IActionResult UpdatePersona(int id, [FromBody] UpdatePersonaDto personaDto )
        {
            if(personaDto == null || id != personaDto.Id)
            {
                return BadRequest();
            }

            //var perso = PersonaStore.personaList.FirstOrDefault(v => v.Id == id);
            //perso.name = personaDto.name;
            //perso.number = personaDto.number;

            Persona modelo = new()
            {
                Id = personaDto.Id,
                name = personaDto.name,
                number = personaDto.number,
            };

            _database.Update(modelo);
            _database.SaveChanges();
            return NoContent();

        }
        ///////////////////////////////////////////////////////PATCH///////////////////////////////////////////////////////////////////////////
        
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult UpdateOnePersona(int id, JsonPatchDocument<UpdatePersonaDto> pachtDto)
        {
            if (pachtDto == null || id == 0)
            {
                return BadRequest();
            }

            //var perso = PersonaStore.personaList.FirstOrDefault(v => v.Id == id);

            var perso = _database.Persona.AsNoTracking().FirstOrDefault(v => v.Id == id);

            UpdatePersonaDto persona = new()
            {
                Id = perso.Id,
                name = perso.name,
                number = perso.number,

            };

            if (perso == null) return BadRequest();

            pachtDto.ApplyTo(persona,ModelState);
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Persona modelo = new()
            {
                Id = persona.Id,
                name = persona.name,
                number = persona.number,
            };

            _database.Update(modelo);
            _database.SaveChanges();

            return NoContent();

        }
    }
}
