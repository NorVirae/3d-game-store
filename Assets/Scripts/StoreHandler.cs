using GameModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoreHandler : MonoBehaviour
{
    [SerializeField] PurchaseItemUi purchasePanel;

    [SerializeField] GameObject storeItemPrefab;



    public static StoreHandler Instance;

    private void Awake()
    {
        Instance = this;
    }
     
    public void OnInventoryButtonClicked()
    {
        SceneManager.LoadScene("Inventory");
    }
    public void UpdateAndDisplayPurchasePanel(StoreItem storeItem)
    {
        purchasePanel.gameObject.SetActive(true);
        //purchasePanel.UpdatePurchasePanel(storeItem);
    }

    public void updateTokenUi()
    {

    }

    public void OnTokenClicked()
    {
        SceneManager.LoadScene("TokenPurchase");
    }

    public void updateStoreItems()
    {

    }
}
