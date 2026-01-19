using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Primitives;

namespace BootcampCLT.Infraestructure.Logger
{
    public static class HeaderPropagationExtensions
    {
        public static IServiceCollection AddHeaderPropagation(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.TryAddEnumerable(ServiceDescriptor
                .Singleton<IHttpMessageHandlerBuilderFilter, HeaderPropagationMessageHandlerBuilderFilter>());
            return services;
        }

        internal class HeaderPropagationMessageHandlerBuilderFilter(IHttpContextAccessor contextAccessor) : IHttpMessageHandlerBuilderFilter
        {
            public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
            {
                return builder =>
                {
                    builder.AdditionalHandlers.Add(new HeaderPropagationMessageHandler(contextAccessor));
                    next(builder);
                };
            }
        }

        public class HeaderPropagationMessageHandler(IHttpContextAccessor contextAccessor) : DelegatingHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                if (contextAccessor.HttpContext != null)
                {
                    var headerValue = contextAccessor.HttpContext.Response.Headers["x-correlation-id"];
                    if (!StringValues.IsNullOrEmpty(headerValue))
                    {
                        request.Headers.TryAddWithoutValidation("x-correlation-id", (string[])headerValue!);
                    }
                }

                return base.SendAsync(request, cancellationToken);
            }
        }
    }
}
