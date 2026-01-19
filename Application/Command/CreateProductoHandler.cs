using BootcampCLT.Api.Response;
using BootcampCLT.Domain.Entity;
using BootcampCLT.Infraestructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BootcampCLT.Application.Command
{
    public class CreateProductoHandler : IRequestHandler<CreateProductoCommand, ProductoResponse>
    {
        private readonly PostegresDbContext _postgresDbContex;

        public CreateProductoHandler(PostegresDbContext postgresDbContex)
        {
            _postgresDbContex = postgresDbContex;
        }

        public async Task<ProductoResponse> Handle(CreateProductoCommand request, CancellationToken cancellationToken)
        {
            var entity = new Producto
            {
                Codigo = request.Codigo,
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                Precio = request.Precio,
                Activo = request.Activo,
                CategoriaId = request.CategoriaId,
                FechaCreacion = DateTime.UtcNow,
                FechaActualizacion = null,
                CantidadStock = 0 // o lo que definas como default
            };

            _postgresDbContex.Productos.Add(entity);
            await _postgresDbContex.SaveChangesAsync(cancellationToken);

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
}
