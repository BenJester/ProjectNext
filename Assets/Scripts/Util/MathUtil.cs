using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtil
{
    public static void DrawDebugBox(Vector2 vecPos, Vector2 vecSize,float fAngle,float fDuration)
    {
        var orientation = Quaternion.Euler(0, 0, fAngle);

        // Basis vectors, half the size in each direction from the center.
        Vector2 right = orientation * Vector2.right * vecSize.x / 2f;
        Vector2 up = orientation * Vector2.up * vecSize.y / 2f;

        // Four box corners.
        Vector2 pos = new Vector2(vecPos.x, vecPos.y);
        var topLeft = pos + up - right;
        var topRight = pos + up + right;
        var bottomRight = pos - up + right;
        var bottomLeft = pos - up - right;

        // Now we've reduced the problem to drawing lines.
        Debug.DrawLine(topLeft, topRight, Color.red, fDuration);
        Debug.DrawLine(topRight, bottomRight, Color.red, fDuration);
        Debug.DrawLine(bottomRight, bottomLeft, Color.red, fDuration);
        Debug.DrawLine(bottomLeft, topLeft, Color.red, fDuration);
    }
    public static float AngleBetween(Vector2 vectorA, Vector2 vectorB)
    {
        float angle = Vector2.Angle(vectorA, vectorB);
        Vector3 cross = Vector3.Cross(vectorA, vectorB);

        if (cross.z > 0)
        {
            angle = 360 - angle;
        }

        return angle;
    }
}
