using System.ComponentModel.DataAnnotations;

namespace Eventos.Entities.DTO
{
    public class OrganizadorDto
    {
        public int Codigo { get; set; }

        [Required(ErrorMessage = "El nombre del organizador es requerido")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre del organizador debe tener entre 3 y 50 caracteres.")]
        public string NombreOrganizador { get; set; } = string.Empty;

        [Required(ErrorMessage = "El cargo del organizador es requerido")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El cargo del organizador debe tener entre 3 y 50 caracteres.")]
        public string Cargo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El evento asociado es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El evento asociado debe ser un identificador válido")]
        public int EventoId { get; set; }
    }
}