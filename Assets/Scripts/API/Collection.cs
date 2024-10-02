using UnityEngine;

//future idea
public class Collection
{
    public string title, description;
    public SpriteReference thumbnail;
    public WallpaperPage wallpapers;

    public Collection(string title, string description, WallpaperPage wallpapers)
    {
        this.title = title;
        this.description = description;
        this.wallpapers = wallpapers;

        if (wallpapers != null && wallpapers.content.Length > 0)
            thumbnail = wallpapers.content[0].thumbnail;
    }
}
