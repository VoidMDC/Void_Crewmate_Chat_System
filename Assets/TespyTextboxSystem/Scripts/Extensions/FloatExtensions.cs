using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public static class FloatExtensions {
    
    public static float ToPercentage(this float num)
    {
        return num / 100f;
    }
}
