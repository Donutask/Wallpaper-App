using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

/// <summary>
/// Stores URL for a Sprite. Can get the sprite when you actually need it.
/// </summary>
public class SpriteReference
{
    private string url;
    private Sprite cached;

    public SpriteReference(string url)
    {
        this.url = url;
        cached = null;
    }

    public async Task<Sprite> Get(bool cacheOnly = false)
    {
        if (cached == null && !cacheOnly)
            cached = await GetSprite(url);

        return cached;
    }

    //public async Task<Texture2D> GetTexture()
    //{
    //    return await GetTextureFromURL(url);
    //}

    ///// <summary>
    ///// Bytes of image encoded to png.
    ///// </summary>
    //public async Task<byte[]> GetBytes()
    //{
    //    var tex = await GetTextureFromURL(url);
    //    if (tex == null)
    //    {
    //        return null;
    //    }
    //    return tex.EncodeToPNG();
    //}

    static async Task<Sprite> GetSprite(string url)
    {
        return CreateSprite(await GetTextureFromURL(url));
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
