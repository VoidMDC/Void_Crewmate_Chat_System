using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;


namespace TeaspoonTools.TextboxSystem.Utils
{

    [System.Serializable]
    public class TextSettings
    {

        public AudioClip audioSample = null;
        TextAdjustmentHelper adjustmentHelper;
        public TextSpeed textSpeed = TextSpeed.medium;

        [HideInInspector]
        public TextSpeed higherTextSpeed;
        [HideInInspector]
        public TextSpeed effectiveTextSpeed = TextSpeed.medium;

        [Range(1, 1000)]
        public int linesPerTextbox = 3;
        public string textToDisplay { get { return textboxController.textToDisplay; } }
        const string defaultFontPath = "Fonts/Marker Felt";
        public Font font;
        public int fontSize = 12;
        public bool autoFontSize = true;

        private TextboxController textboxController;
        private TextboxText textboxText { get { return textboxController.textboxText; } }


        public void Initialize(TextboxController tbText)
        {
            this.textboxController = tbText;

			if (font == null)
				LoadDefaultFont ();

            
            adjustmentHelper = new TextAdjustmentHelper(this);
            adjustmentHelper.Initialize(linesPerTextbox);
            //SetInitialFontSize();
            SetHigherSpeed();


        }


        public void Initialize(TextboxController textboxController,
			TextSpeed textSpeed, int linesPerTextbox)
        {
			this.textboxController = textboxController;

			if (font == null)
				LoadDefaultFont ();

            this.linesPerTextbox = linesPerTextbox;
            adjustmentHelper = new TextAdjustmentHelper(this);
            adjustmentHelper.Initialize(linesPerTextbox);

            this.textSpeed = textSpeed;
			
            SetHigherSpeed();

        }

        public void SetHigherSpeed()
        {
            switch (textSpeed)
            {
                case TextSpeed.verySlow:
                    higherTextSpeed = TextSpeed.slow;
                    break;
                case TextSpeed.slow:
                    higherTextSpeed = TextSpeed.medium;
                    break;
                case TextSpeed.medium:
                    higherTextSpeed = TextSpeed.fast;
                    break;
                case TextSpeed.fast:
                    higherTextSpeed = TextSpeed.instant;
                    break;
                case TextSpeed.instant:
                    higherTextSpeed = TextSpeed.instant;
                    break;

                default:
                    throw new System.NotImplementedException("Text speed not accounted for in text settings.");

            }

            //Debug.Log("Higher speed is:" + higherTextSpeed);

        }
        
        /// <summary>
        /// Automatically calculates the font size, so that it's just as big (or small) as it needs to be to fill the textbox with a given number of lines at a time.
        /// </summary>
        public void SetAutoFontSize()
        {
			
            // Far better than the old algorithm! Hurray for number-crunching!

            // use the label prefab as a measuring stick
			Text labelText = adjustmentHelper.labelText;

			adjustmentHelper.labelRect.SetParent (textboxController.transform.parent, false);

            int testFontSize = 5;
            adjustmentHelper.SyncLabelText(font, testFontSize);
            Canvas.ForceUpdateCanvases();

            // now applying the algorithm
			RectTransform textRect = textboxText.GetComponent<RectTransform>();

            float minSize = adjustmentHelper.labelRect.rect.height;
            float textHeight = textRect.rect.height;
            float targetHeight = textHeight / linesPerTextbox;

            float linesFittable = Mathf.Ceil(targetHeight / minSize);

            int resultSize = (int)Mathf.Ceil( linesFittable * testFontSize / minSize);

            // to adjust for other resolutions
            labelText.text = "";
            for (int i = 0; i < linesPerTextbox - 1; i++)
                labelText.text += "A\nA";

            labelText.fontSize = resultSize;
            Canvas.ForceUpdateCanvases();


            // Sometimes, the chosen font size is too big or small after the initial
            // number-crunch. So, the following should fix that.
			float diff = adjustmentHelper.labelRect.rect.height - textHeight;
            float diffInLines = Mathf.Abs(diff / targetHeight);
            float alterationAmount;
            int passes = 0;

            while (diffInLines >= 1f)
            {
                alterationAmount = Mathf.Ceil(diffInLines);
                
                if (diff > 0)
                    resultSize -= (int)alterationAmount;
                    
                else if (diff < 0)
                    resultSize += (int)alterationAmount;
                
                // It's a bit like in the old algorithm, where the testing label's 
                // height is compared to the text field's height to calculate the size 
                // more accurately. In this version, there is a lot less potential lag! 
                labelText.fontSize = resultSize;
                Canvas.ForceUpdateCanvases();
				diff = adjustmentHelper.labelRect.rect.height - textHeight;

                diffInLines = Mathf.Abs(diff / minSize);
                
                passes++;

                // avoid an infinite loop
                if (passes >= linesPerTextbox * 2)
                    break;

            }

			Debug.Log ("Adjusted the font size to best fit the textbox with " + passes + " extra passes.");

            fontSize = resultSize;

            Debug.Log("Using the simpler, better algorithm, the font size chosen is: " + resultSize);
            textboxText.fontSize = fontSize;

            // won't need this anymore
			MonoBehaviour.Destroy(labelText.gameObject);

        }
        
		void LoadDefaultFont()
		{
			Debug.Log("Font was null! Loading default!");
			font = Resources.Load<Font>(defaultFontPath);
		}
    }

}