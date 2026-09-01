using System.ComponentModel.DataAnnotations;

namespace Eventos.Entities.DTO
{
    public class EventoDto
    {
        public int Codigo { get; set; }

        [Required(ErrorMessage = "El nombre del evento es requerido")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "El nombre del evento debe tener entre 5 y 100 caracteres.")]
        public string NombreEvento { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha del evento es requerida")]
        public DateTime FechaEvento { get; set; }

        [Required(ErrorMessage = "El lugar del evento es requerido")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "El lugar del evento debe tener entre 5 y 100 caracteres.")]
        public string LugarEvento { get; set; } = string.Empty;
    }
}
