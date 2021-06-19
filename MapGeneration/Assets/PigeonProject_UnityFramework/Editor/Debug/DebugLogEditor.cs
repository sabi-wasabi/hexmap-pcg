using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PigeonProject
{
    [CustomEditor( typeof( Perry ) )]
    [CanEditMultipleObjects]
    public class DebugLogEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GUILayout.Space( 20f );

            string info = "Global Debug Log Settings\nto turn on click checkbox";
            EditorGUILayout.HelpBox(info, MessageType.Info, true);

            GUILayout.Space( 20f );

            DrawDefaultInspector();
        }
    }
}