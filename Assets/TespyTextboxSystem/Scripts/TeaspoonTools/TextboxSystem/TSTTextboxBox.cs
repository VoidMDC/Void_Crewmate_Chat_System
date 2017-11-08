using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using TeaspoonTools.Utils.Extensions;

namespace TeaspoonTools.TextboxSystem
{
    public class TSTTextboxBox : TextboxBox
    {

    }
    public class TextboxBox : Image
    {
        /*
         * A helper script for TextboxController, this is meant to be attached to
         * the game object in the textbox prefab that has the textbox graphic.
         */

		new public RectTransform rectTransform { get; protected set;}
		public TextboxController textboxController { get; protected set; }
        
		public float height
		{
			get
			{
				return rectTransform.rect.height;
			}
			set {
				rectTransform.SetHeight (value);
			}
		}

		public float width
		{
			get
			{
				return rectTransform.rect.width;
			}
			set {
				rectTransform.SetWidth (value);
			}
		}

		void Awake()
		{
			rectTransform = GetComponent<RectTransform> ();
		}

        public virtual void Initialize(TextboxController tbController)
        {
            textboxController = tbController;
			
			if (rectTransform == null)
				Debug.LogWarning (this.name + " has a null rect trans!");
        }
			

    }
}