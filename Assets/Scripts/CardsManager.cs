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

    [SerializeField] GameObject template;
    [SerializeField] Transform parent;
    [SerializeField] TMP_InputField searchBox;
    [SerializeField] GameObject exitSearchButton, loadMoreButton, loadingIndicator, noAPIError;
    static WallpaperPage cached;
    string previousSearch;
    List<WallpaperCard> createdCards;

    private void Start()
    {
        if (api == null)
        {
            noAPIError.SetActive(true);
            throw new System.Exception("No API provided. Please implement the interface 'API' and assign it to the CardsManager.api field. This app doesn't do anything if not hooked up to any pictures.");
        }
        else
        {
            if (cached != null)
            {
                //show old results
                CreateCards(cached.content);
            }
            else
            {
                ShowCurated();
            }
        }
    }

    async void ShowCurated()
    {
        await GetWallpapersAndShow(api.GetCuratedWallpapers());
    }

    public async void Search()
    {
        string query = searchBox.text;
        if (string.IsNullOrWhiteSpace(query))
        {
            return;
        }
        if (query == previousSearch)
        {
            return;
        }
        previousSearch = query;

        DestroyCards();
        await GetWallpapersAndShow(api.SearchWallpapers(query));

        exitSearchButton.SetActive(true);
    }

    public void ExitSearch()
    {
        exitSearchButton.SetActive(false);

        DestroyCards();
        ShowCurated();
    }

    public async void LoadMore()
    {
        await GetWallpapersAndShow(api.NextPage(cached.nextPageURL));
    }

    async Task GetWallpapersAndShow(Task<WallpaperPage> task)
    {
        loadingIndicator.SetActive(true);

        var wallpapers = await task;
        cached = wallpapers;

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

    void DestroyCards()
    {
        if (createdCards != null)
            foreach (var item in createdCards)
            {
                Destroy(item.gameObject);
            }

        createdCards = new();
    }
}
