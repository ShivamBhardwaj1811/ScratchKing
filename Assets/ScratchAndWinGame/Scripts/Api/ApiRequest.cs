using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine;

[Serializable]
public class ApiRequest<T>
{

    /// <summary>
    /// The content to be sended as request
    /// </summary>
    [SerializeField]
    public T Content;

}
