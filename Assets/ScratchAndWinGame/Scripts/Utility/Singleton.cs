using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour
{

    /// <summary>
    /// A static instance of the generic class
    /// </summary>
    public static T instance;

    /// <summary>
    /// Initializes the instance
    /// </summary>
    /// <param name="ins"></param>
    protected void Initialize(T ins)
    {
        if (instance == null)
            instance = ins;
    }

}
