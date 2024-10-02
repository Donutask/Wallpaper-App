using UnityEngine;

public class CollectionPage : Page<Collection>
{
    public CollectionPage(int pageNumber, Collection[] content, string nextPage) : base(pageNumber, content, nextPage)
    {
    }
}
