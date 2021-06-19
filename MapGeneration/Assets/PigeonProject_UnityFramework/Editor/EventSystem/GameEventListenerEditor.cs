// The Pigeon Protocol

using UnityEngine;
using UnityEditor;

namespace PigeonProject
{
    [CustomEditor( typeof( GameEventListener ) )]
    public class GameEventListenerEditor : Editor
    {
        SerializedProperty _gameEventsProp = default;
        SerializedProperty _responsesProp = default;
        SerializedProperty _foldoutProp = default;

        GameEventListener _target = default;


        private void OnEnable()
        {
            _gameEventsProp = serializedObject.FindProperty("_gameEvents");
            _responsesProp = serializedObject.FindProperty("_responses");
            _foldoutProp = serializedObject.FindProperty("_editorFoldout");
            _target = target as GameEventListener;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();
            {
                for (int i=0; i<_gameEventsProp.arraySize; i++)
                {
                    var gameEvent = _gameEventsProp.GetArrayElementAtIndex(i).objectReferenceValue as GameEvent;
                    var eventName = new GUIContent(gameEvent != null ? gameEvent.name : "Event not set");

                    EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
                    {
                        EditorGUILayout.LabelField(eventName, EditorStyles.boldLabel, GUILayout.Width(EditorStyles.boldLabel.CalcSize(eventName).x));
                        if (gameEvent != null)
                        {
                            string raiseButtonTooltip = "Invokes the game event with null as parameter. Only works during runtime.";
                            if (GUILayout.Button(new GUIContent("Raise", raiseButtonTooltip), GUILayout.Width(50), GUILayout.ExpandWidth(false)))
                            {
                                gameEvent.Raise(null);
                            }
                        }
                        if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.ExpandWidth(false)))
                        {
                            _target.RemoveEvent(i);
                        }
                    }EditorGUILayout.EndHorizontal();
                    EditorGUILayout.ObjectField(_gameEventsProp.GetArrayElementAtIndex(i), GUIContent.none);

                    if (gameEvent != null)
                    {
                        var foldoutProp = _foldoutProp.GetArrayElementAtIndex(i);
                        foldoutProp.boolValue = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutProp.boolValue, "Responses");
                        if (foldoutProp.boolValue)
                        {
                            EditorGUILayout.PropertyField(_responsesProp.GetArrayElementAtIndex(i));
                        } EditorGUILayout.EndFoldoutHeaderGroup();
                    }
                    EditorPigeonUtility.HorizontalLine();
                }
            } EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            if (GUILayout.Button("Add event", GUILayout.Width(100)))
            {
                _target.AddEvent();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
