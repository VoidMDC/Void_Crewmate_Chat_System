using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using TeaspoonTools.Utils;
using TeaspoonTools.TextboxSystem.Utils;
using TeaspoonTools.Utils.Extensions;

namespace TeaspoonTools.TextboxSystem
{
    public class TSTTextboxText: TextboxText { }

	/// <summary>
	///  A helper script for TextboxController, meant to be attached to the
	/// object that will contain the text in the prefab. 
	/// 
	/// Handles the process of text-parsing and text-displaying.
	/// </summary>
    public class TextboxText : Text, ITextboxComponent, IHasRectTransform
    {
		// events
        public event EventHandler TimeToClose;

		// basic aspects
		new public RectTransform rectTransform { get; protected set; }

		public float width { get { return rectTransform.rect.width; } 
			set { rectTransform.sizeDelta = new Vector2 (value, rectTransform.sizeDelta.y); }}
		
		public float height { get { return rectTransform.rect.height; } 
			set { rectTransform.sizeDelta = new Vector2 (rectTransform.sizeDelta.x, value); }}
		
		// textbox controller and fields borrowed from it
		public TextboxController textboxController { get; protected set; }
		string textToDisplay { get { return textboxController.textToDisplay; } }
		TextSettings textSettings { get { return textboxController.textSettings; } }
		AudioSource sfxPlayer { get { return textboxController.sfxPlayer; } }

        // submodules
		TextSpeedSettings textSpeedSettings;
		TextDisplayer textDisplayer;
		TSTTextParser textParser;

		// debug stuff
        public List<string> boxfuls
        {
            get { return textboxController.boxfuls; }
            private set { textboxController.boxfuls = value; }
        }
			
		bool queuedToShowText = false;
		bool readyToShowText = false;

        public void Initialize(TextboxController tbController, bool showTextImmediately = true)
        {
            textboxController = tbController;

			InitializeBasicAttributes ();
			InitializeSubmodules ();
			GetTextParsed ();
			SubscribeToEvents ();
			//StartCoroutine(GetTextParsedCoroutine());

			if (showTextImmediately) {
				StartShowingText ();
				//StartCoroutine(StartShowingTextCoroutine());
			}
			
            
        }

		/// <summary>
		/// Signal to the textbox controller that it's time for it to die.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void OnDoneDisplayingText(object sender, EventArgs e)
		{
			
			if (TimeToClose != null) {
				TimeToClose (this, new EventArgs ());
				textboxController.DoneDisplayingText.Invoke ();
			}
		}

		void OnDoneParsingText(object sender, EventArgs e)
		{
			boxfuls = textParser.parsedText;
			readyToShowText = true;
		}

		void InitializeBasicAttributes()
		{
			rectTransform = GetComponent<RectTransform> ();
			ApplyTextSettings ();
		}

		void ApplyTextSettings()
		{
			this.fontSize = textSettings.fontSize;
			this.font = textSettings.font;

			// the word-wrapping will be handled by the text parser
			this.horizontalOverflow = HorizontalWrapMode.Overflow;
			this.verticalOverflow = VerticalWrapMode.Overflow;
		}

		void InitializeSubmodules()
		{
			textSpeedSettings = new TextSpeedSettings (textSettings.textSpeed, textSettings.higherTextSpeed);
			textParser = new TSTTextParser(this, textToDisplay, textSettings.linesPerTextbox);
			textDisplayer = new TextDisplayer (this, textParser.parsedText, textSpeedSettings, sfxPlayer, textSettings.audioSample);
		}
			
		void GetTextParsed()
		{
			textParser.ParseText ();
			boxfuls = textParser.parsedText;
		}

		IEnumerator GetTextParsedCoroutine()
		{
			StartCoroutine (textParser.ParseTextCoroutine ());
			yield break;
		}

		public IEnumerator StartShowingTextCoroutine()
		{

			while (true){
				if (readyToShowText) {
					textDisplayer.textToDisplay = textParser.parsedText;
					textDisplayer.DisplayText ();

				}
				yield return null;
			}
		}

		public void StartShowingText()
		{
			
			textDisplayer.textToDisplay = textParser.parsedText;
			textDisplayer.DisplayText ();

		}

		public void StartShowingText(string textToShow)
		{
			textDisplayer.textToDisplay = new List<string>() {textToShow};
			textDisplayer.DisplayText ();
		}

		void SubscribeToEvents()
		{
			textDisplayer.DoneDisplayingText += OnDoneDisplayingText;
			textParser.DoneParsingText += OnDoneParsingText;
		}

	}
}