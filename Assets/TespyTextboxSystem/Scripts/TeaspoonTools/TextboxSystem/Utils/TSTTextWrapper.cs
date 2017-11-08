using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TeaspoonTools.TextboxSystem.Utils
{

	/// <summary>
	/// Module for the TST Text monobehaviour to, of course, parse the text.
	/// </summary>
	public class TSTTextWrapper
	{
		// currently working on making this parse the text with coroutines

		public List<string> wrappedText;
		// ^ arranged into boxfuls
		List<string> lines;
		List<string> words;
		public Text textField { get; set; }
		int linesPerTextbox;

        public TSTTextWrapper()
        {
            SubscribeToEvents();
        }

		public TSTTextWrapper (Text textField, int linesPerTextbox) : this()
		{
			this.textField = textField;
			this.linesPerTextbox = linesPerTextbox;

		}
			
		void SubscribeToEvents()
		{

		}

		// wrapping functions

		public IList<string> WrapText(string textToWrap)
		{
			words = 			new List<string>(SplitIntoWords(textToWrap));
			lines = 			new List<string>(GroupIntoLines(words));
			wrappedText = 		new List<string>(GroupIntoBoxfuls(lines));

			RemoveEmptyBoxfuls();

			return wrappedText;

		}

		public IList<string> WrapText(IList<string> textToWrap)
		{
			words = 			new List<string>(SplitIntoWords(textToWrap));
			lines = 			new List<string>(GroupIntoLines(words));
			wrappedText = 		new List<string>(GroupIntoBoxfuls(lines));

			RemoveEmptyBoxfuls();

			return wrappedText;

		}
			
		#region helper functions
		IList<string> SplitIntoWords(string textToSplit)
		{
			// helper function for WrapText
			return new List<string>(textToSplit.Split(' '));
		}

		IList<string> SplitIntoWords(IList<string> textToSplit)
		{
			IList<string> wordsSplit = new List<string> ();

			foreach (string text in textToSplit) 
				wordsSplit.AddRange (text.Split(' '));
			
			return wordsSplit;
		}

		IList<string> GroupIntoLines(IList<string> words)
		{
			// helper function for WrapText

			IList<string> lines = new List<string>();

			// use a label prefab as a measuring stick
			GameObject testingLabel = 	SetupTestLabel();
			Text labelText = 			testingLabel.GetComponent<Text> ();
			RectTransform labelRect = 	testingLabel.GetComponent<RectTransform> ();

			float widthBoundary = 		textField.rectTransform.rect.width;

			// helps with the word wrapping
			string prevText = "";

			string currentWord;

			// add words until they make a full line on this textbox
			for (int i = 0; i < words.Count; i++)
			{
				currentWord = 			words [i];
				labelText.text += 		currentWord + ' ';

				Canvas.ForceUpdateCanvases();
				// ^ will not work without this

				// evaluate the current state of the label text
				bool onLastWord = 			i == words.Count - 1;
				bool atOrPastBoundary = 	(labelRect.rect.width >= widthBoundary);

				if (atOrPastBoundary && i > 0)
				{
					// do some word wrapping. The i != 0 part is to avoid the first line being
					// empty if there is just one overly-long word to show
					
					lines.Add(prevText + "\n");
					labelText.text = currentWord + ' ';
				}

				if (onLastWord)
				{
					// no words left behind! That is the american dream!
					lines.Add(labelText.text);
					labelText.text = "";
				}

				prevText = labelText.text;
			}
				
			// won't need this anymore!
			MonoBehaviour.Destroy(testingLabel);
			return lines;
		}

		IList<string> GroupIntoBoxfuls(IList<string> lines)
		{
			// helper function for WrapText

			IList<string> result = new List<string>();

			int linesToGoThrough = lines.Count;
			StringBuilder boxFul = new StringBuilder();

			// when the current boxful gets enough content, or we're at the last line to
			// parse, register it in the wrappedText list
			for (int i = 0; i < linesToGoThrough; i++)
			{
				boxFul.Append( lines[i]);

				bool fullBoxful = ((i + 1f) % linesPerTextbox) == 0;
				bool atLastLine = i == linesToGoThrough - 1;

				if (fullBoxful || atLastLine)
				{
					// make sure the last character in this boxful isn't a newline
					// before adding it
					string lastChar = boxFul[boxFul.Length - 1] + "";
					if (lastChar == "\n")
						boxFul.Remove (boxFul.Length - 1, 1);

					result.Add(boxFul.ToString());
					boxFul = new StringBuilder();
				}

			}

			return result;
		}

		void RemoveEmptyBoxfuls()
		{
			// takes care of a glitch during text-parsing that leaves empty wrappedText
			// to print, which leads to empty textboxes shown when they shouldn't be

			int boxfulsToGoThrough = wrappedText.Count;
			for (int i = 0; i < boxfulsToGoThrough; i++)
				if (string.IsNullOrEmpty(wrappedText[i]) || !wrappedText[i].hasLettersOrDigits())
					wrappedText.Remove(wrappedText[i]);
		}
	
		GameObject SetupTestLabel()
		{
			// sets up the test label to be used as a measuring stick
			GameObject testingLabel = 	new GameObject("LineGroupTestingLabel");

			RectTransform labelRect = 	testingLabel.AddComponent<RectTransform>();
			Text labelText = 			testingLabel.AddComponent<Text>();

			labelRect.SetParent(textField.transform.parent, false);

			labelText.text = 			"";
			labelText.font = 			textField.font;
			labelText.fontSize = 		textField.fontSize;

			ContentSizeFitter sizeFitter = testingLabel.AddComponent<ContentSizeFitter> ();
			sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
			sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

			return testingLabel;
		}

		#endregion
	}
	
}

