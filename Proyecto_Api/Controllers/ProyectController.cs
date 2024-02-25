using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Proyecto_Api.Crud;
using Proyecto_Api.Data;
using Proyecto_Api.Models;
using Proyecto_Api.Models.Dtos;
using Proyecto_Api.Repository.IRepository;
using System.Net;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Proyecto_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProyectController : ControllerBase
    {
        private readonly ILogger<ProyectController> _logger;
        private readonly IPersonaRepositorie _personaRepositorie;
        private readonly IMapper _mapper;
        protected Response _response;

        public ProyectController(ILogger<ProyectController> logger, IPersonaRepositorie personaRepositorie, IMapper mapper)
        {
            _logger = logger;
            _personaRepositorie = personaRepositorie;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet]
        public async Task<ActionResult<Response>> GetPersonas()
        {
            try
            {
                _logger.LogInformation("Obtener Personas");

                IEnumerable<Persona> personaList = await _personaRepositorie.findAll();

                _response.result = _mapper.Map<IEnumerable<Persona>>(personaList);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception x)
            {
                _response.isFine = false;
                _response.errorMessage = new List<string> { x.ToString()};
            }
            return _response;
        }

        ///////////////////////////////////////////////////////GET///////////////////////////////////////////////////////////////////////////
        

        [HttpGet("id:int", Name = "GetIdPersona")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Response>> GetIdPersona(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogInformation("Error al traer el Id " + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                //Metodo antiguo para pedir por Id
                //var perso = PersonaStore.personaList.FirstOrDefault(v => v.Id == id);

                var perso = await _personaRepositorie.find(p => p.Id == id);


                if (perso == null)
                {
                    _response.StatusCode=HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.result = _mapper.Map<PersonaDto>(perso);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);

            }
            catch (Exception x)
            {
                _response.isFine = false;
                _response.errorMessage = new List<string> { x.ToString() };
            }
            return _response;
        }

        ///////////////////////////////////////////////////////POST///////////////////////////////////////////////////////////////////////////

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        //El [FromBody] se utiliza para deserializar y transformar los bytes en Objetos (Funciona con el Post y Put, por el momento) // 
        public async Task<ActionResult<Response>> CreatePersona([FromBody] CreatePersonaDto createPersonaDto)
        {
            try
            {
                ////Validacion del Modelo, para verifiar que funcione correctamente el metodo post//
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                //Validacion del Modelo, Personalizado. En caso de que el nombre ya este utilizado//
                //Reemplazando el PersonaStore.personasList por el _database.Persona
                if (await _personaRepositorie.find(v => v.name.ToLower() == createPersonaDto.name.ToLower()) != null)
                {
                    ModelState.AddModelError("El nombre ya existe", "El nombre se encuentra en uso");

                    return BadRequest(ModelState);
                }

                //Haciendo una query para crear un nuevo usuario//
                if (createPersonaDto == null)
                {
                    return BadRequest(createPersonaDto);
                }

                //Forma antigua, antes del mapper y antes de usar BD
                //personaDto.Id = PersonaStore.personaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
                //PersonaStore.personaList.Add(personaDto);

                Persona modelo = _mapper.Map<Persona>(createPersonaDto);

                await _personaRepositorie.create(modelo);
                _response.result = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetIdPersona", new { id = modelo.Id }, _response);
            }
            catch (Exception x)
            {
                _response.isFine = false;
                _response.errorMessage = new List<string> { x.ToString() };
            }

            return _response;
        }

        ///////////////////////////////////////////////////////DELETE///////////////////////////////////////////////////////////////////////////

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        //El Delete no necesita mapeo//

        //El metodo borra permanentemente el id, pero no lo vuelve a uitilar a ese id(Buscar un metodo para volver a buscar el id)//
        public async Task<IActionResult> DeletePersona(int id)
        {
            try
            {

                if (id == 0)
                {
                    _response.isFine = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                //Reemplazando el PersonaStore.personasList por el _database.Persona
                //AssNoTracking... asincrona?(No es Async, es otra cosa)
                var perso = await _personaRepositorie.find(v => v.Id == id);
                if (perso == null)
                {
                    _response.isFine = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                //PersonaStore.personaList.Remove(perso);
                //return NoContent();

               await _personaRepositorie.delete(perso);

                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);

            }
            catch (Exception x)
            {
                _response.isFine = false;
                _response.errorMessage = new List<string> { x.ToString() };
            }

            return BadRequest(_response);
        }


        ///////////////////////////////////////////////////////PUT///////////////////////////////////////////////////////////////////////////
        
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        //En Swagger tienen que coincidir los id al momento de la solicitud, porque sino tira un BadRequest.
        public async Task<IActionResult> UpdatePersona(int id, [FromBody] UpdatePersonaDto updatePersonaDto )
        {
            if(updatePersonaDto == null || id != updatePersonaDto.Id)
            {
                _response.isFine = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            Persona modelo = _mapper.Map<Persona>(updatePersonaDto);

           var updatePersona = await _personaRepositorie.UpdatePersona(modelo);
            _response.StatusCode=HttpStatusCode.NoContent;

            if (updatePersona == null)
            {
                _response.isFine = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
          
            return Ok(_response);

        }
        ///////////////////////////////////////////////////////PATCH///////////////////////////////////////////////////////////////////////////
        
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdateOnePersona(int id, JsonPatchDocument<UpdatePersonaDto> pachtDto)
        {
            if (pachtDto == null || id == 0)
            {
                return BadRequest();
            }

            //var perso = PersonaStore.personaList.FirstOrDefault(v => v.Id == id);

            var perso = await _personaRepositorie.find(v => v.Id == id, Tracked:false);

            UpdatePersonaDto personaDto = _mapper.Map<UpdatePersonaDto>(perso);

            if (perso == null) return BadRequest();

            pachtDto.ApplyTo(personaDto,ModelState);
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Persona modelo = _mapper.Map<Persona>(personaDto);


            var updatePersona = await _personaRepositorie.UpdatePersona(modelo);
            _response.StatusCode = HttpStatusCode.NoContent;

            if (updatePersona == null)
            {
                _response.isFine = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            return Ok(_response);

        }
    }
}
