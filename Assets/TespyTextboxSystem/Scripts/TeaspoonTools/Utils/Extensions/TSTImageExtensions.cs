using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TeaspoonTools.Utils.Extensions
{
    public static class TSTImageExtensions
    {
        /// <summary>
        /// Changes the opacity of the image to the percentage you pass it.
        /// </summary>
        /// <param name="img"></param>
        /// <param name="opacity"></param>
        public static void SetOpacity(this Image img, float opacity)
        {
            img.color.SetOpacity(opacity);
        }
    }
}
