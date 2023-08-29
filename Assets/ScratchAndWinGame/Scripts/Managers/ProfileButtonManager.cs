

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileButtonManager : Singleton<ProfileButtonManager>
{
    #region Public Members

    public GameObject Profile;

    /// <summary>
    /// The list of countries
    /// </summary>
    public List<string> Countries = null;

    #endregion

    #region Private Members

    [Header("Help Popup")]
    /// <summary>
    /// The clip to load in the menu box
    /// </summary>
    [SerializeField]
    private AnimClipPlayer loadIn;
    /// <summary>
    /// The animation clip to load out the menu box
    /// </summary>
    [SerializeField]
    private AnimClipPlayer loadOut;

    /// <summary>
    /// The actual popup box
    /// </summary>
    [SerializeField]
    private GameObject PopupBox;

    /// <summary>
    /// The list of profile input fields
    /// </summary>
    public List<InputField> profileInputs = new List<InputField>();

    /// <summary>
    /// The country drop down
    /// </summary>
    public Dropdown CountryDropDown;

    public List<GameObject> Buttons;
    public List<AnimClipPlayer> LoadInClips;
    public List<AnimClipPlayer> LoadOutClips;

    #endregion

    #region Private Methods

    //Awake is fired before any start method is called
    private void Awake()
    {
        Initialize(this);
        CountryDropDown.ClearOptions();
    }

    /// <summary>
    /// Loads the gameplay
    /// </summary>
    /// <returns></returns>
    private IEnumerator loadGamePlay()
    {
        yield return StartSequenceManager.instance.PlayAnimation();
        ComponentManager.instance.Gameplay.SetActive(true);
        StartCoroutine(GameManager.instance.UpdateTicketSettings());
    }

    /// <summary>
    /// Changes the interaction of the fields
    /// </summary>
    /// <param name="isInteractable"></param>
    private IEnumerator ChangeFieldInteraction(bool isInteractable)
    {
        foreach (InputField input in profileInputs)
        {
            if (input.GetComponent<Interactable>().IsInteractable)
            {
                input.interactable = isInteractable;
                if (isInteractable)
                    input.GetComponentsInChildren<Image>()[1].color = Color.red;
                else
                    input.GetComponentsInChildren<Image>()[1].color = Color.white;
            }
        }

        if (CountryDropDown.GetComponent<Interactable>().IsInteractable)
        {
            CountryDropDown.interactable = isInteractable;
        }

        yield break;
    }

    /// <summary>
    /// Changes the profile state from editing to non-editing and vice versa
    /// </summary>
    /// <returns></returns>
    private IEnumerator changeProfileState(bool isEditing)
    {
        if (isEditing)
        {
            yield return Utility.runCoroutinesAllAtOnceWaitToFinish(this, new List<IEnumerator>()
            {
                LoadOutClips[0].Play(),
                LoadOutClips[1].Play(),
                LoadOutClips[3].Play(),
                LoadOutClips[4].Play(),
            });
            Buttons[0].SetActive(false);
            Buttons[1].SetActive(false);
            Buttons[2].SetActive(true);
            Buttons[3].SetActive(false);
            Buttons[4].SetActive(false);
            yield return LoadInClips[2].Play();
        }
        else
        {
            yield return LoadOutClips[2].Play();
            Buttons[0].SetActive(true);
            Buttons[1].SetActive(true);
            Buttons[2].SetActive(false);
            Buttons[3].SetActive(true);
            Buttons[4].SetActive(true);
            yield return Utility.runCoroutinesAllAtOnceWaitToFinish(this, new List<IEnumerator>()
            {
                LoadInClips[0].Play(),
                LoadInClips[1].Play(),
                LoadInClips[3].Play(),
                LoadInClips[4].Play(),
            });
        }
    }

    /// <summary>
    /// Updates the user data
    /// </summary>
    private User UpdateData(bool reset = false)
    {
        if (!reset)
        {
            User user = new User();
            user.FirstName = profileInputs[0].text;
            user.LastName = profileInputs[1].text;
            user.Username = profileInputs[2].text;
            user.Email = profileInputs[3].text;
            user.PaypalAccountNumber = profileInputs[4].text;
            user.BankAccount = profileInputs[5].text;
            user.Country = CountryDropDown.value == 0 ? "" : Countries[CountryDropDown.value];
            return user;
        }
        else
        {
            profileInputs[0].text = SaveLoadManager.instance.currentUser.FirstName;
            profileInputs[1].text = SaveLoadManager.instance.currentUser.LastName;
            profileInputs[2].text = SaveLoadManager.instance.currentUser.Username;
            profileInputs[3].text = SaveLoadManager.instance.currentUser.Email;
            profileInputs[4].text = SaveLoadManager.instance.currentUser.PaypalAccountNumber;
            profileInputs[5].text = SaveLoadManager.instance.currentUser.BankAccount;
            int index = Countries.IndexOf(SaveLoadManager.instance.currentUser.Country);
            CountryDropDown.value = index == -1 ? 0 : index;
            CountryDropDown.RefreshShownValue();
            return null;
        }
    }

    private IEnumerator saveSequence()
    {
        LoadScreenManager.instance.DisplayLoadingScreen();
        User user = UpdateData();

        if(user.Country == "")
        {
            LoadScreenManager.instance.StopLoadingScreen();
            PopupManager.instance.DisplayMessage("Country Error", "Please select a valid country");

            while (PopupManager.instance.isDisplayed)
                yield return null;

            yield break;
        }

        yield return WebRequestHandler.PostRequest<User>(ApiPathManager.ProfileUpdateUrl, user, SaveLoadManager.instance.getToken());
        APIResponse<bool> response = WebRequestHandler.Response<APIResponse<bool>>();
        LoadScreenManager.instance.StopLoadingScreen();
        if (response.isOk)
        {
            StartCoroutine(changeProfileState(false));
            StartCoroutine(ChangeFieldInteraction(false));
            if(user.Username != SaveLoadManager.instance.currentUser.Username)
            {
                SaveLoadManager.instance.UpdateUsername(user.Username);
            }
        }
        else
        {
            PopupManager.instance.DisplayMessage("Error",response.Errors);
        }

    }

    /// <summary>
    /// Starts the log out sequence
    /// </summary>
    /// <returns></returns>
    private IEnumerator LogoutSequence()
    {
        yield return ProfileManager.instance.LoadOutSequence();
        Profile.SetActive(false);
        StartCoroutine(ComponentManager.instance.LoginSignupSequence());
    }

    private IEnumerator popupCloseSequence()
    {
        yield return loadOut.Play();
        PopupBox.SetActive(false);
    }

    /// <summary>
    /// The sequence for the redeem request
    /// </summary>
    /// <returns></returns>
    private IEnumerator redeemRequestSequence()
    {
        LoadScreenManager.instance.DisplayLoadingScreen();
        yield return WebRequestHandler.GetRequest<string>
            (
                Url: ApiPathManager.RedeemRequestUrl,
                content: null,
                token: SaveLoadManager.instance.getToken()
            );
        //Getting the response of the api
        APIResponse<User> response = WebRequestHandler.Response<APIResponse<User>>();
        LoadScreenManager.instance.StopLoadingScreen();

        if (response == null)
        {
            PopupManager.instance.DisplayMessage("Connection Error", "Unable to get response from server");
        }
        else if (response.isOk)
        {
            PopupManager.instance.DisplayMessage("Message", "Your request has been submitted successfully. Please be patient your request will be processed in few working days");
            SaveLoadManager.instance.setUser(response.Response);
        }
        else
        {
            PopupManager.instance.DisplayMessage("Error",response.Errors);
        }
    }

    private IEnumerator returnSeuqence()
    {
        yield return ProfileManager.instance.LoadOutSequence();
        Profile.SetActive(false);
        StartSequenceManager.instance.ResetLocation();
        StartCoroutine(loadGamePlay());
    }

    #endregion

    #region Button Handlers

    public void returnButtonClicked()
    {
        StartCoroutine(returnSeuqence());
    }

    /// <summary>
    /// Fires when the edit button is clicked
    /// </summary>
    public void Edit()
    {
        StartCoroutine(changeProfileState(true));
        StartCoroutine(ChangeFieldInteraction(true));
    }

    /// <summary>
    /// Handles the request for saving data
    /// </summary>
    public void SaveButtonHandler()
    {
        StartCoroutine(saveSequence());
    }

    /// <summary>
    /// Saves the data edited
    /// </summary>
    public void CancelButtonHandler()
    {
        UpdateData(true);
        StartCoroutine(changeProfileState(false));
        StartCoroutine(ChangeFieldInteraction(false));
    }

    /// <summary>
    /// Moves user to login page
    /// </summary>
    public void Logout()
    {
        SaveLoadManager.instance.deleteToken();
        SaveLoadManager.instance.currentUser = null;
        StartCoroutine(LogoutSequence());
    }

    #endregion

    #region Redeem Button Handlers

    public void RedeemHelpHandler()
    {
        PopupBox.SetActive(true);
        StartCoroutine(loadIn.Play());
    }

    /// <summary>
    /// Closes the popup box
    /// </summary>
    public void helpCloseButtonHandler()
    {
        StartCoroutine(popupCloseSequence());
    }

    /// <summary>
    /// Handles the redeem button
    /// </summary>
    public void RedeemButtonHandler()
    {
        StartCoroutine(redeemRequestSequence());
    }

    #endregion

}
