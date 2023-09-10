using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnUtil
{
    [YarnFunction("is_host")]
    public static bool IsHost(string name)
    {
        var normalized = name.ToLower();
        Debug.Log(normalized.Contains("emily") || normalized.Contains("julie") ? "HOST" : "NOT HOST");
        return normalized.Contains("emily") || normalized.Contains("julie");
    }

    [YarnFunction("to_lower")]
    public static string ToLower(string v) => v.ToLower();

    [YarnFunction("to_upper")]
    public static string ToUpper(string v) => v.ToUpper();

    [YarnFunction("starts_with")]
    public static bool StartsWith(string v, string sub) => v.StartsWith(sub);
}
