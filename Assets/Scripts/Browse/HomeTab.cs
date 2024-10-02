using UnityEngine;

public class HomeTab : ScreenTab
{
    public override void OnOpened()
    {
        CardsManager.Instance.ShowHome();
    }
}
