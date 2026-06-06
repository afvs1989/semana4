using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tarea2_clientes.Models
{
    [Table("Habitaciones")]
    public class HabitacionModel
    {
        [Key]
        [Column("habitacion_id")]
        public int HabitacionId { get; set; }

        [Required(ErrorMessage = "El número es requerido")]
        [StringLength(10, MinimumLength = 1)]
        [Display(Name = "Número")]
        public string Numero { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo es requerido")]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Tipo")]
        public string Tipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio por noche es requerido")]
        [Range(0.01, 999999.99, ErrorMessage = "El precio debe ser mayor a 0")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Precio por noche")]
        public decimal PrecioNoche { get; set; }

        [Required(ErrorMessage = "La capacidad es requerida")]
        [Range(1, 20, ErrorMessage = "La capacidad debe estar entre 1 y 20")]
        [Display(Name = "Capacidad")]
        public int Capacidad { get; set; }

        public ICollection<ReservaModel> Reservas { get; set; } = [];
    }
}
