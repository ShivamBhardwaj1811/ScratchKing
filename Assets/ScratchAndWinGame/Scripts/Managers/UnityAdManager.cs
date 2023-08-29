using System.Collections;

using UnityEngine;
using UnityEngine.Advertisements;

public class UnityAdManager : Singleton<UnityAdManager>, IUnityAdsListener
{

    #region Private Members

    /// <summary>
    /// The error message to display when the rewarded video ad is not available
    /// </summary>
    [SerializeField]
    private string AdUnavailableErrorMessage;

    /// <summary>
    /// The app id for android
    /// </summary>
    private string ANDROID_APP_ID = "3976421";

    /// <summary>
    /// The placement for rewarded video ad
    /// </summary>
    private string placement = "rewardedVideo";

    /// <summary>
    /// Indicates whether the game is in test mode or not
    /// </summary>
    [SerializeField]
    private bool testMode = true;

    #endregion

    #region Private Methods

    /// <summary>
    /// Awake is called before any start method is called
    /// </summary>
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(ANDROID_APP_ID, testMode);
    }

    #endregion

    #region Public Action 

    public void OnUnityAdsDidError(string message)
    {
    }

    /// <summary>
    /// Fires when the ad finishes
    /// </summary>
    /// <param name="placementId"></param>
    /// <param name="showResult"></param>
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        //If player has watched the ad then reward the player
        if (placementId == placement && showResult == ShowResult.Finished)
        {
            StartCoroutine(AdManager.instance.videoAdRewardSequence());
        }
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsReady(string placementId)
    {
    }

    #endregion

    #region Button Handlers

    /// <summary>
    /// Displays the rewarded video ad
    /// </summary>
    public void ShowRewardedVideoAd()
    {
        if (SaveLoadManager.instance.canWatchAd())
        {
            //If the advertisement is ready show the advertisement 
            if (Advertisement.IsReady(placement))
            {
                Advertisement.Show(placement);
                SaveLoadManager.instance.watchAd();
            }
            //If the video ad is not available then
            else
            {
                PopupManager.instance.DisplayMessage("Rewarded Video Ad Unavailable", AdUnavailableErrorMessage);
            }
        }
        else
        {
            PopupManager.instance.DisplayMessage("Rewarded Video Ad Unavailable", AdUnavailableErrorMessage);
        }
    }

    #endregion
}
