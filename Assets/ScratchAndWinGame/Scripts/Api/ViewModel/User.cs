
using System;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// Holds data about user
/// </summary>

[Serializable]
public class User
{
    #region Public Accessors

    /// <summary>
    /// The bank account details of the user
    /// </summary>
    [SerializeField]
    public string BankAccount;

    /// <summary>
    /// The country of the user
    /// </summary>
    [SerializeField]
    public string Country;

    /// <summary>
    /// The username of the user
    /// </summary>
    [SerializeField]
    public string Username;

    /// <summary>
    /// The email of the user
    /// </summary>
    [SerializeField]
    public string Email;

    /// <summary>
    /// The first name of the user
    /// </summary>
    [SerializeField]
    public string FirstName;

    /// <summary>
    /// The last name of the user
    /// </summary>
    [SerializeField]
    public string LastName;

    /// <summary>
    /// The money earned by the user
    /// </summary>
    [SerializeField]
    public float Money;

    /// <summary>
    /// The gold coins earned by the user
    /// </summary>
    [SerializeField]
    public int GoldCoins;

    /// <summary>
    /// The price list for money earning
    /// </summary>
    [SerializeField]
    public List<Prices> MoneyPrices;

    /// <summary>
    /// The price list for gold earning
    /// </summary>
    [SerializeField]
    public List<Prices> GoldPrices;

    /// <summary>
    /// The number of tickets user have
    /// </summary>
    [SerializeField]
    public int Tickets;

    /// <summary>
    /// The list of section prices for money
    /// </summary>
    public List<SectionPrice> MoneySectionPrices => MoneyPrices.convertTo<Prices, SectionPrice>();

    /// <summary>
    /// The list of section prices for gold
    /// </summary>
    public List<SectionPrice> GoldSectionPrices => GoldPrices.convertTo<Prices, SectionPrice>();

    /// <summary>
    /// the previous money amount
    /// </summary>
    [SerializeField]
    public float MoneyOld;

    /// <summary>
    /// the previous gold coins amount
    /// </summary>
    [SerializeField]
    public int GoldOld;

    /// <summary>
    /// the previous tickets amount
    /// </summary>
    [SerializeField]
    public int TicketsOld;

    /// <summary>
    /// The account number for the paypal account
    /// </summary>
    [SerializeField]
    public string PaypalAccountNumber;

    #endregion

    #region Public Methods

    /// <summary>
    /// Returns the values in the specified sequence
    /// </summary>
    /// <returns></returns>
    public List<string> getValuesInSequence()
    {
        return new List<string>()
        {
            FirstName,
            LastName,
            Username,
            Email,
            PaypalAccountNumber,
            BankAccount,
            $"${Money.ToString()}"
        };
    }

    /// <summary>
    /// Adds the amount to the earned money
    /// </summary>
    /// <param name="amount"></param>
    public void addMoney(float amount)
    {
        //Check if the amount earned is equal to any defined price
        foreach (Prices price in MoneyPrices)
            //If amount matches any defined price then add it to the Money
            if (price.Price == amount)
            {
                Money += amount;
                return;
            }
    }

    /// <summary>
    /// Adds the coins to the earned coins
    /// </summary>
    /// <param name="amount"></param>
    public void addGoldCoins(int amount)
    {
        //Check if the amount earned is equal to any defined price
        foreach (Prices price in GoldPrices)
            //If amount matches any defined price then add it to the Money
            if (price.Price == amount)
            {
                GoldCoins += amount;
                return;
            }
    }

    /// <summary>
    /// Adds the price to the money prices list
    /// </summary>
    /// <param name="priceDescription"></param>
    public void addMoneyPrice(Prices priceDescription)
    {
        if (priceDescription == null)
            return;
        MoneyPrices.Add(priceDescription);
    }

    /// <summary>
    /// Removes the last added price from price list
    /// </summary>
    public void removeLastMoneyPrice()
    {
        if (MoneyPrices.Count > 0)
            MoneyPrices.RemoveAt(MoneyPrices.Count - 1);
    }

    /// <summary>
    /// Removes the last added price from price list
    /// </summary>
    public void removeLastGoldPrice()
    {
        if (GoldPrices.Count > 0)
            GoldPrices.RemoveAt(GoldPrices.Count - 1);
    }

    /// <summary>
    /// Adds the price to the gold prices list
    /// </summary>
    /// <param name="priceDescription"></param>
    public void addGoldPrice(Prices priceDescription)
    {
        if (priceDescription == null)
            return;
        GoldPrices.Add(priceDescription);
    }

    /// <summary>
    /// Updates the money from previous to next
    /// </summary>
    /// <param name="newMoney"></param>
    public void updateMoney(float newMoney)
    {
        MoneyOld = Money;
        Money += newMoney;
    }

    /// <summary>
    /// Updates the gold coins from previous to next
    /// </summary>
    /// <param name="goldCoins"></param>
    public void updateGoldCoins(int goldCoins)
    {
        GoldOld = GoldCoins;
        GoldCoins += goldCoins;
    }

    /// <summary>
    /// Updates and store the previous value of the tickets
    /// </summary>
    /// <param name="tickets"></param>
    public void updateTickets(int tickets)
    {
        TicketsOld = Tickets;
        Tickets += tickets;
    }

    /// <summary>
    /// Deducts a ticket
    /// </summary>
    public void DeductTicket()
    {
        if (Tickets > 0)
            Tickets--;
    }

    /// <summary>
    /// Tells whether the user is equal to other object
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
        if (obj is User user)
        {
            if (this.FirstName == user.FirstName &&
                this.LastName == user.LastName &&
                this.Email == user.Email &&
                this.Username == user.Username &&
                this.Country == user.Country &&
                this.PaypalAccountNumber == user.PaypalAccountNumber)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    #endregion

    #region Constructor

    /// <summary>
    /// Default constructor
    /// </summary>
    public User()
    {

    }

    /// <summary>
    /// Constructor to initialize the user
    /// </summary>
    /// <param name="username"></param>
    /// <param name="email"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="country"></param>
    /// <param name="money"></param>
    /// <param name="goldCoins"></param>
    public User(string username, string email, string firstName, string lastName, string paypalAccount, string country, float money, int goldCoins, int tickets)
    {
        Username = username;
        Email = email;
        PaypalAccountNumber = paypalAccount;
        FirstName = firstName;
        LastName = lastName;
        Country = country;
        Money = money;
        GoldCoins = goldCoins;
        Tickets = tickets;
    }

    #endregion
}
