using GameModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryHandler : MonoBehaviour
{
    [SerializeField] GameObject InventoryCardPrefab;
    [SerializeField] Transform inventoryCardItemsContainer;

    GameItem[] InventoryItems
    {
        get
        {
            if (GameManager.Instance == null) return null;
            return GameManager.Instance.InvenoryItems.ToArray();
        }
    }

    void Start()
    {
        Invoke(nameof(UpdateInventoryItems), (InventoryItems.Length > 0) ? 0 : 7);
    
    }

    public void OnStoreClicked()
    {
        SceneManager.LoadScene("InGameStore");
    }

    void UpdateInventoryItems()
    {
        foreach (Transform item in inventoryCardItemsContainer)
        {
            item.gameObject.SetActive(false);
        }


        for (int i = 0; i < InventoryItems.Length; i++)
        {
            if (i < inventoryCardItemsContainer.childCount)
            {
                inventoryCardItemsContainer.GetChild(i).gameObject.SetActive(true);

                inventoryCardItemsContainer.GetChild(i).GetComponent<InventoryCardUi>()
                    .UpdateInventoryItem(InventoryItems[i], i);
            }
            else
            {
                InventoryCardUi storeCard = Instantiate(InventoryCardPrefab).GetComponent<InventoryCardUi>();
                storeCard.UpdateInventoryItem(InventoryItems[i], i);
            }
        }
    }
}
