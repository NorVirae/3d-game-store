using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.EconomyModels;
using PlayFab.Json;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AndroidGooglePlayPurchase : MonoBehaviour, IStoreListener
{
    // Items list, configurable via inspector
    private List<PlayFab.EconomyModels.CatalogItem> Catalog;
    public TextMeshProUGUI balance;
    public Button loginStatus;
    // The Unity Purchasing system
    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;
    // Bootstrap the whole thing
    public void Start()
    {
        loginStatus.GetComponent<UnityEngine.UI.Image>().color = Color.red;

        // Make PlayFab log in
        Login();
    }


    public void ExecuteRedeemCouponApiCall(string url)
    {
        if (AwsApi.Instance != null)
        {
            StartCoroutine(AwsApi.Instance.RedeemCouponApiCall(url));

        }
        else
        {
            Debug.LogError("AwsApi Instance is null.");
        }
    }

    //public void OnGUI()
    //{
    //    // this line just scales the ui up for high-res devices
    //    // comment it out if you find the ui too large.
    //    GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(3, 3, 3));

    //    // if we are not initialized, only draw a message
    //    if (!IsInitialized)
    //    {
    //        GUILayout.Label("initializing iap and logging in...");
    //        return;
    //    }

    //    // draw menu to purchase items
    //    foreach (var item in Catalog)
    //    {
    //        if (GUILayout.Button("buy " + item.Title))
    //        {
    //            // on button click buy a product
    //            BuyProductID(item.Id);
    //        }
    //    }
    //}

    public void BackToStore()
    {
        SceneManager.LoadScene("InGameStore");
    }

    public void FetchPlayerGoldBalance()
    {
        var balanceRequest = new GetInventoryItemsRequest
        {
            Filter = "Type eq 'currency'"
        };
        PlayFabEconomyAPI.GetInventoryItems(balanceRequest, result =>
        {
            Debug.Log(JsonConvert.SerializeObject(result.Items[0].Amount) + " WHOBA");
            if (result.Items != null)
            {
                balance.text = result.Items[0].Amount.ToString();
                Debug.Log(balance.text + " WUTA");
            }
        }, e =>
        {
            Debug.Log(e.ErrorMessage);
        });
    }

    // This is invoked manually on Start to initiate login ops
    private void Login()
    {
        // Login with Android ID
        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
        {

            CreateAccount = true,
            CustomId = "#req45"
            //AndroidDeviceId = SystemInfo.deviceUniqueIdentifier
        }, result =>
        {
            loginStatus.GetComponent<UnityEngine.UI.Image>().color = Color.green;
            Debug.Log("Logged in " + SystemInfo.deviceUniqueIdentifier);
            FetchPlayerGoldBalance();
            // Refresh available items
            RefreshIAPItems();
        }, error =>
        {
            loginStatus.GetComponent<UnityEngine.UI.Image>().color = Color.red;

            Debug.LogError(error.GenerateErrorReport());
        });
    }

    private void RefreshIAPItems()
    {
        var searchItemsRequest = new SearchItemsRequest()
        {
            Filter = "Type eq 'currency'"
        };
        Debug.Log("got in here");
        PlayFabEconomyAPI.SearchItems(searchItemsRequest, result =>
        {
            Catalog = result.Items;
            Debug.Log("Get Catalog");
            // Make UnityIAP initialize
            InitializePurchasing();
        }, error => Debug.LogError(error.GenerateErrorReport()));
    }

    // This is invoked manually on Start to initialize UnityIAP
    public void InitializePurchasing()
    {
        // If IAP is already initialized, return gently
        if (IsInitialized) return;

        // Create a builder for IAP service
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance(
            //AppStore.GooglePlay
            ));
        // Register each item from the catalog
        foreach (var item in Catalog)
        {
            //print(JsonConvert.SerializeObject(item) + " OBJECT");
            builder.AddProduct(item.Id, ProductType.Consumable);
        }

        builder.AddProduct("gold", ProductType.Consumable);
        // Trigger IAP service initialization
        UnityPurchasing.Initialize(this, builder);
    }

    // We are initialized when StoreController and Extensions are set and we are logged in
    public bool IsInitialized
    {
        get
        {
            return m_StoreController != null && Catalog != null;
        }
    }

    // This is automatically invoked automatically when IAP service is initialized
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        print("Initialised");
        m_StoreController = controller;


        //// Purchasing has succeeded initializing. Collect our Purchasing references.
        //// Overall Purchasing system, configured with products for this application.
        //m_StoreController = controller;
        //// Store specific subsystem, for accessing device-specific store features.
        //m_StoreExtensionProvider = extensions;
        //
        //foreach (var product in m_StoreController.products.all)
        //{
        //    Debug.Log(JsonConvert.SerializeObject(product) + " WHOLAGHJD");
        //    m_StoreController.ConfirmPendingPurchase(product);
        //}
    }

    // This is automatically invoked automatically when IAP service failed to initialized
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason: " + error);
    }

    public void AddGoldToUser()
    {
        var AddGoldRequest = new AddInventoryItemsRequest
        {
            Amount = 100,
            Item = new InventoryItemReference
            {
                AlternateId = new AlternateId
                {
                    Type = "FriendlyId",
                    Value = "gold"
                }
            }

        };
        PlayFabEconomyAPI.AddInventoryItems(AddGoldRequest, result =>
        {
            Debug.Log(result + " REUSLT");
            FetchPlayerGoldBalance();
        }, e =>
        {
            Debug.Log(e.ErrorMessage + " Error");
        });
    }

    // This is invoked automatically when successful purchase is ready to be processed
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        // NOTE: this code does not account for purchases that were pending and are
        // delivered on application start.
        // Production code should account for such case:
        // More: https://docs.unity3d.com/ScriptReference/Purchasing.PurchaseProcessingResult.Pending.html

        if (!IsInitialized)
        {
            return PurchaseProcessingResult.Complete;
        }

        // Test edge case where product is unknown
        if (e.purchasedProduct == null)
        {
            Debug.LogWarning("Attempted to process purchase with unknown product. Ignoring");
            return PurchaseProcessingResult.Complete;
        }

        // Test edge case where purchase has no receipt
        if (string.IsNullOrEmpty(e.purchasedProduct.receipt))
        {
            Debug.LogWarning("Attempted to process purchase with no receipt: ignoring");
            return PurchaseProcessingResult.Complete;
        }

        Debug.Log(JsonConvert.SerializeObject(e) + " TUKE");
        AddGoldToUser();

        Debug.Log("Processing transaction: " + e.purchasedProduct.transactionID);

        // Deserialize receipt
        //var googleReceipt = GooglePurchase.FromJson(e.purchasedProduct.receipt);

        // Invoke receipt validation
        // This will not only validate a receipt, but will also grant player corresponding items
        // only if receipt is valid.
        //PlayFabClientAPI.ValidateGooglePlayPurchase(new ValidateGooglePlayPurchaseRequest()
        //{
        //    // Pass in currency code in ISO format
        //    CurrencyCode = e.purchasedProduct.metadata.isoCurrencyCode,
        //    // Convert and set Purchase price
        //    PurchasePrice = (uint)(e.purchasedProduct.metadata.localizedPrice * 100),
        //    // Pass in the receipt
        //    ReceiptJson = googleReceipt.PayloadData.json,
        //    // Pass in the signature
        //    Signature = googleReceipt.PayloadData.signature
        //}, result => Debug.Log("Validation successful!"),
        //   error => Debug.Log("Validation failed: " + error.GenerateErrorReport())
        //);



        return PurchaseProcessingResult.Complete;
    }

    // This is invoked manually to initiate purchase
    public void BuyProductID(string productId)
    {
        // If IAP service has not been initialized, fail hard
        if (!IsInitialized) throw new Exception("IAP Service is not initialized!");

        // Pass in the product id to initiate purchase
        m_StoreController.InitiatePurchase(productId);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("fAILED " + error + " " + message);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureDescription));

    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));

    }
}

// The following classes are used to deserialize JSON results provided by IAP Service
// Please, note that JSON fields are case-sensitive and should remain fields to support Unity Deserialization via JsonUtilities
public class JsonData
{
    // JSON Fields, ! Case-sensitive

    public string orderId;
    public string packageName;
    public string productId;
    public long purchaseTime;
    public int purchaseState;
    public string purchaseToken;
}



public class GooglePurchase
{
    public PayloadData PayloadData;

    // JSON Fields, ! Case-sensitive
    public string Store;
    public string TransactionID;
    public string Payload;

    public static GooglePurchase FromJson(string json)
    {
        var purchase = JsonUtility.FromJson<GooglePurchase>(json);
        purchase.PayloadData = PayloadData.FromJson(purchase.Payload);
        return purchase;
    }

    //Payload


}

public class PayloadData
{
    public JsonData JsonData;

    // JSON Fields, ! Case-sensitive
    public string signature;
    public string json;

    public static PayloadData FromJson(string json)
    {
        var payload = JsonUtility.FromJson<PayloadData>(json);
        payload.JsonData = JsonUtility.FromJson<JsonData>(payload.json);
        return payload;
    }

}