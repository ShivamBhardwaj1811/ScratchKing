using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenuManager : Singleton<MainMenuManager>
{

    public GameObject MainMenuParent;

    public AnimClipPlayer AnimIn;
    public AnimClipPlayer AnimOut;

    bool menuClicked = false;

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

    private void Awake()
    {
        MainMenuParent.SetActive(true);

        if (instance == null) instance = this;
    }

    // Coroutine which waits untill we click on the play button
    public IEnumerator waitUntilClicked()
    {
        MainMenuParent.SetActive(true);
        StartCoroutine(loadPolicies());
        yield return AnimIn.Play();

        menuClicked = false;

        while (menuClicked == false)
        {
            yield return null;
        }

        yield return AnimOut.Play();

        MainMenuParent.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        yield return null;
    }

    public void Clicked()
    {
        menuClicked = true;
    }
}
