using System.ComponentModel.DataAnnotations;

namespace Eventos.Entities.Models
{
    public class Organizador
    {
        [Key]
        public int Id { get; set; }

        public required string Nombre { get; set; }

        public required string Cargo { get; set; }

        public int EventoId { get; set; }
    }
}
