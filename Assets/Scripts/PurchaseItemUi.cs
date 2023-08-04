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

    public void UpdatePurchasePanel(StoreItem storeItem)
    {
        ItemImage.sprite = storeItem.ItemImage;
        ItemAmountText.text="+"+storeItem.Amount.ToString();
        ItemPriceText.text=storeItem.price.ToString();
        ItemDescriptionText.text = storeItem.Description.ToString();
        ItemName.text = storeItem.Name;
    }

    public void OnCancel()
    {
        this.gameObject.SetActive(false);
    }

    public void OnPurChaseItem()
    {
        
    }
}
