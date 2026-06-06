using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tarea2_clientes.Models
{
    [Table("Reservas")]
    public class ReservaModel
    {
        [Key]
        [Column("reserva_id")]
        public int ReservaId { get; set; }

        [Required]
        [Column("fecha_inicio")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de inicio")]
        public DateTime FechaInicio { get; set; }

        [Required]
        [Column("fecha_fin")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de fin")]
        public DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        [AllowedValues("Pendiente", "Confirmada", "Cancelada", "Completada", ErrorMessage = "Seleccione un estado válido")]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = string.Empty;

        [Required(ErrorMessage = "El total es requerido")]
        [Range(0.01, 999999.99, ErrorMessage = "El total debe ser mayor a 0")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Total")]
        public decimal Total { get; set; }

        [Required]
        [Column("huesped_id")]
        [Display(Name = "Huésped")]
        public int HuespedId { get; set; }

        [Required]
        [Column("habitacion_id")]
        [Display(Name = "Habitación")]
        public int HabitacionId { get; set; }

        [Required]
        [Column("empleado_id")]
        [Display(Name = "Empleado")]
        public int EmpleadoId { get; set; }

        [ForeignKey(nameof(HuespedId))]
        public HuespedModel? Huesped { get; set; }

        [ForeignKey(nameof(HabitacionId))]
        public HabitacionModel? Habitacion { get; set; }

        [ForeignKey(nameof(EmpleadoId))]
        public EmpleadoModel? Empleado { get; set; }

        public ICollection<PagoModel> Pagos { get; set; } = [];
    }
}
