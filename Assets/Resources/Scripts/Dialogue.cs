using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class Dialogue 
{

	public Sprite portrait;
	public string name;

	[TextArea(3, 3)]
	public List<string> sentences = null;

	public Dialogue()
	{
		if (sentences == null)
			sentences = new List<string> ();
	}

}
