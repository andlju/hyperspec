﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperspec
{
    public class Embedded
    {
        public Embedded(bool single)
        {
            Single = single;
        }

        private readonly IList<Representation> _resources = new List<Representation>();
        public bool Single { get; }

        public IEnumerable<Representation> GetEmbedded()
        {
            return _resources;
        }

        public void AddResource(Representation representation)
        {
            if (Single && _resources.Any())
                throw new InvalidOperationException("Embeddeds marked as single can only add one resource");
            _resources.Add(representation);
        }
    }

    public abstract class Representation
    {
        private readonly string _selfTemplate;
        private readonly string _profileHref;
        private readonly IDictionary<string, Embedded> _embeddedResources = new Dictionary<string, Embedded>();

        protected Representation(string selfTemplate = null, string profileHref = null)
        {
            _selfTemplate = selfTemplate;
            _profileHref = profileHref;
        }

        internal Representation Parent { get; set; }

        public void EmbedResource(string name, Representation representation)
        {
            EmbedResource(name, representation, false);
        }

        public void EmbedResource(string name, Representation representation, bool single)
        {
            Embedded embedded;
            if (!_embeddedResources.TryGetValue(name, out embedded))
            {
                embedded = new Embedded(single);
                _embeddedResources.Add(name, embedded);
            }
            representation.Parent = this;
            embedded.AddResource(representation);
        }

        protected virtual IEnumerable<string> PropertiesToIgnore()
        {
            yield break;
        }

        protected bool IncludeProperty(string name, object val)
        {
            return !PropertiesToIgnore().Any(p => p == name);
        }

        /// <summary>
        /// Override this to add links specific for this resource representation
        /// </summary>
        /// <param name="linkBuilder"></param>
        protected virtual void AddLinks(ILinkBuilder linkBuilder)
        {

        }

        /// <summary>
        /// Override this to add forms specific for this resource representation
        /// </summary>
        /// <param name="formBuilder"></param>
        protected virtual void AddForms(IFormBuilder formBuilder)
        {

        }


        /// <summary>
        /// Add a self-link to the links for this resource
        /// </summary>
        /// <remarks>Override this if you want to change the default way of adding self-links.</remarks>
        /// <param name="builder"></param>
        protected virtual void AddSelfLink(ILinkBuilder builder)
        {
            if (_selfTemplate != null)
            {
                builder.AddSelfLink(_selfTemplate);
            }
        }

        /// <summary>
        /// Add a profile link to the links for this resource if a profile has been set.
        /// </summary>
        /// <remarks>Override this if you want to change the default way of adding profile-links.</remarks>
        /// <param name="builder"></param>
        protected virtual void AddProfileLink(ILinkBuilder builder)
        {
            if (!string.IsNullOrEmpty(_profileHref))
            {
                builder.AddProfileLink(_profileHref);
            }
        }

        /// <summary>
        /// Get any objects that should be treated as the actual content of this representation
        /// </summary>
        /// <remarks>Used when serializing</remarks>
        /// <returns></returns>
        public virtual IEnumerable<IContentContext> GetContent()
        {
            yield return new ContentContext(this, IncludeProperty);
        }

        /// <summary>
        /// Get all embedded resources
        /// </summary>
        /// <remarks>Used when serializing</remarks>
        /// <returns></returns>
        public IDictionary<string, Embedded> GetEmbedded()
        {
            return _embeddedResources;
        }

        /// <summary>
        /// Get all links for this resource.
        /// </summary>
        /// <remarks>Used when serializing</remarks>
        /// <param name="linkBase">Base url to use for links</param>
        /// <returns></returns>
        public IDictionary<string, IList<ILink>> GetLinks(string linkBase = "")
        {
            var linkBuilder = new LinkBuilder(GetLinkContext(), linkBase);

            // Add common links
            AddSelfLink(linkBuilder);
            AddProfileLink(linkBuilder);

            AddLinks(linkBuilder);
            return linkBuilder.Links;
        }

        /// <summary>
        /// Get all forms for this resource
        /// </summary>
        /// <param name="linkBase">Base url to use for links</param>
        /// <remarks>Used when serializing</remarks>
        /// <returns></returns>
        public IDictionary<string, IList<IForm>> GetForms(string linkBase = "")
        {
            var formsBuilder = new FormBuilder(GetLinkContext(), linkBase);
            AddForms(formsBuilder);
            return formsBuilder.Forms;
        }

        /// <summary>
        /// An enumerable of all context objects for this resource. This will be used when resolving
        /// link and form templates.
        /// </summary>
        protected IEnumerable<IContentContext> GetLinkContext()
        {
            foreach (var content in GetLocalLinkContext())
                yield return content;

            if (Parent == null)
            {
                yield break;
            }

            foreach (var parent in Parent.GetLinkContext())
            {
                yield return parent;
            }
        }

        /// <summary>
        /// Local context objects that should be used when resolving links and form templates for this resource
        /// </summary>
        /// <remarks>This defaults to using the <c>GetContent</c> method.</remarks>
        /// <returns></returns>
        protected virtual IEnumerable<IContentContext> GetLocalLinkContext()
        {
            return GetContent();
        }
    }

    public abstract class Representation<TContent> : Representation
    {
        protected Representation(TContent content, string selfTemplate, string resourceType)
            : base(selfTemplate, resourceType)
        {
            Content = content;
        }

        protected TContent Content { get; set; }

        /// <summary>
        /// The content for a representation with an external model contains all properties on the 
        /// actual representation combined with all properties on the model.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<IContentContext> GetContent()
        {
            yield return new ContentContext(Content, IncludeProperty);
            yield return new ContentContext(this, IncludeProperty);
        }
    }
}
