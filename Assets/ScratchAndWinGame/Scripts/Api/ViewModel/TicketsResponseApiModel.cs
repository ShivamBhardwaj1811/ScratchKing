using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine;

/// <summary>
/// The api model to respond to the request of the user for tickets
/// </summary>
[Serializable]
public class TicketsResponseApiModel
{

    /// <summary>
    /// The amount of tickets which user got
    /// </summary>
    [SerializeField]
    public int TicketsCount;

}
