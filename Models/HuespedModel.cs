using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tarea2_clientes.Models
{
    [Table("Huespedes")]
    public class HuespedModel
    {
        [Key]
        [Column("huesped_id")]
        public int HuespedId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es requerido")]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es requerido")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Ingrese un teléfono de 10 dígitos")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } = string.Empty;

        public ICollection<ReservaModel> Reservas { get; set; } = [];
    }
}
