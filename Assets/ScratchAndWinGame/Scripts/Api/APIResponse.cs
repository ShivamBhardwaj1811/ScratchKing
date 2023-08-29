using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

using UnityEngine;

/// <summary>
/// The response of the api on the request of the user
/// </summary>
[Serializable]
public class APIResponse<T>
    where T : new()
{
    #region Public Properties

    /// <summary>
    /// The errors which occured in the request if any
    /// </summary>
    [SerializeField]
    public string Errors = null;

    /// <summary>
    /// Tells whether any error has occured or not
    /// </summary>
    public bool isOk => string.IsNullOrEmpty(Errors) || string.IsNullOrWhiteSpace(Errors);

    /// <summary>
    /// The response of the api in the form of json
    /// </summary>
    [SerializeField]
    public T Response = new T();

    /// <summary>
    /// The raw response of the server
    /// </summary>
    [SerializeField]
    public string rawResponse;

    /// <summary>
    /// The message from the server to display
    /// By default the value is null
    /// </summary>
    [SerializeField]
    public string Message = null;


    #endregion

}

