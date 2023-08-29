

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static UnityEngine.UI.Dropdown;

public class ProfileManager : Singleton<ProfileManager>
{

    #region Private Members

    /// <summary>
    /// The time between the animations
    /// </summary>
    private float TimeBetweenAnimations = 0.05f;

    /// <summary>
    /// The animation clip for username
    /// </summary>
    public List<AnimClipPlayer> LoadInClips;

    /// <summary>
    /// The clips to run at load out
    /// </summary>
    public List<AnimClipPlayer> LoadOutClips;

    /// <summary>
    /// The animation clip to which reset is to be done
    /// </summary>
    private AnimationClip resetAnimationClip;

    #endregion

    #region Private Methods

    private void Awake()
    {
        Initialize(this);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Loads the profile page
    /// </summary>
    public void loadProfilePage()
    {
        ProfileButtonManager.instance.Profile.SetActive(true);
        StartCoroutine(StartLoadSequence());
    }

    /// <summary>
    /// Performs all the animations in a sequence
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartLoadSequence(int count = 0, AnimationClip clip = null)
    {
        //Setting value of each input
        List<string> values = SaveLoadManager.instance.currentUser.getValuesInSequence();

        //Getting list of countries if not already
        if(ProfileButtonManager.instance.Countries == null || ProfileButtonManager.instance.Countries.Count == 0)
        {
            LoadScreenManager.instance.DisplayLoadingScreen();
            string token = SaveLoadManager.instance.getToken();

            yield return WebRequestHandler.GetRequest<string>(ApiPathManager.CountriesListUrl, null, token: token, checkInternet: true);
            APIResponse<List<string>> countriesList = WebRequestHandler.Response<APIResponse<List<string>>>();

            LoadScreenManager.instance.StopLoadingScreen();

            if(countriesList == null || !countriesList.isOk)
            {
                PopupManager.instance.DisplayMessage("Server Connection Error","Could not retreive data from the server. Please try again later");
                while (PopupManager.instance.isDisplayed)
                    yield return null;
                yield break;
            }

            ProfileButtonManager.instance.Countries = new List<string>();

            foreach (string country in countriesList.Response)
            {
                if (string.IsNullOrEmpty(country) || string.IsNullOrWhiteSpace(country))
                    continue;
                ProfileButtonManager.instance.CountryDropDown.options.Add(new OptionData(country));
                ProfileButtonManager.instance.Countries.Add(country);
            }
        }

        for (int i = 0; i < values.Count; i++)
        {
            ProfileButtonManager.instance.profileInputs[i].SetTextWithoutNotify(values[i]);
        }

        int index = ProfileButtonManager.instance.Countries.IndexOf(SaveLoadManager.instance.currentUser.Country);
        ProfileButtonManager.instance.CountryDropDown.value = index == -1 ? 0 : index;
        ProfileButtonManager.instance.CountryDropDown.RefreshShownValue();

        //Storing animation clip for reset
        if (clip != null)
            resetAnimationClip = LoadInClips[0].AnimClips[0].Animation;

        for (int i = 0; i < count; i++)
        {
            if (clip != null)
                LoadInClips[i].AnimClips[0].Animation = clip;
        }

        yield return Utility.runCoroutinesAllAtOnceWaitToFinish(this, LoadInClips.convertTo<AnimClipPlayer, IEnumerator>(), TimeBetweenAnimations);

        if (resetAnimationClip != null)
        {
            for (int i = 0; i < count; i++)
            {
                LoadInClips[i].AnimClips[0].Animation = resetAnimationClip;
            }
            resetAnimationClip = null;
        }
    }

    /// <summary>
    /// Performs the load out sequence
    /// </summary>
    public IEnumerator LoadOutSequence()
    {
        yield return Utility.runCoroutinesAllAtOnceWaitToFinish(this, LoadOutClips.convertTo<AnimClipPlayer, IEnumerator>(), TimeBetweenAnimations);
    }

    #endregion
}
