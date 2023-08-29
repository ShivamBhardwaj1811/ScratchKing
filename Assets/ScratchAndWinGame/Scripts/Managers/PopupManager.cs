using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class PopupManager : Singleton<PopupManager>
{

    #region Private Members

    /// <summary>
    /// Provides the state of the popup
    /// </summary>
    public bool isDisplayed;

    /// <summary>
    /// The popup to display
    /// </summary>
    [SerializeField]
    private GameObject Popup;

    [Header("Text Boxes")]
    [SerializeField]
    private TextMeshProUGUI Heading;
    [SerializeField]
    private TextMeshProUGUI Body;

    [Header("Popup Animations")]
    [SerializeField]
    private AnimClipPlayer AnimIn;
    [SerializeField]
    private AnimClipPlayer AnimOut;

    #endregion

    #region Private Methods

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Displays the error message with the provided heading and message
    /// </summary>
    /// <param name="heading"></param>
    /// <param name="message"></param>
    public void DisplayMessage(string heading, string message)
    {
        Heading.text = heading;
        Body.text = message;
        isDisplayed = true;
        Popup.SetActive(true);
        StartCoroutine(AnimIn.Play());
    }

    /// <summary>
    /// Closes the popup box
    /// </summary>
    public void CloseButtonHandler()
    {
        StartCoroutine(closeSequence());
    }

    private IEnumerator closeSequence()
    {
        yield return AnimOut.Play();
        isDisplayed = false;
        Popup.SetActive(false);
    }

    #endregion
}
