using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class DialogueChain{

	[SerializeField]
	private List<Dialogue> _dialogues = null;

	public List<Dialogue> dialogues { 
		get { return _dialogues; } 
		private set { _dialogues = value; }
	}

	public DialogueChain()
	{
		if (dialogues == null) 
			dialogues = new List<Dialogue> ();
		
	}


}
