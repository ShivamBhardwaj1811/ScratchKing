using System;
using System.Collections.Generic;
using System.Text;


/// <summary>
/// The data format of the user registration request which is acceptable by the api
/// </summary>
[Serializable]
public class UserData
{

    /// <summary>
    /// The name of the user
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// The email of the user
    /// </summary>
    public string Email { get; set; }

}
