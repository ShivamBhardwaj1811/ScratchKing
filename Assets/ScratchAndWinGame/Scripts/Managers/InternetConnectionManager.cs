using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternetConnectionManager : Singleton<InternetConnectionManager>
{
    #region Private Members

    [Header("Popup")]
    [SerializeField]
    GameObject PopupBox;
    [SerializeField]
    AnimClipPlayer AnimIn;
    [SerializeField]
    AnimClipPlayer AnimOut;
    [SerializeField]
    AnimClipPlayer Idle;


    #endregion

    #region Private Methods

    //Awake method is called before any start method is called
    private void Awake()
    {
        Initialize(this);
    }

    /// <summary>
    /// Performs the sequence to close the popup
    /// </summary>
    /// <returns></returns>
    private IEnumerator closeSequence()
    {
        yield return AnimOut.Play();
        PopupBox.SetActive(false);
    }

    /// <summary>
    /// Tells whether application is connected to internet or not
    /// </summary>
    /// <returns></returns>
    private bool isConnectedToInternet()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
            return false;
        else
            return true;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Handles the close button request
    /// </summary>
    public void refreshButtonHandler()
    {
        StartCoroutine(Idle.Play());
        if(isConnectedToInternet())
            StartCoroutine(closeSequence());
        Idle.StopPlayback();
    }

    /// <summary>
    /// Checks whether internet connection is available or not
    /// </summary>
    /// <returns></returns>
    public bool checkInternetConnection()
    {
        if (!isConnectedToInternet())
        {
            PopupBox.SetActive(true);
            StartCoroutine(AnimIn.Play());
            return false;
        }
        return true;
    }

    #endregion
}
