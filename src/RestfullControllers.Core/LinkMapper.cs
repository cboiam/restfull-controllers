using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Routing;
using RestfullControllers.Core.Attributes;
using RestfullControllers.Core.Responses;
using RestfullControllers.Core.Metadata;

namespace RestfullControllers.Core
{
    public class LinkMapper<TEntity> : ILinkMapper<TEntity>
        where TEntity : HateoasResponse
    {
        private const char PathSeparator = '/';
        private const string pathArgumentPattern = "{.*}";
        private const string ParameterNullMessage = "Path parameter can't be null";
        private const string SelfRel = "Self";
        private readonly RestfullControllerOptions options;
        private readonly HttpContext context;
        private readonly IEnumerable<ControllerMetadata> controllerMetadatas;
        private readonly ControllerMetadata controller;

        public LinkMapper(IHttpContextAccessor contextAccessor,
            IEnumerable<ControllerMetadata> controllerMetadatas,
            RestfullControllerOptions options)
        {
            this.options = options;
            context = contextAccessor.HttpContext;
            this.controllerMetadatas = controllerMetadatas;
            controller = controllerMetadatas.Where(c =>
                c.Controller.BaseType.GenericTypeArguments.Contains(typeof(TEntity)))
                .FirstOrDefault();
        }

        public IEnumerable<Link> MapControllerLinks()
        {
            return controller.Actions.Where(a => !a.Methods.Any(IsActionScoped)).SelectMany(a =>
                a.Methods.SelectMany(m =>
                    m.HttpMethods.Select(h =>
                    {
                        var link = BuildLink(controller.Template, m.Template);
                        return new Link
                        {
                            Rel = options.RelNamingStrategy.GetPropertyName(a.Action.Name, false),
                            Href = link,
                            Method = h
                        };
                    })
                ));
        }

        public IEnumerable<Link> MapEntityLinks(TEntity entity)
        {
            var idName = entity.GetType().GetProperties()
                .FirstOrDefault(member => member.GetCustomAttribute<IdAttribute>() != null);

            return controller.Actions.Where(a => a.Methods.Any(IsActionScoped)).SelectMany(a =>
                a.Methods.SelectMany(m =>
                    m.HttpMethods.Select(h =>
                    {
                        var isSelf = HttpMethods.IsGet(h) &&
                            idName != null &&
                            m.Template.Contains("{" + idName.Name + "}", StringComparison.InvariantCultureIgnoreCase);

                        string rel = isSelf ? SelfRel : a.Action.Name;
                        return new Link
                        {
                            Rel = options.RelNamingStrategy.GetPropertyName(rel, false),
                            Href = BuildLink(controller.Template, m.Template, entity),
                            Method = h
                        };
                    })
                ));
        }

        public IEnumerable<Link> MapEntityLinks(IEnumerable<TEntity> entities)
        {
            return entities.SelectMany(MapEntityLinks);
        }

        public IEnumerable<Link> MapSubEntityLinks(object entity)
        {
            var controller = controllerMetadatas.Where(c =>
                c.Controller.BaseType.GenericTypeArguments.Contains(entity.GetType()))
                .FirstOrDefault();

            if (controller == null)
                return null;

            return controller.Actions.Where(a => a.Methods.Any(IsActionScoped))
                .SelectMany(a => a.Methods.Where(m => m.HttpMethods.Any(h => HttpMethods.IsGet(h)))
                    .SelectMany(m =>
                        m.HttpMethods.Select(h =>
                        {
                            return new Link
                            {
                                Rel = options.RelNamingStrategy.GetPropertyName(SelfRel, false),
                                Href = BuildLink(controller.Template, m.Template, entity),
                                Method = h
                            };
                        })
                    )
                );
        }

        private bool IsActionScoped(HttpMethodAttribute method) =>
            (controller.Template != null && Regex.IsMatch(controller.Template, pathArgumentPattern)) ||
            (method.Template != null && Regex.IsMatch(method.Template, pathArgumentPattern));

        private string BuildLink(string controllerTemplate,
            string actionTemplate,
            object entity = null)
        {
            var template = controllerTemplate;
            if (!string.IsNullOrEmpty(actionTemplate))
            {
                template += $"/{actionTemplate}";
            }

            var path = InjectArgumentsInTemplate(entity, template);
            return UriHelper.BuildAbsolute(context.Request.Scheme,
                context.Request.Host,
                path: $"/{path}");
        }

        private static string InjectArgumentsInTemplate(object entity, string template)
        {
            Type entityType = entity?.GetType();
            var pathSegments = template.Trim(PathSeparator)
                .Split(PathSeparator);

            var parametizedPath = new List<string>();
            foreach (var item in pathSegments)
            {
                if (!Regex.IsMatch(item, pathArgumentPattern))
                {
                    parametizedPath.Add(item);
                    continue;
                }

                var parameterName = item.Trim('{', '}');
                object parameter = GetParameterValue(entity, entityType, parameterName);
                if (parameter == null)
                {
                    throw new ArgumentNullException(entityType.Name, ParameterNullMessage);
                }
                parametizedPath.Add(parameter.ToString());
            }
            return string.Join(PathSeparator, parametizedPath);
        }

        private static object GetParameterValue(object entity, Type entityType, string parameterName)
        {
            var routeParameter = entityType.GetCustomAttributes<RouteParameterAttribute>()
                .FirstOrDefault(a => a.Name.Equals(parameterName, StringComparison.InvariantCultureIgnoreCase));

            var properties = entityType.GetProperties()
                .Select(p => new
                {
                    Property = p,
                    RouteParameter = p.GetCustomAttribute<RouteParameterAttribute>()
                })
                .Where(p => p.Property.Name.Equals(parameterName, StringComparison.InvariantCultureIgnoreCase) ||
                    (p.RouteParameter != null && p.RouteParameter.Name.Equals(parameterName, StringComparison.InvariantCultureIgnoreCase)));

            if (!properties.Any())
            {
                throw new MissingMemberException(entityType.Name, parameterName);
            }

            return properties.First().Property.GetValue(entity);
        }
    }
}