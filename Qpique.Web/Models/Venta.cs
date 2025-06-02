public class Venta
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
    public string Comprobante { get; set; } // nombre del archivo

    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; }

    public string UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    public ICollection<DetalleVenta> Detalles { get; set; }
}
