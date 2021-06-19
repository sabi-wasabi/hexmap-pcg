using UnityEngine;
using UnityEditor;

namespace PigeonProject
{
    [CustomEditor(typeof(CamBehaviorCreator), true)]
    public class CamBehaviourCreatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (target is CamBehaviorCreator targetWithComponent)
            {
                GUILayout.Space(15f);

                if (GUILayout.Button("Create Camera Behaviour"))
                {
                    targetWithComponent.CreateBehavior();
                }
            }
        }
    }
}
