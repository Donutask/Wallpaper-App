using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;

public class CollectionsTab : ScreenTab
{
    public static CollectionsTab Instance;
    [SerializeField] GameObject collectionHeader;
    [SerializeField] LayoutElement collectionHeaderLayout;
    [SerializeField] GameObject backButton;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] LocalizedString defaultTitle, defaultDescription;
    [SerializeField] GameObject loadingIndicator;
    static CollectionPage currentCollectionPage;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowCollection(Collection collection)
    {
        loadingIndicator.SetActive(true);

        collectionHeader.SetActive(true);
        backButton.SetActive(true);
        title.text = collection.title;

        if (string.IsNullOrWhiteSpace(collection.description))
        {
            description.gameObject.SetActive(false);
            collectionHeaderLayout.minHeight = 150;
        }
        else
        {
            description.gameObject.SetActive(true);
            description.text = collection.description;
            collectionHeaderLayout.minHeight = 200;
        }

        CardsManager.Instance.ChangeGrid(2);
        CardsManager.Instance.DestroyCards();
        CardsManager.Instance.ShowScreen(collection.wallpapers, destroyPrevious: true);
        CardsManager.Instance.SetLoadMoreAction(CardsManager.Instance.LoadMoreWallpapers);

        loadingIndicator.SetActive(false);
    }

    public async void LoadMoreCollections()
    {
        loadingIndicator.SetActive(true);

        ShowCollectionCards(await CardsManager.api.NextCollectionsPage(currentCollectionPage.nextPageURL));
    }

    public override void OnOpened()
    {
        base.OnOpened();
        MainCollectionScreen();
    }

    public override void OnClosed()
    {
        base.OnClosed();

        collectionHeader.SetActive(false);
    }

    public void Back()
    {
        MainCollectionScreen();
    }

    async void MainCollectionScreen()
    {
        loadingIndicator.SetActive(true);

        collectionHeader.SetActive(true);
        backButton.SetActive(false);
        title.text = defaultTitle.GetLocalizedString();
        description.text = defaultDescription.GetLocalizedString();

        CardsManager.Instance.DestroyCards();

        currentCollectionPage = await CardsManager.api.GetCollections();
        ShowCollectionCards(currentCollectionPage);

        CardsManager.Instance.SetLoadMoreAction(LoadMoreCollections);
    }

    void ShowCollectionCards(CollectionPage collections)
    {
        loadingIndicator.SetActive(true);

        currentCollectionPage = collections;

        CardsManager.Instance.ChangeGrid(1);

        if (collections != null)
            CardsManager.Instance.collectionCardManager.CreateCards(collections.content);

        CardsManager.Instance.loadMoreButton.gameObject.SetActive(true);

        loadingIndicator.SetActive(false);
    }

}
