using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using System;

public class VoucherCodeHandler : MonoBehaviour
{

    struct ResultData
    {
        public bool Success;
        public string Message;
        public string VoucherCode;
        public int GoldQuantity;
    }
    [SerializeField]
    TMP_InputField voucherInput;

    // void Start()
    // {
    // StartCoroutine(SendJsonRequest());
    // }
    // IEnumerator SendJsonRequest()
    // {
    //     string url = "https://rk297h1zsj.execute-api.eu-north-1.amazonaws.com/Test/%7Bvoucher+%7D";

    //     string jsonRequestBody = "{\"Email\": \"nkwuap@gmail.com\",\"GoldQuanity\":100}";

    //     var requestHeaders = new Dictionary<string, string>();
    //     requestHeaders.Add("Content-Type", "application/json");

    //     using (var request = UnityWebRequest.Post(url, jsonRequestBody))
    //     {
    //         foreach (var header in requestHeaders)
    //         {
    //             request.SetRequestHeader(header.Key, header.Value);
    //         }

    //         byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonRequestBody);
    //         request.uploadHandler = new UploadHandlerRaw(jsonBytes);
    //         request.downloadHandler = new DownloadHandlerBuffer();
    //         yield return request.SendWebRequest();
    //         if (request.result != UnityWebRequest.Result.Success)
    //         {
    //             Debug.LogError("Error: " + request.error);
    //         }
    //         else
    //         {
    //             Debug.Log("Response: " + JsonConvert.DeserializeObject(request.downloadHandler.text));
    //         }
    //     }
    // }

    public void onRedeemCodeClciked()
    {

        StartCoroutine(ConsumeVoucher(voucherInput.text));
    }

    IEnumerator ConsumeVoucher(string vcCode)
    {
        PreshPlayFabApiHandler preshFab = GameObject.Find("PlayfabApi").GetComponent<PreshPlayFabApiHandler>();
        string url = "https://mwpadqpx4m.execute-api.eu-north-1.amazonaws.com/Test/{redeem+}";

        string jsonRequestBody = "{\"VoucherCode\":" + "\"" + vcCode + "\"" + "}";

        var requestHeaders = new Dictionary<string, string>();
        requestHeaders.Add("Content-Type", "application/json");

        using (var request = UnityWebRequest.Post(url, jsonRequestBody))
        {
            foreach (var header in requestHeaders)
            {
                request.SetRequestHeader(header.Key, header.Value);
            }

            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonRequestBody);
            request.uploadHandler = new UploadHandlerRaw(jsonBytes);
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                Debug.Log("Response: " + JsonConvert.DeserializeObject(request.downloadHandler.text));
                ResultData result = JsonConvert.DeserializeObject<ResultData>(request.downloadHandler.text);
                preshFab.AddGold(result.GoldQuantity);
            }
        };
    }
}

