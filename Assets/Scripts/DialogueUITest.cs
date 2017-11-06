using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Yarn.Unity;
using TeaspoonTools.TextboxSystem;
using TeaspoonTools.TextboxSystem.Utils;
using TeaspoonTools.Utils.Extensions;

public class DialogueUITest : DialogueUIBehaviour {

	public UnityEvent StartedDialogue 		= 		new UnityEvent();
	public UnityEvent EndedDialogue 		= 		new UnityEvent();

	StringBuilder textToShow = new StringBuilder(); //not including choices
	List<string> dialogueChoices = new List<string>();

	GameObject textbox = null;
	TextboxController textboxController = null;
	GameObject optionWindow;

	public GameObject textboxPrefab;
	public GameObject optionWindowPrefab;
	public int linesPerTextbox = 3;
	public TextSpeed textSpeed = TextSpeed.medium;

	TSTTextParser textWrapper = new TSTTextParser();

	bool readingDialogue = false;

	DialogueRunner dialogueRunner;
	Yarn.Options optionContainer;

	string nameTagText = "";
	Sprite portrait = null;

	/// A delegate (ie a function-stored-in-a-variable) that
	/// we call to tell the dialogue system about what option
	/// the user selected
	private Yarn.OptionChooser SetSelectedOption;

	void Awake()
	{
		dialogueRunner = FindObjectOfType<DialogueRunner> ();
	}

	public override IEnumerator DialogueStarted ()
	{
		StartedDialogue.Invoke ();
		return base.DialogueStarted ();
	}

	public override IEnumerator DialogueComplete ()
	{
		EndedDialogue.Invoke ();
		return base.DialogueComplete ();
	}

	public override IEnumerator RunLine (Yarn.Line line)
	{
		//Debug.Log (this.name + ": Running a line!");
		//throw new System.NotImplementedException ();

		// Add the lines to the text to show. Note that grouping up the 
		// lines like this is why I added a paused flag to DialogueRunner
		if (readingDialogue)
			textToShow.Append(line.text.Replace('\n', ' '));
		
		yield return null;
	}

	public override IEnumerator RunCommand (Yarn.Command command)
	{
		//Debug.Log (this.name + ": Running a command! The text: " + command.text);

		switch (command.text) 
		{
		case "Textbox":
			// clear the text to show so it can be read
			//Debug.Log("At start of a textbox!");
			readingDialogue = true;
			textToShow = new StringBuilder ();
			break;

		case "/Textbox":
			// its time to display the text read up to this point
			//Debug.Log(this.name + ": At end of a textbox!");
			dialogueRunner.paused = true;
			readingDialogue = false;
			GetTextDisplayed ();
			break;

		}


		string imageName;
		if (command.text.ToLower ().Contains ("name|")) 
			// for reading in nametags
			nameTagText = command.text.Remove (0, "name|".Length);

		else if (command.text.ToLower ().Contains ("portrait|")) 
		{
			// for choosing which portrait to show
			imageName = command.text.Remove(0, "portrait|".Length);
			portrait = Resources.Load<Sprite> ("Graphics/Portraits/" + imageName);

		}
			
		yield return null;
	}

	public override IEnumerator RunOptions (Yarn.Options optionsCollection, Yarn.OptionChooser optionChooser)
	{
		// create the option dialog window, giving it one button for each option
		Debug.Log("Running options!");
		optionWindow = CreateOptionWindow(optionsCollection);

		SetSelectedOption = optionChooser;

		// until an option is chosen, keep the dialogue runner effectively paused
		// (good thing we don't need to use its paused flag here, eh?)
		while (SetSelectedOption != null)
			yield return null;

		Destroy(optionWindow); // won't need this anymore!

		optionWindow = null;
		yield return null;
	}

	public override IEnumerator NodeComplete (string nextNode)
	{
		return base.NodeComplete (nextNode);
	}


	public void SetOption(int selectedOption)
	{
		// Call the delegate to tell the dialogue system that we've
		// selected an option.
		SetSelectedOption (selectedOption);

		// Now remove the delegate so that the loop in RunOptions will exit
		SetSelectedOption = null;
	}

	void GetTextDisplayed()
	{
		if (textbox == null) 
		{
			textbox 			= Textbox.Create (textboxPrefab, "", linesPerTextbox, textSpeed);
			textboxController 	= textbox.GetComponent<TextboxController> ();
			textboxController.DoneDisplayingText.AddListener (ResumeDialogueRunning);
			EndedDialogue.AddListener (textboxController.Close);
			textbox.transform.SetParent (GameObject.Find ("Canvas").transform, false);
			textboxController.PlaceOnScreen (new Vector2 (0.5f, 0.15f));

		}

		Debug.Log ("Text to display, gathered between the textbox tags: " + textToShow.ToString ());
		// get the text parsed
		textWrapper.textToParse = textToShow.ToString();
		textWrapper.textObj = textboxController.textboxText;
		textWrapper.ParseText ();

		textboxController.boxfuls = textWrapper.parsedText;
		textboxController.nameTagText = nameTagText;
		textboxController.ChangePortrait (portrait);
		textboxController.StartShowingText (textWrapper.parsedText);
	}

	void ResumeDialogueRunning()
	{
		// signals to the dialogue runner to keep running dialogue for this module
		// to receive
		dialogueRunner.paused = false;
	}

	GameObject CreateOptionWindow(Yarn.Options optionsCollection)
	{
		GameObject optionWindow = 			Instantiate<GameObject> (optionWindowPrefab);
		OptionSetController setController = optionWindow.GetComponent<OptionSetController> ();
		setController.Init (this);
		setController.transform.SetParent (GameObject.Find ("Canvas").transform, false);
		//setController.rectTransform.sizeDelta = new Vector2 ();
		GameObject newButton;
		GameObject buttonText;

		// populate it with buttons
		foreach (var optionString in optionsCollection.options) 
		{
			newButton = Instantiate<GameObject>(setController.buttonPrefab);
			setController.AddOption (newButton, optionString);
		}

		// let's put it in the middle-right of the screen
		//setController.rectTransform.ApplyAnchorPreset(TextAnchor.MiddleRight, false, false);
		setController.rectTransform.PositionRelativeToParent(TextAnchor.MiddleRight);

		return optionWindow;
	}
}
