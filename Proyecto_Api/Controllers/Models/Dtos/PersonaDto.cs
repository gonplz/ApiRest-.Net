using System.ComponentModel.DataAnnotations;

namespace Proyecto_Api.Controllers.Models.Dtos
{
    public class PersonaDto
    {
        [Key]
        public int Id { get; set; }

       [Required]
       [MaxLength(30)]
        public string name { get; set; }

        public int number { get; set; }
    }
}
