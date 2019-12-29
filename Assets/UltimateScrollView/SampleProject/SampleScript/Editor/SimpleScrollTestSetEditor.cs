using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hsinpa.Ultimate.Scrollview.Sample
{

    [CustomEditor(typeof(SimpleScrollTestSet))]
    public class SimpleScrollTestSetEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SimpleScrollTestSet myScript = (SimpleScrollTestSet)target;
            if (GUILayout.Button("Insert Object"))
            {
                myScript.Insert();
            }

            if (GUILayout.Button("Delete Object"))
            {
                myScript.Delete();
            }

        }
    }
}