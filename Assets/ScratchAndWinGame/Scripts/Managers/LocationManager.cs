using System.Collections;

using UnityEngine;
using UnityEngine.Android;

public class LocationManager : Singleton<LocationManager>
{
    [SerializeField]
    private string GeoCodeApiKey = "";

    // Start is called before the first frame update
    void Start()
    {
        Initialize(this);
        #if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
        #endif
    }

    public bool IsLocationEnabled() => Input.location.isEnabledByUser;

    public IEnumerator DetectLocation()
    {
        if (!Input.location.isEnabledByUser)
        {
            yield break;
        }
        Input.location.Start();

        while(Input.location.status == LocationServiceStatus.Initializing)
        {
            yield return new WaitForSeconds(1);
        }

        if(Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Location service failed to start");
            yield break;
        }

        float latitude = Input.location.lastData.latitude;
        float longitude = Input.location.lastData.longitude;

        string url = "https://maps.googleapis.com/maps/api/geocode/json";

        yield return WebRequestHandler.GetGeocoderData(url, latitude, longitude, GeoCodeApiKey);
        string result = WebRequestHandler.ReceivedContent;
    }
}
