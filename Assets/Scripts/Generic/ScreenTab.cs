using UnityEngine;
using UnityEngine.UI;

public class ScreenTab : MonoBehaviour
{
    [SerializeField] Image tabIcon;
    public virtual void OnOpened() { }
    public virtual void OnClosed() { }

    public void SetTabIconColour(Color32 c)
    {
        tabIcon.color = c;
    }
}
