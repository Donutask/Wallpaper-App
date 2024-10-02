using UnityEngine;
using System.Threading.Tasks;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.UI;

public class CardsManager : MonoBehaviour
{
    /// <summary>
    /// Assign the api through another script
    /// </summary>
    public static API api;
    public static CardsManager Instance;
    [SerializeField] RecycleView<Wallpaper> wallpaperCardManager;

    [SerializeField] GameObject loadMoreButton, loadingIndicator, noAPIError;
    [SerializeField] GridLayoutGroup gridLayout;

    static WallpaperPage cached;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        if (api == null)
        {
            noAPIError.SetActive(true);
            throw new System.Exception("No API provided. Please implement the interface 'API' and assign it to the CardsManager.api field. This app doesn't do anything if not hooked up to any pictures.");
        }
    }

    public async void ShowHome()
    {
        if (cached != null)
        {
            //show old results
            wallpaperCardManager.CreateCards(cached.content);
        }
        else
        {
            await GetWallpapersAndShow(api.GetCuratedWallpapers(), true);
        }
    }

    public async void LoadMore()
    {
        await GetWallpapersAndShow(api.NextPage(cached.nextPageURL), false);
    }

    public async Task Search(string query, Color32? colour = null)
    {
        if (colour == null)
        {
            await GetWallpapersAndShow(api.SearchWallpapers(query), true);
        }
        else
        {
            await GetWallpapersAndShow(api.SearchWallpapersWithColour(query, colour.Value), true);
        }
    }

    public void ShowNewScreen(Wallpaper[] content)
    {
        DestroyCards();
        wallpaperCardManager.CreateCards(content);
    }

    async Task GetWallpapersAndShow(Task<WallpaperPage> task, bool destroyPrevious)
    {
        loadingIndicator.SetActive(true);

        var wallpapers = await task;
        cached = wallpapers;

        if (destroyPrevious)
        {
            DestroyCards();
        }
        wallpaperCardManager.CreateCards(wallpapers.content);

        loadingIndicator.SetActive(false);
        loadMoreButton.SetActive(true);
    }

    public void DestroyCards()
    {
        loadMoreButton.SetActive(false);
        wallpaperCardManager.DestroyCards();
    }

    const int cellWidth = 300;
    public void ChangeGrid(float aspectRatio)
    {
        gridLayout.cellSize = new Vector2(cellWidth, cellWidth * aspectRatio);
    }
}
