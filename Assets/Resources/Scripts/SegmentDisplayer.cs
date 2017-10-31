using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TeaspoonTools.TextboxSystem;
using TeaspoonTools.TextboxSystem.Utils;

public class SegmentDisplayer : MonoBehaviour {

	/*
	 * Takes a dialogue segment, arranges it to be displayed in the textbox system, and displays
	 * the segment using said textbox system. 
	 */ 

	public GameObject textboxPrefab;
	GameObject textbox;
	TextboxController textboxController;

	public DialogueChainController chainToDisplay;
	TSTTextParser textParser 	= 		new TSTTextParser();
	TextDisplayer textDisplayer = 		new TextDisplayer();

	bool showingSegment = false;

	[SerializeField]
	protected DialogueChain parsedDialogueChain = new DialogueChain();



	public SegmentDisplayer()
	{
		
	}

	public void ShowDialogueSegment()
	{
		if (!showingSegment) 
		{
			showingSegment = true;

			textbox = Textbox.Create (textboxPrefab);
			//textbox.transform.localScale = new Vector3 (1, 1, 1);
			textbox.transform.SetParent (GameObject.Find ("Canvas").transform, false);
			textboxController = textbox.GetComponent<TextboxController> ();
			textboxController.PlaceOnScreen (new Vector2 (0.5f, 0.15f));

			RearrangeDialogChain ();
			StartCoroutine (DisplayBoxfulCoroutine ());

		}


	}

	void ReallowSegmentShowing()
	{
		showingSegment = true;
		textboxController.DoneDisplayingText.RemoveListener (ReallowSegmentShowing);
	}

	void RearrangeDialogChain()
	{
		// takes the dialog chain to parse, and splits it (if necessary), putting 
		// the results in a new dialog chain

		// going through each dialog in the chain, get all sentences in them parsed
		// into boxfuls, having a separate dialog with the same name and portrait 
		// assigned to each boxful

		string dialogueText = "";
		Dialogue dialogueToAdd;
		textParser.textObj = textboxController.textboxText;

		foreach (Dialogue dialogue in chainToDisplay.dialogueChain.dialogues) 
		{
			// put all sentences from the current dialogue into a string
			foreach (string sentence in dialogue.sentences)
				dialogueText += sentence + " ";

			// parse the text into boxfuls
			//textParser = new TSTTextParser();
			textParser.textToParse = dialogueText;
			//textParser.textObj = textboxController.textboxText;

			textParser.ParseText ();

			dialogueToAdd = new Dialogue ();
			dialogueToAdd.portrait = dialogue.portrait;
			dialogueToAdd.name = dialogue.name;

			foreach (string boxful in textParser.parsedText) 
				dialogueToAdd.sentences.Add (boxful);

			parsedDialogueChain.dialogues.Add (dialogueToAdd);

			dialogueText = "";
				
		}
	}

	void DisplayParsedDialogChain()
	{
		
	}

	void DisplayABoxful()
	{

	}

	IEnumerator DisplayBoxfulCoroutine()
	{
		yield return null;

		foreach (Dialogue dialogue in parsedDialogueChain.dialogues) {

			foreach (string boxful in dialogue.sentences)
			{
				textboxController.boxfuls = new List<string> () { boxful };
				textboxController.ChangePortrait (dialogue.portrait);
				textboxController.nameTagText = dialogue.name;

				textboxController.StartShowingText (boxful);
				yield return null;

				while (textboxController.isDisplayingText)
					yield return null;
			}

		}

		//textboxController.Close ();


		showingSegment = false;


	}

	List<string> ParseDialogChain(DialogueChain chainToParse)
	{
		// go through all the sentences, put each of them in a single string 
		// for the parser to parse

		List<string> boxfuls = new List<string> ();

		string dialogueText = "";
		foreach (Dialogue dialogue in chainToParse.dialogues) 
		{

			foreach (string sentence in dialogue.sentences) 
				dialogueText += sentence + " ";

			textParser = new TSTTextParser ();
			textParser.textToParse = string.Copy(dialogueText);
			textParser.textObj = textboxController.textboxText;
			textParser.ParseText ();
			boxfuls.AddRange (textParser.parsedText);
		}

		return boxfuls;
	}
}

