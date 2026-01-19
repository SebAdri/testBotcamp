using MediatR;
using BootcampCLT.Api.Response;

public record GetProductoByIdQuery(int Id) : IRequest<ProductoResponse?>;
