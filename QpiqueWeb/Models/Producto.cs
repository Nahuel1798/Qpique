using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QpiqueWeb.Models;
public class Producto
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string Nombre { get; set; }

    [StringLength(500)]
    public string Descripcion { get; set; }

    public string? ImagenUrl { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero.")]
    public decimal Precio { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser un número positivo.")]
    public int Stock { get; set; }
    public bool Estado { get; set; } = false;
    [Required]
    [Display(Name = "Categoría")]
    public int CategoriaId { get; set; }
    [ForeignKey("CategoriaId")]
    public Categoria? Categoria { get; set; }

    public ICollection<DetalleVenta>? DetallesVenta { get; set; }
}
