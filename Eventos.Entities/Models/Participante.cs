using System.ComponentModel.DataAnnotations;

namespace Eventos.Entities.Models
{
    public class Participante
    {
        [Key]
        public int Id { get; set; }

        public required string Nombre { get; set; }

        public required string Email { get; set; }

        public int EventoId { get; set; }
    }
}
