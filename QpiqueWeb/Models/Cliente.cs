using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QpiqueWeb.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(30)]
        public string Apellido { get; set; }

        [Required]
        [StringLength(15)]
        public string Telefono { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(30)]
        public string Email { get; set; }

        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    }
}

