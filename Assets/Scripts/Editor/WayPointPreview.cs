using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

    public class WayPointPreview
    {
        static public WayPointPreview s_Preview = null;
        static public GameObject preview;

        static protected MovingObjectByWayPoint movingPlatform;

        static WayPointPreview()
        {
            Selection.selectionChanged += SelectionChanged;
        }

        static void SelectionChanged()
        {
            if (movingPlatform != null && Selection.activeGameObject != movingPlatform.gameObject)
            {
                DestroyPreview();
            }
        }

        static public void DestroyPreview()
        {
            if (preview == null)
                return;

            Object.DestroyImmediate(preview);
            preview = null;
            movingPlatform = null;
        }

        static public void CreateNewPreview(MovingObjectByWayPoint origin)
        {
            if(preview != null)
            {
                Object.DestroyImmediate(preview);
            }

            movingPlatform = origin; 

            preview = Object.Instantiate(origin.gameObject);
            preview.hideFlags = HideFlags.DontSave;
            MovingObjectByWayPoint plt = preview.GetComponentInChildren<MovingObjectByWayPoint>();
            Object.DestroyImmediate(plt);


            Color c = new Color(0.2f, 0.2f, 0.2f, 0.4f);
            SpriteRenderer[] rends = preview.GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < rends.Length; ++i)
                rends[i].color = c;
        }
    }