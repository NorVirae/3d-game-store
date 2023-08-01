using GameModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreCardItemUi : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI storeItemName;
    [SerializeField] TextMeshProUGUI storeItemAmount;
    [SerializeField] TextMeshProUGUI storeItemPrice;
    void Start()
    {
        
    }

    void UpdateCardItem(StoreItem storeItem)
    {
        storeItemName.text = storeItem.Name;
        storeItemAmount.text="+"+storeItem.Amount.ToString();
        storeItemPrice.text=storeItemPrice.ToString();
    }

    public void OnStoreItemClicked()
    {
        StoreHandler.Instance.UpdateAndDisplayPurchasePanel(null);
    }
}
