using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TeaspoonTools.TextboxSystem.Utils
{

	/// <summary>
	/// Module for the TST Text monobehaviour to, of course, parse the text.
	/// </summary>
	public class TSTTextParser
	{
		// currently working on making this parse the text with coroutines

		// events
		public event EventHandler DoneParsingText;
		private event EventHandler DoneSplittingIntoWords;
		private event EventHandler DoneGroupingIntoLines;


		public string textToParse;
		public List<string> parsedText;
		// ^ arranged into boxfuls
		List<string> lines;
		List<string> words;
		public Text textObj;
		public int linesPerTextbox = 3;

		int charsToParsePerFrame = 500;

		public TSTTextParser()
		{
			SubscribeToEvents ();

			InitializeParsingContainers ();
		}

		public TSTTextParser (Text textObj, string textToParse, int linesPerTextbox)
		{
			this.textObj = textObj;
			this.textToParse = string.Copy(textToParse);

			this.linesPerTextbox = linesPerTextbox;
			SubscribeToEvents ();

			InitializeParsingContainers ();

		}

		void InitializeParsingContainers()
		{
			lines = new List<string> (); 
			words = new List<string>();
			parsedText = new List<string>();

		}

		void SubscribeToEvents()
		{

		}

		// parsing functions
		public void ParseText()
		{
			
			words 			= 		new List<string>(SplitIntoWords(textToParse));
			lines 			= 		new List<string>(GroupIntoLines(words));
			parsedText 		= 		new List<string>(GroupIntoBoxfuls(lines));

			RemoveEmptyBoxfuls();

		}

		public IEnumerator ParseTextCoroutine()
		{
			// starts a coroutine chain, continued by event responses
			textObj.StartCoroutine (SplitIntoWordsCoroutine ());
			yield return null;
		}

		IEnumerator SplitIntoWordsCoroutine()
		{
			textToParse.Trim ();
			char separator = ' ';

			int charsToParse = textToParse.Length;
			int parseCounter = 0;

			string currentWord;
			char currentChar;

			// as the words are parsed, they are removed from the text to parse
			while (textToParse.Length > 0) {


				bool timeToTakeABreak = parseCounter >= charsToParsePerFrame;
				if (timeToTakeABreak)
				{
					yield return null;
					parseCounter = 0;
				}


				int nextSeparatorIndex = textToParse.IndexOf (separator);

				if (nextSeparatorIndex >= 0) {
					currentWord = textToParse.Substring (0, nextSeparatorIndex);
					words.Add (currentWord);
					parseCounter += currentWord.Length + 1;
					textToParse = textToParse.Substring (nextSeparatorIndex + 1);
				}
			}

			if (DoneSplittingIntoWords != null)
				DoneSplittingIntoWords (this, null);

			yield return null;
			/*
			for (int i = 0; i < charsToParse; i++) {
				currentWord = "";

				bool timeToTakeABreak = parseCounter >= charsToParsePerFrame;
				if (timeToTakeABreak)
				{
					yield return new WaitForFixedUpdate();
					parseCounter = 0;
				}
					
				int nextSeparatorIndex = textToParse.Substring(i).IndexOf (separator);
				if (nextSeparatorIndex != 0) {
					currentWord = textToParse.Substring (i, nextSeparatorIndex - i);
					words.Add (currentWord);
					parseCounter += currentWord.Length;
					i += currentWord.Length;

				}

			}
			*/

		}

		IEnumerator GroupIntoLinesCoroutine()
		{
			// use a label prefab as a measuring stick
			GameObject testingLabel = SetupTestLabel();

			// cached components and values
			Text labelText = testingLabel.GetComponent<Text> ();
			RectTransform labelRect = testingLabel.GetComponent<RectTransform> ();

			int wordsToGoThrough = words.Count;
			float widthBoundary = textObj.rectTransform.rect.width;

			// helps with the word wrapping
			string prevText = "";

			string currentWord = "";
			int parseCounter = 0;

			// add words until they make a full line on this textbox, stopping at intervals
			for (int i = 0; i < wordsToGoThrough; i++)
			{
				
				bool timeToTakeABreak = parseCounter >= charsToParsePerFrame;
				if (timeToTakeABreak) {
					yield return new WaitForFixedUpdate ();
					parseCounter = 0;
				}

				currentWord = words[i];
				labelText.text += currentWord + " ";

				Canvas.ForceUpdateCanvases();
				// ^ will not work without this

				// evaluate the current state of the label text
				bool onLastWord = i == wordsToGoThrough - 1;
				bool atBoundary = labelRect.rect.width == widthBoundary;
				bool pastBoundary = labelRect.rect.width > widthBoundary;
				bool labelHasSomething = !string.IsNullOrEmpty(labelText.text);

				if (pastBoundary && i != 0)
				{
					// do some word wrapping. The i != 0 part is to avoid the first line being
					// empty if there is just one overly-long word to show
					lines.Add(prevText + "\n");
					labelText.text = currentWord + ' ';
				}

				if (atBoundary || (onLastWord && labelHasSomething))
				{
					// no words left behind! That is the american dream!
					lines.Add(labelText.text + "\n");
					labelText.text = "";
				}

				prevText = labelText.text;
				parseCounter += currentWord.Length + 1;
			}

			if (DoneGroupingIntoLines != null)
				DoneGroupingIntoLines (this, null);

			// won't need this anymore!
			MonoBehaviour.Destroy(testingLabel);
		}

		IEnumerator GroupIntoBoxfulsCoroutine()
		{
			int linesToGoThrough = lines.Count;
			string currentBoxful = "";
			string currentLine;

			int parseCounter = 0;

			for (int i = 0; i < linesToGoThrough; i++) {
				
				bool timeToTakeABreak = parseCounter >= charsToParsePerFrame;
				if (timeToTakeABreak) {
					yield return new WaitForFixedUpdate ();
					parseCounter = 0;
				}


				currentLine = lines [i];
				currentBoxful += currentLine;

				bool fullBoxful = ( (i + 1) % linesPerTextbox) == 0;
				bool atLastLine = i == linesToGoThrough - 1;
				bool boxfulHasSomething = !string.IsNullOrEmpty (currentBoxful);

				if (fullBoxful || (atLastLine && boxfulHasSomething))
				{
					parsedText.Add(currentBoxful);
					currentBoxful = "";
				}

				parseCounter += currentLine.Length;

			}

			if (DoneParsingText != null)
				DoneParsingText (this, null);
		}

		IList<string> SplitIntoWords(string textToSplit)
		{
			// helper function for ParseText
			IList<string> words = new List<string>(textToSplit.Split(' '));

			return words;
		}

		IList<string> GroupIntoLines(IList<string> words)
		{
			// helper function for ParseText

			IList<string> lines = new List<string>();

			// use a label prefab as a measuring stick
			GameObject testingLabel 	= 		SetupTestLabel();
			Text labelText 				= 		testingLabel.GetComponent<Text>();
			RectTransform labelRect 	= 		testingLabel.GetComponent<RectTransform>();

			// cached values
			float widthBoundary = textObj.rectTransform.rect.width;

			// helps with the word wrapping
			string prevText = "";

			// add words until they make a full line on this textbox
			for (int i = 0; i < words.Count; i++)
			{
				
				labelText.text += words[i] + " ";

				Canvas.ForceUpdateCanvases();
				// ^ will not work without this

				// evaluate the current state of the label text
				bool onLastWord 		= 		i == words.Count - 1;
				bool atOrPastBoundary 	= 		labelRect.rect.width >= widthBoundary;
				bool labelHasSomething 	= 		!string.IsNullOrEmpty(labelText.text);

				if (atOrPastBoundary && i > 0)
				{
					// do some word wrapping. The i != 0 part is to avoid the first line being
					// empty if there is just one overly-long word to show
					lines.Add(prevText + "\n");
					labelText.text = words[i] + " ";
				}

				if (onLastWord && labelHasSomething)
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
			// helper function for ParseText

			IList<string> result = new List<string>();

			string boxful = "";

			// helps evaluate when to add what was in the boxful
			bool fullBoxful;
			bool atLastLine;

			// when the current boxful gets enough content, or we're at the last line to
			// parse, register it in the parsedText list
			for (int i = 0; i < lines.Count; i++)
			{
				boxful += lines[i];

				fullBoxful 		= 		((i + 1) % linesPerTextbox) == 0;
				atLastLine 		= 		i == lines.Count - 1;

				if (fullBoxful || atLastLine)
				{
					result.Add(boxful);
					boxful = "";
				}

			}

			return result;
		}

		void RemoveEmptyBoxfuls()
		{
			// takes care of a glitch during text-parsing that leaves empty parsedText
			// to print, which leads to empty textboxes shown when they shouldn't be

			int boxfulsToGoThrough = parsedText.Count;
			for (int i = 0; i < boxfulsToGoThrough; i++)
				if (string.IsNullOrEmpty(parsedText[i]) || !parsedText[i].hasLettersOrDigits())
					parsedText.Remove(parsedText[i]);
		}
	
		GameObject SetupTestLabel()
		{
			// sets up the test label to be used as a measuring stick
			GameObject testingLabel = TSTSpecialObjects.testLabel;
			testingLabel.name = "LineGroupTestingLabel";

			Text labelText = testingLabel.GetComponent<Text>();
			RectTransform labelRect = testingLabel.GetComponent<RectTransform>();

			labelRect.SetParent(textObj.transform.parent, false);

			labelText.rectTransform.localScale = textObj.rectTransform.localScale;
			labelText.text = "";
			labelText.font = textObj.font;
			labelText.fontSize = textObj.fontSize;

			return testingLabel;
		}
	
	}
	
}

