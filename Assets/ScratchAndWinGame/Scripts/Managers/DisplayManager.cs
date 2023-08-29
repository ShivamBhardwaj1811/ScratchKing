using System.Collections;

using TMPro;

using UnityEngine;

public class DisplayManager : Singleton<DisplayManager>
{

    #region public Members

    /// <summary>
    /// The privacy policy of the game
    /// </summary>
    public string PrivacyPolicy;

    /// <summary>
    /// The terms of services of the game
    /// </summary>
    public string TOS;

    #region Display Area Members

    /// <summary>
    /// The display area to display privacy policy or Terms of service
    /// </summary>
    [SerializeField]
    private GameObject displayArea;

    [Header("Animations")]
    [SerializeField]
    private AnimClipPlayer AnimIn;
    [SerializeField]
    private AnimClipPlayer AnimOut;

    [Header("Text Fields")]
    [SerializeField]
    private TextMeshProUGUI Header;
    [SerializeField]
    private TextMeshProUGUI Body;

    [Header("Content")]
    [SerializeField]
    private GameObject content;

    #endregion

    #endregion

    #region Private Methods

    /// <summary>
    /// Fires before any start method is called
    /// </summary>
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void resetContent()
    {
        Vector3 position = content.transform.position;
        position.y = 0;
        content.transform.position = position;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Displays the privacy policy 
    /// </summary>
    public void DisplayPrivacyPolicy()
    {
        Header.text = "Privacy Policy";
        Body.text = PrivacyPolicy;
        StartCoroutine(displayAreaPopupSequence());
    }

    private IEnumerator displayAreaPopupSequence()
    {
        displayArea.SetActive(true);
        yield return AnimIn.Play();
        resetContent();
    }

    /// <summary>
    /// Displays terms of service to user
    /// </summary>
    public void DisplayTOS()
    {
        Header.text = "Terms of Service";
        Body.text = TOS;
        StartCoroutine(displayAreaPopupSequence());
    }

    /// <summary>
    /// Handles the close button
    /// </summary>
    public void CloseButtonHandler()
    {
        StartCoroutine(closeButtonSequence());
    }

    /// <summary>
    /// Performs the actions in a sequence to close things up
    /// </summary>
    /// <returns></returns>
    private IEnumerator closeButtonSequence()
    {
        yield return AnimOut.Play();
        displayArea.SetActive(false);
    }

    #endregion

}
