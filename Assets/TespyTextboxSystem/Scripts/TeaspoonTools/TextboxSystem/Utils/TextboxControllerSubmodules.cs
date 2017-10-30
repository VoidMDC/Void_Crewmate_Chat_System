using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TeaspoonTools.TextboxSystem;

namespace TeaspoonTools.TextboxSystem.Utils
{
	[System.Serializable]
	public class TextboxControllerSubmodules
	{

		TextboxController textboxController;

		public TextboxBox textboxBox;
		public TextboxText textboxText;
		public TextboxPortrait textboxPortrait;
		public TextSettings textSettings;


		public TextboxControllerSubmodules ()
		{
			
		}

		public void Initialize(string textToDisplay, TextSpeed textSpeed, int linesPerTextbox)
		{
			textSettings.Initialize(textboxController, textSpeed, linesPerTextbox);
			textSettings.SetAutoFontSize();
			textboxText.Initialize(textboxController, false);
			textboxBox.Initialize(textboxController);
		}
	}
}

