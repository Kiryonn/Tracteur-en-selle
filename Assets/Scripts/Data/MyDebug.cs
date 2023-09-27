using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public static class MyDebug
{
    [Conditional("DEVELOPMENT_BUILD")]

    public static void Log(string text)
    {
        UnityEngine.Debug.Log(text);
    }

    public static void Log(int text)
    {
        UnityEngine.Debug.Log(text);
    }
}
