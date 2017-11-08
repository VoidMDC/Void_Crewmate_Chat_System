using UnityEngine;
using UnityEngine.Events;
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
    public class TextboxText : Text, IHasRectTransform
    {
		// events
		[HideInInspector]
		public UnityEvent StartedDisplayingText = new UnityEvent();
		[HideInInspector]
		public UnityEvent DoneDisplayingText = new UnityEvent();

		// basic aspects
		new public RectTransform rectTransform { get; protected set; }

		public float width 
		{ 
			get { return rectTransform.rect.width; } 
			set { rectTransform.sizeDelta = new Vector2 (value, rectTransform.sizeDelta.y); }
		}
		
		public float height 
		{ 
			get { return rectTransform.rect.height; } 
			set { rectTransform.sizeDelta = new Vector2 (rectTransform.sizeDelta.x, value); }
		}
		
		// textbox controller and fields borrowed from it
		protected TextboxController textboxController { get; set; }
		protected TextSettings textSettings { get { return textboxController.textSettings; } }
		protected string textToDisplay { get { return textboxController.textToDisplay; } }
		protected AudioSource sfxPlayer { get { return textboxController.sfxPlayer; } }

        // submodules
		TextSpeedSettings textSpeedSettings;
		TextDisplayer textDisplayer;
		TSTTextWrapper textWrapper;

		// debug stuff
        public List<string> boxfuls
        {
            get { return textboxController.boxfuls; }
            private set { textboxController.boxfuls = value; }
        }
			
		protected override void Awake()
		{
			base.Awake ();
			rectTransform = GetComponent<RectTransform> ();

		}

        public void Initialize(TextboxController tbController, bool showTextImmediately = true)
        {
            textboxController = tbController;

			InitializeBasicAttributes ();
			InitializeSubmodules ();
			WrapText (textboxController.textToDisplay);

			SubscribeToEvents ();

			if (showTextImmediately) 
				DisplayText (textboxController.textToDisplay);
			

        }

		/// <summary>
		/// Signal to the textbox controller that it's time for it to die.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
			
		void InitializeBasicAttributes()
		{
			
			ApplyTextSettings ();
		}

		void ApplyTextSettings()
		{
			this.fontSize = 			textSettings.fontSize;
			this.font = 				textSettings.font;

			// the word-wrapping will be handled by the text parser
			this.horizontalOverflow = 	HorizontalWrapMode.Overflow;
			this.verticalOverflow = 	VerticalWrapMode.Overflow;
		}

		void InitializeSubmodules()
		{
			textSpeedSettings = 	new TextSpeedSettings (textSettings.textSpeed, textSettings.higherTextSpeed);
			textWrapper = 			new TSTTextWrapper(this, textSettings.linesPerTextbox);
			textDisplayer =	 		new TextDisplayer (this, textWrapper.wrappedText, textSpeedSettings, sfxPlayer, 
														textSettings.audioSample);
		}
			
		IList<string> WrapText(string textToWrap)
		{
			// arranges the passed text into chunks that can be neatly displayed in this text field
			// if nothing is passed, the textbox controller's textToDisplay is wrapped.
			// Returns the result.
			IList<string> result =	textWrapper.WrapText (textToWrap);
			return result;
		}

		IList<string> WrapText(IList<string> textToWrap)
		{
			/*
			 * The passed text to wrap may be prewrapped by a TextWrapper object from outside,
			 * but this function makes sure that it's wrapped just right for this text field.
			 */ 

			IList<string> result = new List<string> ();

			for (int i = 0; i < textToWrap.Count; i++)
				result.AddRange (textWrapper.WrapText (textToWrap[i]));
			

			return result;
		}


		public void DisplayText(string textToDisplay)
		{
			IList<string> readyToDisplay = WrapText(textToDisplay) as List<string>;

			textDisplayer.DisplayText (readyToDisplay);

		}

		public void DisplayText(IList<string> textToDisplay)
		{
			if (boxfuls == null)
				boxfuls = new List<string> ();
			
			foreach (string text in textToDisplay) 
				boxfuls.AddRange (WrapText (text));

			textDisplayer.textToDisplay = boxfuls;
			textDisplayer.DisplayText ();
			
		}

		void SubscribeToEvents()
		{
			textDisplayer.DoneDisplayingText.AddListener (DoneDisplayingText.Invoke);
		}

	}
}