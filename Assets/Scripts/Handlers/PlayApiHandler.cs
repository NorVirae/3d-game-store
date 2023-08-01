using PlayFab.AuthenticationModels;
using PlayFab.SharedModels;
using PlayFab;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using PlayFab.EconomyModels;
using System.Xml;
using PlayFab.ClientModels;

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
        GuestLogin();
        

    }
    // ENABLE_PLAYFABSERVER_API symbol denotes this is an admin-level game server and not a game client.
    private static async Task PlayFabEconomyInit()
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
    public static async Task PublishGameItem()
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
        PlayFabResult<CreateDraftItemResponse> gameDraftItemResponse = null;
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

    public static async Task PurchaseGameItem()
    {
        // Continued from above example...
        PurchaseInventoryItemsRequest purchaseRequest = new()
        {
            AuthenticationContext = gameAuthContext,
            Amount = 2,
            Item = new InventoryItemReference
            {
                Id = "0f25236c-35f2-4696-9bf7-eef5d79ae40a",
                AlternateId = new AlternateId
                {
                    Type = "Friendly ID",
                    Value = "yul"
                }
            },

            PriceAmounts = new List<PurchasePriceAmount>{
                new PurchasePriceAmount
                {
                    Amount = 20,
                    ItemId = "a1188a84-e59a-45c5-9c8c-82bf426b3eae"
                }
            }
        };

        try
        {
            PlayFabEconomyAPI.PurchaseInventoryItems(purchaseRequest,
            (PurchaseInventoryItemsResponse successData) => 
            {
                Debug.Log("Item Purchased");

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

    public static async Task GetItemsInventory()
    {
        GetInventoryItemsRequest inventoryItemRequest = new()
        {
            AuthenticationContext = gameAuthContext,
            CustomTags = new Dictionary<string, string>
            {
                { "server", Guid.NewGuid().ToString() }
            }
        };

        PlayFabEconomyAPI.GetInventoryItems(inventoryItemRequest, (GetInventoryItemsResponse successData) =>
        {
            Debug.Log(successData);
        }, (PlayFabError e) =>
        {
            Debug.Log(e.ErrorMessage);
        });
    }

    public static async Task GuestLogin()
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
            PurchaseGameItem();
            GetItemsInventory();
        },
        (PlayFabError e) =>
        {
            Debug.Log("Unable to Login" + e.ErrorMessage);
        }
        );
    }
}
