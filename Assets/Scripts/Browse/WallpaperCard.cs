using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class WallpaperCard : MonoBehaviour, Card<Wallpaper>
{
    [SerializeField] Image wallpaperImage;
    [SerializeField] TextMeshProUGUI wallpaperName;
    [SerializeField] AspectRatioFitter aspectRatioFitter;
    [SerializeField] Button button;

    public async void ApplyCardValues(Wallpaper photo)
    {
        wallpaperName.text = photo.artist;

        Sprite s = await photo.thumbnail.Get();
        if (s != null)
        {
            wallpaperImage.sprite = s;
            aspectRatioFitter.aspectRatio = (float)s.texture.width / s.texture.height;
        }

        button.onClick.AddListener(() => WallpaperScreen.ShowFullPreview(photo));
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
