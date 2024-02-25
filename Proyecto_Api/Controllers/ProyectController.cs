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

        ///////////////////////////////////////////////////////GET///////////////////////////////////////////////////////////////////////////

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
                    _response.isFine = false;
                    return BadRequest(_response);
                }

                var perso = await _personaRepositorie.find(p => p.Id == id);


                if (perso == null)
                {
                    _response.StatusCode=HttpStatusCode.NotFound;
                    _response.isFine = false;
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

        public async Task<ActionResult<Response>> CreatePersona([FromBody] CreatePersonaDto createPersonaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (await _personaRepositorie.find(v => v.name.ToLower() == createPersonaDto.name.ToLower()) != null)
                {
                    ModelState.AddModelError("El nombre ya existe", "El nombre se encuentra en uso");

                    return BadRequest(ModelState);
                }

                if (createPersonaDto == null)
                {
                    return BadRequest(createPersonaDto);
                }

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

                var perso = await _personaRepositorie.find(v => v.Id == id);
                if (perso == null)
                {
                    _response.isFine = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

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
