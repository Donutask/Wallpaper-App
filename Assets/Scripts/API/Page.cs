using UnityEngine;

/// <summary>
/// A collection of wallpapers.
/// </summary>
public class Page
{
    public int pageNumber;
    public Wallpaper[] wallpapers;
    public int itemCount
    {
        get
        {
            if (wallpapers == null)
            {
                return 0;
            }
            else
            {
                return wallpapers.Length;
            }
        }
    }
    /// <summary>
    /// Use this with API.NextPage to get another page with more results
    /// </summary>
    public string nextPageURL;

    public Page(int pageNumber, Wallpaper[] wallpapers, string nextPage)
    {
        this.pageNumber = pageNumber;
        this.wallpapers = wallpapers;
        this.nextPageURL = nextPage;
    }
}
