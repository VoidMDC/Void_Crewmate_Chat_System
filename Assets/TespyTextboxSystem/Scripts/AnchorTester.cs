using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TeaspoonTools.TextboxSystem;
using TeaspoonTools.Utils.Extensions;

public class AnchorTester : MonoBehaviour {

    public GameObject parent;
    RectTransform parentRect {  get { return parent.GetComponent<RectTransform>(); } }
    RectTransform rectTransform;
    public Vector2 vecAnchor;
    public TextAnchor enumAnchor;
    public bool useEnum = false;

    void Start()
    {
        parent = transform.parent.gameObject;
        rectTransform = GetComponent<RectTransform>();
        //PlaceRelativeToParent(vecAnchor);
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Placing child object relative to its parent!");

            if (useEnum)
                rectTransform.PositionRelativeToParent(enumAnchor);
            else
                rectTransform.PositionRelativeToParent(vecAnchor);
        }
    }

    void PlaceRelativeToParent(Vector2 anchor, bool showFull = true)
    {

        rectTransform.PositionRelativeToParent(vecAnchor);


        /*
        // Thinking in terms of the Cartesian coordinate system will make this easier,
        // so first, find the lower left corner of the parent in this object's local
        // space
        Vector3 lowerLeftCorner = parentRect.GetLowerLeftCorner();
        Debug.Log("The lower left corner of the parent is : " + lowerLeftCorner);

        // Now place this object based on the anchor
        float shiftAmountX = parentRect.rect.width * anchor.x;
        float shiftAmountY = parentRect.rect.height * anchor.y;
        rectTransform.localPosition = lowerLeftCorner + new Vector3(shiftAmountX, shiftAmountY, 0);
        rectTransform.KeepWithinParentBounds(parent);
        */
    }
}
