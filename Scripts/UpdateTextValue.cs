using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UpdateTextValue : MonoBehaviour
{
    private TextMeshProUGUI text;
    
    [SerializeField] private Slider slider; // Reference to the Slider component
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private TextMeshProUGUI reputationText;// Reference to the TextMeshPro text (or use UnityEngine.UI.Text if not using TMP)

    private void Awake()
    {
        // Automatically get the Slider if not assigned
        if (slider == null)
        {
            slider = GetComponent<Slider>();
        }

        // Add listener for value changes
        slider.onValueChanged.AddListener(UpdateText);
        
        // Initialize text with starting value
        UpdateText(slider.value);
    }

    private void UpdateText(float value)
    {
        if (valueText != null)
        {
            valueText.text = Mathf.RoundToInt(value).ToString(); // Or format as needed, e.g., value.ToString("F2") for 2 decimal places
        }
    }
}
