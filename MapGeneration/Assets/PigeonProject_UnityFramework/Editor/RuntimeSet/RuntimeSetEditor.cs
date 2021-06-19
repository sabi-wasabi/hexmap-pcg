// The Pigeon Protocol

using UnityEngine;
using UnityEditor;

namespace PigeonProject
{
    public abstract class RuntimeSetEditor<T> : Editor where T : UnityEngine.Object
    {
        RuntimeSet<T> _target;

        private void OnEnable()
        {
            _target = (RuntimeSet<T>)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();

            if(_target.Count > 0)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Items: ");
                foreach (var item in _target.Items)
                    EditorGUILayout.LabelField(item.name);
                EditorGUILayout.EndVertical();

                EditorGUILayout.Space();

                if (GUILayout.Button("Clear"))
                {
                    _target.Clear();
                }
            }
        }
    }

    #region Implementations
    [CustomEditor(typeof(GameObjectSet))]
    public class GameObjectSetEditor : RuntimeSetEditor<GameObject> { }
    #endregion
}
