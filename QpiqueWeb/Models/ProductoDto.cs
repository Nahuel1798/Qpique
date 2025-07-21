public class ProductoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string? Descripcion { get; set; }
    public string ImagenUrl { get; set; } = "/img/sinimagen.jpg";
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public string CategoriaNombre { get; set; } = "";
}
