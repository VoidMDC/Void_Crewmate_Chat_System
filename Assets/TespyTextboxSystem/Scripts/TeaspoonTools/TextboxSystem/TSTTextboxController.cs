using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using TeaspoonTools.TextboxSystem.Utils;
using TeaspoonTools.Utils.Extensions;
using TeaspoonTools.Utils;

namespace TeaspoonTools.TextboxSystem
{
    public class TSTTextboxController : TextboxController
    {

    }

    public class TextboxController : MonoBehaviour, IHasRectTransform
    {
        // events
        public event EventHandler<Utils.TextboxEventArgs> StartedShowingText;
		public UnityEvent DoneDisplayingText = new UnityEvent ();
		public bool isDisplayingText = true;

        // basic attributes
		public RectTransform rectTransform { get; protected set; }
		public AudioSource sfxPlayer 
		{
			get {
				GameObject player = GameObject.Find ("SFXPlayer");
				if (player == null) {
					player = new GameObject ("SFXPlayer");
					AudioSource audioSource = player.AddComponent<AudioSource> ();
					return audioSource;

				} 
				else {
					return player.GetComponent<AudioSource>();
				}

			}
		}

		bool isClosing = false;

		// submodules
		public TextboxControllerSubmodules controllerComponents;

		public TextboxBox textboxBox;
		public TextboxText textboxText;
		public TextboxPortrait textboxPortrait;
		public TSTSTextboxNameTag nameTag;
		public TextSettings textSettings;


		// for debugging
		public string textToDisplay;
		public List<string> boxfuls;

        public bool testing = false;

		public string nameTagText {

			get { return nameTag.name; }
			set { nameTag.name = value; }
		}

		private void Awake()
		{
			DoneDisplayingText.AddListener (OnDoneDisplayingText);
		}

        private void Start()
		{
            if (testing)
                InitializeForTesting();

        }
        
		void InitializeForTesting()
        {
			InitializeBasicAttributes (textToDisplay, textSettings.textSpeed, textSettings.linesPerTextbox);
			InitializeSubmodulesForTesting (textToDisplay, textSettings.textSpeed, textSettings.linesPerTextbox);

			AlignSizeToBox();
            SubscribeToEvents();

        }

		void OnDoneDisplayingText()
		{
			isDisplayingText = false;
		}

        public void Initialize(string textToDisplay, TextSpeed textSpeed,
                               int linesPerTextbox)
        {

			InitializeBasicAttributes (textToDisplay, textSpeed, linesPerTextbox);
			InitializeSubmodules (textToDisplay, textSpeed, linesPerTextbox);


			AlignSizeToBox();
			SubscribeToEvents();

        }
        
		void InitializeBasicAttributes(string textToDisplay, TextSpeed textSpeed, int linesPerTextbox)
		{
			rectTransform = GetComponent<RectTransform>();
			this.textToDisplay = textToDisplay;
		}
			
		void InitializeSubmodules(string textToDisplay, TextSpeed textSpeed, int linesPerTextbox)
		{
			textSettings.Initialize(this, textSpeed, linesPerTextbox);
			textSettings.SetAutoFontSize();
			textboxBox.Initialize(this);
			textboxText.Initialize(this, false);

		}

		void InitializeSubmodulesForTesting(string textToDisplay, TextSpeed textSpeed, int linesPerTextbox)
		{
			textSettings.Initialize(this);
			textSettings.SetAutoFontSize();
			textboxBox.Initialize(this);
			textboxText.Initialize(this, true);
		}
			
        void AlignSizeToBox()
        {
            rectTransform.sizeDelta = textboxBox.rectTransform.sizeDelta;
        }
			
        public void StartShowingText()
        {
			isDisplayingText = true;
            textboxText.StartShowingText();
			//StartCoroutine(textboxText.StartShowingTextCoroutine());

			if (StartedShowingText != null)
            	StartedShowingText(this, new Utils.TextboxEventArgs(this));
        }

		public void StartShowingText(string textToShow)
		{
			isDisplayingText = true;
			textboxText.StartShowingText (textToShow);

		}

		public void StartShowingText(List<string> textToShow)
		{
			isDisplayingText = true;
			textboxText.StartShowingText (textToShow);
		}

		public void Close()
		{
			if (!isClosing) {
				isClosing = true;
				StartCoroutine (CloseCoroutine ());
			}
		}

        /// <summary>
        ///  Places a textbox on screen based on the anchor passed. Works off viewport coords.
        /// </summary>
        /// <param name="anchorOnScreen"></param>
		public void PlaceOnScreen(TextboxAnchor anchorOnScreen, bool stayInBounds = true)
        {
			rectTransform.PositionRelativeToParent(anchorOnScreen.ToVector2(), stayInBounds);
			//rectTransform.ApplyAnchorPresetRecursively(anchorOnScreen.ToTextAnchor(), true, true);
        }

		public void PlaceOnScreen(Vector2 anchorOnScreen, bool stayInBounds = true)
		{
			rectTransform.PositionRelativeToParent (anchorOnScreen, stayInBounds);
		}

        void SubscribeToEvents()
        {
            textboxText.TimeToClose += this.OnTimeToClose;
        }

		public void ChangePortrait(Sprite newPortrait)
		{
			textboxPortrait.sprite = newPortrait;
		}

        void OnTimeToClose(object sender, EventArgs args)
        {
            //StartCoroutine(Close());
        }

        IEnumerator CloseCoroutine()
        {
            // squishes this textbox into nonexistence
            //yield return null;

            float currentXScale = transform.localScale.x;
            float scaleStep = 0.25f;
            float pauseDuration = 0.01f;

            while (transform.localScale.x > 0)
            {
                currentXScale -= scaleStep;
                transform.SetLocalXScale(currentXScale);
                yield return new WaitForSeconds(pauseDuration);
            }

            textboxText.TimeToClose -= OnTimeToClose;
			isClosing = false;
            Destroy(this.gameObject);
        }

		void OnDestroy()
		{
			Debug.Log (this.name + " has been destroyed!");
			Textbox.textboxesOnScreen--;
		}
    }

}