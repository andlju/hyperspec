using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using Resta.UriTemplates;

namespace Hyperspec
{
    public class TemplateParameterInfo
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public bool IsRequired { get; set; }
        public bool InTemplate { get; set; }
        public bool ForceTemplated { get; set; }
    }

    /// <summary>
    /// Common base class for ResourceLink and ResourceForm. 
    /// </summary>
    public abstract class ResourceLinkBase : ILink
    {
        private readonly Dictionary<string, TemplateParameterInfo> _parameterInfos;

        private readonly UriTemplate _originalUriTemplate;
        private readonly UriTemplate _extendedUriTemplate;

        protected readonly IEnumerable<IContentContext> Contexts;
        private readonly string _title;

        protected ResourceLinkBase(string linkTemplate, IEnumerable<IContentContext> contexts, string title, IEnumerable<TemplateParameterInfo> parameterInfos = null)
        {
            _originalUriTemplate = _extendedUriTemplate = new UriTemplate(linkTemplate);
            _title = title;

            Contexts = contexts;

            _parameterInfos = new Dictionary<string, TemplateParameterInfo>();
            foreach (var variable in _originalUriTemplate.Variables)
            {
                ParameterInfos[variable.Name.ToLowerInvariant()] = new TemplateParameterInfo()
                {
                    Name = variable.Name,
                    IsRequired = true,
                    InTemplate = true
                };
            }

            if (parameterInfos == null)
                return;

            var extendedUriBuilder = new UriTemplateBuilder(_originalUriTemplate);
            var isFirst = !_originalUriTemplate.Template.Contains("?");
            var varspecs = new List<VarSpec>();
            foreach (var param in parameterInfos)
            {
                // Add up all parameters to the template. Append any new ones to the template
                if (AddParameterInfo(param))
                {
                    varspecs.Add(new VarSpec(param.Name));
                }
            }
            extendedUriBuilder.Append(isFirst ? '?' : '&', varspecs.ToArray());
            _extendedUriTemplate = extendedUriBuilder.Build();
        }

        public virtual string Title { get { return _title; } }

        public abstract string Href { get; }
        public abstract IDictionary<string, FormParameterInfo> Template { get; }

        protected IDictionary<string, TemplateParameterInfo> ParameterInfos
        {
            get { return _parameterInfos; }
        }

        protected string GetHref(bool includeExtraParameters)
        {
            var template = includeExtraParameters ? _extendedUriTemplate : _originalUriTemplate;
            var linkParts = GetParameters();

            var templateResolver = template.GetResolver();
            foreach (var part in linkParts.Where(p => !p.ForceTemplated)) // Don't resolve parameters that have been forced as templated
            {
                var value = GetParameter(Contexts, part.Name);
    
                if (value.Any())
                {
                    templateResolver.Bind(part.Name, value);
                }
            }

            var resolvedUri = templateResolver.ResolveTemplate().Template;

            return resolvedUri;
        }

        public IEnumerable<TemplateParameterInfo> GetParameters()
        {
            return ParameterInfos.Values;
        }

        private bool AddParameterInfo(TemplateParameterInfo parameter)
        {
            TemplateParameterInfo existingParam;
            if (ParameterInfos.TryGetValue(parameter.Name.ToLowerInvariant(), out existingParam))
            {
                // This parameter info already existed. Let's update it.
                existingParam.Title = parameter.Title;
                existingParam.Type = parameter.Type;
                existingParam.IsRequired |= parameter.IsRequired;
                return false;
            }
            // No such parameter. Add it to the template.
            parameter.InTemplate = false;
            ParameterInfos[parameter.Name.ToLowerInvariant()] = parameter;
            return true;
        }

        protected static IEnumerable<string> GetParameter(IEnumerable<IContentContext> contexts, string paramName)
        {
            object val = null;

            foreach (var context in contexts)
            {
                var content = context.Content;
                var dict = content as IDictionary<string, object>;
                if (dict != null)
                {
                    // The content object is a dictionary. Let's try to find a value.
                    if (dict.TryGetValue(paramName, out val))
                    {
                        // We found the value. Let's see if we want to use it?
                        if (context.IncludeProperty(paramName, val))
                        {
                            // Yep, no need to check any other context objects
                            break;
                        }
                        // Not found, let's continue with the next context object
                        val = null;
                    }
                    continue;
                }

                // So we didn't get a dictionary. What did we get then?
                var contextType = content.GetType();
                
                // Find out if there's a property with this name
                var prop = contextType.GetProperty(paramName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                if (prop != null)
                {
                    // Get the value of the property if possible
                    if (prop.CanRead)
                    {
                        val = prop.GetValue(content);
                        if (context.IncludeProperty(paramName, val))
                        {
                            // Found the value, no need to check any more context objects
                            break;
                        }
                    }

                    // Found a value but not supposed to use it. Let's reset and try the next context object
                    val = null;
                }
            }
            
            // No value found
            if (val == null)
                yield break;
            
            if (val is DateTime)
            {
                yield return ((DateTime)val).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                yield break;
            }

            var ints = val as int[];
            if (ints != null)
            {
                foreach (var i in ints)
                {
                    yield return i.ToString();
                }
                yield break;
            }

            var objects = val as IEnumerable<object>;
            if (objects != null)
            {
                foreach (var o in objects)
                {
                    yield return o.ToString();
                }
                yield break;
            }

            yield return val.ToString();
        }

        protected IDictionary<string, FormParameterInfo> GetTemplate()
        {
            return ParameterInfos.Values.Where(pi => !pi.InTemplate).ToDictionary(pi => pi.Name, pi => new FormParameterInfo()
            {
                Title = pi.Title,
                Type = pi.Type,
                IsRequired = pi.IsRequired,
                DefaultValue = GetParameter(Contexts, pi.Name).FirstOrDefault()
            });
        }
    }

    /// <summary>
    /// A Resource Link with a template. The 
    /// </summary>
    /// <typeparam name="TTemplate"></typeparam>
    public abstract class ResourceLinkBase<TTemplate> : ResourceLinkBase
    {
        protected ResourceLinkBase(string linkTemplate, IEnumerable<IContentContext> contexts, string title, IEnumerable<TemplateParameterInfo> parameterInfos) : 
            base(linkTemplate, contexts, title, GetParameterInfos(parameterInfos))
        {

        }

        private static IEnumerable<TemplateParameterInfo> GetParameterInfos(IEnumerable<TemplateParameterInfo> overriddenParameters)
        {
            var templateType = typeof(TTemplate);
            var props = templateType.GetProperties();
            var overriddenParametersArray = overriddenParameters as TemplateParameterInfo[] ?? overriddenParameters?.ToArray();

            foreach (var property in props)
            {
                if (!property.CanWrite)
                    continue;

                var propertyName = property.Name;
                
                if (overriddenParametersArray != null && overriddenParametersArray.Any(p => p.Name == propertyName))
                    continue;

                var type = property.PropertyType;
                string typeName = null;
                string title = null;
                bool isRequired = type.IsValueType;

                typeName = GetTypeName(type);

                if (type.IsNullableType())
                {
                    typeName = GetTypeName(type.GetTypeOfNullable());
                    isRequired = false;
                }
                var requiredAttr = property.GetCustomAttribute<RequiredAttribute>();
                if (requiredAttr != null)
                {
                    isRequired = true;
                }

                var descAttr = property.GetCustomAttribute<DisplayAttribute>();
                if (descAttr != null)
                {
                    title = descAttr.GetName();
                }
                yield return new TemplateParameterInfo()
                {
                    Name = propertyName,
                    Type = typeName,
                    IsRequired = isRequired,
                    Title = title
                };
            }
            if (overriddenParametersArray != null)
            {
                foreach (var parameterInfo in overriddenParametersArray)
                {
                    yield return parameterInfo;
                }
            }
        }

        private static string GetTypeName(Type type)
        {
            if (type.IsArray)
                return GetTypeName(type.GetElementType()) + "[]";
            if (type == typeof(string))
                return "string";
            if (type == typeof(short) || type == typeof(int) || type == typeof(long))
                return "int";
            if (type == typeof(DateTime))
                return "date";
            if (type == typeof(Guid))
                return "guid";

            return type.Name;
        }
    }
}