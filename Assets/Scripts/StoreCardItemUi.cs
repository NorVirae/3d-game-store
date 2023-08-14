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

    public void UpdateCardItem(GameStoreItem storeItem, int index)
    {
        storeItemName.text = storeItem.GameItemName;
        storeItemAmount.text = "+" + storeItem.GameItemAmount.ToString();
        storeItemPrice.text = storeItem.GameItemPrice.Amount.ToString() + "Gold";
        // storeItemImage.sprite = storeItem.ItemImage;
        Index = index;
    }

    public void OnStoreItemClicked()
    {
        StoreHandler.Instance.UpdateAndDisplayPurchasePanel(Index);
    }
}
