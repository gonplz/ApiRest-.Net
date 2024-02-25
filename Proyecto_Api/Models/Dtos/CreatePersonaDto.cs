using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_Api.Models.Dtos
{
    public class CreatePersonaDto
    {
        [Required]
        [MaxLength(30)]
        public string name { get; set; }

        public int number { get; set; }
    }
}
