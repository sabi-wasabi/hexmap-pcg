using System.Reflection;
using UnityEngine;
using UnityEditor;
using PigeonProject;

namespace PigeonProject
{
    [CustomEditor(typeof(DebugMethodCaller))]
    public class DebugMethodCallerEditor : Editor
    {
        DebugMethodCaller _target;

        Component[] _components;


        private void Awake()
        {
            _target = target as DebugMethodCaller;

            _components = _target.GetComponents<Component>();

            if (_components.Length != _target.ComponentToggles.Length)
                _target.ComponentToggles = new bool[_components.Length];
        }

        public override void OnInspectorGUI()
        {
            for (int i=0; i<_components.Length; i++)
            {
                var comp = _components[i];
                var binding = BindingFlags.Instance | BindingFlags.DeclaredOnly;
                var methodsPrivate = comp.GetType().GetMethods(binding | BindingFlags.NonPublic);
                var methodsPublic = comp.GetType().GetMethods(binding | BindingFlags.Public);

                if (methodsPrivate.Length > 0 && methodsPublic.Length > 0)
                {
                    _target.ComponentToggles[i] = EditorGUILayout.BeginFoldoutHeaderGroup(_target.ComponentToggles[i], comp.GetType().Name);
                    if (_target.ComponentToggles[i])
                    {
                        if (methodsPrivate.Length > 0)
                        {
                            EditorGUILayout.LabelField("Private methods", EditorStyles.boldLabel);
                            EditorPigeonUtility.ButtonsForMethods(comp, true);
                        }
                        if (methodsPublic.Length > 0)
                        {
                            EditorGUILayout.LabelField("Public methods", EditorStyles.boldLabel);
                            EditorPigeonUtility.ButtonsForMethods(comp);
                        }
                    } EditorGUILayout.EndFoldoutHeaderGroup();
                }
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}
