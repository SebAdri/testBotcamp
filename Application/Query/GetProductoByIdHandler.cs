using BootcampCLT.Api.Response;
using BootcampCLT.Infraestructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetProductoByIdHandler 
    : IRequestHandler<GetProductoByIdQuery, ProductoResponse?>
{
    private readonly PostegresDbContext _postgresDbContex;

    public GetProductoByIdHandler(PostegresDbContext postgresDbContex)
    {
        _postgresDbContex = postgresDbContex;
    }

    public async Task<ProductoResponse?> Handle(
        GetProductoByIdQuery request, 
        CancellationToken cancellationToken)
    {
        var entity = await _postgresDbContex.Productos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (entity is null)
            return null;

        return new ProductoResponse(
            Id: entity.Id,
            Codigo: entity.Codigo,
            Nombre: entity.Nombre,
            Descripcion: entity.Descripcion ?? string.Empty,
            Precio: (double)entity.Precio,
            Activo: entity.Activo,
            CategoriaId: entity.CategoriaId,
            FechaCreacion: entity.FechaCreacion,
            FechaActualizacion: entity.FechaActualizacion,
            CantidadStock: entity.CantidadStock
        );
    }
}
