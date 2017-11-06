using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace TeaspoonTools.Utils.Extensions
{
    public static class TSTRectTransformExtensions
    {
        
        public static void SetLocalXScale(this RectTransform rectTransform, float newXScale)
        {
            Vector3 newScale = new Vector3(newXScale,
                                           rectTransform.localScale.y,
                                           rectTransform.localScale.z);

            rectTransform.localScale = newScale;
        }

        /// <summary>
        /// Sets the rectTransform's anchors to a given point.
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="anchorPoint"></param>
        public static void AnchorToPoint(this RectTransform rectTransform, Vector2 anchorPoint)
        {
            // sets both anchors of the rect transform to a single point
            rectTransform.anchorMin = anchorPoint;
            rectTransform.anchorMax = anchorPoint;
        }

        /// <summary>
        /// Sets the rectTransform's anchors and pivot to a given point.
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="point"></param>
        public static void AnchorAndPivotToPoint(this RectTransform rectTransform, Vector2 point)
        {
            rectTransform.AnchorToPoint(point);
            rectTransform.pivot = point;
        }

        public static void SetAnchors(this RectTransform rectTransform,
                                      Vector2 anchorMin, Vector2 anchorMax)
        {
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;

        }

        public static void ResetPosition(this RectTransform rectTransform)
        {
            rectTransform.localPosition = Vector3.zero;
        }

        public static void ResetScale(this RectTransform rectTransform)
        {
            Debug.Log(rectTransform.name + " reset its scale!");
            rectTransform.localScale = new Vector3(1, 1, 1);
        }

        public static float RightEdgeX(this RectTransform rectTransform)
        {
            /* tried to use world coords
            Vector3 transDistance = rectTransform.position - rectTransform.anchoredPosition3D;
            Vector3 effectiveAnchoredPosition = rectTransform.anchoredPosition3D + transDistance;

            float xCoord = effectiveAnchoredPosition.x + 
                         (rectTransform.rect.width * (1f - rectTransform.pivot.x));
            */

            // uses anchored pos
            //float worldAnchoredX = rectTransform.TransformPoint(rectTransform.anchoredPosition).x;

            float xCoord = rectTransform.anchoredPosition.x +
                          (rectTransform.rect.width * (1f - rectTransform.pivot.x));

            //
            //Debug.Log(rectTransform.name + "'s right edge X: " + xCoord);

            return xCoord;
        }

        public static float LeftEdgeX(this RectTransform rectTransform)
        {
            float xCoord = rectTransform.anchoredPosition3D.x -
                          (rectTransform.rect.width * (rectTransform.pivot.x));

            //Debug.Log(rectTransform.name + "'s left edge X: " + xCoord);
            return xCoord;
        }

        public static float UpperEdgeY(this RectTransform rectTransform)
        {

            float yCoord = rectTransform.anchoredPosition3D.y +
                           (rectTransform.rect.height * (1f - rectTransform.pivot.y));

            //Debug.Log(rectTransform.name + "'s upper edge Y: " + yCoord);
            return yCoord;
        }

        public static float LowerEdgeY(this RectTransform rectTransform)
        {
            float yCoord = rectTransform.anchoredPosition.y -
                           (rectTransform.rect.height * (rectTransform.pivot.y));

            //Debug.Log(rectTransform.name + "'s lower edge Y: " + yCoord);

            return yCoord;
        }

        /// <summary>
        /// Returns the coordinates of the Rect Transform's rect's lower left corner
        /// in its local space.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static Vector2 GetLowerLeftCorner(this RectTransform rectTransform)
        {
            return new Vector2(-rectTransform.rect.width * rectTransform.pivot.x,
                               -rectTransform.rect.height * rectTransform.pivot.y);
            
        }

        /// <summary>
        /// Why wasn't this functionality built-in?
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="presetToApply"></param>
        /// <param name="alsoSetPivot"></param>
        /// <param name="alsoSetPosition"></param>
        public static void ApplyAnchorPreset(this RectTransform rectTransform, 
                                             TextAnchor presetToApply,
                                             bool alsoSetPivot = false, 
                                             bool alsoSetPosition = false)
        {
            
            // just applying the anchors
            Vector2 anchorsToApply = presetToApply.ToViewportCoords();
            rectTransform.anchorMin = anchorsToApply;
            rectTransform.anchorMax = anchorsToApply;

            if (alsoSetPivot)
                rectTransform.pivot = anchorsToApply;
            
            if (alsoSetPosition)
                rectTransform.PositionRelativeToParent(anchorsToApply);

        }

		public static void ApplyAnchorPresetRecursively(this RectTransform rectTransform, 
														TextAnchor presetToApply, 
														bool alsoSetPivot = false, 
														bool alsoSetPosition = false)
		{
			rectTransform.ApplyAnchorPreset (presetToApply, alsoSetPivot, alsoSetPosition);

			foreach (RectTransform child in rectTransform) 
				child.ApplyAnchorPresetRecursively (presetToApply, alsoSetPivot, alsoSetPosition);

			
		}


        /// <summary>
        /// position - Viewport coords to place this rectTransform with. i.e (0.5, 0.5) places
        /// this rectTransform at the center of the parent's rect.
        /// <para/>stayInBounds - self-explanatory.
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="position"></param>
        /// <param name="stayInBounds"></param>
        public static void PositionRelativeToParent(this RectTransform rectTransform, 
                                                    Vector2 position, 
                                                    bool stayInBounds = true)
        {
            //Debug.Log("Coords to anchor to: " + position);
            RectTransform parentRect = rectTransform.parent.GetComponent<RectTransform>();
            if (parentRect == null)
            {
                // So cruel!
                string errorMessageFormat = "{0}: I don't have any parents! Why would you";
                errorMessageFormat += "apply an anchor preset to an orphan, and then tell it";
                errorMessageFormat += "to move itself, you sick bastard?!";

                throw new ArgumentException(string.Format(errorMessageFormat, rectTransform.name));
            }

            // It's easier to do this by treating the lower left corner as an origin point
            Vector2 origin = parentRect.GetLowerLeftCorner();
            Vector2 newPos = origin;

            // better account for scale, or it'll get wonky
			float effectiveWidth = parentRect.rect.width * rectTransform.localScale.x;
			float effectiveHeight = parentRect.rect.height * rectTransform.localScale.y;

			float xShift = effectiveWidth * position.x;
			float yShift = effectiveHeight * position.y;
            newPos += new Vector2(xShift, yShift);
            

            rectTransform.localPosition = newPos;

            if (!stayInBounds)
                return;
            
            else
            {
            

                // not using the edge-getting extension functions for the following few floats, 
                // since this needs to know the parent's edge coords in the local space of 
                // this rectTransform
				float parentLeftEdgeX = -effectiveWidth * parentRect.pivot.x;
                float parentRightEdgeX = effectiveWidth * (1f - parentRect.pivot.x);
                float parentUpperEdgeY = effectiveHeight * (1f - parentRect.pivot.x);
                float parentLowerEdgeY = -effectiveHeight * parentRect.pivot.y;

                bool tooFarLeft = rectTransform.LeftEdgeX() < parentLeftEdgeX;
                bool tooFarRight = rectTransform.RightEdgeX() > parentRightEdgeX;
                bool tooFarUp = rectTransform.UpperEdgeY() > parentUpperEdgeY;
                bool tooFarDown = parentLowerEdgeY > rectTransform.LowerEdgeY();

                float howFarOff;
                float leftEdgeDistance = (parentLeftEdgeX - rectTransform.LeftEdgeX());
                float rightEdgeDistance = (rectTransform.RightEdgeX() - parentRightEdgeX);
                float upperEdgeDistance = rectTransform.UpperEdgeY() - parentUpperEdgeY;
                float lowerEdgeDistance = parentLowerEdgeY - rectTransform.LowerEdgeY();
                
                if (tooFarLeft)
                {
                    howFarOff = leftEdgeDistance * (1f - position.x);

                    rectTransform.localPosition += new Vector3(howFarOff, 0, 0);
                }

                if (tooFarRight)
                {
                    howFarOff =  rightEdgeDistance * position.x;

                    rectTransform.localPosition -= new Vector3(howFarOff, 0, 0);
                }

                
                if (tooFarUp)
                {
                    howFarOff = upperEdgeDistance * position.y;

                    rectTransform.localPosition -= new Vector3(0, howFarOff, 0);
                }
                
                if (tooFarDown)
                {
                    howFarOff = lowerEdgeDistance * (1f - position.y);

                    rectTransform.localPosition += new Vector3(0, howFarOff, 0);
                }
				

            }
        }

        /// <summary>
        /// position - Viewport coords to place this rectTransform with. i.e (0.5, 0.5) places
        /// this rectTransform at the center of the parent's rect.
        /// <para/>stayInBounds - self-explanatory.
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="position"></param>
        /// <param name="stayInBounds"></param>
        public static void PositionRelativeToParent(this RectTransform rectTransform,
                                                    TextAnchor position,
                                                    bool stayInBounds = true)
        {
            Vector2 effectivePos = position.ToViewportCoords();
            RectTransform parentRect = rectTransform.parent.GetComponent<RectTransform>();
            if (parentRect == null)
            {
                // So cruel!
                string errorMessageFormat = "{0}: I don't have any parents! Why would you";
                errorMessageFormat += "apply an anchor preset to an orphan, and then tell it";
                errorMessageFormat += "to move itself, you sick bastard?!";

                throw new ArgumentException(string.Format(errorMessageFormat, rectTransform.name));
            }

            // It's easier to do this by treating the lower left corner as an origin point, 
            // and relocating the rect trans from there
            Vector2 origin = parentRect.GetLowerLeftCorner();
            Vector2 newPos = origin;


            float xShift = parentRect.rect.width * effectivePos.x;
            float yShift = parentRect.rect.height * effectivePos.y;
            newPos += new Vector2(xShift, yShift);
            
			// Before we move this rect transform to the lower left corner, we need to set its pivot and
			// anchors to the center; makes the full reposiitoning easier. 
			// They'll be reverted after the repositioning is fully done.
			Vector2 originalAnchorMin = rectTransform.anchorMin;
			Vector2 originalAnchorMax = rectTransform.anchorMax;
			Vector2 originalPivot = rectTransform.pivot;

			rectTransform.ApplyAnchorPreset (TextAnchor.MiddleCenter, true, false);
				
            rectTransform.localPosition = newPos;

			if (!stayInBounds) {
				rectTransform.SetAnchors (originalAnchorMin, originalAnchorMax);
				rectTransform.pivot = originalPivot;
				return;
			}

            else
            {
                // not using the edge-getting extension functions for the following few floats, 
                // since this needs to know the parent's edge coords in the local space of 
                // this rectTransform
                float parentLeftEdgeX = -parentRect.rect.width * parentRect.pivot.x;
                float parentRightEdgeX = parentRect.rect.width * (1f - parentRect.pivot.x);
                float parentUpperEdgeY = parentRect.rect.height * (1f - parentRect.pivot.x);
                float parentLowerEdgeY = -parentRect.rect.height * parentRect.pivot.y;

                bool tooFarLeft = rectTransform.LeftEdgeX() < parentLeftEdgeX;
                bool tooFarRight = rectTransform.RightEdgeX() > parentRightEdgeX;
                bool tooFarUp = rectTransform.UpperEdgeY() > parentUpperEdgeY;
                bool tooFarDown = parentLowerEdgeY > rectTransform.LowerEdgeY();

                float howFarOff;
                float leftEdgeDistance = (parentLeftEdgeX - rectTransform.LeftEdgeX());
                float rightEdgeDistance = (rectTransform.RightEdgeX() - parentRightEdgeX);
                float upperEdgeDistance = rectTransform.UpperEdgeY() - parentUpperEdgeY;
                float lowerEdgeDistance = parentLowerEdgeY - rectTransform.LowerEdgeY();
			
                if (tooFarLeft)
                {
                    howFarOff = leftEdgeDistance * (1f - effectivePos.x);

                    rectTransform.localPosition += new Vector3(howFarOff, 0, 0);
                }

                if (tooFarRight)
                {
                    howFarOff = rightEdgeDistance * effectivePos.x;

                    rectTransform.localPosition -= new Vector3(howFarOff, 0, 0);
                }


                if (tooFarUp)
                {
                    howFarOff = upperEdgeDistance * effectivePos.y;
                    rectTransform.localPosition -= new Vector3(0, howFarOff, 0);
                }

                if (tooFarDown)
                {
                    howFarOff = lowerEdgeDistance * (1f - effectivePos.y);
                    rectTransform.localPosition += new Vector3(0, howFarOff, 0);
                }


            }
        	
			// We're done! Revert the anchors.
			rectTransform.SetAnchors(originalAnchorMin, originalAnchorMax);
			rectTransform.pivot = originalPivot;
		}

		public static void SetWidth(this RectTransform rectTransform, float newWidth)
		{
			rectTransform.sizeDelta = new Vector2 (newWidth, rectTransform.sizeDelta.y);
		}

		public static void SetHeight(this RectTransform rectTransform, float newHeight)
		{
			rectTransform.sizeDelta = new Vector2 (rectTransform.sizeDelta.x, newHeight);
		}

    }
}
