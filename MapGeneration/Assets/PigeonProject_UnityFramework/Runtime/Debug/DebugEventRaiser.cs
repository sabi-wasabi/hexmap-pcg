using PigeonProject;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices;

namespace PigeonProject
{
    public class DebugEventRaiser : MonoBehaviour
    {
        #region Variables
        [SerializeField] private GameEvent _event = default;
        #endregion

        public void RaiseEvent() => _event?.Raise();
    }
}
