namespace Proyecto_Api.Controllers.Models
{
    public class Persona
    {
        public int Id { get; set; }
        public string name { get; set; }

        public int number { get; set; }
    }
}

//Para utilizar el metodo Delete logico, hay que actualizar la clase de la siguiente forma

//public class Persona
//{ public int Id { get; set; }
 //   public string Nombre { get; set; }
  //  public string Apellido { get; set; }
   // public bool IsActive { get; set; }}
