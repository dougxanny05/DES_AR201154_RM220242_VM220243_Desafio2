using System.ComponentModel.DataAnnotations;

namespace Eventos.Entities.Models
{
    public class Evento
    {
        [Key]
        public int Id { get; set; }

        public required string Nombre { get; set; }

        public DateTime Fecha { get; set; }

        public required string Lugar { get; set; }
    }
}
