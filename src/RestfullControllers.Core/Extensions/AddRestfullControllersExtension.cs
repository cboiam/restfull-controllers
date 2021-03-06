using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using RestfullControllers.Core.Metadata;

namespace RestfullControllers.Core.Extensions
{
    public static class AddRestfullControllersExtension
    {
        public static IServiceCollection AddRestfullControllers(this IServiceCollection services, 
            Assembly assembly,
            Action<RestfullControllerOptions> optionsBuilder = null)
        {
            var options = new RestfullControllerOptions();
            optionsBuilder.Invoke(options);
            
            var controllerMetadatas = assembly.GetTypes()
                .Where(t => t.BaseType.FullName.Contains(typeof(RestfullController<>).FullName))
                .Select(c => new ControllerMetadata
                {
                    Controller = c,
                    Template = c.GetCustomAttribute<RouteAttribute>().Template,
                    Actions = c.GetMethods()
                        .Where(m => m.IsPublic &&
                            typeof(IActionResult).IsAssignableFrom(m.ReturnType))
                        .Select(m => new ActionMetadata
                        {
                            Action = m,
                            Methods = m.GetCustomAttributes<HttpMethodAttribute>()
                        })
                });
                
            services.AddSingleton(options);
            services.AddSingleton(controllerMetadatas);
            services.AddScoped(typeof(ILinkMapper<>), typeof(LinkMapper<>));
            services.AddScoped(typeof(IResponseMapper<>), typeof(ResponseMapper<>));
            services.AddHttpContextAccessor();

            return services;
        }
    }
}