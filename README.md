# Hyperspec

This is very much a work-in-progress trying to build a reasonably easy way of creating a hypermedia API in .NET

So far only the combination HAL and Nancy is supported.

## Representations

Hypermedia is handled by either deriving your models from `Representation` or wrapping an existing model in a `Representation<TModel>`:

```csharp
public class FriendRepresentation : Representation<Friend>
{
    public FriendRepresentation(Friend content) 
        : base(content, FriendsLinks.Friend, "friend")
    {
    }

    protected override void AddLinks(IResourceLinkBuilder linkBuilder)
    {
        linkBuilder.AddLink("image", FriendsLinks.Image, prompt: "Image");
        linkBuilder.AddLink("blog", new Link(Content.Blog), prompt: "Blog");
    }
}

public class FriendsRepresentation : Representation
{
    public FriendsRepresentation() : base(FriendsLinks.Friends, "friends")
    {
        
    }
}
```

## Nancy
So far, the only thing supported is serializing to Hal and hosting in Nancy. A simple Nancy module could look like this:

```csharp
public class FriendsModule : NancyModule
{
    public FriendsModule()
    {
        Get[FriendsLinks.Friends.GetPathTemplate()] = _ =>
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
