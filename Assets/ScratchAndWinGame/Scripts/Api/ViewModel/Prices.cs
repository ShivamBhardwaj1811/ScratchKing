
using System;


[Serializable]
public class Prices : IConvertible<SectionPrice>
{
    /// <summary>
    /// Public accessor for the field
    /// </summary>
    public float Probability = 0.5f;

    /// <summary>
    /// The price which user can get for scratching
    /// </summary>
    public float Price = 1;

    /// <summary>
    /// Converts Price to section price
    /// </summary>
    /// <returns></returns>
    public SectionPrice Convert()
    {
        return new SectionPrice() { Price = this.Price, probability = this.Probability };
    }

}

