namespace BootcampCLT.Api.Request
{
    public record PatchProductoRequest
    {
        public string? Codigo { get; init; }
        public string? Nombre { get; init; }
        public string? Descripcion { get; init; }
        public decimal? Precio { get; init; }
        public bool? Activo { get; init; }
        public int? CategoriaId { get; init; }
    }


}
