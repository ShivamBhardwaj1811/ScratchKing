using System;
using System.Collections.Generic;
using System.Text;


/// <summary>
/// The signup request model for the client to send api requests
/// </summary>
[Serializable]
public class SignUpRequestApiModel
{

    /// <summary>
    /// The firstname of the person
    /// </summary>
    public string FirstName;

    /// <summary>
    /// The last name of the person
    /// </summary>
    public string LastName;

    /// <summary>
    /// The username which will be used to login
    /// </summary>
    public string Username;

    /// <summary>
    /// The email of the user
    /// </summary>
    public string Email;

    /// <summary>
    /// The country of the user
    /// </summary>
    public string Country;

    /// <summary>
    /// The password of the user. This password is confirmed by the user
    /// </summary>
    public string Password;

}

