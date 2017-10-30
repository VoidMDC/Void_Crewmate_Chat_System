using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace TeaspoonTools.TextboxSystem.Utils
{
	public class TextboxEventArgs : EventArgs
	{
		public GameObject textbox { get; private set; }
		public TextboxController textboxController { get; private set; }
		public TextboxBox textboxBox { get; private set; }
		public TextboxText textboxText { get; private set; }
		public string textToDisplay { get; private set; }
		public float width { get; private set; }
		public float height { get; private set; }

		public TextboxEventArgs(TextboxController textboxController)
		{
			this.textboxController = textboxController;
			CacheProperties ();
		}

		void CacheProperties()
		{
			textbox = textboxController.gameObject;
			textboxBox = textboxController.textboxBox;
			textboxText = textboxController.textboxText;

			textToDisplay = textboxController.textToDisplay;
			width = textboxBox.width;
			height = textboxBox.height;
		}
	}
}

