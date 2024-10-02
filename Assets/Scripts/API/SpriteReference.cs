using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.IO;

/// <summary>
/// Stores URL for a Sprite. Can get the sprite when you actually need it.
/// </summary>
public class SpriteReference
{
    /// <summary>
    /// Store here instead of pinging an API constantly. Wallpapers shouldn't need to be dynamically updated.
    /// </summary>
    public static string CachePath
    {
        get
        {
            return Path.Combine(Application.persistentDataPath, "APICache");
        }
    }

    private string url;
    private Sprite cached;

    public SpriteReference(string url)
    {
        this.url = url;
        cached = null;
    }

    public async Task<Sprite> Get(bool cacheOnly = false, bool useFileCache = true)
    {
        if (cached == null && !cacheOnly)
            cached = await GetSprite(url, useFileCache);

        return cached;
    }

    static async Task<Sprite> GetSprite(string url, bool useCache = true)
    {
        Directory.CreateDirectory(CachePath);//ensures it exists
        string cachePath = Path.Combine(CachePath, url.GetHashCode() + ".bytes"); //Hash code isn't unique, but hopefully good enough

        Texture2D tex;
        if (File.Exists(cachePath) && useCache)
        {
            tex = await GetTextureFromFile(cachePath);
        }
        else
        {
            tex = (await GetTextureFromURL(url));
            if (useCache)
                await File.WriteAllBytesAsync(cachePath, tex.EncodeToPNG());
        }

        return CreateSprite(tex);
    }

    static async Task<Texture2D> GetTextureFromFile(string path)
    {
        var bytes = await File.ReadAllBytesAsync(path);
        Texture2D tex = new(1, 1);

        tex.LoadImage(bytes);

        return tex;
    }

    static async Task<Texture2D> GetTextureFromURL(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

        await www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            return myTexture;
        }
        else
        {
            Debug.Log(www.error);
            return null;
        }
    }

    /// <summary>
    /// Turns Texture2D into a Sprite, hopefully pivot and stuff is correct :)
    /// </summary>
    /// <returns></returns>
    static Sprite CreateSprite(Texture2D tex)
    {
        if (tex == null)
        {
            return null;
        }
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
        return sprite;
    }
}
