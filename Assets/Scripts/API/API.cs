using UnityEngine;
using System.Threading.Tasks;

public interface API
{
    public Task<WallpaperPage> GetCuratedWallpapers();
    public Task<WallpaperPage> SearchWallpapers(string query);
    public Task<WallpaperPage> SearchWallpapersWithColour(string query, Color32 colour);
    public Task<WallpaperPage> NextPage(string nextPageURL);

    public Task<CollectionPage> GetCollections();
    //too tired to make the code better
    public Task<CollectionPage> NextCollectionsPage(string nextPageURL);
}

