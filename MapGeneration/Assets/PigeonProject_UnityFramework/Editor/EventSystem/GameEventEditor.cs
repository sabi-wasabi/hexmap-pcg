// The Pigeon Protocol

using UnityEditor;
using UnityEngine;

namespace PigeonProject
{
    [CustomEditor(typeof(GameEvent))]
    public class GameEventEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GUILayout.Space(15f);

            GameEvent gameEvent = target as GameEvent;

            if (GUILayout.Button("Raise"))
            {
                gameEvent.Raise();
            }
        }
    }
}
