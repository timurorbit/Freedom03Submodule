using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_Dropdown))]
public class DropdownRankOptions : MonoBehaviour
{
    private TMP_Dropdown dropdown;
    
    public event Action<Rank> onRankChanged;
    
    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        PopulateDropdown();
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void OnDestroy()
    {
        if (dropdown != null)
            dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
    }

    private void OnDropdownValueChanged(int value)
    {
        onRankChanged?.Invoke(GetSelectedRank());
    }

    private void PopulateDropdown()
    {
        dropdown.ClearOptions();
        
        var ranks = Enum.GetValues(typeof(Rank))
            .Cast<Rank>()
            .ToArray();

        foreach (Rank rank in ranks)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(rank.ToString()));
        }
        
        // Optional: select first one
        dropdown.value = 0;
        dropdown.RefreshShownValue();
    }
    
    public Rank GetSelectedRank()
    {
        string selectedText = dropdown.options[dropdown.value].text;
        return (Rank)Enum.Parse(typeof(Rank), selectedText);
    }

    public void SetSelectedRank(Rank statsRank)
    {
        int index = dropdown.options.FindIndex(option => option.text == statsRank.ToString());
        if (index >= 0)
        {
            dropdown.value = index;
            dropdown.RefreshShownValue();
        }
    }
}
