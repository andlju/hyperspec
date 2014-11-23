using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperspec
{
    public abstract class Representation
    {
        private readonly TemplatedLink _self;
        private readonly string _profileHref;

        private readonly IDictionary<string, IList<Representation>> _embeddedResources = new Dictionary<string, IList<Representation>>();

        protected Representation(TemplatedLink self, string profileHref = null)
        {
            _self = self;
            _profileHref = profileHref;
        }

        internal Representation Parent { get; set; }

        public void EmbedResource(string name, Representation representation)
        {
            IList<Representation> resourceList;
            if (!_embeddedResources.TryGetValue(name, out resourceList))
            {
                resourceList = new List<Representation>();
                _embeddedResources.Add(name, resourceList);
            }
            representation.Parent = this;
            resourceList.Add(representation);
        }

        protected virtual void AddLinks(ILinkBuilder linkBuilder)
        {

        }

        protected virtual void AddForms(IFormBuilder resourceFormBuilder)
        {

        }

        public IDictionary<string, IList<ILink>> GetLinks()
        {
            var linkBuilder = new LinkBuilder(Context);

            // Add common links
            AddSelfLink(linkBuilder);
            AddProfileLink(linkBuilder);

            AddLinks(linkBuilder);
            return linkBuilder.Links;
        }

        /// <summary>
        /// Add a self-link to the links for this resource
        /// </summary>
        /// <remarks>Override this if you want to change the default way of adding self-links.</remarks>
        /// <param name="builder"></param>
        protected virtual void AddSelfLink(ILinkBuilder builder)
        {
            builder.AddSelfLink(_self);
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

        public IDictionary<string, IList<Representation>> GetEmbedded()
        {
            return _embeddedResources;
        }

        public IDictionary<string, IList<IResourceForm>> GetForms()
        {
            var formsBuilder = new FormBuilder(Context);
            AddForms(formsBuilder);
            return formsBuilder.Forms;
        }

        internal IEnumerable<object> Context
        {
            get
            {
                foreach (var content in GetLinkContext())
                    yield return content;

                if (Parent == null)
                {
                    yield break;
                }

                foreach (var parent in Parent.Context)
                {
                    yield return parent;
                }
            }
        }

        protected internal TemplatedLink Self
        {
            get { return _self; }
        }

        public virtual IEnumerable<object> GetLinkContext()
        {
            yield return this;
        }

        public virtual IEnumerable<object> GetContent()
        {
            yield return this;
        }
    }

    public abstract class Representation<TContent> : Representation
    {
        protected Representation(TContent content, TemplatedLink self, string resourceType)
            : base(self, resourceType)
        {
            Content = content;
        }

        protected TContent Content { get; set; }

        public override IEnumerable<object> GetContent()
        {
            yield return this;
            yield return Content;
        }

        public override IEnumerable<object> GetLinkContext()
        {
            yield return this;
            yield return Content;
        }
    }
}
