using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


public class AwsApi : Singleton<AwsApi>
{
    // Start is called before the first frame update
    public IEnumerator RedeemCouponApiCall(string api)
    {
        using(UnityWebRequest webRequest = new UnityWebRequest(api))
        {
            yield return webRequest.SendWebRequest();


            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + webRequest.error);

            }
            else
            {
                string responseText;
                if (webRequest.downloadHandler != null)
                {
                    responseText = webRequest.downloadHandler.text;
                    print(responseText);
                }
                else
                {
                    Debug.LogError("DownloadHandler is null.");
                }
            }
        }
    }


    
    
}
