using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Settings database class for changing winning sprites count, winning prices, sprites and colors

public class SettingsDatabase : Singleton<SettingsDatabase>
{

    [Header("Win BoardimagesCount Default")]
    public List<int> WinBoardImagesCountDefault;
    [Header("Win OneByOneTicketCount")]
    public List<int> WinCountOneByOneTicket;
    [Header("Win AllAtOnceTicketCount")]
    public List<int> WinCountAllAtOnceTicket;
    public List<SectionPrice> GoldPriceList;
    public List<SectionPrice> MoneyPriceList;
    [Header("Bonus Sprite")]
    public Sprite BonusSprite;
    [Header("Money Sprite")]
    public Sprite MoneySprite;
    [Header("Ticket Sprite")]
    public Sprite TicketSprite;
    [Header("Any Sprite")]
    public Sprite AnySprite;
    [Header("All Board Sprites")]
    public List<Sprite> AllBoardSprites;
    [Header("Section Enabled Color")]
    public List<Color> SectionEnabledColorList;
    [Header("Section Disabled Color")]
    public Color SectionDisabledColor;

    private void Awake()
    {
        Initialize(this);
    }

    /// <summary>
    /// Updates the section prices
    /// </summary>
    /// <param name="goldPrices"></param>
    /// <param name="moneyPrices"></param>
    public void UpdateSectionPrices(List<SectionPrice> goldPrices,List<SectionPrice> moneyPrices)
    {
        GoldPriceList = goldPrices;
        MoneyPriceList = moneyPrices;
    }

}
