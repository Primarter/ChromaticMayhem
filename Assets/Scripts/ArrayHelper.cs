using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayHelper
{
    public static T PickRandom<T>(T[] arr)
    {
        if (arr.Length == 0)
            return default(T);
        return arr[Random.Range(0, arr.Length)];
    }
}
