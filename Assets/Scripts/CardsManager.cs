using UnityEngine;
using System.Threading.Tasks;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class CardsManager : MonoBehaviour
{
    /// <summary>
    /// Assign the api through some other script
    /// </summary>
    public static API api;

    [SerializeField] GameObject template;
    [SerializeField] Transform parent;
    [SerializeField] TMP_InputField searchBox;
    [SerializeField] GameObject exitSearchButton, loadMoreButton, loadingIndicator;
    Page cached;
    string previousSearch;

    private void Start()
    {
        if (api == null)
        {
            Debug.LogError("No API provided. Please implement the interface 'API' and assign it to the CardsManager.api field. This app doesn't do anything if not hooked up to any pictures.");
        }
        ShowCurated();
    }

    async void ShowCurated()
    {
        await GetWallpapersAndShow(api.GetCuratedWallpapers());
    }

    public async void Search()
    {
        string query = searchBox.text;
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

    async Task GetWallpapersAndShow(Task<Page> task)
    {
        loadingIndicator.SetActive(true);

        var wallpapers = await task;
        cached = wallpapers;

        CreateCards(wallpapers.wallpapers);

        loadingIndicator.SetActive(false);
        loadMoreButton.SetActive(true);
    }

    List<WallpaperCard> cards;
    void CreateCards(Wallpaper[] wallpapers)
    {
        if (cards == null)
        {
            cards = new();
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

            cards.Add(card);
        }
    }

    void DestroyCards()
    {
        if (cards != null)
            foreach (var item in cards)
            {
                Destroy(item.gameObject);
            }

        cards = new();
    }
}
