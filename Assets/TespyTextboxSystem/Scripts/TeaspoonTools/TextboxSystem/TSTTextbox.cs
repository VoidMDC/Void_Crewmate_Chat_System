using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TeaspoonTools.TextboxSystem
{
	public class TextboxEvent : UnityEvent<TextboxController> {}

    /// <summary>
    /// For things like instantiating TST Textbox objects programmatically.
    /// </summary>
    public static class Textbox
    {
		public static int textboxesOnScreen = 0;
		public static TextboxEvent ATextboxSpawned = new TextboxEvent ();


        public static GameObject Create(string prefabPath, int linesPerTextbox = 3,
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

            textboxController.Initialize(textSpeed, linesPerTextbox);

			textboxesOnScreen++;
			ATextboxSpawned.Invoke (textboxController);
            return textbox;
        }

        public static GameObject Create(GameObject prefab,
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

            textboxController.Initialize(textSpeed, linesPerTextbox);

			textboxesOnScreen++;
			ATextboxSpawned.Invoke (textboxController);
            return textbox;
        }
    }
}
