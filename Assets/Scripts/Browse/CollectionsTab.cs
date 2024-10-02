using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;

public class CollectionsTab : ScreenTab
{
    public static CollectionsTab Instance;
    [SerializeField] GameObject collectionHeader;
    [SerializeField] GameObject backButton;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] LocalizedString defaultTitle, defaultDescription;
    [SerializeField] RecycleView<Collection> collectionCardManager;
    [SerializeField] GameObject loadingIndicator;

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
        description.text = collection.description;

        CardsManager.Instance.ChangeGrid(2);
        collectionCardManager.DestroyCards();
        CardsManager.Instance.ShowNewScreen(collection.wallpapers.content);

        loadingIndicator.SetActive(false);
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

        ShowCollectionCards(await CardsManager.api.GetCollections());

        loadingIndicator.SetActive(false);
    }

    void ShowCollectionCards(CollectionPage collections)
    {
        CardsManager.Instance.ChangeGrid(1);
        collectionCardManager.DestroyCards();
        collectionCardManager.CreateCards(collections.content);
    }
}
