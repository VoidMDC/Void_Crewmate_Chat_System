using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TeaspoonTools.TextboxSystem
{
    public static class TSTSpecialObjectCache
    {
        public static Canvas mainCanvas { get { return GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>(); } }
    }
}
