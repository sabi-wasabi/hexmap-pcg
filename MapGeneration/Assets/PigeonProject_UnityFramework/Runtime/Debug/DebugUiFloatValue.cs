using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace PigeonProject
{
    public class DebugUiFloatValue : MonoBehaviour
    {
        [SerializeField] FloatVariable[] _floatVariables = default;
        [SerializeField] BoolVariable[] _boolVariables = default;

        Text _debugText;

        private void OnEnable()
        {
            _debugText = GetComponent<Text>();
        }

        private void OnGUI()
        {
            string text = "";

            foreach (var variable in _floatVariables)
                text += GetVariableInfo(variable);
            text += "\n";

            foreach (var variable in _boolVariables)
                text += GetVariableInfo(variable);

            _debugText.text = text;
        }

        private string GetVariableInfo<T>(BaseReferenceVariable<T> variable)
        {
            return variable.name + ": " + variable.RuntimeValue.ToString() + " / " + variable.InitialValue + "\n";
        }
    }
}
