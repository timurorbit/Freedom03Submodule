using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShopElement : MonoBehaviour
{
    
    [Header("Default")] 
    [SerializeField]
    private GameObject lockedState;
    [SerializeField]
    private GameObject unlockedState;
    [SerializeField]
    private TextMeshProUGUI ReputationText;
    [SerializeField]
    private TextMeshProUGUI PriceText;
    
    [SerializeField]
    private MMF_Player buyFeedback;

    private Button button;
    
    
    
    [Header("Shop element settings")]
    [SerializeField] public ShopElementType shopSlotType;
    [SerializeField] public List<GameObject> correspondingObjectToActivate;
    [SerializeField] public GameObject statModifierToCreateAndAdd;
    [SerializeField] public bool bought;
    [SerializeField] public int reputationRequirement;
    [SerializeField] public int priceRequirement;

    public enum ShopElementType
    {
        ObjectActivate,
        AddPotion,
    }
    
    

    private void Awake()
    {
  //      IconImage.sprite = imageSprite;
        button = GetComponent<Button>();
        button.onClick.AddListener(Buy);
        ReputationText.text = reputationRequirement.ToString();
        PriceText.text = priceRequirement.ToString();
    }

    public void Buy()
    {
        int gold = GuildRepository.Instance.Gold;
        if (gold < priceRequirement)
        {
            //Feedback fail to buy
            return;
        }
        
        GuildRepository.Instance.Gold -= priceRequirement;
        switch (shopSlotType)
        {
            case ShopElementType.ObjectActivate:
                foreach (GameObject obj in correspondingObjectToActivate)
                {
                    obj.SetActive(true);
                    buyFeedback?.PlayFeedbacks();
                }
                bought = true;
                gameObject.SetActive(false);
                break;
            case ShopElementType.AddPotion:
                var slot = GuildRepository.Instance.GetFreeSlotFromShelves();
                if (slot == null)
                {
                    //Feedback no space
                    GuildRepository.Instance.Gold += priceRequirement;
                    return;
                }
                var item = Instantiate(statModifierToCreateAndAdd, GuildRepository.Instance.gameObject.transform);
                GuildRepository.Instance.addPotionToSlot(item, slot);
                buyFeedback?.PlayFeedbacks();
                break;
        }
    }

    private void OnEnable()
    {
        if (bought)
        {
            gameObject.SetActive(false);
            return;
        }
        if (GuildRepository.Instance.Reputation < reputationRequirement)
        {
            lockedState.SetActive(true);
            unlockedState.SetActive(false);
            button.interactable = false;
            
        }
        else
        {
            lockedState.SetActive(false);
            unlockedState.SetActive(true);
            UpdatePriceState();
        }
    }

    public void UpdatePriceState()
    {
        int guildGold = GuildRepository.Instance.Gold;
        if (guildGold >= priceRequirement)
        {
            PriceText.color = Color.white;
            button.interactable = true;
        }
        else
        {
            PriceText.color = Color.red;
            button.interactable = false;
        }
    }
}
