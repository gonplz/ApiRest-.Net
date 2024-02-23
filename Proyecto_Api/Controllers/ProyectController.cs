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
            return Ok(PersonaStore.personaListDtos);
        }

        [HttpGet("id")]
        public ActionResult<PersonaDto> GetIdPersona (int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var perso = PersonaStore.personaListDtos.FirstOrDefault(v=> v.Id == id);

            if (perso == null)
            {
                return NotFound();
            }

            return Ok(perso);
        }
    }
}
