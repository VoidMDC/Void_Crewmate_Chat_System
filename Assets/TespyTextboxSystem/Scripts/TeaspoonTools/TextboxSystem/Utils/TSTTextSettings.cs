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
		TextboxText textboxText { get { return textboxController.text; } }

        public void Initialize(TextboxController tbText)
        {
            this.textboxController = tbText;

			if (font == null)
				LoadDefaultFont ();

            SetHigherSpeed();

			if (autoFontSize)
				SetAutoFontSize ();

        }


        public void Initialize(TextboxController textboxController,
			TextSpeed textSpeed, int linesPerTextbox)
        {
			this.textboxController = textboxController;

			if (font == null)
				LoadDefaultFont ();

            this.linesPerTextbox = linesPerTextbox;
            this.textSpeed = textSpeed;

			if (autoFontSize)
				SetAutoFontSize ();
			
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

            // set up a label prefab as a measuring stick
			GameObject testLabel = 		CreateTestLabel();
			Text labelText = 			testLabel.GetComponent<Text> ();
			RectTransform labelRect = 	testLabel.GetComponent<RectTransform> ();

			labelRect.SetParent (textboxController.rectTransform.parent, false);

			// Using a small font size and seeing how tall the label gets with one line of
			// text, use that to figure out a good font size for the main text field to use
            int baseFontSize = 5;
			labelText.fontSize = 		baseFontSize;
			labelText.text = "A";
			
            Canvas.ForceUpdateCanvases();
			float baseHeight = 			labelRect.rect.height;

			//  further preparations to calculate the aforementioned good size
			RectTransform textRect = 	textboxText.rectTransform;
			float heightLimit = 		textRect.rect.height;
			float targetHeightPerLine = heightLimit / linesPerTextbox;

			float linesFittable = 		Mathf.Ceil(targetHeightPerLine / baseHeight); // with the current font size
			int resultSize = 			Mathf.FloorToInt (baseFontSize * (targetHeightPerLine / baseHeight));

			// now test that good size...
            labelText.text = 			"";
			labelText.fontSize = 		resultSize;
            for (int i = 0; i < linesPerTextbox - 1; i++)
                labelText.text += "A\nA";
				            
            Canvas.ForceUpdateCanvases();

			float diffInHeight = 		labelRect.rect.height - heightLimit;
			float diffInLines = 		Mathf.Abs(diffInHeight / targetHeightPerLine); 
			// ^lines taken up

            int alterationAmount;
            int passes = 0; // just for debugging
			float differenceLimit = 0.1f;

            while (diffInLines >= differenceLimit) // ... and change it as necessary.
            {
                alterationAmount = Mathf.CeilToInt(diffInLines);
                
				// Raise the size? Lower it?
                if (diffInHeight > 0)
                    resultSize -= alterationAmount;
                    
                else if (diffInHeight < 0)
                    resultSize += alterationAmount;
                
				// compare the text heights again for a more accurate result
                labelText.fontSize = resultSize;
                Canvas.ForceUpdateCanvases();
				diffInHeight = labelRect.rect.height - heightLimit;

                diffInLines = Mathf.Abs(diffInHeight / baseHeight);
                
                passes++;

                // avoid an infinite loop, cutting our losses
                if (passes >= linesPerTextbox * 2)
                    break;

            }

			fontSize = resultSize;

			Debug.Log ("Adjusted the font size to best fit the textbox with " + passes + " extra passes.");
            Debug.Log("Using the simpler, better algorithm, the font size chosen is: " + resultSize);
      
            // won't need this anymore!
			MonoBehaviour.Destroy(labelText.gameObject);

        }
        
		void LoadDefaultFont()
		{
			Debug.Log("Font was null! Loading default!");
			font = Resources.Load<Font>(defaultFontPath);
		}

		GameObject CreateTestLabel()
		{
			// to help with the auto-font-sizing

			GameObject label = new GameObject ("PrefabTestingLabel");
			label.AddComponent<RectTransform> ();

			Text labelText = label.AddComponent<Text> ();
			labelText.text = "";
			labelText.font = font;
			labelText.fontSize = 5;

			ContentSizeFitter sizeFitter = label.AddComponent<ContentSizeFitter> ();
			sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

			// make the label invisible
			CanvasGroup canvasGroup = label.AddComponent<CanvasGroup> ();
			canvasGroup.alpha = 0;
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;

			return label;
		}
    }

}