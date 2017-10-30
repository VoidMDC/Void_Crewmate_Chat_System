using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace TeaspoonTools.TextboxSystem
{
	public class TSTSTextboxNameTag : Image
	{
		Text nameText;
		public string name {
			get { return nameText.text; }
			set {
				nameText.text = value;
			}
		}

		void Awake()
		{
			if (transform.Find ("NameText") == null) {
				GameObject nameTextObj = new GameObject ("NameText");
				nameTextObj.AddComponent<RectTransform> ();
				nameTextObj.AddComponent<Text> ();
				nameTextObj.transform.SetParent (this.transform, false);
			}

			nameText = transform.Find ("NameText").GetComponent<Text>();
		}
	}
}

