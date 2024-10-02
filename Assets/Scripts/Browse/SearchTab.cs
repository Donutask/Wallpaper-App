using UnityEngine;
using TMPro;

public class SearchTab : ScreenTab
{
    [SerializeField] TMP_InputField searchBox;
    [SerializeField] GameObject exitSearchButton;
    [SerializeField] GameObject searchContainer;

    string previousSearch;

    public override void OnOpened()
    {
        base.OnOpened();
        searchContainer.SetActive(true);
        exitSearchButton.SetActive(false);

        CardsManager.Instance.DestroyCards();
    }

    public override void OnClosed()
    {
        base.OnClosed();
        searchContainer.SetActive(false);
        exitSearchButton.SetActive(false);
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

        await CardsManager.Instance.Search(query);

        exitSearchButton.SetActive(true);
    }
}
