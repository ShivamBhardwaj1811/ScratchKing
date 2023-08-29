using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

// class for saving and loading game progress to player prefs
public class SaveLoadManager : Singleton<SaveLoadManager>
{

    /// <summary>
    /// The current user of the application
    /// </summary>
    public User currentUser = new User();

    /// <summary>
    /// The list of countries available to the user
    /// </summary>
    [HideInInspector]
    public List<string> countries = new List<string>();

    /// <summary>
    /// The date and time when the unity ads clicks need to be reset
    /// </summary>
    public DateTime clicksCountResetTime { get; set; }

    /// <summary>
    /// The total number of clicks which user can do per day on unity ads
    /// </summary>
    public int TotalClicksCount => 10;

    /// <summary>
    /// The number of user clicks count
    /// </summary>
    public int UserClicksCount { get; set; } = 0;

    /// <summary>
    /// Fires before any start method is called
    /// </summary>
    private void Awake()
    {
        Initialize(this);
    }

    /// <summary>
    /// Loads ticket data from player prefs
    /// </summary>
    public void LoadClickData()
    {
        if (!PlayerPrefs.HasKey("UnityAdsClickCountResetDate"))
            clicksCountResetTime = DateTime.Now.AddDays(1);
        else
            clicksCountResetTime = DateTime.Parse(PlayerPrefs.GetString("UnityAdsClickCountResetDate"));

        if (PlayerPrefs.HasKey("UnityAdsClicksCount"))
            UserClicksCount = PlayerPrefs.GetInt("UnityAdsClicksCount");
    }

    /// <summary>
    /// Checks whether the user exists on the machine
    /// </summary>
    /// <returns></returns>
    public bool UserExists() => PlayerPrefs.HasKey("Username");

    public NewUserInformationModel GetUserData()
    {
        NewUserInformationModel model = new NewUserInformationModel();

        if (!UserExists())
            return null;

        model.Username = PlayerPrefs.GetString("Username");
        model.Password = PlayerPrefs.GetString("Password");

        return model;
    }

    /// <summary>
    /// Saves user data
    /// </summary>
    /// <param name="model"></param>
    public void SaveUserData(NewUserInformationModel model)
    {
        PlayerPrefs.SetString("Username", model.Username);
        PlayerPrefs.SetString("Password", model.Password);
    }

    /// <summary>
    /// Deletes the users data from the player prefs
    /// </summary>
    public void DeleteUserData()
    {
        PlayerPrefs.DeleteKey("Username");
        PlayerPrefs.DeleteKey("Password");
    }

    /// <summary>
    /// Resets click count to 0
    /// Resets clicks reset date
    /// </summary>
    public void ResetClickCount()
    {
        //Resetting the data of the user clicks 
        if (DateTime.Now > clicksCountResetTime)
        {
            clicksCountResetTime = DateTime.Now.AddDays(1);
            SaveLoadManager.instance.SaveClickData();
            UserClicksCount = 0;
        }
    }

    /// <summary>
    /// Saves the data about user clicks
    /// </summary>
    public void SaveClickData()
    {
        PlayerPrefs.SetString("UnityAdsClickCountResetDate", clicksCountResetTime.ToString());
        PlayerPrefs.SetInt("UnityAdsClicksCount", UserClicksCount);
    }

    /// <summary>
    /// Increases the user click count
    /// </summary>
    public void watchAd()
    {
        UserClicksCount++;
        SaveClickData();
    }


    /// <summary>
    /// Tells whether the user can watch ad
    /// </summary>
    /// <returns></returns>
    public bool canWatchAd()
    {
        ResetClickCount();
        if (UserClicksCount < TotalClicksCount)
            return true;
        else
            return false;
    }

    public void setUser(User user)
    {
        currentUser = user;
        ScoreBoardManager.instance.UpdateScoreBoard();
        SettingsDatabase.instance.UpdateSectionPrices(currentUser.GoldSectionPrices, currentUser.MoneySectionPrices);
    }

    public void increaseValues(AreaType type, float value)
    {
        if (type == AreaType.Money) 
            currentUser.updateMoney(value);
        else if (type == AreaType.Gold) 
            currentUser.updateGoldCoins((int)value);
        else if (type == AreaType.Ticket) currentUser.updateTickets((int)value);

        //send data to the server for updating
        StartCoroutine(sendUpdate());
    }

    /// <summary>
    /// Updates the stored username
    /// </summary>
    /// <param name="username"></param>
    public void UpdateUsername(string username)
    {
        PlayerPrefs.SetString("Username", username);
        deleteToken();
    }

    /// <summary>
    /// sends update to the server
    /// </summary>
    /// <returns></returns>
    private IEnumerator sendUpdate()
    {
        yield return WebRequestHandler.PostRequest<User>(ApiPathManager.GameDataUpdate, currentUser, getToken());
        APIResponse<User> response = WebRequestHandler.Response<APIResponse<User>>();
        if (response.Response != null)
        {
            setUser(response.Response);
        }
    }

    /// <summary>
    /// Returns the token being saved
    /// </summary>
    /// <returns></returns>
    public string getToken()
    {
        if (PlayerPrefs.HasKey("token"))
            return PlayerPrefs.GetString("token");
        return null;
    }

    /// <summary>
    /// Removes the token so that user can login by it self
    /// </summary>
    public void deleteToken()
    {
        PlayerPrefs.DeleteKey("token");
    }

    /// <summary>
    /// Saves the token in the player prefs
    /// </summary>
    /// <param name="token"></param>
    public void saveToken(string token)
    {
        //Storing the token
        PlayerPrefs.SetString("token", token);
    }

}
