using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Yarn;
using Yarn.Unity;


public class OptionSetController : MonoBehaviour {
	/*
	 * 
	 * Made to work with YarnSpinner options.
	 */ 

	List<Button> optionButtons = new List<Button>();
	new public RectTransform rectTransform;
	public GameObject buttonPrefab;
	public Vector2 spaceBuffer = new Vector2(5f, 5f);
	DialogueUITest dialogueUI;

	void Awake()
	{
		rectTransform = GetComponent<RectTransform> ();
	}

	public void Init(DialogueUITest dialogueUIBehaviour)
	{
		// sets which dialogue ui behaviour this option set corresponds to

		dialogueUI = dialogueUIBehaviour;
	}

	void ResizeToFitOptions()
	{
		float newWidth = 	0f;
		float newHeight = 	spaceBuffer.y;
		RectTransform optionRect;

		foreach (Button option in optionButtons) 
		{
			optionRect = 	option.GetComponent<RectTransform> ();
			newHeight += 	optionRect.rect.height;

			Debug.Log ("Option width: " + optionRect.rect.width);
			Debug.Log ("Option height: " + optionRect.rect.height);

			if (optionRect.rect.width > newWidth)
				newWidth = optionRect.rect.width + spaceBuffer.x;

		}
			
		rectTransform.sizeDelta = new Vector2 (newWidth, newHeight);
	}

	public void AddOption(GameObject option, string optionText)
	{
		// make sure the added option is an option by seeing if it has a button
		// script attached to it
		Button optionButton = option.GetComponent<Button> ();
		Text optionTextObj;

		if (optionButton != null) 
		{
			// fit that option into this set, changing its text accordingly
			optionButton.transform.SetParent (this.rectTransform, false);
			Canvas.ForceUpdateCanvases ();
			optionTextObj = 		optionButton.GetComponentInChildren<Text> ();
			optionTextObj.text = 	optionText;

		} 
		else
			throw new System.InvalidOperationException ("Cannot add a non-button as an option to an option set!");
	


		// when this option is clicked, make it set off the dialogue behaviour based on 
		// its order in the list, so the dialogue runner knows which node to go to next
		int ind = optionButtons.Count;
		optionButton.onClick.AddListener ( () => dialogueUI.SetOption (ind) );


		optionButtons.Add (optionButton);

		ResizeToFitOptions ();
	}
}
