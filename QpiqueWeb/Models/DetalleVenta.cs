using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QpiqueWeb.Models;
namespace QpiqueWeb.Models;
public class DetalleVenta
{
    [Key]
    public int Id { get; set; }
    [Required]
    [Display(Name = "Venta")]
    public int VentaId { get; set; }
    [ForeignKey("VentaId")]
    public Venta? Venta { get; set; }
    [Required]
    [Display(Name = "Producto")]
    public int ProductoId { get; set; }
    [ForeignKey("ProductoId")]
    public Producto? Producto { get; set; }
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero.")]
    public int Cantidad { get; set; }
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor que cero.")]
    public decimal PrecioUnitario { get; set; }
}
