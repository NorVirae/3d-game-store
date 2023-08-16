using Newtonsoft.Json;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


public class AwsApi : Singleton<AwsApi>
{
    // Start is called before the first frame update
    public IEnumerator RedeemCouponApiCall(string api)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(api))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = api.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + JsonConvert.DeserializeObject(webRequest.downloadHandler.text));
                    break;
            }
        }
    }


    
    
}
