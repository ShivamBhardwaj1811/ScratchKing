using System;
using System.Collections.Generic;
using System.Text;


public static class StringExtensionMethods
{

    /// <summary>
    /// Tells whether a string is null or empty
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool isNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }


    /// <summary>
    /// Checks whether the string contains only alphabets or not
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool containsAlphabets(this string str)
    {
        foreach (char ch in str.ToCharArray())
            if (!char.IsWhiteSpace(ch) && !char.IsLetter(ch))
                return false;
        return true;
    }

}

