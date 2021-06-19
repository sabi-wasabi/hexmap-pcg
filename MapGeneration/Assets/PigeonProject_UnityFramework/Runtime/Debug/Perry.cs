using UnityEditor;
using UnityEngine;
using System;

namespace PigeonProject
{
    public class Perry : ScriptableObject
    {
        #region Settings
        [Header( "Global Settings" )]
        [Tooltip( "turns off all Logging" )]
        [SerializeField] private bool _turnOffLogging = default;
        [Tooltip( "turns off all Warnings" )]
        [SerializeField] private bool _turnOffWarnings = default;
        [Tooltip( "turns off all Errors" )]
        [SerializeField] private bool _turnOffErrors = default;
        [Tooltip("isolate specific Layer")]
        [Space(20f)]
        [SerializeField] private Layer _isolatedLayer = default;

        [Header( "Pigeon Project Layer Settings" )]
        [Space( 10f )]
        [SerializeField] private bool _toggleLoggingPigeon = default;
        [SerializeField] private bool _toggleWarningsPigeon = default;
        [SerializeField] private bool _toggleErrorsPigeon = default;

        [Header( "Layer A Settings" )]
        [Space( 10f )]
        [SerializeField] private bool _toggleLoggingLayerA = default;
        [SerializeField] private bool _toggleWarningsLayerA = default;
        [SerializeField] private bool _toggleErrorsLayerA = default;

        [Header( "Layer B Settings" )]
        [Space( 10f )]
        [SerializeField] private bool _toggleLoggingLayerB = default;
        [SerializeField] private bool _toggleWarningsLayerB = default;
        [SerializeField] private bool _toggleErrorsLayerB = default;

        [Header( "Layer C Settings" )]
        [Space( 10f )]
        [SerializeField] private bool _toggleLoggingLayerC = default;
        [SerializeField] private bool _toggleWarningsLayerC = default;
        [SerializeField] private bool _toggleErrorsLayerC = default;
        #endregion

        #region Properties
        public static Perry Instance { get; private set; }
        #endregion


        private void OnEnable()
        {
            if (Instance == null)
                Instance = this;
        }

        #region static Methods
        public static void Log(string msg, GameObject obj = null, Layer layer = Layer.None)
        {
            if (Instance == null)
                return;

            switch (layer)
            {
                case Layer.A:
                    Instance.Print( Instance._toggleLoggingLayerA, layer, msg, obj );
                    break;
                case Layer.B:
                    Instance.Print( Instance._toggleLoggingLayerB, layer, msg, obj );
                    break;
                case Layer.C:
                    Instance.Print( Instance._toggleLoggingLayerC, layer, msg, obj );
                    break;
                default:
                    if (Instance._turnOffLogging && !Instance._toggleLoggingPigeon) return;
                    Instance.Print( Instance._toggleLoggingPigeon, Layer.Pigeon, msg, obj);
                    break;
            }
        }

        public static void Log( object msg, GameObject obj = null, Layer layer = Layer.None )
        {
            if (Instance == null)
                return;

            switch (layer)
            {
                case Layer.A:
                    Instance.Print( Instance._toggleLoggingLayerA, layer, msg.ToString(), obj );
                    break;
                case Layer.B:
                    Instance.Print( Instance._toggleLoggingLayerB, layer, msg.ToString(), obj );
                    break;
                case Layer.C:
                    Instance.Print( Instance._toggleLoggingLayerC, layer, msg.ToString(), obj );
                    break;
                default:
                    if (Instance._turnOffLogging && !Instance._toggleLoggingPigeon) return;
                    Instance.Print( Instance._toggleLoggingPigeon, Layer.Pigeon, msg.ToString(), obj );
                    break;
            }
        }

        public static void LogWarning(string msg, GameObject obj = null, Layer layer = Layer.None)
        {
            switch (layer)
            {
                case Layer.A:
                    Instance.Warning( Instance._toggleWarningsLayerA, layer, msg, obj );
                    break;
                case Layer.B:
                    Instance.Warning( Instance._toggleWarningsLayerB, layer, msg, obj );
                    break;
                case Layer.C:
                    Instance.Warning( Instance._toggleWarningsLayerC, layer, msg, obj );
                    break;
                default:
                    if (Instance._turnOffLogging && !Instance._toggleWarningsPigeon) return;
                    Instance.Warning( Instance._toggleWarningsPigeon, Layer.Pigeon, msg, obj );
                    break;
            }
        }

        public static void LogWarning( object msg, GameObject obj = null, Layer layer = Layer.None )
        {
            switch (layer)
            {
                case Layer.A:
                    Instance.Warning( Instance._toggleWarningsLayerA, layer, msg.ToString(), obj );
                    break;
                case Layer.B:
                    Instance.Warning( Instance._toggleWarningsLayerB, layer, msg.ToString(), obj );
                    break;
                case Layer.C:
                    Instance.Warning( Instance._toggleWarningsLayerC, layer, msg.ToString(), obj );
                    break;
                default:
                    if (Instance._turnOffLogging && !Instance._toggleWarningsPigeon) return;
                    Instance.Warning( Instance._toggleWarningsPigeon, Layer.Pigeon, msg.ToString(), obj );
                    break;
            }
        }

        public static void LogError( string msg, GameObject obj = null, Layer layer = Layer.None )
        {
            switch (layer)
            {
                case Layer.A:
                    Instance.Error( Instance._toggleErrorsLayerA, layer, msg, obj );
                    break;
                case Layer.B:
                    Instance.Error( Instance._toggleErrorsLayerB, layer, msg, obj );
                    break;
                case Layer.C:
                    Instance.Error( Instance._toggleErrorsLayerC, layer, msg, obj );
                    break;
                default:
                    if (Instance._turnOffLogging && !Instance._toggleErrorsPigeon) return;
                    Instance.Error( Instance._toggleErrorsPigeon, Layer.Pigeon, msg, obj );
                    break;
            }
        }

        public static void LogError( object msg, GameObject obj = null, Layer layer = Layer.None )
        {
            switch (layer)
            {
                case Layer.A:
                    Instance.Error( Instance._toggleErrorsLayerA, layer, msg.ToString(), obj );
                    break;
                case Layer.B:
                    Instance.Error( Instance._toggleErrorsLayerB, layer, msg.ToString(), obj );
                    break;
                case Layer.C:
                    Instance.Error( Instance._toggleErrorsLayerC, layer, msg.ToString(), obj );
                    break;
                default:
                    if (Instance._turnOffLogging && !Instance._toggleErrorsPigeon) return;
                    Instance.Error( Instance._toggleErrorsPigeon, Layer.Pigeon, msg.ToString(), obj );
                    break;
            }
        }
        #endregion

        #region Print Methods
        private void Print( bool layerToggle, Layer layer, string msg, GameObject obj = null )
        {
            if (!layerToggle || (Instance._isolatedLayer != Layer.None && Instance._isolatedLayer != layer) || Instance._turnOffLogging) return;

            Debug.Log($"<color=white>{msg}</color>\n{obj}\nLayer {layer}" );
        }

        private void Warning(bool layerToggle, Layer layer, string msg, GameObject obj = null)
        {
            if (!layerToggle || (Instance._isolatedLayer != Layer.None && Instance._isolatedLayer != layer) || Instance._turnOffWarnings) return;

            Debug.LogWarning($"<color=orange>{msg}</color>\n{obj}\nLayer {layer}" );
        }

        private void Error(bool layerToggle, Layer layer, string msg, GameObject obj = null)
        {
            if (!layerToggle || (Instance._isolatedLayer != Layer.None && Instance._isolatedLayer != layer) || Instance._turnOffErrors) return;

            Debug.LogError( $"<color=red>{msg}</color>\n{obj}\nLayer {layer}" );
        }
        #endregion
    }

    /// <summary>
    /// Information about the Debug Log Layer
    /// </summary>
    public enum Layer
    {
        None, A, B, C, Pigeon
    }
}