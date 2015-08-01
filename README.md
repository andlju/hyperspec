# Hyperspec

This is very much a work-in-progress trying to build a reasonably easy way of creating a hypermedia API in .NET

So far only HAL is supported with Nancy or WebAPI as hosts.

## Getting started

Creating your first Hyperspec project is fairly simple. Let's see how it's done when
hosting with Nancy.

### Create a web, install requisites

Create an ASP.NET Web Application. Choose the Empty template.

Hyperspec works best when Nancy is OWIN hosted, so let's install the following Nuget packages:

```ps
Install-Package Nancy.Owin
Install-Package Microsoft.Owin.Host.SystemWeb
```

Next, we'll install the Hyperspec stuff

```ps
Install-Package Hyperspec.Hal
Install-Package Hyperspec.Nancy
```

As always, you'll need to hook up the Nancy host in an Owin Startup class:

```csharp
public class Startup
{
    public void Configuration(IAppBuilder app)
    {
        app.UseNancy();
    }
}
```

And finally enable the Hyperspec serializer in a Nancy Bootstrapper.

```csharp
public class MyBootstrapper : DefaultNancyBootstrapper
{
    protected override void ConfigureApplicationContainer(TinyIoCContainer container)
    {
        base.ConfigureApplicationContainer(container);

        container.Register<ISerializer, HalNancySerializer>();
    }
}
```

That's all there is to enabling Hyperspec. Now let's get started building some
representations!

## Representations

Hypermedia is handled by either deriving your models from `Representation` or 
wrapping an existing model in a `Representation<TModel>`:

```csharp
/// <summary>
/// Representation for a single friend. 
/// </summary>
public class FriendRepresentation : Representation<Friend>
{
    public FriendRepresentation(Friend content) 
        : base(content, FriendsLinks.Friend, "frapi:friend")
    {
    }

    // The self link is added automatically
    protected override void AddLinks(ILinkBuilder linkBuilder)
    {
        linkBuilder.AddLink("image", FriendsLinks.Image, prompt: "Image");
        linkBuilder.AddLink("blog", Content.Blog, prompt: "Blog");
    }
}

/// <summary>
/// A collection of friends
/// </summary>
public class FriendsRepresentation : Representation
{
    public FriendsRepresentation() : base(FriendsLinks.Friends, "frapi:friends")
    {
        
    }
}
```

## Nancy

A simple Nancy module could look like this:

```csharp
public class FriendsModule : NancyModule
{
    public FriendsModule()
    {
        // Make sure to call with the HTTP header
        // Accept: application/hal+json
        Get[FriendsLinks.Friends] = _ =>
        {
            var friend1 = new Friend()
            {
                FullName = "Anders Ljusberg",
                Slug = "anderslj",
                Blog = "http://coding-insomnia.com"
            };
            var friend2 = new Friend()
            {
                FullName = "Glenn Block",
                Slug = "gblock",
                Blog = "http://codebetter.com/glennblock/"
            };

            // Create the represenation for a list of friends
            var friends = new FriendsRepresentation();

            // Create each friend resource and embed them in the list
            friends.EmbedResource("friend", new FriendRepresentation(friend1));
            friends.EmbedResource("friend", new FriendRepresentation(friend2));

            return friends;
        };
    }
}

```


## Output
```
GET http://localhost:50248/friends
Accept: application/hal+json
```

The above sample should output something similar to this:

```json
{
  "_embedded": {
    "friend": [
      {
        "FullName": "Anders Ljusberg",
        "Blog": "http://coding-insomnia.com",
        "Slug": "anderslj",
        "_links": {
          "self": {
            "href": "/friends/anderslj"
          },
          "profile": {
            "href": "frapi:friend"
          },
          "image": {
            "href": "/image/anderslj",
            "title": "Image"
          },
          "blog": {
            "href": "http://coding-insomnia.com",
            "title": "Blog"
          }
        }
      },
      {
        "FullName": "Glenn Block",
        "Blog": "http://codebetter.com/glennblock/",
        "Slug": "gblock",
        "_links": {
          "self": {
            "href": "/friends/gblock"
          },
          "profile": {
            "href": "frapi:friend"
          },
          "image": {
            "href": "/image/gblock",
            "title": "Image"
          },
          "blog": {
            "href": "http://codebetter.com/glennblock/",
            "title": "Blog"
          }
        }
      }
    ]
  },
  "_links": {
    "self": {
      "href": "/friends"
    },
    "profile": {
      "href": "frapi:friends"
    }
  }
}
```
