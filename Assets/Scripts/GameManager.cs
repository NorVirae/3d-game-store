using System.Collections;
using System.Collections.Generic;
using GameModels;
using PlayFab.EconomyModels;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameItem> InvenoryItems;

    public List<GameStoreItem> StoreItems;

    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateStoreItems(CatalogItem catItem)
    {
        if (catItem != null)
        {
            if (StoreItems.Exists((item) => item.GameItemId == catItem.Id)) return;
            GameStoreItem item = new GameStoreItem
            {
                GameItemId = catItem.Id,
                GameItemName = catItem.Title.GetValueOrDefault("NEUTRAL"),
                GameItemDescription = catItem.Description.GetValueOrDefault("NEUTRAL"),
                GameItemAmount = 10,
                GameItemPrice = catItem.PriceOptions.Prices[0].Amounts[0].Amount,
                // ItemImage = catItem.Images[0];
            };
            StoreItems.Add(item);
        }
    }

    public void PurchaseStoreItem(GameStoreItem storeItem)
    {
        FindFirstObjectByType<PreshPlayFabApiHandler>().PurchaseGameItem(storeItem.GameItemId, storeItem.GameItemAmount);
    }

}
