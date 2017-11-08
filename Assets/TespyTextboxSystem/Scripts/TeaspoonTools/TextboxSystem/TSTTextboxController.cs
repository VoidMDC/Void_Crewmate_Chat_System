using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
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

	[System.Serializable]
	class ControllerComponents
	{

		public TextboxBox textboxBox;

		public TextboxText textboxText;

		public TextboxPortrait textboxPortrait;

		public TextboxNametag nameTag;

		public TextSettings textSettings;
	}

    public class TextboxController : MonoBehaviour, IHasRectTransform
    {
        // events
		[HideInInspector]
		public UnityEvent StartedShowingText = new UnityEvent();
		[HideInInspector]
		public UnityEvent DoneDisplayingText = new UnityEvent();

        // basic attributes
		[HideInInspector]
		public RectTransform rectTransform { get; set; }
		[HideInInspector]
		public RectTransform TSTSAnchor;

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
			
		[SerializeField]
		ControllerComponents controllerComponents;

		// properties to interface with those components
		public TextSettings textSettings 
		{
			get { return controllerComponents.textSettings; } 
			private set { controllerComponents.textSettings = value; }
		}

		public TextboxText text
		{
			get { return controllerComponents.textboxText; }
			private set {controllerComponents.textboxText = value;}
		}

		public TextboxBox box
		{
			get { return controllerComponents.textboxBox; }
			private set {controllerComponents.textboxBox = value;}
		}

		public TextboxPortrait portrait
		{
			get { return controllerComponents.textboxPortrait; }
			private set { controllerComponents.textboxPortrait = value; }
		}

		public TextboxNametag nameTag 
		{
			get { return controllerComponents.nameTag; }
			protected set { controllerComponents.nameTag = value; }
		}
					
		public Sprite boxGraphic
		{
			get { return box.sprite; }
			set { box.sprite = value; }
		}

		public Sprite nameTagGraphic 
		{
			get 
			{ 
				if (nameTag == null) 
				{
					Debug.Log (this.name + " has no TSTNameTag component to get the graphic of.");
					return null;
				}
				return nameTag.sprite; 
			}
			set 
			{ 
				if (nameTag == null) 
				{
					Debug.Log (this.name + " has no TSTNameTag component to set the graphic of.");
					return;
				}
				nameTag.sprite = value; 
			}
		}

		public string nameTagText
		{
			get 
			{ 
				if (nameTag == null) 
				{
					Debug.Log (this.name + " has no TSTNameTag component to get the text of.");
					return null;
				}
				return nameTag.text; 
			}
			set 
			{ 
				if (nameTag == null) 
				{
					Debug.Log (this.name + " has no TSTNameTag component to set the text of.");
					return;
				}
				nameTag.text = value; 
			}
		}
					
		public Sprite portraitSprite
		{
			get 
			{ 
				if (portrait == null) 
				{
					Debug.Log (this.name + " has no TextboxPortrait component to get the graphic of.");
					return null;
				}
				return portrait.sprite; 
			}
			set 
			{ 
				if (portrait == null) 
				{
					Debug.Log (this.name + " has no TextboxPortrait component to set the graphic of.");
					return;
				}
				portrait.sprite = value; 
			}
		}
				
		public Font font
		{
			get { return textSettings.font; }
			set 
			{ 
				textSettings.font = value;
				text.font = value;

                if (nameTag != null)
                    nameTag.font = value;
			}
		}

		public int fontSize
		{
			get { return textSettings.fontSize; }
			set 
			{
				textSettings.fontSize = value;
				text.fontSize = value;
			}
		}
			

		[Header("For Debugging")] 
		public string textToDisplay;
		public List<string> boxfuls;

		[SerializeField]
        private bool testing = false;
        
		void Awake()
		{
			rectTransform = GetComponent<RectTransform> ();
		}

        private void Start()
		{
            if (testing)
                Initialize();

        }
        
		void Initialize()
        {
			// for testing
			InitializeSubmodules (textSettings.textSpeed, textSettings.linesPerTextbox);

			AlignSizeToBox();
            SubscribeToEvents();
            SetupCallbacks();

        }

        public void Initialize(TextSpeed textSpeed, int linesPerTextbox)
        {
			InitializeSubmodules ( textSpeed, linesPerTextbox);
			AlignSizeToBox();
            SubscribeToEvents();
            SetupCallbacks();
        }
    
		void InitializeSubmodules(TextSpeed textSpeed, int linesPerTextbox)
		{
			textSettings.Initialize(this, textSpeed, linesPerTextbox);
			textSettings.SetAutoFontSize();
			box.Initialize(this);
			text.Initialize(this, testing);

            // Not all textboxes will need portraits or nametags, hence the safety-checking here
            if (nameTag != null)
            {
                nameTag.Initialize(this);
                nameTag.font = font;
            }
			if (portrait != null)
				portrait.Initialize (this);


		}
			
        void AlignSizeToBox()
		{
            rectTransform.sizeDelta = box.rectTransform.sizeDelta;
        }

        /// <summary>
        ///  Places a textbox on screen based on the anchor passed. Works off viewport coords.
        /// </summary>
        /// <param name="anchorOnScreen"></param>
		public void PlaceOnScreen(TextboxAnchor anchorOnScreen, bool stayInBounds = true)
        {
			rectTransform.PositionRelativeToParent(anchorOnScreen.ToVector2(), stayInBounds);
        }

		public void PlaceOnScreen(Vector2 anchorOnScreen, bool stayInBounds = true)
		{
			rectTransform.PositionRelativeToParent (anchorOnScreen, stayInBounds);
		}

        public void Close()
        {
            StartCoroutine(CloseSelf());
        }
	
		#region To interface with the submodules

		public void DisplayText(string textToDisplay)
		{
			StartedShowingText.Invoke ();
			this.textToDisplay = textToDisplay;
			text.DisplayText (textToDisplay);
		}

		public void DisplayText(IList<string> textToDisplay)
		{
			StartedShowingText.Invoke ();
			text.DisplayText (textToDisplay);
		}
						
		#endregion

        IEnumerator CloseSelf()
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

            
            Destroy(this.gameObject);
        }

		void OnDestroy()
		{
			Debug.Log (this.name + " has been destroyed!");
			Textbox.textboxesOnScreen--;
		}
    
		void SetupSystemAnchor()
		{
			// the anchor for things made by this textbox system
			GameObject anchor = GameObject.Find ("TSTSCanvas");

			if (anchor == null) 
			{
				anchor = new GameObject ("TSTSCanvas");
				TSTSAnchor = anchor.AddComponent<RectTransform> ();
				anchor.transform.position = Camera.main.transform.position;
				anchor.AddComponent<Canvas> ();
			} 
			else
				TSTSAnchor = anchor.GetComponent<RectTransform> ();

		}
	
		void SubscribeToEvents()
		{
			text.DoneDisplayingText.AddListener (DoneDisplayingText.Invoke);
            
		}

        void SetupCallbacks()
        {
            DoneDisplayingText.AddListener(() => Debug.Log(this.name + ": done displaying text!"));
        }
	}

}