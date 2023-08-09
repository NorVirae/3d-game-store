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

    public void UpdatePurchasePanel(GameStoreItem storeItem)
    {
        ItemImage.sprite = storeItem.ItemImage;
        ItemAmountText.text = "+" + storeItem.GameItemAmount.ToString();
        ItemPriceText.text = storeItem.GameItemPrice.ToString();
        ItemDescriptionText.text = storeItem.GameItemDescription.ToString();
        ItemName.text = storeItem.GameItemName;
    }

    public void OnCancel()
    {
        this.gameObject.SetActive(false);
    }

    public void OnPurChaseItem()
    {
        // FindAnyObjectByType<PlayApiHandler>()
    }
}
