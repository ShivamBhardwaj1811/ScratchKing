using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WinPanelManager : Singleton<WinPanelManager>
{
    public GameObject WiningPanel;

    public Image WinTypeImage;
    public TextMeshProUGUI PriceText;

    public bool Clicked = false;

    public GameObject toInstatiate;

    private void Awake()
    {
        if (instance == null) instance = this;
        WiningPanel.SetActive(false);
    }

    public void WinPanelClicked()
    {
        Clicked = true;
    }

    //Main Coroutine to show winning panel
    public IEnumerator ShowWiningPanel(WinSettings winSettings)
    {
        yield return new WaitForSeconds(1f);

        if (winSettings.DidIWon == true)
        {
            SaveLoadManager.instance.increaseValues(winSettings.type, winSettings.PricePreset.Price);

            WiningPanel.SetActive(true);

            SetWinningUI(winSettings);

            Clicked = false;

            while (Clicked == false)
            {
                yield return null;
            }

            WiningPanel.SetActive(false);

            if (winSettings.type == AreaType.Gold) yield return PanelFinishAnimation(SaveLoadManager.instance.currentUser.GoldOld, SaveLoadManager.instance.currentUser.GoldCoins, ScoreBoardManager.instance.MainPart,winSettings.type);
            if (winSettings.type == AreaType.Money) yield return PanelFinishAnimation(SaveLoadManager.instance.currentUser.MoneyOld, SaveLoadManager.instance.currentUser.Money, ScoreBoardManager.instance.BonusPart, winSettings.type);
            if (winSettings.type == AreaType.Ticket) yield return PanelFinishAnimation(SaveLoadManager.instance.currentUser.TicketsOld, SaveLoadManager.instance.currentUser.Tickets, ScoreBoardManager.instance.TicketPart, winSettings.type);
        }
    }

    // Coroutine to play additional animations after we clicked on claim button inside winning panel
    public IEnumerator PanelFinishAnimation(float OldScore, float newScore, UIPart uiPart, AreaType areaType, int count = 4, bool add = true, Transform transform = null)
    {
        List<IEnumerator> ListOfAnimationRoutines = new List<IEnumerator>();

        ListOfAnimationRoutines.Add(uiPart.AnimationIn.Play());
        ListOfAnimationRoutines.Add(Utility.ChangeNumbersGradually(OldScore, newScore, uiPart.UICountText, 0.7f));

        List<GameObject> myGoList = InstantiateGameObject(count, toInstatiate, WinTypeImage.sprite, add, areaType, uiPart.UISpriteTransform);

        foreach (var item in myGoList)
        {
            if (add)
            {
                ListOfAnimationRoutines.Add(Utility.MoveTo(item.transform.position, uiPart.UISpriteTransform.position, item.transform, 0.5f));
            }
            else
            {
                ListOfAnimationRoutines.Add(Utility.MoveTo(uiPart.UISpriteTransform.position, transform.position, item.transform, 0.8f));
            }
            ListOfAnimationRoutines.Add(item.GetComponent<AnimClipPlayer>().Play());
        }

        yield return Utility.runCoroutinesAllAtOnceWaitToFinish(this, ListOfAnimationRoutines, 0.08f);

        foreach (var item in myGoList)
        {
            Destroy(item.gameObject);
        }
    }

    // Method to fill sprite for the WinTypeImage
    public void SetWinningUI(WinSettings winSettings, bool usePrice = true)
    {
        if (usePrice)
            PriceText.text = winSettings.PricePreset.Price.ToString();

        if (winSettings.type == AreaType.Gold) WinTypeImage.sprite = SettingsDatabase.instance.MoneySprite;
        if (winSettings.type == AreaType.Money) WinTypeImage.sprite = SettingsDatabase.instance.BonusSprite;
        else if (winSettings.type == AreaType.Ticket)
            WinTypeImage.sprite = SettingsDatabase.instance.TicketSprite;
    }


    // Method to insantiate multiple copies of gameobject
    public List<GameObject> InstantiateGameObject(int howMuch, GameObject toInstatiate, Sprite mySprite, bool add, AreaType areaType, Transform uiTransform=null)
    {
        List<GameObject> myGameObjectList = new List<GameObject>();

        for (int i = 0; i < howMuch; i++)
        {
            GameObject myGO = Instantiate(toInstatiate, add ? toInstatiate.transform.position : uiTransform.position, Quaternion.identity);
            myGO.GetComponent<SpriteRenderer>().sprite = mySprite;
            if (areaType == AreaType.Gold)
                myGO.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            else if (areaType == AreaType.Ticket)
                myGO.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            else if (areaType == AreaType.Money)
                myGO.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            myGameObjectList.Add(myGO);
        }

        return myGameObjectList;
    }
}
