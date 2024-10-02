using UnityEngine;
using System.IO;
using UnityEngine.Android;

public static class WallpaperChanger
{
    // Call this method with the file path of the image
    public static void SetWallpaper(string imagePath)
    {
#if UNITY_ANDROID
        // Check if the image file exists
        if (!File.Exists(imagePath))
        {
            Debug.LogError("Image file not found at path: " + imagePath);
            return;
        }

        // Access the Android activity
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        // Prepare the wallpaper setting
        AndroidJavaObject wallpaperManager = currentActivity.Call<AndroidJavaObject>("getSystemService", "wallpaper");

        // Load the image as a Bitmap
        AndroidJavaClass bitmapFactory = new AndroidJavaClass("android.graphics.BitmapFactory");
        AndroidJavaObject bitmap = bitmapFactory.CallStatic<AndroidJavaObject>("decodeFile", imagePath);

        if (bitmap == null)
        {
            Debug.LogError("Failed to decode the image file.");
            return;
        }

        // Set the wallpaper
        wallpaperManager.Call("setBitmap", bitmap);
        Debug.Log("Wallpaper set successfully!");
#else
Debug.LogWarning("Can only change wallpaper for Android devices");
#endif 
    }
}
