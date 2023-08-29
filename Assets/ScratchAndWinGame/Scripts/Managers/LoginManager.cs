

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : Singleton<LoginManager>
{
    #region Public Members

    /// <summary>
    /// The panel which is used for logging in
    /// </summary>
    public GameObject LoginPanel;

    #endregion

    #region Private Members

    /// <summary>
    /// The animations to play for gameobject to enter
    /// </summary>
    [SerializeField]
    private List<AnimClipPlayer> AnimIn;

    /// <summary>
    /// The animations to play when leaving login page
    /// </summary>
    [SerializeField]
    private List<AnimClipPlayer> AnimOut;

    /// <summary>
    /// The input fields for login page
    /// </summary>
    [SerializeField]
    private List<InputField> InputFields = new List<InputField>();

    [SerializeField]
    private float TimeBetweenAnimations = 0.15f;

    /// <summary>
    /// The info text to be displayed
    /// </summary>
    [SerializeField]
    private GameObject InfoText;

    #endregion

    #region Private Methods

    private void Awake()
    {
        Initialize(this);
    }

    /// <summary>
    /// Loads privacy policy and 
    /// </summary>
    /// <returns></returns>
    private IEnumerator loadPolicies()
    {
        yield return WebRequestHandler.GetRequest<string>(ApiPathManager.PrivacyPolicyUrl, null, checkInternet: false);
        DisplayManager.instance.PrivacyPolicy = WebRequestHandler.ReceivedContent;
        yield return WebRequestHandler.GetRequest<string>(ApiPathManager.TermsOfServiceUrl, null, checkInternet: false);
        DisplayManager.instance.TOS = WebRequestHandler.ReceivedContent;
    }

    #endregion

    #region Public Coroutines

    /// <summary>
    /// Displays the login page and the enter animations
    /// </summary>
    /// <returns></returns>
    public IEnumerator Login()
    {
        LoginPanel.SetActive(true);
        yield return StartSequenceManager.instance.PlayAnimation();
        InfoText.SetActive(true);
        yield return Utility.runCoroutinesAllAtOnceWaitToFinish(this, AnimIn.convertTo<AnimClipPlayer, IEnumerator>(), TimeBetweenAnimations);

        LoadScreenManager.instance.DisplayLoadingScreen();
        yield return loadPolicies();
        LoadScreenManager.instance.StopLoadingScreen();

        //check if any token exists then call the login button handler
        if (SaveLoadManager.instance.getToken() != null)
            LoginButtonHandler();
    }

    #endregion

    #region Button Handlers

    /// <summary>
    /// Handles the request for the login button
    /// </summary>
    public void LoginButtonHandler()
    {
        StartCoroutine(loginSequence());
    }

    /// <summary>
    /// The sequence to log user in
    /// </summary>
    /// <returns></returns>
    private IEnumerator loginSequence()
    {
        LoadScreenManager.instance.DisplayLoadingScreen();
        bool isTokenValidated = false;
        APIResponse<LoginResponseApiModel> response = new APIResponse<LoginResponseApiModel>();
        if (SaveLoadManager.instance.getToken() != null)
        {
            ApiRequest<string> token = new ApiRequest<string>();
            token.Content = SaveLoadManager.instance.getToken();
            yield return WebRequestHandler.PostRequest<ApiRequest<string>>(ApiPathManager.TokenValidationUrl, token, checkInternet: false);
            response = WebRequestHandler.Response<APIResponse<LoginResponseApiModel>>();
            LoadScreenManager.instance.StopLoadingScreen();

            if (response == null)
            {
                PopupManager.instance.DisplayMessage("Connection Error","Unable to get response from server\nPlease check your internet connection");
                SaveLoadManager.instance.deleteToken();
                yield break;
            }
            if (response.isOk)
            {
                isTokenValidated = true;
            }
            else
            {
                SaveLoadManager.instance.deleteToken();
                yield break;
            }
        }
        //if the token is invalid
        if (!isTokenValidated)
        {
            yield return WebRequestHandler.PostRequest<LoginRequestApiModel>(ApiPathManager.LoginUrl, getLoginModel(), checkInternet: false);
            response = WebRequestHandler.Response<APIResponse<LoginResponseApiModel>>();
            LoadScreenManager.instance.StopLoadingScreen();
            if (response == null)
            {
                PopupManager.instance.DisplayMessage("Connection Error","Unable to get response from server\nPlease check your internet connection");
                yield break;
            }
            //check if the api response if not ok or null then show error screen and return
            else if (!response.isOk)
            {
                //clearing input fields
                InputFields[1].text = "";
                PopupManager.instance.DisplayMessage("Error",response.Errors);
                yield break;
            }

            //If the api response is ok and user has logged in then
            SaveLoadManager.instance.saveToken(response.Response.Token);
        }

        SaveLoadManager.instance.setUser(response.Response.UserData);

        yield return PlayOutAnimations();
        InfoText.SetActive(false);
        clearFields();
        LoginPanel.SetActive(false);
        yield return StartSequenceManager.instance.PlayAnimation();
        ComponentManager.instance.StartGameplay();
    }

    /// <summary>
    /// Gets data from input fields and return the <see cref="LoginRequestApiModel"/>
    /// </summary>
    /// <returns></returns>
    private LoginRequestApiModel getLoginModel()
    {
        return new LoginRequestApiModel()
        {
            UserName = InputFields[0].text,
            Password = InputFields[1].text,
        };
    }

    /// <summary>
    /// Clears data from the fields
    /// </summary>
    private void clearFields()
    {
        foreach (InputField field in InputFields)
            field.text = "";
    }

    /// <summary>
    /// Playes the exit animations for the login page
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayOutAnimations()
    {
        yield return Utility.runCoroutinesAllAtOnceWaitToFinish(this, AnimOut.convertTo<AnimClipPlayer, IEnumerator>(), TimeBetweenAnimations);
    }

    /// <summary>
    /// Handles the signup button requests
    /// </summary>
    public void SignupButtonHandler()
    {
        StartCoroutine(signupPageLoadSequence());
    }

    private IEnumerator signupPageLoadSequence()
    {
        yield return PlayOutAnimations();
        InfoText.SetActive(false);
        clearFields();
        LoginPanel.SetActive(false);
        yield return StartSequenceManager.instance.PlayAnimation();
        StartCoroutine(SignupManager.instance.Signup());
    }

    #endregion

    #region Forgot Handlers

    /// <summary>
    /// Sends a request to the server for username
    /// </summary>
    public void ForgetUsername()
    {
        StartCoroutine(forgotSequence(true));
    }

    public void ForgetPassword()
    {
        StartCoroutine(forgotSequence(false));
    }

    /// <summary>
    /// The sequence to perform when user name forgot link is clicked
    /// </summary>
    /// <returns></returns>
    public IEnumerator forgotSequence(bool isUsername)
    {
        LoadScreenManager.instance.DisplayLoadingScreen();
        yield return WebRequestHandler.PostRequest<ApiRequest<bool>>(ApiPathManager.ForgetUrl, new ApiRequest<bool>() { Content = isUsername }, checkInternet: false);
        APIResponse<bool> response = WebRequestHandler.Response<APIResponse<bool>>();
        LoadScreenManager.instance.StopLoadingScreen();
        if (response == null)
        {
            PopupManager.instance.DisplayMessage("Connection Error", "Unable to get response from server\nPlease check your internet connection");
        }
        else if (response.Response || response.isOk)
        {
            Application.OpenURL(response.Message);
        }
    }

    #endregion

}
