using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace TeaspoonTools.TextboxSystem
{
    /// <summary>
    /// For things like instantiating TST Textbox objects programmatically.
    /// </summary>
    public static class Textbox
    {
		public static int textboxesOnScreen = 0;
        public static event EventHandler<Utils.TextboxEventArgs> AnyTextboxSpawned;


        public static GameObject Create(string prefabPath,
                                        string textToDisplay = "",
                                        int linesPerTextbox = 3,
                                        TextSpeed textSpeed = TextSpeed.medium)
        {
            GameObject textbox = MonoBehaviour.Instantiate<GameObject>(Resources.Load<GameObject>
                                                                      (prefabPath));
            textbox.SetActive(true);

            TextboxController textboxController = textbox.GetComponent<TextboxController>();

            // for safety
			if (textboxController == null) {
				string errorMessage = "Prefab passed in TST Textbox instantiation has no TST Textbox Controller.";
				throw new ArgumentException (errorMessage);
			}

            textboxController.Initialize(textToDisplay, textSpeed, linesPerTextbox);

			textboxesOnScreen++;
			if (AnyTextboxSpawned != null)
            	AnyTextboxSpawned(null, new Utils.TextboxEventArgs(textboxController));
            return textbox;
        }

        public static GameObject Create(GameObject prefab,
                                        string textToDisplay = "",
                                        int linesPerTextbox = 3,
                                        TextSpeed textSpeed = TextSpeed.medium)
        {
            GameObject textbox = MonoBehaviour.Instantiate<GameObject>(prefab);
            textbox.SetActive(true);

            TextboxController textboxController = textbox.GetComponent<TextboxController>();

			// for safety
			if (textboxController == null) {
				string errorMessage = "Prefab passed in TST Textbox instantiation has no TST Textbox Controller.";
				throw new ArgumentException (errorMessage);
			}

            textboxController.Initialize(textToDisplay, textSpeed, linesPerTextbox);

			textboxesOnScreen++;
			if (AnyTextboxSpawned != null)
				AnyTextboxSpawned(null, new Utils.TextboxEventArgs(textboxController));
            return textbox;
        }
    }
}
