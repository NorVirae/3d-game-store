using GameModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseItemUi : MonoBehaviour
{
    [SerializeField] Image ItemImage;
    [SerializeField] TextMeshProUGUI ItemAmountText;
    [SerializeField] TextMeshProUGUI ItemPriceText;
    [SerializeField] TextMeshProUGUI ItemDescriptionText;
    [SerializeField] TextMeshProUGUI ItemName;

    GameStoreItem GameStoreItem;
    public void UpdatePurchasePanel(GameStoreItem storeItem)
    {
        ItemAmountText.text = "+" + storeItem.GameItemAmount.ToString();
        ItemPriceText.text = storeItem.GameItemPrice.Amount.ToString() + "CC";
        ItemDescriptionText.text = storeItem.GameItemDescription.ToString();
        ItemName.text = storeItem.GameItemName;
        GameStoreItem = storeItem;
        // ItemImage.sprite = storeItem.ItemImage;
    }

    public void OnCancel()
    {
        this.gameObject.SetActive(false);
    }

    public void OnPurchaseItem()
    {
        print("e don start");
        GameManager.Instance.PurchaseStoreItem(GameStoreItem);
    }
}
