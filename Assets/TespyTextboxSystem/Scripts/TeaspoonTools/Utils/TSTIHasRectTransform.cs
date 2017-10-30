using System;
using UnityEngine;
using UnityEngine.UI;

namespace TeaspoonTools.Utils
{
	public interface IHasRectTransform
	{
		RectTransform rectTransform { get; }
	}
}

