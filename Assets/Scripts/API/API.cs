using UnityEngine;
using System.Threading.Tasks;

public interface API
{
    public Task<Page> GetCuratedWallpapers();
    public Task<Page> SearchWallpapers(string query);
    public Task<Page> NextPage(string nextPageURL);
}

