using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Localization;

public class WallpaperScreen : MonoBehaviour
{
    static Wallpaper selectedWallpaper;

    [SerializeField] Image wallpaperImage;
    [SerializeField] TextMeshProUGUI artistName;
    [SerializeField] AspectRatioFitter aspectRatioFitter;
    [SerializeField] LocalizedString attributionString;

    public static void ShowFullPreview(Wallpaper p)
    {
        selectedWallpaper = p;
        SceneManager.LoadSceneAsync("Wallpaper");
    }

    private async void Start()
    {
        artistName.text = attributionString.GetLocalizedString(selectedWallpaper.artist);

        aspectRatioFitter.aspectRatio = (float)selectedWallpaper.width / (float)selectedWallpaper.height;

        wallpaperImage.sprite = await selectedWallpaper.thumbnail.Get(cacheOnly: true);
        wallpaperImage.sprite = await selectedWallpaper.preview.Get();
    }


    public void Donate()
    {
        Application.OpenURL(selectedWallpaper.artistURL);
    }

    public static string WallpaperDownloadPath
    {
        get
        {
            return Path.Combine(Application.temporaryCachePath, "wallpaper.png");
        }
    }

    public async void Download()
    {
        byte[] bytes = await selectedWallpaper.original.GetBytes();

        if (bytes != null)
        {
            await File.WriteAllBytesAsync(WallpaperDownloadPath, bytes);

            WallpaperChanger.SetWallpaper(WallpaperDownloadPath, WallpaperChanger.WallpaperTarget.HomeScreen);
        }
    }

    public void Back()
    {
        SceneManager.LoadSceneAsync("Browse");
    }
}
