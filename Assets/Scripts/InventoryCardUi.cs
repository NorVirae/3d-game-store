using GameModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCardUi : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI ItemName;
    [SerializeField] TextMeshProUGUI ItemAmount;
    [SerializeField] Image ItemImage;

    int Index;

    void Start()
    {

    }

    public void UpdateInventoryItem(GameItem item, int index)
    {
        // ItemImage.sprite = item.ItemImage;
        ItemAmount.text = item.GameItemAmount.ToString();
        ItemName.text = item.GameItemName;
        Index = index;
    }
}
