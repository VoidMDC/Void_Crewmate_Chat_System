using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class Prefabs
{
    // caches various prefabs you may want to use
    public static class PrefabPaths
    {
        public static string testingLabel = "Prefabs/TestingLabel";
    }
    public static GameObject testingLabel {
        get
        {
            return Resources.Load<GameObject>(PrefabPaths.testingLabel);
        }
    }
}
