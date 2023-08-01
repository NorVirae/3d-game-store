using UnityEngine;
using PlayFab;

#if UNITY_5_3_OR_NEWER
[CreateAssetMenu(fileName = "PlayFabSharedSettings", menuName = "PlayFab/CreateSharedSettings", order = 1)]
#endif
public class PlayFabSharedSettings : ScriptableObject
{
    public string TitleId = "10F24";

    internal string VerticalName = null;
#if ENABLE_PLAYFABSERVER_API || ENABLE_PLAYFABADMIN_API || UNITY_EDITOR || ENABLE_PLAYFAB_SECRETKEY
    public string DeveloperSecretKey = "Z85MNMYUMKHD8HA66JTIFT1UKSOWGBUWETZABX6CJ7O7UWQDCM";
#endif
    public string ProductionEnvironmentUrl = "";

#if UNITY_2017_2_OR_NEWER
    public WebRequestType RequestType = WebRequestType.UnityWebRequest;
#else
    public WebRequestType RequestType = WebRequestType.UnityWww;
#endif

    public bool DisableDeviceInfo;
    public bool DisableFocusTimeCollection;

    public int RequestTimeout = 2000;
    public bool RequestKeepAlive = true;

    public PlayFabLogLevel LogLevel = PlayFabLogLevel.Warning | PlayFabLogLevel.Error;
    public string LoggerHost = "";
    public int LoggerPort = 0;
    public bool EnableRealTimeLogging = false;
    public int LogCapLimit = 30;
}
