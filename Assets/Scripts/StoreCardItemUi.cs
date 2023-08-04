using GameModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreCardItemUi : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI storeItemName;
    [SerializeField] TextMeshProUGUI storeItemAmount;
    [SerializeField] TextMeshProUGUI storeItemPrice;
    [SerializeField] Image storeItemImage;

    int Index;
    void Start()
    {
        
    }

    public void UpdateCardItem(StoreItem storeItem,int index)
    {
        storeItemName.text = storeItem.Name;
        storeItemAmount.text="+"+storeItem.Amount.ToString();
        storeItemPrice.text=storeItem.price.ToString()+"CC";
        storeItemImage.sprite = storeItem.ItemImage;
        Index = index;
    }

    public void OnStoreItemClicked()
    {
        StoreHandler.Instance.UpdateAndDisplayPurchasePanel(Index);
    }
}
