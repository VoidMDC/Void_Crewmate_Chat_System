using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace TeaspoonTools.Utils.Extensions
{
    public static class TSTTransformExtensions
    {
        public static void SetLocalXScale(this Transform trans, float newXScale)
        {
			Vector3 newScale = trans.localScale;
			newScale.x = newXScale;
            trans.localScale = newScale;
        }

        public static void SetLocalYScale(this Transform trans, float newYScale)
        {
			Vector3 newScale = trans.localScale;
			newScale.y = newYScale;
			trans.localScale = newScale;

        }

		public static void SetLocalZScale(this Transform trans, float newZScale)
		{
			Vector3 newScale = trans.localScale;
			newScale.z = newZScale;
			trans.localScale = newScale;

		}

        public static void ResetPosition(this Transform trans)
        {
            trans.position = Vector3.zero;
        }

        public static void ResetLocalPosition(this Transform trans)
        {
            trans.localPosition = Vector3.zero;
        }
        public static void SetXPosition(this Transform trans, float newXPosition)
        {
            Vector3 newPos = trans.position;
            newPos.x = newXPosition;
            trans.position = newPos;
        }

        public static void SetYPosition(this Transform trans, float newYPosition)
        {
            Vector3 newPos = trans.position;
            newPos.y = newYPosition;
            trans.position = newPos;
        }

        public static void SetZPosition(this Transform trans, float newZPosition)
        {
            Vector3 newPos = trans.position;
            newPos.z = newZPosition;
            trans.position = newPos;
        }

        public static void SetLocalXPosition(this Transform trans, float newXLocalPosition)
        {
            Vector3 newPos = trans.localPosition;
            newPos.x = newXLocalPosition;
            trans.localPosition = newPos;
        }

        public static void SetLocalYPosition(this Transform trans, float newYLocalPosition)
        {
            Vector3 newPos = trans.localPosition;
            newPos.y = newYLocalPosition;
            trans.localPosition = newPos;
        }

        public static void SetLocalZPosition(this Transform trans, float newZLocalPosition)
        {
            Vector3 newPos = trans.localPosition;
            newPos.z = newZLocalPosition;
            trans.localPosition = newPos;
        }
    }

}
