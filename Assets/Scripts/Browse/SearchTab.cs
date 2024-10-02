using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class SearchTab : ScreenTab
{
    [SerializeField] TMP_InputField searchBox;
    [SerializeField] GameObject searchContainer;
    [SerializeField] GameObject noSearchIndicator;
    [SerializeField] GameObject colourChooser;
    [SerializeField] ColourChipData[] colours;
    [SerializeField] ColourChips colourChips;
    public static Color32? searchColour;

    string previousSearch;

    private void Start()
    {
        colourChips.CreateCards(colours);
    }

    public override void OnOpened()
    {
        base.OnOpened();
        searchContainer.SetActive(true);

        searchColour = null;
        CardsManager.Instance.DestroyCards();
        CardsManager.Instance.ChangeGrid(2);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(searchContainer);

        noSearchIndicator.SetActive(true);
        colourChooser.SetActive(true);


        CardsManager.Instance.SetLoadMoreAction(CardsManager.Instance.LoadMoreWallpapers);
    }

    public override void OnClosed()
    {
        base.OnClosed();
        searchContainer.SetActive(false);
        noSearchIndicator.SetActive(false);
        colourChooser.SetActive(false);
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
        noSearchIndicator.SetActive(false);

        await CardsManager.Instance.Search(query, searchColour);

        //exitSearchButton.SetActive(true);
        //colourChooser.SetActive(false);
    }
}
