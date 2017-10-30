using System;
using UnityEngine;
using UnityEngine.UI;


namespace TeaspoonTools.TextboxSystem
{
	public interface ITextboxComponent
	{
		RectTransform rectTransform { get; }
		TextboxController textboxController { get; }
		float width { get; set; }
		float height { get; set; }

	}
}

