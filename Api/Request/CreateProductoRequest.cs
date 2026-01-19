namespace BootcampCLT.Api.Request
{
    public record CreateProductoRequest
    {
        public string Codigo { get; init; } = default!;
        public string Nombre { get; init; } = default!;
        public string? Descripcion { get; init; }
        public decimal Precio { get; init; }
        public bool Activo { get; init; } = true;
        public int CategoriaId { get; init; }
    }

}
