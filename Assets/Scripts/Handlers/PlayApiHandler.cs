using PlayFab.AuthenticationModels;
using PlayFab.SharedModels;
using PlayFab;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using PlayFab.EconomyModels;
using PlayFab.ClientModels;
using Newtonsoft.Json;
using PlayFab.AdminModels;

public class PlayApiHandler : MonoBehaviour
{
    private static PlayFabAuthenticationContext gameAuthContext;
    private static PlayFab.EconomyModels.EntityKey gameEntityKey;

    private void Start()
    {
        gameEntityKey = new()
        {
            Type = "title",
            Id = PlayFabSettings.staticSettings.TitleId
        };
        //GuestLogin();

    }
    // ENABLE_PLAYFABSERVER_API symbol denotes this is an admin-level game server and not a game client.
    private static void PlayFabEconomyInit()
    {

        PlayFabSettings.staticSettings.TitleId = "10F24";
        PlayFab.EconomyModels.EntityKey gameEntityKey = new()
        {
            Type = "title",
            Id = PlayFabSettings.staticSettings.TitleId
        };
        Debug.Log("Wetin");
        try
        {
            PlayFabAuthenticationAPI.GetEntityToken(
                new GetEntityTokenRequest()
                {
                    CustomTags = new Dictionary<string, string>
                    {
                        { "server", Guid.NewGuid().ToString() }
                    }
                },
                (GetEntityTokenResponse) =>
                {
                    Debug.Log(GetEntityTokenResponse.EntityToken);
                    PlayFabAuthenticationContext gameAuthContext = new()
                    {
                        EntityToken = GetEntityTokenResponse.EntityToken,
                    };
                },
                (PlayFabError e) =>
                {
                    Debug.Log(e.ErrorMessage);
                }
            );

        }
        catch (Exception e)
        {
            Debug.Log(string.Format("PlayFab Auth Error: {0}", e));
            return;
        }

    }
    public static void PublishGameItem()
    {
        // Continued from above example...
        CreateDraftItemRequest gameFireItem = new()
        {
            AuthenticationContext = gameAuthContext,
            Item = new PlayFab.EconomyModels.CatalogItem
            {
                CreatorEntity = gameEntityKey,
                Type = "catalogItem",
                ContentType = "Game Time",
                Title = new Dictionary<string, string>
                {
                    { "NEUTRAL", "My Amazing Fire Item" },
                    { "en-us", "My Lit Lit Item" }
                },
                StartDate = DateTime.Now,
                Tags = new List<string>
                {
                  "desert"
                }
            },
            Publish = true,
            CustomTags = new Dictionary<string, string>
        {
            { "server", Guid.NewGuid().ToString() }
        }
        };
        //PlayFabResult<CreateDraftItemResponse> gameDraftItemResponse = null;
        try
        {
            PlayFabEconomyAPI.CreateDraftItem(gameFireItem, (CreateDraftItemResponse successData) =>
            {
                PlayFabEconomyAPI.AddInventoryItems(new AddInventoryItemsRequest { Amount = 50000, CollectionId = "" }, (GetEntityTokenResponse) =>
                {
                    Debug.Log(successData);
                },
                (PlayFabError e) =>
                {
                    Debug.Log(e.ErrorMessage);
                });

            }, (PlayFabError e) =>
            {
                Debug.Log(e.ErrorMessage);
            });


        }
        catch (Exception e)
        {
            Debug.Log(string.Format("PlayFab CreateDraftItem Error: {0}", e));
            return;
        }

    }

    public static void PurchaseGameItem(int amount, PlayFab.EconomyModels.EntityKey entity, List<PurchasePriceAmount> priceAmounts)
    {
        // Continued from above example...
        PurchaseInventoryItemsRequest purchaseRequest = new()
        {
            AuthenticationContext = gameAuthContext,
            Amount = amount,

            //StoreId = "1ea5d018-b9a8-4f9a-ba69-9a73935b3457",
            Item = new InventoryItemReference
            {
                Id = "87203d93-c228-44b8-b81b-8d278cdcda9e"
                //AlternateId = new AlternateId
                //{
                //    Type = "FriendlyId",
                //    Value = "yul"
                // }
            },
            Entity = entity,
            PriceAmounts = priceAmounts
        };

        try
        {
            PlayFabEconomyAPI.PurchaseInventoryItems(purchaseRequest,
            (PurchaseInventoryItemsResponse successData) =>
            {
                Debug.Log("Item Purchased WEEEEEEEEEE");

            }, (PlayFabError e) =>
            {
                Debug.Log(e.ErrorMessage);
            });


        }
        catch (Exception e)
        {
            Debug.Log(string.Format("PlayFab Buy Error: {0}", e));
            return;
        }
    }

    //public static substractfrominventory()
    //{
    //    subtractinventoryitemsrequest subtractinventoryitemsrequest = new()
    //    {
    //        amount = 20,
    //        item = new()
    //        {
    //            alternateid = new()
    //            {
    //                type = "friendlyid",
    //                value = "gold"
    //            }
    //        }
    //    },
    //    playfabeconomyapi.subtractinventoryitems()
    //}

    public static void GetItemsInventory()
    {
        GetInventoryItemsRequest inventoryItemRequest = new()
        {
            AuthenticationContext = gameAuthContext
            //customtags = new dictionary<string, string>
            //{
            //    { "server", guid.newguid().tostring() }
            //}
        };

        PlayFabEconomyAPI.GetInventoryItems(inventoryItemRequest, (GetInventoryItemsResponse successData) =>
        {
            Debug.Log(JsonConvert.SerializeObject(successData) + " CHECK THIS");
            foreach(var item in successData.Items)
            {
                Debug.Log(JsonConvert.SerializeObject(item) + " Callous");
            }
        }, (PlayFabError e) =>
        {
            Debug.Log(e.ErrorMessage);
        });
    }


    public static void GuestLogin()
    {
        LoginWithCustomIDRequest loginWithCustomIDRequest = new()
        {
            CreateAccount = true,
            CustomId = "#req45",
            TitleId = "10F24"
        };



        PlayFabClientAPI.LoginWithCustomID(loginWithCustomIDRequest, (LoginResult result) =>
        {
            Debug.Log("Login successful");
            PlayFabEconomyInit();
            PublishGameItem();
            PurchaseGameItem(1, new PlayFab.EconomyModels.EntityKey
            {
                Type = "title_player_account",
                Id = "362BF6719D3C0236",
            }, new List<PurchasePriceAmount>{
                new PurchasePriceAmount
                {
                    Amount = 1,
                    ItemId = "a1188a84-e59a-45c5-9c8c-82bf426b3eae"
                }
            });
            //FetchApiPolicy();
            GetItemsInventory();

        },
        (PlayFabError e) =>
        {
            Debug.Log("Unable to Login" + e.ErrorMessage);
        }
        );
    }
}
