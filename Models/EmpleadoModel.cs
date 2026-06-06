using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tarea2_clientes.Models
{
    [Table("Empleados")]
    public class EmpleadoModel
    {
        [Key]
        [Column("empleado_id")]
        public int EmpleadoId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El cargo es requerido")]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Cargo")]
        public string Cargo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El turno es requerido")]
        [StringLength(20, MinimumLength = 3)]
        [Display(Name = "Turno")]
        public string Turno { get; set; } = string.Empty;

        public ICollection<ReservaModel> Reservas { get; set; } = [];
    }
}
