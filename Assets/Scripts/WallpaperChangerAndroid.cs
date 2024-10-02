using UnityEngine;
using System.IO;
using UnityEngine.Android;

public static class WallpaperChanger
{
    public enum WallpaperTarget
    {
        HomeScreen,
        LockScreen,
        Both
    }

    /// <summary>
    /// Set wallpaper from image file, either for home screen, lock screen, or both
    /// Only works on android 
    /// </summary>
    public static void SetWallpaper(string imagePath, WallpaperTarget target)
    {

        if (Application.isEditor)
        {
            Debug.Log("Attempting to set wallpaper in editor");
        }

        // ChatGPT wrote this; hopefully it works!
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

        // Access WallpaperManager
        AndroidJavaObject wallpaperManager = currentActivity.Call<AndroidJavaObject>("getSystemService", "wallpaper");

        // Load the image as a Bitmap
        AndroidJavaClass bitmapFactory = new AndroidJavaClass("android.graphics.BitmapFactory");
        AndroidJavaObject bitmap = bitmapFactory.CallStatic<AndroidJavaObject>("decodeFile", imagePath);

        if (bitmap == null)
        {
            Debug.LogError("Failed to decode the image file.");
            return;
        }

        // Get WallpaperManager API version for lock screen support (added in Android N)
        AndroidJavaClass buildVersion = new AndroidJavaClass("android.os.Build$VERSION");
        int sdkInt = buildVersion.GetStatic<int>("SDK_INT");

        // Set the wallpaper based on the target
        try
        {
            switch (target)
            {
                case WallpaperTarget.HomeScreen:
                    // Set wallpaper for the home screen only
                    wallpaperManager.Call("setBitmap", bitmap);
                    Debug.Log("Home screen wallpaper set successfully!");
                    break;

                case WallpaperTarget.LockScreen:
                    if (sdkInt >= 24) // Check if Android version supports lock screen wallpaper (API level 24+)
                    {
                        wallpaperManager.Call("setBitmap", bitmap, null, true, 2); // 2 = lock screen
                        Debug.Log("Lock screen wallpaper set successfully!");
                    }
                    else
                    {
                        Debug.LogError("Lock screen wallpaper is not supported on this Android version.");
                    }
                    break;

                case WallpaperTarget.Both:
                    if (sdkInt >= 24) // API level 24+ for lock screen
                    {
                        wallpaperManager.Call("setBitmap", bitmap, null, true, 1); // 1 = home screen
                        wallpaperManager.Call("setBitmap", bitmap, null, true, 2); // 2 = lock screen
                        Debug.Log("Home screen and lock screen wallpaper set successfully!");
                    }
                    else
                    {
                        // Set for home screen only if lock screen isn't supported
                        wallpaperManager.Call("setBitmap", bitmap);
                        Debug.Log("Only home screen wallpaper set (Lock screen not supported on this version).");
                    }
                    break;
            }
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("Error setting wallpaper: " + ex.Message);
        }
#else
Debug.LogWarning("Can only change wallpaper for Android devices");
#endif
    }
}
