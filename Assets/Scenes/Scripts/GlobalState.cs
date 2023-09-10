using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathsSeen
{
    public bool Shark = false;
    public bool Pirate = false;
    public bool InfoBot = false;
    public bool DuckGirl = false;

    public bool Time1920s = false;
    public bool Time2060s = false;
    public bool ClotheSock = false;
    public bool ClotheRing = false;

    public bool SeenAll => Shark && Pirate && InfoBot && DuckGirl && Time1920s && Time2060s && ClotheSock && ClotheRing;
}

public static class GlobalState
{
    public static bool PlayedOnce = false;
    public static string PlayerName = null;
    public static PathsSeen PathsSeen = new();
}
