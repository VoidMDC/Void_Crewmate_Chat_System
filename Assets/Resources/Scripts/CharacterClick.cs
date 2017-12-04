using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yarn.Unity {
	public class CharacterClick : MonoBehaviour {
		private DialogueRunner dialogue;
		public string talkToNode;
		private Collider coll;

		// Use this for initialization
		void Start () {
			dialogue = GameObject.Find("Dialogue").GetComponent<DialogueRunner>();
			coll = GetComponent<BoxCollider>();
		}
		
		// Update is called once per frame
		void Update () {
			if(dialogue.isDialogueRunning) {
				coll.enabled = false;
			} else {
				coll.enabled = true;
			}
		}

		void OnMouseDown() {
			Debug.Log("Click Detected!");
			dialogue.StartDialogue(talkToNode);
		}
	}
}
