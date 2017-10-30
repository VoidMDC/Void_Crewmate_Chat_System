using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace TeaspoonTools.Utils.Extensions
{
    public static class TSTColorExtensions
    {

        static float maxRgbValue = 255f;

        public static void SetOpacity(this Color color, float percent)
        {
            bool negativePercentage = (percent < 0f);
            bool over100Percentage = (percent > 100f);

            if (negativePercentage)
                Debug.Log("Cannot have a negative percentage (" + percent + "%) when changing the opacity of a color!");
            else if (over100Percentage)
                Debug.Log("Cannot have a percentage over 100 (" + percent + "%) when changing the opacity of a color!");
            else
            {
                //color.a = valueToSet;
				color.a = percent / 100f;
            }
        }

        public static void SetRgb(this Color color, float r, float g, float b)
        {
			color.r = r;
            color.g = g;
            color.b = b;
        }

        public static Color Clone(this Color color)
        {
            return new Color(color.r, color.g, color.b, color.a);
        }
    }
}