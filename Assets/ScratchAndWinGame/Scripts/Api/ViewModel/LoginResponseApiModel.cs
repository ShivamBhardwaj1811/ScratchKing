using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine;

/// <summary>
/// An api model to respond to the request of the api
/// </summary>
[Serializable]
public class LoginResponseApiModel
{
    /// <summary>
    /// The token which will be used for further validation
    /// </summary>
    [SerializeField]
    public string Token;

    /// <summary>
    /// The user data being stored in the application
    /// </summary>
    [SerializeField]
    public User UserData;

}

