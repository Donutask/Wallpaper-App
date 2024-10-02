using UnityEngine;
using System.Threading.Tasks;

public interface API
{
    public Task<WallpaperPage> GetCuratedWallpapers();
    public Task<WallpaperPage> SearchWallpapers(string query);
    public Task<WallpaperPage> NextPage(string nextPageURL);

    public Task<CollectionPage> GetCollections();

}

