using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Api.Controllers.Models;
using Proyecto_Api.Controllers.Models.Dtos;
using Proyecto_Api.Data;

namespace Proyecto_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProyectController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PersonaDto>> GetPersonas()
        {
            return Ok(PersonaStore.personaList);
        }

        [HttpGet("id:int", Name = "GetIdPersona")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PersonaDto> GetIdPersona(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var perso = PersonaStore.personaList.FirstOrDefault(v => v.Id == id);

            if (perso == null)
            {
                return NotFound();
            }

            return Ok(perso);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        //El "siempre" se utiliza el [FromBody] para indicar que se va arecibir datos// 
        public ActionResult<PersonaDto> CreatePersona([FromBody] PersonaDto personaDto)
        {
            ////Validacion del Modelo, para verifiar que funcione correctamente el metodo post//
            if(!ModelState.IsValid)
            {
              return BadRequest(ModelState);
                }

                //Validacion del Modelo, Personalizado. En caso de que el nombre ya este utilizado//

                 if (PersonaStore.personaList.FirstOrDefault(v=> v.name.ToLower() == personaDto.name.ToLower()) != null)
                 {
                     ModelState.AddModelError("El nombre ya existe", "El nombre se encuentra en uso");

                   return BadRequest(ModelState);
                 }

                //Haciendo una query para crear un nuevo usuario//
                if (personaDto == null)
                {
                    return BadRequest(personaDto);
                }
                if (personaDto.Id > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                personaDto.Id = PersonaStore.personaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
                PersonaStore.personaList.Add(personaDto);

                return CreatedAtRoute("GetIdPersona", new { id = personaDto.Id }, personaDto);
            }
        }
}
