using System.ComponentModel.DataAnnotations;

namespace Eventos.Entities.DTO
{
    public class ParticipanteDto
    {
        public int Codigo { get; set; }

        [Required(ErrorMessage = "El nombre del participante es requerido")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre del participante debe tener entre 3 y 50 caracteres.")]
        public string NombreParticipante { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El evento asociado es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El evento asociado debe ser un identificador válido")]
        public int EventoId { get; set; }
    }
}