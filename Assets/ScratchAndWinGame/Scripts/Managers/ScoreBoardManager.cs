using TMPro;

using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIPart
{
    public TextMeshProUGUI UICountText;
    public Transform UISpriteTransform;
    public AnimClipPlayer AnimationIn;
    public Transform SpriteEndTransform;
}

// class for Managing Scoreboard Panel
public class ScoreBoardManager : Singleton<ScoreBoardManager>
{
    public UIPart MainPart;
    public UIPart BonusPart;
    public UIPart TicketPart;

    [Space]
    public TextMeshProUGUI LeftText;
    public TextMeshProUGUI NumberText;

    public GameObject NumberPanel;
    public Image CenterImage;

    [Header("Animations")]
    public AnimClipPlayer AnimMainIn;
    public AnimClipPlayer AnimAddonIn;
    public AnimClipPlayer AnimAddonOut;
    public AnimClipPlayer AnimNumberIn;
    public AnimClipPlayer AnimNumberOut;
    public AnimClipPlayer AdditionalPanelsIn;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void UpdateScoreBoard()
    {
        MainPart.UICountText.text = SaveLoadManager.instance.currentUser.GoldCoins.ToString();
        BonusPart.UICountText.text = SaveLoadManager.instance.currentUser.Money.ToString();
        TicketPart.UICountText.text = SaveLoadManager.instance.currentUser.Tickets.ToString();
    }

    public void ConnectMoneyPartUI(bool showAnySprite, int winningSpriteCount, WinSettings winSettings, Sprite winSprite)
    {
        NumberPanel.SetActive(true);

        string LeftText = "MATCH ";
        if (showAnySprite == true && winningSpriteCount != 1) LeftText = "ANY ";
        string Number = winningSpriteCount.ToString();

        FillAddonUI(LeftText, winSprite, Number);
    }

    public void ConnectBonusPartUI(TextMeshProUGUI BonusBoardText, WinSettings winSettings)
    {
        NumberPanel.SetActive(false);

        string LeftText = "TRY";

        Sprite winSprite = SettingsDatabase.instance.BonusSprite;

        FillAddonUI(LeftText, winSprite, "");

        if (winSettings.DidIWon == true) BonusBoardText.text = winSettings.PricePreset.Price.ToString();
        else BonusBoardText.text = "No Luck!";
    }

    private void FillAddonUI(string leftText, Sprite winSprite, string number)
    {
        LeftText.text = leftText;
        NumberText.text = number;
        CenterImage.sprite = winSprite;
    }
}
