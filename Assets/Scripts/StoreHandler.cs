using GameModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoreHandler : MonoBehaviour
{
    [SerializeField] PurchaseItemUi purchasePanel;

    [SerializeField] GameObject storeItemPrefab;

    [SerializeField] Transform storeCardItemsContainer;

    GameStoreItem[] storeItems
    {
        get
        {
            if (GameManager.Instance == null) return null;
            return GameManager.Instance.StoreItems.ToArray();
        }
    }

    public static StoreHandler Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        purchasePanel.gameObject.SetActive(false);
        Invoke(nameof(updateStoreItems), (storeItems.Length > 0) ? 0 : 10);
    }

    public void OnInventoryButtonClicked()
    {
        SceneManager.LoadScene("Inventory");
    }
    public void UpdateAndDisplayPurchasePanel(int index)
    {
        purchasePanel.gameObject.SetActive(true);
        purchasePanel.UpdatePurchasePanel(storeItems[index]);
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
        foreach (Transform item in storeCardItemsContainer)
        {
            item.gameObject.SetActive(false);
        }
        print(storeItems.Length);
        for (int i = 0; i < storeItems.Length; i++)
        {
            if (i < storeCardItemsContainer.childCount)
            {
                storeCardItemsContainer.GetChild(i).gameObject.SetActive(true);

                storeCardItemsContainer.GetChild(i).GetComponent<StoreCardItemUi>()
                    .UpdateCardItem(storeItems[i], i);
            }
            else
            {
                StoreCardItemUi storeCard = Instantiate(storeItemPrefab).GetComponent<StoreCardItemUi>();
                storeCard.UpdateCardItem(storeItems[i], i);
            }
            //storeCard
        }
    }


}
