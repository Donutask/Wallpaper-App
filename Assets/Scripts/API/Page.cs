using UnityEngine;

/// <summary>
/// A collection of wallpapers.
/// </summary>
public class Page<T>
{
    public int pageNumber;
    public T[] content;
    public int itemCount
    {
        get
        {
            if (content == null)
            {
                return 0;
            }
            else
            {
                return content.Length;
            }
        }
    }
    /// <summary>
    /// Use this with API.NextPage to get another page with more results
    /// </summary>
    public string nextPageURL;

    public Page(int pageNumber, T[] content, string nextPage)
    {
        this.pageNumber = pageNumber;
        this.content = content;
        this.nextPageURL = nextPage;
    }
}
