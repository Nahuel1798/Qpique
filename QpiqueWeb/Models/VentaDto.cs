public class VentaDto {
    public int Id { get; set; }
    public string ClienteNombre { get; set; }
    public decimal Total { get; set; }
    public DateTime Fecha { get; set; }
    public List<DetalleDto> Detalles { get; set; }
}