using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Threading.Tasks;

/// <summary>
/// An image
/// </summary>
public class Wallpaper
{
    public string artist, artistURL;
    public SpriteReference original, preview, thumbnail;

    /// <summary>
    /// Dimensions of full image
    /// </summary>
    public int width, height;


    public Wallpaper(string artist, string artistURL, string originalURL, string previewURL, string thumbnailURL, int width, int height)
    {
        this.artist = artist;
        this.artistURL = artistURL;

        this.original = new SpriteReference(originalURL);
        this.preview = new SpriteReference(previewURL);
        this.thumbnail = new SpriteReference(thumbnailURL);
    }

}
