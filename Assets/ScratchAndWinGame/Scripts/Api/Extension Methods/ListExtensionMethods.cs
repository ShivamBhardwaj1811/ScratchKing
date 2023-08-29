using System.Collections;
using System.Collections.Generic;


public static class ListExtensionMethods
{
    /// <summary>
    /// Converts a list from specified data type to specified data type
    /// </summary>
    /// <typeparam name="In"></typeparam>
    /// <typeparam name="Out"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<Out> convertTo<In, Out>(this List<In> list)
        where In : IConvertible<Out>
    {
        List<Out> temp = new List<Out>();
        foreach (In item in list)
            temp.Add(item.Convert());
        return temp;
    }
}
