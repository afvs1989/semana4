using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tarea2_clientes.Models
{
    [Table("Pagos")]
    public class PagoModel
    {
        [Key]
        [Column("pago_id")]
        public int PagoId { get; set; }

        [Required(ErrorMessage = "El monto es requerido")]
        [Range(0.01, 999999.99, ErrorMessage = "El monto debe ser mayor a 0")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Monto")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El método es requerido")]
        [AllowedValues("Efectivo", "Tarjeta de crédito", "Tarjeta de débito", "Transferencia", ErrorMessage = "Seleccione un método válido")]
        [Display(Name = "Método")]
        public string Metodo { get; set; } = string.Empty;

        [Required]
        [Column("fecha_pago")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de pago")]
        public DateTime FechaPago { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        [AllowedValues("Completado", "Pendiente", "Reembolsado", ErrorMessage = "Seleccione un estado válido")]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = string.Empty;

        [Required]
        [Column("reserva_id")]
        [Display(Name = "Reserva")]
        public int ReservaId { get; set; }

        [ForeignKey(nameof(ReservaId))]
        public ReservaModel? Reserva { get; set; }
    }
}
