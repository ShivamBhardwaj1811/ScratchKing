using GoogleMobileAds.Api;



using System;
using System.Collections;
using System.Threading.Tasks;

using UnityEditor;

using UnityEngine;

public class AdManager : Singleton<AdManager>
{
    #region Private Members

    [SerializeField]
    private string VideoAdUnavailableError = "No ad available at the moment. Please try again later";

    private RewardBasedVideoAd rewardAd;

    [SerializeField]
    private string rewardBasedVideoAdId;

    #endregion

    #region Private Methods

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    /// <summary>
    /// Load the rewarded ad at the start
    /// </summary>
    [Obsolete]
    private void Start()
    {
        rewardAd = RewardBasedVideoAd.Instance;
        MobileAds.Initialize(init => { });
        rewardAd.OnAdCompleted += RewardAd_OnAdCompleted;
        rewardAd.OnAdFailedToLoad += RewardAd_OnAdFailedToLoad;
        rewardAd.OnAdClosed += RewardAd_OnAdClosed;
        LoadAd();
    }

    private void RewardAd_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        StartCoroutine(adLoadSequence());
    }

    private IEnumerator adLoadSequence()
    {
        yield return new WaitForSeconds(120);
        LoadAd();
    }

    private void RewardAd_OnAdClosed(object sender, System.EventArgs e)
    {
        LoadAd();
    }

    private void RewardAd_OnAdCompleted(object sender, System.EventArgs e)
    {
        StartCoroutine(videoAdRewardSequence());
        LoadAd();
    }

    private void LoadAd()
    {
        AdRequest request = new AdRequest
            .Builder()
            .Build();
        rewardAd.LoadAd(request, rewardBasedVideoAdId);
    }

    private void DisplayVideoAd()
    {
        if (rewardAd.IsLoaded())
        {
            rewardAd.Show();
        }
        else
        {
            PopupManager.instance.DisplayMessage("Video Ad Unavailable", VideoAdUnavailableError);
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Displays the reward based video ad
    /// </summary>
    public void DisplayRewardedAd()
    {
        DisplayVideoAd();
    }

    /// <summary>
    /// The reward sequence to reward tickets to player
    /// </summary>
    /// <param name="ticketsReward"></param>
    /// <returns></returns>
    public IEnumerator adRewardSequence(int ticketsReward)
    {
        bool shouldUpdate = SaveLoadManager.instance.currentUser.Tickets == 0;
        yield return WinPanelManager.instance.ShowWiningPanel(new WinSettings(new SectionPrice()
        {
            Price = ticketsReward,
            probability = 0,
        }, AreaType.Ticket));
        if (shouldUpdate)
        {
            yield return GameManager.instance.UpdateTicketSettings();
        }
    }

    /// <summary>
    /// Performs the sequence to get video ad reward for user
    /// </summary>
    /// <returns></returns>
    public IEnumerator videoAdRewardSequence()
    {
        LoadScreenManager.instance.DisplayLoadingScreen();
        yield return WebRequestHandler.GetRequest<string>(ApiPathManager.VideoAdsUrl, null, SaveLoadManager.instance.getToken());
        //Getting server response
        APIResponse<TicketsResponseApiModel> response = WebRequestHandler.Response<APIResponse<TicketsResponseApiModel>>();
        LoadScreenManager.instance.StopLoadingScreen();
        //In case of error
        if (!response.isOk)
        {
            PopupManager.instance.DisplayMessage("Error", response.Errors);
        }
        else if (response.Message != null && response.Message!="")
        {
            PopupManager.instance.DisplayMessage("Message", response.Message);
        }
        //Rewarding user the tickets count received
        else 
        {
            yield return adRewardSequence(response.Response.TicketsCount);
        }
    }

    #endregion
}
