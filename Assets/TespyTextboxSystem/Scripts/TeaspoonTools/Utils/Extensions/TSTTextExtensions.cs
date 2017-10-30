using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TeaspoonTools.Utils.Extensions
{
    public static class TSTTextExtensions
    {
        public static void SetOpacity(this Text txt, float opacity)
        {
            txt.color.SetOpacity(opacity);
        }
        
    }
}
