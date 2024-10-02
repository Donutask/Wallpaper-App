using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TabbedScreen : MonoBehaviour
{
    [SerializeField] ScreenTab[] tabs;
    [SerializeField] Color32 selectedTabColour;
    [SerializeField] Color32 deselectedTabColour;

    int previousTab;

    private void Start()
    {
        //Close everything and open home
        foreach (var item in tabs)
        {
            item.OnClosed();
        }
        SelectTab(2);
    }

    public void SelectTab(int t)
    {
        tabs[previousTab].SetTabIconColour(deselectedTabColour);
        tabs[t].SetTabIconColour(selectedTabColour);

        tabs[previousTab].OnClosed();
        tabs[t].OnOpened();

        previousTab = t;
    }
}

