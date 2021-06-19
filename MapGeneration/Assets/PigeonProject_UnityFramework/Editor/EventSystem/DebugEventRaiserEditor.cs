using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PigeonProject
{
    [CustomEditor(typeof( DebugEventRaiser ) )]
    public class DebugEventRaiserEditor : Editor
    {
        #region Variables

        #endregion

        #region Unity Methods
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space( 15f );

            DebugEventRaiser gameEvent = target as DebugEventRaiser;

            if (GUILayout.Button( "Raise Event" ))
            {
                gameEvent.RaiseEvent();
            }
        }
        #endregion
    }
}
