using System;


[Serializable]
public class DisplayItemModel
{

    /// <summary>
    /// The username of the person
    /// </summary>
    public string Username;

    /// <summary>
    /// The country of the person
    /// </summary>
    public string Country;

    /// <summary>
    /// The earning of the person
    /// </summary>
    public float Earning;

    /// <summary>
    /// The area type of the Display Item
    /// By default area type is money
    /// </summary>
    public AreaType AreaType = AreaType.Money;

}
