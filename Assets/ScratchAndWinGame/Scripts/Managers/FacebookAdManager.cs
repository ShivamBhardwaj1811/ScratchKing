using UnityEngine;
//using AudienceNetwork;

public class FacebookAdManager : Singleton<FacebookAdManager>
{
    //#region Private Members

    ///// <summary>
    ///// The rewarded video ad
    ///// </summary>
    //private RewardedVideoAd rewardedVideoAd;

    ///// <summary>
    ///// The error to display when the video ad is unavailable
    ///// </summary>
    //[SerializeField]
    //private string VideoAdUnavailableError = "";

    ///// <summary>
    ///// The placement id for rewarded video ad
    ///// </summary>
    //[SerializeField]
    //private string placementId = "";

    ///// <summary>
    ///// Flag indicating whether the ad has been loaded or not
    ///// </summary>
    //private bool isLoaded = false;

    //#endregion

    #region Private Methods

    //Awake is fired before any start method is called
    private void Awake()
    {
        Initialize(this);
    }

    //private void Start()
    //{
    //    LoadRewardedVideoAd();
    //}

    ///// <summary>
    ///// Loads the rewarded video ad
    ///// </summary>
    //private void LoadRewardedVideoAd()
    //{
    //    //initializing the rewarede video ad
    //    rewardedVideoAd = new RewardedVideoAd(placementId);
    //    rewardedVideoAd.Register(this.gameObject);
    //    this.rewardedVideoAd.RewardedVideoAdDidLoad += RewardedVideoAdDidLoad;
    //    this.rewardedVideoAd.RewardedVideoAdActivityDestroyed += RewardedVideoAdDestroyed;
    //    this.rewardedVideoAd.RewardedVideoAdComplete += rewardedVideoAdComplete;
    //    this.rewardedVideoAd.RewardedVideoAdDidFailWithError += (delegate (string error) { RewardedVideoAdDidFailWithError(error); });
    //    this.rewardedVideoAd.RewardedVideoAdDidClose +=
    //    this.rewardedVideoAd.RewardedVideoAdDidSucceed += rewardedVideoAdSucceeded;
    //    rewardedVideoAd.LoadAd();
    //}

    ///// <summary>
    ///// Fires when the rewarded video ad closes
    ///// </summary>
    //private void RewardedVideoAdDidClose()
    //{
    //    Debug.Log("Ad Closed");
    //}

    ///// <summary>
    ///// Fires when the rewarded video ad is destroyed
    ///// </summary>
    //private void RewardedVideoAdDestroyed()
    //{
    //    rewardedVideoAd.LoadAd();
    //    Debug.Log("Ad Activity Destroyed");
    //}

    ///// <summary>
    ///// Fires when the rewarded video ad loads
    ///// </summary>
    //private void RewardedVideoAdDidLoad()
    //{
    //    isLoaded = true;
    //    Debug.Log("Ad Loaded");
    //}

    ///// <summary>
    ///// Fires when the rewarded video ad fails because of error
    ///// </summary>
    //private void RewardedVideoAdDidFailWithError(string error)
    //{
    //    isLoaded = false;
    //    //Displaying the message
    //    Debug.Log(error);
    //}

    ///// <summary>
    ///// Fires when the rewarded video ad completes
    ///// </summary>
    //private void rewardedVideoAdComplete()
    //{
    //    Debug.Log("Rewarded Video Ad Complete");
    //}

    ///// <summary>
    ///// Fires when the rewarded video ad succeeds
    ///// </summary>
    //private void rewardedVideoAdSucceeded()
    //{
    //    Debug.Log("Ad Succeeded");
    //}

    ///// <summary>
    ///// Shows the rewarded video ad
    ///// </summary>
    //private void showRewardedVideoAd()
    //{
    //    if (isLoaded)
    //    {
    //        rewardedVideoAd.Show();
    //        isLoaded = false;
    //    }
    //    else
    //        PopupManager.instance.DisplayMessage("Video Ad Unavailable", VideoAdUnavailableError);
    //}

    #endregion

    #region Public Methods

    /// <summary>
    /// Displays the facebook video ad
    /// </summary>
    public void ShowRewardedVideoAd()
    {
        //showRewardedVideoAd();
    }

    #endregion
}
