using UnityEngine;
using System.Threading.Tasks;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System;

public class CardsManager : MonoBehaviour
{
    /// <summary>
    /// Assign the api through another script
    /// </summary>
    public static API api;
    public static CardsManager Instance;

    [SerializeField] GameObject template;
    [SerializeField] Transform parent;
    [SerializeField] GameObject loadMoreButton, loadingIndicator, noAPIError;
    static WallpaperPage cached;
    List<WallpaperCard> createdCards;

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
            CreateCards(cached.content);
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

    public async Task Search(string query)
    {
        await GetWallpapersAndShow(api.SearchWallpapers(query), true);
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
        CreateCards(wallpapers.content);

        loadingIndicator.SetActive(false);
        loadMoreButton.SetActive(true);
    }

    void CreateCards(Wallpaper[] wallpapers)
    {
        if (createdCards == null)
        {
            createdCards = new();
        }
        foreach (var item in wallpapers)
        {
            if (item == null)
            {
                continue;
            }
            GameObject obj = Instantiate(template, parent);
            WallpaperCard card = obj.GetComponent<WallpaperCard>();

            card.ApplyCardValues(item);

            createdCards.Add(card);
        }
    }

    public void DestroyCards()
    {
        if (createdCards != null)
            foreach (var item in createdCards)
            {
                Destroy(item.gameObject);
            }

        createdCards = new();

        loadMoreButton.SetActive(false);
    }
}
