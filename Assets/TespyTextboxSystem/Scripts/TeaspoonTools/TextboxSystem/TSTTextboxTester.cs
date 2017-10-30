using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using TeaspoonTools.TextboxSystem.Utils;

namespace TeaspoonTools.TextboxSystem
{
    public class TSTTextboxTester : TextboxTester
    {

    }

    public class TextboxTester : MonoBehaviour
    {

        public string textToDisplay;

        public Vector2 anchoredPos = new Vector2(0.5f, 0.5f);
        bool textboxIsThere = false;

        public TextboxAnchor enumAnchor = TextboxAnchor.LowerCenter;
        public bool useEnum = true;

        public GameObject textboxPrefab;
        GameObject textbox;
        TextboxController textboxController;

        Canvas mainCanvas { get { return GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>(); } }

        private void FixedUpdate()
        {
            SpawnTextboxOnInput();
            
			textboxIsThere = Textbox.textboxesOnScreen > 0;
            
        }

        void SpawnTextboxOnInput()
        {
            if (Input.GetKey(KeyCode.P) && !textboxIsThere)
            {

                textbox = Textbox.Create(textboxPrefab, 
                                         textToDisplay, 2);
                textbox.transform.SetParent(mainCanvas.transform, false);
                textboxController = textbox.GetComponent<TextboxController>();

                
                if (useEnum)
                    textboxController.PlaceOnScreen(enumAnchor);
                else
                    textboxController.PlaceOnScreen(anchoredPos);
                
                textboxController.StartShowingText();
                
                textboxIsThere = true;

            }
        }

    }
}