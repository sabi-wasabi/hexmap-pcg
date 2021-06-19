using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PigeonProject
{
    [CustomEditor(typeof(PrefabReferenceBehaviour))]
    public class PrefabReferenceBehaviourEditor : Editor
    {
        SerializedProperty _markerProp = default;

        private void OnEnable()
        {
            _markerProp = serializedObject.FindProperty("_marker");
        }
        public override void OnInspectorGUI()
        {
            DrawPropertiesExcluding(serializedObject, "m_Script");
        }
    }
}
