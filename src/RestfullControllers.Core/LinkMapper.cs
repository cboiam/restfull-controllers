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
        private readonly HttpContext context;
        private readonly ControllerMetadata controller;

        public LinkMapper(IHttpContextAccessor contextAccessor, IEnumerable<ControllerMetadata> controllerMetadatas)
        {
            context = contextAccessor.HttpContext;
            controller = controllerMetadatas.Where(c =>
                c.Controller.IsAssignableTo(typeof(RestfullController<TEntity>)))
                .FirstOrDefault();
        }

        public IEnumerable<Link> MapControllerLinks()
        {
            return controller.Actions.Where(a => !a.Methods.Any(IsActionScoped)).SelectMany(a =>
                a.Methods.SelectMany(m =>
                    m.HttpMethods.Select(h => {
                        var link = BuildLink(controller.Template, m.Template);
                        return new Link
                        {
                            Rel = a.Action.Name,
                            Href = link,
                            Method = h
                        };
                    })
                ));
        }

        public IEnumerable<Link> MapEntityLinks(TEntity entity)
        {
            var idName = entity.GetType().GetMembers()
                .FirstOrDefault(member => member.GetCustomAttribute<IdAttribute>() != null);
            
            return controller.Actions.Where(a => a.Methods.Any(IsActionScoped)).SelectMany(a =>
                a.Methods.SelectMany(m =>
                    m.HttpMethods.Select(h => {
                        var isSelf = HttpMethods.IsGet(h) && 
                            idName != null && 
                            m.Template.Contains("{" + idName.Name + "}", StringComparison.InvariantCultureIgnoreCase);
                        
                        return new Link
                        {
                            Rel = isSelf ? SelfRel : a.Action.Name,
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

        private bool IsActionScoped(HttpMethodAttribute method) =>
            (controller.Template != null && Regex.IsMatch(controller.Template, pathArgumentPattern)) ||
            (method.Template != null && Regex.IsMatch(method.Template, pathArgumentPattern));

        private string BuildLink(string controllerTemplate,
            string actionTemplate,
            TEntity entity = null)
        {
            var template = controllerTemplate;
            if(!string.IsNullOrEmpty(actionTemplate))
            {
                template += $"/{actionTemplate}";
            }

            var path = InjectArgumentsInTemplate(entity, template);
            return UriHelper.BuildAbsolute(context.Request.Scheme,
                context.Request.Host,
                path: $"/{path}");
        }

        private static string InjectArgumentsInTemplate(TEntity entity, string template)
        {
            var entityType = typeof(TEntity);
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

        private static object GetParameterValue(TEntity entity, Type entityType, string parameterName)
        {
            var member = entityType.GetMembers()
                .Where(m => m.Name.Equals(parameterName, StringComparison.InvariantCultureIgnoreCase));

            if (!member.Any())
            {
                throw new MissingMemberException(entityType.Name, parameterName);
            }

            return entityType.InvokeMember(member.First().Name, BindingFlags.GetField |
                BindingFlags.GetProperty,
                null,
                entity,
                null);
        }
    }
}