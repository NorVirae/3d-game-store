using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.EconomyModels;
using System;
using Newtonsoft.Json;
using GameModels;
using System.Threading.Tasks;

public class PreshPlayFabApiHandler : MonoBehaviour
{

    //Get Inventory Item
    //Get Store Item
    // Get Currency
    void Start()
    {
        AutoLogin();
    }
    //Login
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
        // print("user Auth success");
        // Invoke(nameof(AddInventoryItem), 1);
        // Invoke(nameof(GetPlayerInventoryItems), 1);
        GetStoreItems();
        // PurchaseGameItem("87203d93-c228-44b8-b81b-8d278cdcda9e");
    }

    //Adds Iventory Item
    void AddInventoryItem()
    {
        var request = new AddInventoryItemsRequest
        {
            Amount = 10,
            Entity = new PlayFab.EconomyModels.EntityKey
            {
                Id = "362BF6719D3C0236",
                Type = "title_player_account",
            },
            Item = new InventoryItemReference
            {
                Id = "87203d93-c228-44b8-b81b-8d278cdcda9e",
                StackId = "default"
            }
        };
        //print("working now");
        PlayFabEconomyAPI.AddInventoryItems(request, onInventoryItemAdded, OnInventoryError);

    }

    private void onInventoryItemAdded(AddInventoryItemsResponse result)
    {
        print("worked");
    }

    private void OnInventoryError(PlayFabError error)
    {
        print(error.ErrorMessage);
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
        print(JsonConvert.SerializeObject(result));
        for (int i = 0; i < result.Items.Count; i++)
        {
            var item = result.Items[i];
            print(JsonConvert.SerializeObject(item.DisplayProperties));
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

    public void PurchaseGameItem(string id, int amount)
    {

        PurchaseInventoryItemsRequest purchaseRequest = new()
        {
            // AuthenticationContext = gameAuthContext,
            Amount = amount,

            Item = new InventoryItemReference
            {
                Id = id,
                StackId = "default"
            },
            Entity = new PlayFab.EconomyModels.EntityKey
            {
                Id = "362BF6719D3C0236",
                Type = "title_player_account",
            },
            PriceAmounts = new List<PurchasePriceAmount>{
                new PurchasePriceAmount{
                    ItemId=id,
                    StackId="default",
                    Amount=amount
                }
            }
        };
        print(purchaseRequest.Item.Id);
        PlayFabEconomyAPI.PurchaseInventoryItems(purchaseRequest,
            (PurchaseInventoryItemsResponse successData) =>
            {

            }, onError);
    }
    private void onError(PlayFabError error)
    {
        Debug.Log(error.ErrorMessage);
    }

}
