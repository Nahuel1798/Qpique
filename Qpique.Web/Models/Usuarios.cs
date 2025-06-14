using System.ComponentModel.DataAnnotations;

public class Usuario
{
    [Key]
    public int Id { get; set; }

    [Required, EmailAddress]
    [Display(Name = "Correo electrónico")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener al menos 2 caracteres.")]
    [Display(Name = "Nombre")]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
    [DataType(DataType.Password)]
    public string Clave { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Rol")]
    [RegularExpression("^(Administrador|Empleado)$", ErrorMessage = "El rol debe ser Administrador o Empleado.")]
    public string Rol { get; set; } = "Empleado";

    [StringLength(255)]
    public string? Avatar { get; set; }
}
