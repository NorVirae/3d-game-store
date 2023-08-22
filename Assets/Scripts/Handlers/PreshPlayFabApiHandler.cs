using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.EconomyModels;
using Newtonsoft.Json;
using GameModels;
using UnityEditor.PackageManager;
using System.Threading.Tasks;
using System;

public class PreshPlayFabApiHandler : MonoBehaviour
{

    public event Action onLoginSuccessfull;
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

    // public async Task CheckUserLoginStatus()
    // {
    //     var request = new GetPlayerCombinedInfoRequest
    //     {
    //         PlayFabId = "USER_PLAYFAB_ID", // Replace with the user's PlayFab ID
    //         InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
    //         {
    //             GetPlayerProfile = true
    //         }
    //     };

    //     try
    //     {
    //         var result = await PlayFabClientAPI.GetPlayerCombinedInfoAsync(request);

    //         // Check if the player profile was returned
    //         if (result.InfoResultPayload.PlayerProfile != null)
    //         {
    //             // Player is logged in
    //             Debug.Log("Player is logged in.");
    //         }
    //         else
    //         {
    //             // Player is not logged in
    //             Debug.Log("Player is not logged in.");
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         Debug.LogError("Error checking user login status: " + ex.Message);
    //     }
    // }


    private void onLoginSuccess(LoginResult obj)
    {
        // GetPlayerInventoryItems();
        // GetStoreItems();
        print("Login success");
        onLoginSuccessfull();
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

    public bool CheckLoggin()
    {
        return PlayFabClientAPI.IsClientLoggedIn();
    }

    public void AddGold(float amount, Action callback)
    {

        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            Debug.Log("Please login");
            return;
        }
        var request = new AddInventoryItemsRequest
        {
            Amount = (int?)amount,
            Item = new InventoryItemReference
            {
                Id = "a1188a84-e59a-45c5-9c8c-82bf426b3eae"
            },
            Entity = new PlayFab.EconomyModels.EntityKey
            {
                Id = "362BF6719D3C0236",
                Type = "title_player_account",
            },
        };
        PlayFabEconomyAPI.AddInventoryItems(request, (AddInventoryItemsResponse result) =>
        {
            print(result.CustomData);
            callback();
        }, (PlayFabError err) =>
        {
            print(err);
        });
    }

    public void updatePlayerGoldBalanceUi(Action<string> callback)
    {
        var balanceRequest = new GetInventoryItemsRequest
        {
            Filter = "Type eq 'currency'"
        };
        PlayFabEconomyAPI.GetInventoryItems(balanceRequest, result =>
        {
            if (result.Items != null)
            {
                callback(result.Items[0].Amount.ToString());
            }
        }, e =>
        {
            Debug.Log(e.ErrorMessage);
        });
    }
}