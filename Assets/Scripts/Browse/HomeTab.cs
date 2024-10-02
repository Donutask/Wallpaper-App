using UnityEngine;
using UnityEngine.UI;

public class HomeTab : ScreenTab
{
    public override void OnOpened()
    {
        CardsManager.Instance.ChangeGrid(2f);
        CardsManager.Instance.ShowHome();
    }
}
