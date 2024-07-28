using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SAS.StateMachineGraph.Editor
{
    public static class EditorUtilities
    {
        public static void VerticalLine(Rect rect, float width, Color color)
        {
            var style = new GUIStyle();
            style.normal.background = EditorGUIUtility.whiteTexture;
            style.fixedWidth = width;

            var c = GUI.color;
            GUI.color = color;
            GUI.Box(rect, GUIContent.none, style);
            GUI.color = c;
        }

        public static void HorizontalLine(Rect rect, float width, Color color)
        {
            var style = new GUIStyle();
            style.normal.background = EditorGUIUtility.whiteTexture;
            style.fixedHeight = width;

            var c = GUI.color;
            GUI.color = color;
            GUI.Box(rect, GUIContent.none, style);
            GUI.color = c;
        }

        public static void DrawArrowLine(Vector3 start, Vector3 end)
        {
            DrawLine(start, end);
            DrawArrow(start, end, false);
        }

        public static void DrawLine(Vector3 start, Vector3 end)
        {
            var arrowLine = new Vector3[2];
            arrowLine[0] = start;
            arrowLine[1] = end;
            Handles.color = Color.grey;
            Handles.DrawAAPolyLine(5, arrowLine);
        }

        public static void DrawArrow(Vector3 start, Vector3 end, bool inverted)
        {
            if (inverted)
            {
                var temp = start;
                start = end;
                end = temp;
            }
            var arrowHead = new Vector3[3];

            var right = (end - start).normalized;
            var up = Vector3.Cross(Vector3.forward, right).normalized;
            var size = 20;//HandleUtility.GetHandleSize(end);
            var width = size * 0.3f;
            var height = size * 0.3f;
            var mid = Vector3.Lerp(start, end, 0.50f);

            arrowHead[0] = mid + right * height;
            arrowHead[1] = mid - right * height + up * width;
            arrowHead[2] = mid - right * height - up * width;

            Handles.color = Color.white;
            Handles.DrawAAConvexPolygon(arrowHead);
        }

        public static void DrawTrippleArrow(Vector3 start, Vector3 end, bool inverted)
        {
            if (inverted)
            {
                var temp = start;
                start = end;
                end = temp;
            }
            var arrowHead = new Vector3[3];

            var right = (end - start).normalized;
            var up = Vector3.Cross(Vector3.forward, right).normalized;
            var size = 20;// HandleUtility.GetHandleSize(end);
            var width = size * 0.3f;
            var height = size * 0.3f;

            Handles.color = Color.white;

            var mid = Vector3.Lerp(start, end, 0.50f);

            for (int i = -1; i <= 1; i++)
            {
                var pos = mid - right * height * i * 1.5f;
                arrowHead[0] = pos + right * height;
                arrowHead[1] = pos - right * height + up * width;
                arrowHead[2] = pos - right * height - up * width;
                Handles.DrawAAConvexPolygon(arrowHead);
            }
        }
    }
}
