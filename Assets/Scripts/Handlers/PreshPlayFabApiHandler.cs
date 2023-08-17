using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.EconomyModels;
using Newtonsoft.Json;
using GameModels;

public class PreshPlayFabApiHandler : MonoBehaviour
{
    void Start()
    {
        AutoLogin();
    }
    void AutoLogin()
    {
        var request = new LoginWithCustomIDRequest
        {
            CreateAccount = true,
            CustomId = "#req45",
            TitleId = "10F24",
        };

        PlayFabClientAPI.LoginWithCustomID(request, onLoginSuccess, onError);
    }

    private void onLoginSuccess(LoginResult obj)
    {
        GetPlayerInventoryItems();
        GetStoreItems();
    }

    void GetPlayerInventoryItems()
    {
        var request = new GetInventoryItemsRequest
        {
            CollectionId = "default",
            Filter = "Type eq 'catalogItem'",
            Entity = new PlayFab.EconomyModels.EntityKey
            {
                Id = "362BF6719D3C0236",
                Type = "title_player_account",
            }
        };
        PlayFabEconomyAPI.GetInventoryItems(request, OnGetInventorySuccess, onError);
    }

    private void OnGetInventorySuccess(GetInventoryItemsResponse result)
    {
        for (int i = 0; i < result.Items.Count; i++)
        {
            var item = result.Items[i];
            GameManager.Instance.UpdateInventoryItems(item);
        }
    }

    private void GetStoreItems()
    {
        var request = new SearchItemsRequest
        {
            Filter = "Id eq '1ea5d018-b9a8-4f9a-ba69-9a73935b3457'"
        };
        PlayFabEconomyAPI.SearchItems(request, OnStoreItemsRetrieved, onError);
    }

    private void OnStoreItemsRetrieved(SearchItemsResponse result)
    {
        for (int i = 0; i < result.Items[0].ItemReferences.Count; i++)
        {
            var itemRef = result.Items[0].ItemReferences[i];
            UpdateGameStoreItem(itemRef.Id);
        }
    }

    public void UpdateGameStoreItem(string id)
    {
        var request = new GetItemRequest
        {
            Id = id
        };
        PlayFabEconomyAPI.GetItem(request, (GetItemResponse result) =>
        {
            GameManager.Instance.UpdateStoreItems(result.Item);
        }, onError);
    }

    public void PurchaseGameItem(string id, float amount, GameCurrency currency)
    {

        PurchaseInventoryItemsRequest purchaseRequest = new()
        {
            // AuthenticationContext = gameAuthContext,
            Amount = (int?)amount,

            Item = new InventoryItemReference
            {
                Id = id,
            },
            Entity = new PlayFab.EconomyModels.EntityKey
            {
                Id = "362BF6719D3C0236",
                Type = "title_player_account",
            },
            PriceAmounts = new List<PurchasePriceAmount>{
                new PurchasePriceAmount{
                    ItemId="a1188a84-e59a-45c5-9c8c-82bf426b3eae",
                    Amount= (int)currency.Amount
                }
            }
        };
        PlayFabEconomyAPI.PurchaseInventoryItems(purchaseRequest,
            (PurchaseInventoryItemsResponse successData) =>
            {
                GetPlayerInventoryItems();
            }, onError);
    }
    private void onError(PlayFabError error)
    {
        Debug.Log(error);
    }

    public void RedeemCouponItem()
    {
        var request = new RedeemCouponRequest
        {
            
        };

        PlayFabClientAPI.RedeemCoupon(request, (RedeemCouponResult result) =>
        {
            print(result.GrantedItems[0].DisplayName);
        }, onError);
    }
}