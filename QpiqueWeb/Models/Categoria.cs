using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QpiqueWeb.Models;
public class Categoria
{
    [Key]
    public int Id { get; set; }
    [Required]
    [StringLength(30)]
    public string Nombre { get; set; }

    public ICollection<Producto> Productos { get; set; }= new List<Producto>();
}
