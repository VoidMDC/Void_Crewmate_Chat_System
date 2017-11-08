using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TeaspoonTools.TextboxSystem;


namespace TeaspoonTools.TextboxSystem.Utils
{
	/// <summary>
	/// Module for the TST Text Monobehaviour to, of course, handle the text-displaying.
	/// </summary>
	public class TextDisplayer
	{
		public UnityEvent StartedDisplayingText = new UnityEvent();
		public UnityEvent DoneDisplayingText = new UnityEvent();

		public IList<string> textToDisplay { get; set; }
		Text textField;
		AudioSource sfxPlayer;
		AudioClip textSound;
		TextSpeedSettings textSpeedSettings;

		bool showingText = false;

		public TextDisplayer(Text textField, ICollection textToDisplay,
							 TextSpeedSettings textSpeedSettings, AudioSource sfxPlayer, 
						     AudioClip textSound = null)
		{
			this.textField = textField;
			this.textToDisplay = textToDisplay as List<string>;
			this.textSpeedSettings = textSpeedSettings;
			this.sfxPlayer = sfxPlayer;
			this.textSound = textSound;
		}

		public void DisplayText()
		{
			if (!showingText) 
			{
				textField.StartCoroutine (ShowText ());
				showingText = true;
			}
		}

		public void DisplayText(IList<string> textToDisplay)
		{
			this.textToDisplay = textToDisplay;

			if (!showingText) 
			{
				textField.StartCoroutine (ShowText ());
				showingText = true;
			}
		}

		IEnumerator ShowText()
		{
			StartedDisplayingText.Invoke ();

			string currentBoxful = "";

			int boxfulsToGoThrough = textToDisplay.Count;

			for (int i = 0; i < boxfulsToGoThrough; i++)
			{
				textField.text = "";
				currentBoxful = textToDisplay[i];

				yield return textField.StartCoroutine(PrintBoxful(currentBoxful));
				//Debug.Log("Printed boxful #" + (i + 1) );
				yield return textField.StartCoroutine(WaitForUserInput());
			}

			//DoneDisplayingText (this, null);
			DoneDisplayingText.Invoke();
			showingText = false;

		}

		IEnumerator PrintBoxful(string boxful)
		{
			// helper function for ShowText()
			float pauseDuration = 0;
			NormalizeScrollingSpeed(ref pauseDuration);

			float timeWaited = 0f;
			float timeDelay = 0.0f;
			// ^makes it so the scrolling speed doesn't get raised too quickly in the boxful-
			// printing

			// Update: Apparently the time delay is no longer necessary, at least in Unity 5.6.2f1.

			IEnumerator playSoundByte = PlaySoundByte(pauseDuration);

			textField.StartCoroutine(playSoundByte);

			for (int j = 0; j < boxful.Length; j++)
			{
				SetScrollingSpeed(ref pauseDuration, timeWaited < timeDelay);

				if (pauseDuration == 1f / (float)TextSpeed.instant )
				{
					textField.text = boxful;
					SetScrollingSpeed(ref pauseDuration);
					timeWaited += pauseDuration;
					yield return new WaitForSeconds(pauseDuration);
					//j = charsToGoThrough;
					break;
				}
				else 
					textField.text = string.Concat(textField.text, boxful[j]);

				timeWaited += pauseDuration;
				yield return new WaitForSeconds(pauseDuration);

			}

			textField.StopCoroutine(playSoundByte);


		}

		IEnumerator PlaySoundByte(	float pauseDuration,
									float volume = 0.55f,
									TextSpeed minSpeedToAbideBy = TextSpeed.verySlow,
									TextSpeed maxSpeedToAbideBy = TextSpeed.fast)
		{
			// plays the sound byte while keeping it from playing too many times at once,
			// which avoids that ugly sound error

			float actualPause = Mathf.Clamp(pauseDuration,
				1f / (float)maxSpeedToAbideBy,
				1f / (float)minSpeedToAbideBy);


			while (true)
			{
				if (!sfxPlayer.isPlaying && pauseDuration != 1f / (float) TextSpeed.instant
					&& textSound != null)
					sfxPlayer.PlayOneShot(textSound, volume);
				yield return new WaitForSeconds(actualPause);
			}


		}

		IEnumerator WaitForUserInput()
		{
			// helper function for ShowText(), this subcoroutine finishes when the player
			// gives the proper input.

			bool progressTheText = Input.GetKeyDown(KeyCode.W) || Input.GetMouseButtonDown(0);
			while (!progressTheText)
			{
				//Debug.Log("Waiting for player input.");
				progressTheText = Input.GetKeyDown(KeyCode.W) || Input.GetMouseButtonDown(0);
				yield return null;
			}

			
		}

		void SetScrollingSpeed(ref float pauseDuration, bool waitedLongEnough = false)
		{
			/*
             * The waitedLongEnough arg is to help remove the glitch where if the normal
             * speed is fast, and you make the text speed up, the rest of the boxfuls
             * for that textbox are displayed instantly.
             */

			if (!waitedLongEnough && (Input.GetKey(KeyCode.W) || Input.GetMouseButton(0)) )
				RaiseScrollingSpeed(ref pauseDuration, waitedLongEnough);
			else if (waitedLongEnough && (Input.GetKey(KeyCode.W) || Input.GetMouseButton(0)) )
				RaiseScrollingSpeed(ref pauseDuration, waitedLongEnough);
			else
				NormalizeScrollingSpeed(ref pauseDuration);
		}

		void RaiseScrollingSpeed(ref float pauseDuration, bool waitedLongEnough = false)
		{

			if (!waitedLongEnough)
				pauseDuration = 1f / (float)textSpeedSettings.higherSpeed;
			else
				NormalizeScrollingSpeed(ref pauseDuration);

		}

		void NormalizeScrollingSpeed(ref float pauseDuration)
		{
			pauseDuration = 1f / (float)textSpeedSettings.normalSpeed;
		}

	}
}

