using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectionCard : MonoBehaviour, Card<Collection>
{
    [SerializeField] Image collectionImage;
    [SerializeField] TextMeshProUGUI collectionName;
    [SerializeField] AspectRatioFitter aspectRatioFitter;
    [SerializeField] Button button;

    public async void ApplyCardValues(Collection collection)
    {
        collectionName.text = collection.title;

        Sprite s = await collection.thumbnail.Get();
        if (s != null)
        {
            collectionImage.sprite = s;
            aspectRatioFitter.aspectRatio = (float)s.texture.width / s.texture.height;
        }

        button.onClick.AddListener(() => CollectionsTab.Instance.ShowCollection(collection));
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
