using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TeaspoonTools.TextboxSystem;

[RequireComponent(typeof(Image))]
public class TSTTextboxNametag : TextboxNametag {}


public class TextboxNametag : MonoBehaviour 
{

	new public RectTransform rectTransform;
	TextboxController textboxController;
	Image image;
	Text textField;

	public Sprite sprite
	{
		get { return image.sprite; }
		set { image.sprite = value; }
	}

	public string text
	{
		get { return textField.text; }
		set { textField.text = value; }
	}

    public Font font
    {
        get { return textField.font; }
        set { textField.font = value; }
    }

	void Awake()
	{
		rectTransform = 		GetComponent<RectTransform> ();
		image = 				GetComponent<Image> ();
		textField = 			GetComponentInChildren<Text> ();
	}

	public void Initialize(TextboxController tbController)
	{
		textboxController = tbController;
	}

}
