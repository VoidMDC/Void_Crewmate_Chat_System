using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace TeaspoonTools.TextboxSystem.Utils
{

    class TextAdjustmentHelper
    {
        // contains data to help auto adjust the font size, and other things related to
        // displaying the text
        public GameObject testLabel;
		public Text labelText;
		public RectTransform labelRect;
		TextSettings helpee;
        public List<int> sizesUsed;
        public int sizeChangeStep = 2;
		CanvasGroup canvasGroup;
    
        int lineLimit;

        public TextAdjustmentHelper(TextSettings helpee)
        {
			this.helpee = helpee;
        }

        public void Initialize(int lineLimit)
        {
            sizesUsed = new List<int>();
            this.lineLimit = lineLimit;

			testLabel = CreateTestLabel();
			testLabel.name = "PrefabTestingLabel";
			labelText = testLabel.GetComponent<Text> ();
			labelRect = testLabel.GetComponent<RectTransform> ();
			canvasGroup = testLabel.GetComponent<CanvasGroup> ();

            MakeLabelInvisible();
        }

        public void SyncLabelText(Font font, int fontSize)
        {
            // Empties the label text's text, and makes it have the font and font size passed.
            // For max accuracy in auto-adjusting the font size.
            labelText.text = "";
            labelText.font = font;
            labelText.fontSize = fontSize;
        }
        void MakeLabelInvisible()
        {
            // makes the test label invisible
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        GameObject CreateTestLabel()
        {
			return TSTSpecialObjects.testLabel;

        }


    }
}
