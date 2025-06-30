using QpiqueWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace QpiqueWeb.Models;
public class Venta
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime Fecha { get; set; }

    [Required]
    public decimal Total { get; set; }
    [Required(ErrorMessage = "Debe seleccionar un cliente.")]
    public int ClienteId { get; set; }

    [ForeignKey("ClienteId")]
    public Cliente? Cliente { get; set; }

    
    [Display(Name = "Usuario")]
    [BindNever] // <- evita que Razor lo valide desde el form
    public string? UsuarioId { get; set; }
    [ForeignKey("UsuarioId")]
    public Usuario? Usuario { get; set; }

    public List<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();

}