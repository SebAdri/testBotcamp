namespace BootcampCLT.Api.Response
{
    public record ProductoResponse
    (
        int Id,
        string Codigo,
        string Nombre,
        string Descripcion,
        double Precio,
        bool Activo,
        int CategoriaId, 
        DateTime FechaCreacion,
        DateTime? FechaActualizacion,
        int CantidadStock
    );
}
