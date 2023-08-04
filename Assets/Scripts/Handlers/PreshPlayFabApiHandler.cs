using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.EconomyModels;
using System;


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
        var request =new LoginWithCustomIDRequest{
            CreateAccount = true,
            CustomId = "#req45",
            TitleId= "10F24",
        };

        PlayFabClientAPI.LoginWithCustomID(request, onLoginSuccess, onError);
    }

    private void onLoginSuccess(LoginResult obj)
    {
        print("user Auth success");
        Invoke(nameof(AddInventoryItem), 5);
    }

    //Adds Iventory Item
    void AddInventoryItem()
    {
        var request = new AddInventoryItemsRequest
        {
            Amount = 10,
            Entity =new PlayFab.EconomyModels.EntityKey 
            { 
                Id = "362BF6719D3C0236",
                Type= "title_player_account",
            },
            Item =new InventoryItemReference
            {
                Id= "87203d93-c228-44b8-b81b-8d278cdcda9e",
                StackId="default"
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
        print(error);
    }

    private void onError(PlayFabError error)
    {
        Debug.Log(error.ErrorMessage);
    }
}
