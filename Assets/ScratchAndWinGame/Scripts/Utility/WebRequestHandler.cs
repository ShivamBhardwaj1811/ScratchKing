using System.Collections;
using System.Text;

using UnityEngine;
using UnityEngine.Networking;

public static class WebRequestHandler
{

    /// <summary>
    /// The content received from the api
    /// </summary>
    public static string ReceivedContent = null;

    /// <summary>
    /// Sends the request to the specific api gets the user request
    /// </summary>
    /// <typeparam name="T"> The content to send to the api </typeparam>
    /// <param name="Url"></param>
    /// <param name="content"></param>
    /// <param name="token"> The token to add to the request to validate user </param>
    /// <returns></returns>
    public static IEnumerator PostRequest<T>(string Url, T content, string token = null, bool checkInternet = true)
    {
        if (checkInternet)
        {
            if (!InternetConnectionManager.instance.checkInternetConnection())
                yield break;
        }

        ReceivedContent = null;
        string text;
        //If content is null then return
        if (content != null)
            text = JsonUtility.ToJson(content);
        else
            text = "";
        UploadHandler uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(text));
        DownloadHandler downloadHandler = new DownloadHandlerBuffer();
        UnityWebRequest webRequest = new UnityWebRequest(Url, "POST", downloadHandler, uploadHandler);
        webRequest.SetRequestHeader("Content-Type", "application/json");
        if (token != null)
            webRequest.SetRequestHeader("Authorization", $"Bearer {token}");
        webRequest.certificateHandler = new ApiCertificateHandler();
        //waiting to send and receive the request
        yield return webRequest.SendWebRequest();
        //Getting the api response
        ReceivedContent = webRequest.downloadHandler.text;
    }

    /// <summary>
    /// Sends a get request to the api
    /// </summary>
    /// <typeparam name="T"> The content to send to the api </typeparam>
    /// <param name="Url"></param>
    /// <param name="content"></param>
    /// <param name="token"> The token to add to the request to validate user </param>
    /// <returns></returns>
    public static IEnumerator GetRequest<T>(string Url, T content, string token = null, bool checkInternet = true)
    {
        if (checkInternet)
        {
            if (!InternetConnectionManager.instance.checkInternetConnection())
                yield break;
        }
        ReceivedContent = null;
        string text = "";
        UploadHandler uploadHandler = null;
        UnityWebRequest webRequest;
        DownloadHandler downloadHandler = new DownloadHandlerBuffer();
        if (content != null)
        {
            text = JsonUtility.ToJson(content);
            uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(text));
            webRequest = new UnityWebRequest(Url, "GET", downloadHandler, uploadHandler);
        }
        else
            webRequest = UnityWebRequest.Get(Url);
        webRequest.SetRequestHeader("Content-Type", "application/json");
        if (token != null)
            webRequest.SetRequestHeader("Authorization", $"Bearer {token}");
        webRequest.certificateHandler = new ApiCertificateHandler();
        //waiting to send and receive the request
        yield return webRequest.SendWebRequest();
        //Getting the api response
        ReceivedContent = webRequest.downloadHandler.text;
    }

    /// <summary>
    /// Sends a get request to the geocoding api of the google
    /// </summary>
    /// <param name="Url"></param>
    /// <param name="checkInternet"> Checks internet connection before sending api request </param>
    /// <returns></returns>
    public static IEnumerator GetGeocoderData(string Url, float lattitude, float longitude, string key, bool checkInternet = true)
    {
        if (checkInternet)
        {
            if (!InternetConnectionManager.instance.checkInternetConnection())
                yield break;
        }
        ReceivedContent = null;
        string text = "";
        UploadHandler uploadHandler = null;
        UnityWebRequest webRequest;
        DownloadHandler downloadHandler = new DownloadHandlerBuffer();
        Url += $"?latlng={lattitude},{longitude}&key={key}";
        webRequest = UnityWebRequest.Get(Url);
        webRequest.SetRequestHeader("Content-Type", "application/json");
        webRequest.certificateHandler = new ApiCertificateHandler();
        //waiting to send and receive the request
        yield return webRequest.SendWebRequest();
        //Getting the api response
        ReceivedContent = webRequest.downloadHandler.text;
    }

    /// <summary>
    /// Returns the response casted to the specific type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T Response<T>()
    {
        if (ReceivedContent == null)
            return default(T);
        T obj;
        try
        {
            obj = JsonUtility.FromJson<T>(ReceivedContent);
        }
        catch
        {
            obj = default(T);
        }
        return obj;
    }


}
