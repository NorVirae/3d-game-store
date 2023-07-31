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

public class PlayApiHandler : MonoBehaviour
{
    private static PlayFabAuthenticationContext gameAuthContext;
    private static PlayFab.EconomyModels.EntityKey gameEntityKey = new()
    {
        Type = "title",
        Id = PlayFabSettings.staticSettings.TitleId
    };

    private void Start()
    {
        PlayFabEconomyInit();
    }
    // ENABLE_PLAYFABSERVER_API symbol denotes this is an admin-level game server and not a game client.
    private static async Task PlayFabEconomyInit()
    {
        #if ENABLE_PLAYFABSERVER_API
            string systemGUID = Environment.GetEnvironmentVariable("SYSTEM_GUID", EnvironmentVariableTarget.Process);
            PlayFabSettings.staticSettings.DeveloperSecretKey =
                Environment.GetEnvironmentVariable("PLAYFAB_SECRET_KEY", EnvironmentVariableTarget.Process);

        #endif
        PlayFabSettings.staticSettings.TitleId =
            Environment.GetEnvironmentVariable("PLAYFAB_TITLE_ID", EnvironmentVariableTarget.Process);
        PlayFab.EconomyModels.EntityKey gameEntityKey = new()
        {
            Type = "title",
            Id = PlayFabSettings.staticSettings.TitleId
        };

        try
        {
            PlayFabAuthenticationAPI.GetEntityToken(
                new GetEntityTokenRequest()
                {
                    CustomTags = new Dictionary<string, string>
                    {
                        #if ENABLE_PLAYFABSERVER_API
                                        { "server", systemGUID }
                        #endif
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
                (PlayFabError) =>
                {

                }
            );
            
        }
        catch (Exception e)
        {
            Console.WriteLine(string.Format("PlayFab Auth Error: {0}", e));
            return;
        }

    }
    public static async Task PublishGameItem()
    {
        // Continued from above example...
        CreateDraftItemRequest gameFireItem = new()
        {
            AuthenticationContext = gameAuthContext,
            Item = new CatalogItem
            {
                CreatorEntity = gameEntityKey,
                Type = "catalogItem",
                ContentType = "gameitem",
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
            Console.WriteLine(string.Format("PlayFab CreateDraftItem Error: {0}", e));
            return;
        }
       
    }

    public static async Task PurchaseGameItem()
    {
        // Continued from above example...
        PurchaseInventoryItemsRequest purchaseRequest = new()
        {
            AuthenticationContext = gameAuthContext,
            Item = new InventoryItemReference
            {
                Id = Guid.NewGuid().ToString(),
            },
            CustomTags = new Dictionary<string, string>
            {
                { "server", Guid.NewGuid().ToString() }
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
            Console.WriteLine(string.Format("PlayFab CreateDraftItem Error: {0}", e));
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
}
