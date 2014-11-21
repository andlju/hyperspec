using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hyperspec
{
    public abstract class Representation
    {
        private readonly Link _self;
        private readonly string _resourceProfile;

        private readonly IDictionary<string, IList<Representation>> _embeddedResources = new Dictionary<string, IList<Representation>>();

        protected Representation(Link self, string resourceProfile)
        {
            _self = self;
            _resourceProfile = resourceProfile;
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

        protected virtual void AddLinks(IResourceLinkBuilder linkBuilder)
        {

        }

        protected virtual void AddForms(IResourceFormBuilder resourceFormBuilder)
        {

        }

        internal IDictionary<string, IList<IResourceLink>> GetLinks()
        {
            var linkBuilder = new ResourceLinkBuilder(Context);

            AddSelfLink(linkBuilder);

            linkBuilder.AddProfileLink(_resourceProfile);
            AddLinks(linkBuilder);
            return linkBuilder.Links;
        }

        internal virtual void AddSelfLink(ResourceLinkBuilder builder)
        {
            builder.AddSelfLink(_self);
        }

        internal IDictionary<string, IList<Representation>> GetEmbedded()
        {
            return _embeddedResources;
        }

        internal IDictionary<string, IList<IResourceForm>> GetForms()
        {
            var formsBuilder = new ResourceFormBuilder(Context);
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

        protected internal Link Self
        {
            get { return _self; }
        }

        internal virtual IEnumerable<object> GetLinkContext()
        {
            yield return this;
        }

        internal virtual IEnumerable<object> GetContent()
        {
            yield return this;
        }
    }

    public abstract class Representation<TContent> : Representation
    {
        protected Representation(TContent content, Link self, string resourceType)
            : base(self, resourceType)
        {
            Content = content;
        }

        protected TContent Content { get; set; }

        internal override IEnumerable<object> GetContent()
        {
            yield return this;
            yield return Content;
        }

    }

}
