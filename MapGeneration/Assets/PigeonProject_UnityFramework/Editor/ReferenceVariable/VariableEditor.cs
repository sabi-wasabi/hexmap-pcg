// The Pigeon Protocol

using UnityEngine;
using UnityEditor;

namespace PigeonProject
{
    public abstract class VariableEditor<T> : Editor
    {
        //TODO: support updated game events from dialog system

        // Varaible
        SerializedProperty _initialValueProp = default;
        SerializedProperty _runtimeValueProp = default;
        SerializedProperty _isReadonlyProp = default;
        SerializedProperty _raiseEventProp = default;

        // Game Event
        SerializedProperty _parameterTypeProp = default;
        SerializedProperty _additionalNotesProp = default;

        BaseReferenceVariable<T> _target = default;

        private void OnEnable()
        {
            _initialValueProp = serializedObject.FindProperty("_initialValue");
            _runtimeValueProp = serializedObject.FindProperty("_runtimeValue");
            _isReadonlyProp = serializedObject.FindProperty("_isReadonly");
            _raiseEventProp = serializedObject.FindProperty("_raiseGameEvent");

            _parameterTypeProp = serializedObject.FindProperty("_parameterType");
            _additionalNotesProp = serializedObject.FindProperty("_additionalNotes");

            _target = target as BaseReferenceVariable<T>;
        }

        public override void OnInspectorGUI()
        {
            // Variable
            EditorGUILayout.LabelField("Variable Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_initialValueProp);
            // EditorGUILayout.PropertyField(_runtimeValueProp);
            EditorGUILayout.LabelField("Runtime Value\t" + _target.RuntimeValue);
            EditorGUILayout.PropertyField(_isReadonlyProp);

            EditorGUILayout.Space();

            if (GUILayout.Button("Update Initial Value"))
            {
                if (EditorUtility.DisplayDialog("Update Initial Value", "Are you sure you want to override Inital Value with the current Runtime Value?", "Yes"))
                {
                    _target.UpdateInitialValue();
                }
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Reset Runtime Value"))
            {
                _target.ResetRuntimeValue();
            }

            // TODO: draw line
            EditorGUILayout.Space(20f);

            // Game Event
            _raiseEventProp.boolValue = EditorGUILayout.BeginToggleGroup("Raise Game Event", _raiseEventProp.boolValue);
            if (_raiseEventProp.boolValue)
            {
                EditorGUILayout.PropertyField(_parameterTypeProp);
                EditorGUILayout.PropertyField(_additionalNotesProp);
            }EditorGUILayout.EndToggleGroup();

            serializedObject.ApplyModifiedProperties();
        }
    }


    #region Implementations
    [CustomEditor(typeof(FloatVariable))] public class FloatVariableEditor : VariableEditor<float> { }

    [CustomEditor(typeof(IntegerVariable))] public class IntegerVariableEditor : VariableEditor<int> { }

    [CustomEditor(typeof(StringVariable))] public class StringVariableEditor : VariableEditor<string> { }

    [CustomEditor(typeof(Vector2Variable))] public class Vector2VariableEditor : VariableEditor<Vector2> { }

    [CustomEditor(typeof(Vector3Variable))] public class Vector3VariableEditor : VariableEditor<Vector3> { }

    [CustomEditor(typeof(BoolVariable))] public class BoolVariableEditor : VariableEditor<bool> { }
    #endregion
}