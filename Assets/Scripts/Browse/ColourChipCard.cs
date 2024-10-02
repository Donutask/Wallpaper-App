using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using System;
using TMPro;

public class ColourChipCard : MonoBehaviour, Card<ColourChipData>
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] Button button;

    public void ApplyCardValues(ColourChipData item)
    {
        image.color = item.colour;
        label.text = item.colourName.GetLocalizedString();

        label.color = IsDarkColour(item.colour) ? Color.white : Color.black;

        button.onClick.AddListener(() => SearchTab.searchColour = item.colour);
    }

    private static bool IsDarkColour(Color32 colour)
    {
        // Convert byte values (0-255) to float (0-1) for the luminance calculation
        float r = colour.r / 255f;
        float g = colour.g / 255f;
        float b = colour.b / 255f;

        // Use luminance formula to calculate brightness (rec709)
        float luminance = (0.2126f * r + 0.7152f * g + 0.0722f * b);

        // Threshold for determining dark/light background
        return luminance < 0.5f;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}

[Serializable]
public class ColourChipData
{
    public Color32 colour;
    public LocalizedString colourName;
}
