using UnityEngine;
using UnityEngine.UI;

internal class RankButton : MonoBehaviour
{
    [SerializeField] private Image buttonBackground;
    public void setActiveColor()
    {
        buttonBackground.color = new Color(0, 1, 0, 0.25f);
    }

    public void setInactiveColor()
    {
        buttonBackground.color = Color.clear;
    }
}