using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScreenManager : Singleton<LoadScreenManager>
{

    #region Private Members

    /// <summary>
    /// The loading screen to display
    /// </summary>
    [SerializeField]
    private GameObject LoadingScreen;

    /// <summary>
    /// The animation to display for the loading process
    /// </summary>
    [SerializeField]
    private AnimClipPlayer LoadingAnimation;

    #endregion

    #region Public Members

    /// <summary>
    /// Tells whether the loading screen is visible or not
    /// </summary>
    private bool _IsVisible = false;

    /// <summary>
    /// Tells whether the loading screen is visible or not
    /// </summary>
    public bool IsVisible => _IsVisible;

    #endregion

    #region Private Methods

    //Awake is fired before any start method is called
    private void Awake()
    {
        Initialize(this);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Displays the loading screen
    /// </summary>
    public void DisplayLoadingScreen()
    {
        _IsVisible = true;
        LoadingScreen.SetActive(true);
        StartCoroutine(LoadingAnimation.Play());
    }

    /// <summary>
    /// Hides the loading screen and stops the loading animation
    /// </summary>
    public void StopLoadingScreen()
    {
        _IsVisible = false;
        LoadingScreen.SetActive(false);
        LoadingAnimation.StopPlayback();
    }

    #endregion

}
