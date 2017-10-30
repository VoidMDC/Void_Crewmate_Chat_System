using System;
using UnityEngine;
using UnityEngine.UI;


namespace TeaspoonTools.TextboxSystem.Utils
{
	public class TextSpeedSettings
	{
		public TextSpeed normalSpeed { get; protected set; }
		public TextSpeed higherSpeed { get; protected set; }
		public TextSpeed effectiveTextSpeed { get; protected set; }

		public TextSpeedSettings (TextSpeed normalSpeed, TextSpeed higherSpeed)
		{
			this.normalSpeed = normalSpeed;
			this.higherSpeed = higherSpeed;
			effectiveTextSpeed = normalSpeed;
		}
	}
}

