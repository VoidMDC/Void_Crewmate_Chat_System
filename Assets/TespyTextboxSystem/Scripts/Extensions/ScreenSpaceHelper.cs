using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class ScreenSpaceHelper
{

    public static float GetScreenSpaceWidth()
    {
        // remember that in screen coords, the center of the screen is the origin
        Vector2 leftEdge = Camera.main.ViewportToScreenPoint(-Vector2.one);
        // hence why the left edge is a negative coordinate

        Vector2 rightEdge = Camera.main.ViewportToScreenPoint(Vector2.one);

        float result = (rightEdge.x - leftEdge.x);

        return result;
    }

    public static float GetScreenSpaceHeight()
    {
        // remember that in screen coords, the center of the screen is the origin
        Vector2 leftEdge = Camera.main.ViewportToScreenPoint(-Vector2.one);
        // hence why the left edge is a negative coordinate

        Vector2 rightEdge = Camera.main.ViewportToScreenPoint(Vector2.one);

        float result = (rightEdge.y - leftEdge.y);

        return result;
    }

    public static Vector2 GetScreenSize()
    {
        Vector2 size = Vector2.zero;
        size.x += GetScreenSpaceWidth();
        size.y += GetScreenSpaceHeight();

        return size;
    }
}
