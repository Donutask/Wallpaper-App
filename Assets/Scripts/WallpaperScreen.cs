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

    [SerializeField] TextMeshProUGUI downloadButtonLabel;
    [SerializeField] GameObject applyMenu, loadingSpinner;

    [SerializeField] LocalizedString attributionString, downloadingString, appliedString;

    public static string WallpaperDownloadPath
    {
        get
        {
            return Path.Combine(Application.temporaryCachePath, "wallpaper.png");
        }
    }

    public static void ShowFullPreview(Wallpaper p)
    {
        selectedWallpaper = p;
        SceneManager.LoadSceneAsync("Wallpaper");
    }

    private async void Start()
    {
        loadingSpinner.SetActive(true);

        artistName.text = attributionString.GetLocalizedString(selectedWallpaper.artist);

        aspectRatioFitter.aspectRatio = selectedWallpaper.aspectRatio;

        //load existing thumbnail, then switch to higher res preview
        wallpaperImage.sprite = await selectedWallpaper.thumbnail.Get(cacheOnly: true);
        wallpaperImage.sprite = await selectedWallpaper.preview.Get();

        loadingSpinner.SetActive(false);
    }

    public void Donate()
    {
        Application.OpenURL(selectedWallpaper.artistURL);
    }

    bool canDownload;

    public async void Download()
    {
        //prevent spam downloading
        if (canDownload == true)
        {
            return;
        }
        canDownload = true;

        downloadButtonLabel.text = downloadingString.GetLocalizedString();
        loadingSpinner.SetActive(true);

        var sprite = await selectedWallpaper.original.Get();
        wallpaperImage.sprite = sprite;

        var bytes = sprite.texture.EncodeToPNG();
        if (bytes != null)
        {
            await File.WriteAllBytesAsync(WallpaperDownloadPath, bytes);
            applyMenu.SetActive(true);
            loadingSpinner.SetActive(false);
        }
    }

    public void ApplyWallpaper(int target)
    {
        WallpaperChanger.SetWallpaper(WallpaperDownloadPath, (WallpaperChanger.WallpaperTarget)target);
        AppliedWallpaper();
    }

    public void SaveWallpaperToGallery()
    {
        NativeGallery.SaveImageToGallery(WallpaperDownloadPath, "Wallpapers", "wallpaper.png", SaveToGalleryCallback);
        AppliedWallpaper();
    }

    void AppliedWallpaper()
    {
        applyMenu.SetActive(false);
        downloadButtonLabel.text = appliedString.GetLocalizedString();
    }

    void SaveToGalleryCallback(bool success, string path)
    {
        Debug.Log(success + ": " + path);
    }

    public void Back()
    {
        SceneManager.LoadSceneAsync("Browse");
    }
}
