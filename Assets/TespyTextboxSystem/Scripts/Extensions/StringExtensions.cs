using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

public static class StringExtensions {

    public static bool hasLettersOrDigits(this string str)
    {
        for (int i = 0; i < str.Length; i++)
            if (Char.IsLetterOrDigit(str[i]))
                return true;

        return false;
    }
	
}
