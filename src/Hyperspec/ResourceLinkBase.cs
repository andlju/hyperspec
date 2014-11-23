using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using Resta.UriTemplates;

namespace Hyperspec
{  
    /// <summary>
    /// Common base class for ResourceLink and ResourceForm. 
    /// </summary>
    public abstract class ResourceLinkBase : ILink
    {
        public class TemplateParameterInfo
        {
            public string Name { get; set; }
            public string Title { get; set; }
            public string Type { get; set; }
            public bool IsRequired { get; set; }
            public bool InTemplate { get; set; }
        }

        private readonly Dictionary<string, TemplateParameterInfo> _parameterInfos;

        private readonly UriTemplate _uriTemplate;
        private readonly UriTemplate _extendedUriTemplate;

        protected readonly IEnumerable<object> Resources;
        private readonly string _title;

        protected ResourceLinkBase(TemplatedLink templatedLink, IEnumerable<object> resources, string title, IEnumerable<TemplateParameterInfo> parameterInfos = null)
        {
            _uriTemplate = _extendedUriTemplate = templatedLink.GetTemplate();
            _title = title;

            Resources = resources;

            _parameterInfos = new Dictionary<string, TemplateParameterInfo>();
            foreach (var variable in _uriTemplate.Variables)
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

            var extendedUriBuilder = new UriTemplateBuilder(_uriTemplate);
            var isFirst = !_uriTemplate.Template.Contains("?");
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

        protected IDictionary<string, TemplateParameterInfo> ParameterInfos
        {
            get { return _parameterInfos; }
        }

        protected string GetHref(bool includeExtraParameters)
        {
            var template = includeExtraParameters ? _extendedUriTemplate : _uriTemplate;
            var linkParts = GetParameters();

            var templateResolver = template.GetResolver();
            foreach (var part in linkParts)
            {
                var value = GetParameter(Resources, part.Name);

                if (value != null || !part.IsRequired)
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

        protected static string GetParameter(IEnumerable<object> contextObjects, string paramName)
        {
            object context = null;
            PropertyInfo prop = null;
            foreach (var o in contextObjects)
            {
                var contextType = o.GetType();

                prop = contextType.GetProperty(paramName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                if (prop != null)
                {
                    context = o;
                    break;
                }
            }
            if (prop == null)
                return null;

            var val = prop.GetValue(context);
            if (val == null)
                return null;
            if (val is DateTime)
                return ((DateTime)val).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            var ints = val as int[];
            if (ints != null)
                return string.Join(",", ints);

            return val.ToString();
        }
    }

    /// <summary>
    /// A Resource Link with a template. The 
    /// </summary>
    /// <typeparam name="TTemplate"></typeparam>
    public abstract class ResourceLinkBase<TTemplate> : ResourceLinkBase
    {
        public ResourceLinkBase(TemplatedLink templatedLink, IEnumerable<object> resources, string title) : base(templatedLink, resources, title, GetParameterInfos())
        {

        }

        private static IEnumerable<TemplateParameterInfo> GetParameterInfos()
        {
            var templateType = typeof(TTemplate);
            var props = templateType.GetProperties();
            foreach (var property in props)
            {

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
                    Name = property.Name,
                    Type = typeName,
                    IsRequired = isRequired,
                    Title = title
                };
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