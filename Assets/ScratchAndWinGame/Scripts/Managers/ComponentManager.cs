
using System.Collections;
using System.Security.Cryptography;

using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;

public class ComponentManager : Singleton<ComponentManager>
{
    #region Public Members

    /// <summary>
    /// The gameplay object
    /// </summary>
    public GameObject Gameplay;

    #endregion

    #region Private Members

    /// <summary>
    /// Indicates whether the server error has occurred or not
    /// </summary>
    bool HasServerErrorOccurred = false;

    /// <summary>
    /// Indicates whether the server faliure has recalled server faliure
    /// </summary>
    bool recalledLoadingScreen = false;

    #endregion

    #region Private Methods

    private void Awake()
    {
        Initialize(this);
    }

    /// <summary>
    /// Start is only called once
    /// </summary>
    private void Start()
    {
        StartCoroutine(LoginSignupSequence());
    }

    public IEnumerator LoginSignupSequence()
    {
        #region Variables

        //Flag indicating whether this is first time application logging on this device or not
        bool firstLogin = false;

        //Tells whether the user has signed up successfully or not
        bool signedUp = false;

        //Indicates whether the user has logged in via token
        bool tokenLogin = false;

        #endregion
        SaveLoadManager.instance.LoadClickData();
        Gameplay.SetActive(false);
        yield return StartSequenceManager.instance.PlayAnimation();
        yield return MainMenuManager.instance.waitUntilClicked();
        LoadScreenManager.instance.DisplayLoadingScreen();

        if (!SaveLoadManager.instance.UserExists())
        {
            yield return WebRequestHandler.GetRequest<string>(ApiPathManager.NewUserGenerationUrl, null);
            NewUserInformationModel model = WebRequestHandler.Response<NewUserInformationModel>();

            if (model == null)
            {
                ServerErrorOccurred();
                yield break;
            }

            SaveLoadManager.instance.SaveUserData(model);
            firstLogin = true;
        }
        //Getting the saved user data
        NewUserInformationModel userInformationModel = SaveLoadManager.instance.GetUserData();

        if (firstLogin)
        {
            SignUpRequestApiModel signupModel = new SignUpRequestApiModel()
            {
                Username = userInformationModel.Username,
                Password = userInformationModel.Password
            };
            yield return WebRequestHandler.PostRequest<SignUpRequestApiModel>(ApiPathManager.SignupUrl, signupModel);
            APIResponse<User> user = WebRequestHandler.Response<APIResponse<User>>();

            if (user == null || !user.isOk)
                signedUp = false;
            else
                signedUp = true;
        }

        //Signup fail check
        if (firstLogin && !signedUp)
        {
            ServerErrorOccurred();
            yield break;
        }

        APIResponse<LoginResponseApiModel> loginResponse = null;
        string token = SaveLoadManager.instance.getToken();

        //Token Login
        if (token != null && !firstLogin)
        {
            ApiRequest<string> tokenRequest = new ApiRequest<string>()
            {
                Content = token,
            };
            yield return WebRequestHandler.PostRequest<ApiRequest<string>>(ApiPathManager.TokenValidationUrl, tokenRequest);
            loginResponse = WebRequestHandler.Response<APIResponse<LoginResponseApiModel>>();

            if (loginResponse == null || !loginResponse.isOk)
            {
                tokenLogin = false;
                SaveLoadManager.instance.deleteToken();
            }
            else
                tokenLogin = true;
        }

        //Username Password Login
        if (!tokenLogin)
        {
            //Logging in the user
            LoginRequestApiModel loginRequest = new LoginRequestApiModel()
            {
                UserName = userInformationModel.Username,
                Password = userInformationModel.Password,
            };
            yield return WebRequestHandler.PostRequest<LoginRequestApiModel>(ApiPathManager.LoginUrl, loginRequest);
            loginResponse = WebRequestHandler.Response<APIResponse<LoginResponseApiModel>>();
        }

        if (loginResponse == null || !loginResponse.isOk)
        {
            ServerErrorOccurred();
            yield break;
        }
        else
        {
            SaveLoadManager.instance.setUser(loginResponse.Response.UserData);

            if (!tokenLogin)
                SaveLoadManager.instance.saveToken(loginResponse.Response.Token);

            LoadScreenManager.instance.StopLoadingScreen();
        }

        HasServerErrorOccurred = false;

        StartCoroutine(StartGameplay());
    }

    #endregion

    #region Private Methods

    private void ServerErrorOccurred()
    {
        PopupManager.instance.DisplayMessage("Server Connection Error", "Unable to connect to ther server. Please try again later.");
        LoadScreenManager.instance.StopLoadingScreen();
        HasServerErrorOccurred = true;
        recalledLoadingScreen = false;
    }

    /// <summary>
    /// Fires each second
    /// </summary>
    private void Update()
    {
        if(HasServerErrorOccurred && !recalledLoadingScreen && !PopupManager.instance.isDisplayed)
        {
            StartCoroutine(LoginSignupSequence());
            recalledLoadingScreen = true;
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Starts the game
    /// </summary>
    public IEnumerator StartGameplay()
    {
        Gameplay.SetActive(true);
        yield return StartSequenceManager.instance.PlayAnimation();
        GameManager.instance.StartGameplay();
    }

    #endregion

}
