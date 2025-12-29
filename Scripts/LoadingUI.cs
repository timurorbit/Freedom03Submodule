using UnityEngine;
using UnityEngine.UI;
using TMPro;  // If using TextMeshPro

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TMP_Text percentageText;  // Or Text if non-TMP

    public void UpdateProgress(float progress)
    {
        progressSlider.value = progress;
        int percent = Mathf.RoundToInt(progress * 100);
    }

    public void Hide() => gameObject.SetActive(false);
}