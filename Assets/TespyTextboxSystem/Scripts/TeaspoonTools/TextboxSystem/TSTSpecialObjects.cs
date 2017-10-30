using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace TeaspoonTools.TextboxSystem
{
	public static class TSTSpecialObjects
	{
		public static GameObject testLabel {
			get {
				GameObject tLabel = new GameObject ("TestLabel");
				tLabel.AddComponent<RectTransform> ();
				Text tText = tLabel.AddComponent<Text> ();
				tText.color = Color.black;

				ContentSizeFitter cSizeFitter = tLabel.AddComponent<ContentSizeFitter> ();
				cSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
				cSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

				tLabel.AddComponent<CanvasGroup> ();
				return tLabel;

			}
		}
	}
}

