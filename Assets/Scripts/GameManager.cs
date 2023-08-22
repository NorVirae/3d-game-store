using System.Collections;
using System.Collections.Generic;
using GameModels;
using Newtonsoft.Json;
using PlayFab.EconomyModels;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public List<GameItem> InvenoryItems;

    public List<GameStoreItem> StoreItems;


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
                GameItemPrice = new GameCurrency
                {
                    // CurrencyName= catItem.PriceOptions.Prices[0].Amounts[0].
                    currencyId = catItem.PriceOptions.Prices[0].Amounts[0].ItemId,
                    Amount = catItem.PriceOptions.Prices[0].Amounts[0].Amount
                },
                // ItemImage = catItem.Images[0];
            };
            StoreItems.Add(item);
        }
    }

    public void UpdateInventoryItems(InventoryItem inventoryItem)
    {
        {
            if (InvenoryItems.Exists((item) => item.GameItemId == inventoryItem.Id))
            {
                InvenoryItems.Find((item) => item.GameItemId == inventoryItem.Id).GameItemAmount = (float)inventoryItem.Amount;
                return;
            }
            
            print(JsonConvert.SerializeObject(inventoryItem));
            
            GameItem item = new GameItem
            {
                GameItemId = inventoryItem.Id,
                GameItemName = inventoryItem.Id,
                // GameItemDescription = catItem.Description.GetValueOrDefault("NEUTRAL"),
                GameItemAmount = (float)inventoryItem.Amount,
                // ItemImage = catItem.Images[0];
            };
            InvenoryItems.Add(item);
        }
    }
    public void PurchaseStoreItem(GameStoreItem storeItem)
    {
        FindFirstObjectByType<PreshPlayFabApiHandler>().PurchaseGameItem(storeItem.GameItemId, storeItem.GameItemAmount, storeItem.GameItemPrice);
    }

}
