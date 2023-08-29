using System;

using UnityEngine;

/// <summary>
/// The api model for the login request
/// </summary>
[Serializable]
public class LoginRequestApiModel
{

    #region Public Properties

    /// <summary>
    /// The username of the user
    /// </summary>
    [SerializeField]
    public string UserName;

    /// <summary>
    /// The email of the user
    /// </summary>
    [SerializeField]
    public string Email;

    /// <summary>
    /// The password of the user account
    /// </summary>
    [SerializeField]
    public string Password;

    #endregion

}

