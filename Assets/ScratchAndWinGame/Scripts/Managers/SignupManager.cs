using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignupManager : Singleton<SignupManager>
{
    #region Private Members

    /// <summary>
    /// The drop down for country
    /// </summary>
    [SerializeField]
    private Dropdown CountryDropDown;

    [SerializeField]
    private float TimeBetweenAnimations = 0.15f;

    /// <summary>
    /// The panel to load the signup page
    /// </summary>
    [SerializeField]
    private GameObject SignupPanel;

    /// <summary>
    /// The input fields on the sign up page
    /// </summary>
    [SerializeField]
    private List<InputField> InputFields = new List<InputField>();

    /// <summary>
    /// The enter animations for the objects on the signup page
    /// </summary>
    [SerializeField]
    private List<AnimClipPlayer> AnimIn = new List<AnimClipPlayer>();

    /// <summary>
    /// The exit animations for the objects on the signup page
    /// </summary>
    [SerializeField]
    private List<AnimClipPlayer> AnimOut = new List<AnimClipPlayer>();

    [SerializeField]
    private GameObject EmailIssueDisplayBox;
    [SerializeField]
    private AnimClipPlayer BoxAnimIn;
    [SerializeField]
    private AnimClipPlayer BoxAnimOut;

    #endregion

    #region Private Methods

    /// <summary>
    /// Fires before any start method
    /// </summary>
    private void Awake()
    {
        Initialize(this);
    }

    /// <summary>
    /// Clears the fields
    /// </summary>
    private void clearFields()
    {
        foreach (InputField field in InputFields)
            field.text = "";
        CountryDropDown.value = 0;
    }

    #endregion

    #region Coroutines

    /// <summary>
    /// Can be called in order to load signup page
    /// </summary>
    /// <returns></returns>
    public IEnumerator Signup()
    {
        SignupPanel.SetActive(true);
        CountryDropDown.ClearOptions();
        LoadScreenManager.instance.DisplayLoadingScreen();
        yield return WebRequestHandler.GetRequest<APIResponse<List<string>>>(ApiPathManager.CountriesListUrl, null, checkInternet: false);
        APIResponse<List<string>> response = WebRequestHandler.Response<APIResponse<List<string>>>();
        if (response != null)
        {
            CountryDropDown.AddOptions(response.Response);
        }
        LoadScreenManager.instance.StopLoadingScreen();
        yield return Utility.runCoroutinesAllAtOnceWaitToFinish(this, AnimIn.convertTo<AnimClipPlayer, IEnumerator>(), TimeBetweenAnimations);
    }


    #endregion

    #region Button Handlers

    /// <summary>
    /// Handles the signup request
    /// </summary>
    public void SignupButtonHandler()
    {
        StartCoroutine(postSignUpSequence());
    }

    private IEnumerator PlayOutAnimations()
    {
        yield return Utility.runCoroutinesAllAtOnceWaitToFinish(this, AnimOut.convertTo<AnimClipPlayer, IEnumerator>(), TimeBetweenAnimations);
    }

    /// <summary>
    /// Closes the email box
    /// </summary>
    public void CloseButtonHandler()
    {
        StartCoroutine(emailBoxCloseSequence());
    }

    private IEnumerator emailBoxCloseSequence()
    {
        yield return BoxAnimOut.Play();
        EmailIssueDisplayBox.SetActive(false);
    }

    /// <summary>
    /// The sequence to follow after the signup button is pressed
    /// </summary>
    /// <returns></returns>
    private IEnumerator postSignUpSequence()
    {
        //displaying loading screen
        LoadScreenManager.instance.DisplayLoadingScreen();
        bool isValid = true;
        string error = "";
        dynamic data = ValidateModel();
        APIResponse<User> response = null;
        bool shouldContinue = true;

        if (data is string str)
        {
            isValid = false;
            error = str;
        }
        else if (data is SignUpRequestApiModel model)
        {
            string[] emailComponents = model.Email.Trim().Split('@');
            if (emailComponents.Length == 2)
            {
                if (emailComponents[1] != "gmail.com")
                {
                    EmailIssueDisplayBox.SetActive(true);
                    yield return BoxAnimIn.Play();
                    shouldContinue = false;
                }
            }
            else
            {
                EmailIssueDisplayBox.SetActive(true);
                yield return BoxAnimIn.Play();
                shouldContinue = false;
            }

            if(shouldContinue)
            {
                yield return WebRequestHandler.PostRequest<SignUpRequestApiModel>(ApiPathManager.SignupUrl, model, checkInternet: false);
                response = WebRequestHandler.Response<APIResponse<User>>();
            }
        }

        LoadScreenManager.instance.StopLoadingScreen();

        if (shouldContinue)
        {
            if (!isValid)
            {
                PopupManager.instance.DisplayMessage("Data Validation Error", error);
                clearPassword();
                yield break;
            }
            else if (response == null)
            {
                PopupManager.instance.DisplayMessage("Connection Error", "Unable to get response from server\nPlease check your internet connection");
                yield break;
            }
            else if (isValid && !response.isOk)
            {
                PopupManager.instance.DisplayMessage("Login Error", response.Errors);
                clearPassword();
                yield break;
            }
            else if (response.Message != null)
            {
                PopupManager.instance.DisplayMessage("Message", response.Message);
                while (PopupManager.instance.isDisplayed)
                    yield return null;
            }

            yield return PlayOutAnimations();
            clearFields();
            SignupPanel.SetActive(false);
            StartCoroutine(LoginManager.instance.Login());
        }
    }

    /// <summary>
    /// Clears the password fields
    /// </summary>
    private void clearPassword()
    {
        int count = InputFields.Count;
        InputFields[count - 1].text = "";
        InputFields[count - 2].text = "";
    }

    /// <summary>
    /// Validates the data and stores it in the model
    /// </summary>
    /// <returns> Returns null if data is invalid else returns  </returns>
    private dynamic ValidateModel()
    {
        if (CountryDropDown.options.Count == 0)
            return "Unable to connect to server.\nPlease check your internet connection";
        int count = InputFields.Count;
        //Compare the password and confirm password
        if (InputFields[count - 1].text.CompareTo(InputFields[count - 2].text) != 0)
            return "Password match failed. Please re-type password";

        //Check whether the person has selected any country
        if (CountryDropDown.options[CountryDropDown.value].text.ToLower().Contains("select"))
            return "Country is requried";

        return new SignUpRequestApiModel()
        {
            FirstName = InputFields[0].text,
            LastName = InputFields[1].text,
            Username = InputFields[2].text,
            Email = InputFields[3].text,
            Password = InputFields[4].text,
            Country = CountryDropDown.options[CountryDropDown.value].text.Trim('\r'),
        };
    }


    /// <summary>
    /// Handles the login button request
    /// </summary>
    public void LoginButtonHandler()
    {
        StartCoroutine(loginPageLoadSequence());
    }

    private IEnumerator loginPageLoadSequence()
    {
        //TODO: Validate data from server
        yield return PlayOutAnimations();
        clearFields();
        SignupPanel.SetActive(false);
        StartCoroutine(LoginManager.instance.Login());
    }

    #endregion
}
