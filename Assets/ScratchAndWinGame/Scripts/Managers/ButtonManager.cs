

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using static UnityEngine.UI.Button;

[System.Serializable]
public class OnClick
{
    public string name;
    public Button myButton;
    public ButtonClickedEvent ButtonClick;

    private void OnValidate()
    {
        if (myButton != null) ButtonClick = myButton.onClick;
    }
}
// centralised Button Manager
[ExecuteInEditMode]
public class ButtonManager : Singleton<ButtonManager>
{ 
    public List<OnClick> myButtons = new List<OnClick>();

    private void OnEnable()
    {
        foreach (var button in myButtons)
        {
            button.myButton.onClick = button.ButtonClick;
        }
    }

    private void OnDisable()
    {
        foreach (var button in myButtons)
        {
            button.myButton.onClick = null;
        }
    }

    private void Awake()
    {
        if (instance == null) instance = this;
    }


    #region Private Members

    [SerializeField]
    private GameObject GamePlayArea;

    #endregion

    #region Public Methods

    public void startGameplay()
    {
        GamePlayArea.SetActive(true);
        StartCoroutine(GameManager.instance.UpdateTicketSettings());
    }

    #endregion

    #region Profile Button Handlers

    /// <summary>
    /// Opens the profile page
    /// </summary>
    public void ProfileButtonHandler()
    {
        StartCoroutine(profileManagerLoadSequence());
    }

    private IEnumerator profileManagerLoadSequence()
    {
        GamePlayArea.SetActive(false);
        yield return StartSequenceManager.instance.PlayAnimation();
        ProfileManager.instance.loadProfilePage();
    }

    #endregion

    #region Ad Button Handler

    [Header("Ad Handlers")]
    [SerializeField]
    private GameObject adPopup;

    [SerializeField]
    private AnimClipPlayer AdPopupIn;
    [SerializeField]
    private AnimClipPlayer AdPopupOut;

    /// <summary>
    /// Handles the request from the ad button
    /// </summary>
    public void AdButtonHandler()
    {
        adPopup.SetActive(true);
        StartCoroutine(AdPopupIn.Play());
    }

    /// <summary>
    /// Handles the request for the close button
    /// </summary>
    public void CloseButtonHandler()
    {
        StartCoroutine(closeSequence());
    }

    private IEnumerator closeSequence()
    {
        yield return AdPopupOut.Play();
        adPopup.SetActive(false);
    }

    /// <summary>
    /// Handles the request for the free ticket
    /// </summary>
    public void freeTicketClaimHandler()
    {
        StartCoroutine(freeTicketClaimSequence());
    }

    /// <summary>
    /// Sequence to claim free tickets
    /// </summary>
    /// <returns></returns>
    private IEnumerator freeTicketClaimSequence()
    {
        yield return closeSequence();
        LoadScreenManager.instance.DisplayLoadingScreen();
        yield return WebRequestHandler.GetRequest<string>(ApiPathManager.FreeTicketClaimUrl, null, SaveLoadManager.instance.getToken());
        APIResponse<TicketsResponseApiModel> response = WebRequestHandler.Response<APIResponse<TicketsResponseApiModel>>();
        LoadScreenManager.instance.StopLoadingScreen();
        if (response != null)
        {
            if (response.isOk)
            {
                yield return AdManager.instance.adRewardSequence(response.Response.TicketsCount);
            }
            else
                PopupManager.instance.DisplayMessage("Error", response.Errors);
        }
        else
            PopupManager.instance.DisplayMessage("Server Connection Error", "Unable to connect to the server at the moment. Please try again later.");
    }

    /// <summary>
    /// Handles the request to watch video ad
    /// </summary>
    public void watchVideoAdHandler()
    {
        StartCoroutine(watchVideoAdSequence());
    }

    /// <summary>
    /// Displays the video in a sequence
    /// </summary>
    /// <returns></returns>
    private IEnumerator watchVideoAdSequence()
    {
        yield return closeSequence();
        AdManager.instance.DisplayRewardedAd();
    }

    /// <summary>
    /// Displays the unity rewarded video ad
    /// </summary>
    public void watchUnityRewardedVideoAd()
    {
        StartCoroutine(watchUnityVideoAdSequence());
    }

    /// <summary>
    /// Displays the video in a sequence
    /// </summary>
    /// <returns></returns>
    private IEnumerator watchUnityVideoAdSequence()
    {
        yield return closeSequence();
        UnityAdManager.instance.ShowRewardedVideoAd();
    }

    /// <summary>
    /// Handler for facebook video ad
    /// </summary>
    public void watchFacebookVideoAd()
    {
        StartCoroutine(facebookVideoAdDisplaySequence());
    }
    
    /// <summary>
    /// The sequence for displaying video ad
    /// </summary>
    /// <returns></returns>
    private IEnumerator facebookVideoAdDisplaySequence()
    {
        yield return closeSequence();
        FacebookAdManager.instance.ShowRewardedVideoAd();
    }

    #endregion

    #region Winning List Button

    /// <summary>
    /// Opens the winning page
    /// </summary>
    public void WinningListPageOpen()
    {
        StartCoroutine(WinningPanelLoadSequence());
    }

    private IEnumerator WinningPanelLoadSequence()
    {
        ComponentManager.instance.Gameplay.SetActive(false);
        yield return StartSequenceManager.instance.PlayAnimation();
        StartCoroutine(WinningListManager.instance.WinningPanelLoadSequence());
    }

    #endregion

}
