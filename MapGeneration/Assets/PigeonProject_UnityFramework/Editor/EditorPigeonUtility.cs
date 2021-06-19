using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace PigeonProject
{
    public static class EditorPigeonUtility
    {
        public static void RefreshInspector(Object target)
        {
            Selection.activeObject = null;
            void handler()
            {
                Selection.activeObject = target;
                EditorApplication.delayCall -= handler;
            }
            EditorApplication.delayCall += handler;
        }


        #region Horizontal Line
        public static readonly Color DEFAULT_COLOR = new Color(0f, 0f, 0f, 0.3f);
        public static readonly Vector2 DEFAULT_LINE_MARGIN = new Vector2(2f, 2f);
        public const float DEFAULT_LINE_HEIGHT = 1f;
        public static void HorizontalLine(Color color, float height, Vector2 margin)
        {
            GUILayout.Space(margin.x);

            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, height), color);

            GUILayout.Space(margin.y);
        }
        public static void HorizontalLine(Color color, float height) => HorizontalLine(color, height, DEFAULT_LINE_MARGIN);
        public static void HorizontalLine(Color color, Vector2 margin) => HorizontalLine(color, DEFAULT_LINE_HEIGHT, margin);
        public static void HorizontalLine(float height, Vector2 margin) => HorizontalLine(DEFAULT_COLOR, height, margin);
        public static void HorizontalLine(Color color) => HorizontalLine(color, DEFAULT_LINE_HEIGHT, DEFAULT_LINE_MARGIN);
        public static void HorizontalLine(float height) => HorizontalLine(DEFAULT_COLOR, height, DEFAULT_LINE_MARGIN);
        public static void HorizontalLine(Vector2 margin) => HorizontalLine(DEFAULT_COLOR, DEFAULT_LINE_HEIGHT, margin);
        public static void HorizontalLine() => HorizontalLine(DEFAULT_COLOR, DEFAULT_LINE_HEIGHT, DEFAULT_LINE_MARGIN);
        #endregion


        public static void ButtonsForMethods(Object target, bool showPrivate = false)
        {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.DeclaredOnly;
            if (showPrivate)
                bindingFlags |= BindingFlags.NonPublic;
            else
                bindingFlags |= BindingFlags.Public;
            MethodInfo[] methodInfos = target.GetType().GetMethods(bindingFlags);
            foreach (var method in methodInfos)
            {
                if (method.GetCustomAttribute<HideInInspector>() != null)
                    continue;
                if (method.GetParameters().Length == 0 &&
                    method.ReturnType == typeof(void))
                {
                    EditorGUILayout.Space();
                    var headerAttribute = method.GetCustomAttribute<MethodHeaderAttribute>();
                    if (headerAttribute != null)
                        EditorGUILayout.LabelField(headerAttribute.Header, EditorStyles.boldLabel);
                    if (GUILayout.Button(method.Name))
                        method.Invoke(target, null);
                }
            }
        }
    }
}
