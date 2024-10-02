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
    public RecycleView<Collection> collectionCardManager;

    public Button loadMoreButton;
    [SerializeField] GameObject loadingIndicator, noAPIError;
    [SerializeField] GridLayoutGroup gridLayout;

    static WallpaperPage currentPage;

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
        await GetWallpapersAndShow(api.GetCuratedWallpapers(), true);

        //API implementation should cache stuff for you
        //if (cached != null)
        //{
        //    //show old results
        //    wallpaperCardManager.CreateCards(cached.content);
        //}
        //else
        //{
        //}
    }

    public void SetLoadMoreAction(UnityEngine.Events.UnityAction action)
    {
        loadMoreButton.onClick.RemoveAllListeners();
        loadMoreButton.onClick.AddListener(action);
    }

    public async void LoadMoreWallpapers()
    {
        await GetWallpapersAndShow(api.NextPage(currentPage.nextPageURL), false);
    }

    public async Task Search(string query, Color32? colour)
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



    async Task GetWallpapersAndShow(Task<WallpaperPage> task, bool destroyPrevious)
    {
        loadingIndicator.SetActive(true);

        ShowScreen(await task, destroyPrevious);

        loadingIndicator.SetActive(false);
    }

    public void ShowScreen(WallpaperPage page, bool destroyPrevious)
    {
        currentPage = page;

        if (destroyPrevious)
        {
            DestroyCards();
        }
        wallpaperCardManager.CreateCards(page.content);

        loadMoreButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Destroys all cards and hides page button
    /// </summary>
    public void DestroyCards()
    {
        loadMoreButton.gameObject.SetActive(false);
        wallpaperCardManager.DestroyCards();
        collectionCardManager.DestroyCards();
    }

    const int cellWidth = 300;
    /// <summary>
    /// Set grid to aspect ratio. 1 is square, 2 is rectangle
    /// </summary>
    /// <param name="aspectRatio"></param>
    public void ChangeGrid(float aspectRatio)
    {
        gridLayout.cellSize = new Vector2(cellWidth, cellWidth * aspectRatio);
    }
}
