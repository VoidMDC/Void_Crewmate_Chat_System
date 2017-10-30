using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TeaspoonTools.TextboxSystem.Utils;

namespace TeaspoonTools.Utils.Extensions
{
    public static class TSTTextboxAnchorExtensions
    {
        static float upper = 1f;
        static float middle = 0.5f;
        static float lower = 0f;

        static float right = 1f;
        static float center = 0.5f;
        static float left = 0f;

        public static Vector2 ToVector2(this TextboxAnchor textboxAnchor)
        {

            switch (textboxAnchor)
            {
                case TextboxAnchor.UpperLeft:
                    return new Vector2(left, upper);
                case TextboxAnchor.UpperCenter:
                    return new Vector2(center, upper);
                case TextboxAnchor.UpperRight:
                    return new Vector2(right, upper);

                case TextboxAnchor.MiddleLeft:
                    return new Vector2(left, middle);
                case TextboxAnchor.MiddleCenter:
                    return new Vector2(center, middle);
                case TextboxAnchor.MiddleRight:
                    return new Vector2(right, middle);

                case TextboxAnchor.LowerLeft:
                    return new Vector2(left, lower);
                case TextboxAnchor.LowerCenter:
                    return new Vector2(center, lower);
                case TextboxAnchor.LowerRight:
                    return new Vector2(right, lower);

                default:
                    throw new NotImplementedException(textboxAnchor.ToString() + " of TST TextboxAnchor enum not yet implemented.");

            }

        }

		public static TextAnchor ToTextAnchor(this TextboxAnchor textboxAnchor)
		{
			switch (textboxAnchor)
			{
			case TextboxAnchor.UpperLeft:
				return TextAnchor.UpperLeft;
			case TextboxAnchor.UpperCenter:
				return TextAnchor.UpperCenter;
			case TextboxAnchor.UpperRight:
				return TextAnchor.UpperRight;

			case TextboxAnchor.MiddleLeft:
				return TextAnchor.MiddleLeft;
			case TextboxAnchor.MiddleCenter:
				return TextAnchor.MiddleCenter;
			case TextboxAnchor.MiddleRight:
				return TextAnchor.MiddleRight;

			case TextboxAnchor.LowerLeft:
				return TextAnchor.LowerLeft;
			case TextboxAnchor.LowerCenter:
				return TextAnchor.LowerCenter;
			case TextboxAnchor.LowerRight:
				return TextAnchor.LowerRight;

			default:
				throw new NotImplementedException(textboxAnchor.ToString() + " of TST TextboxAnchor enum not yet implemented.");
		}
    }
	}
}
