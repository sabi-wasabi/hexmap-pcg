using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PigeonProject
{
    /// <summary>
    /// Acts as a scripo interface for SaveData.
    /// </summary>
    [CreateAssetMenu(menuName = "PigeonProject/SaveSystem/SaveDataSettings")]
    public class SaveDataSettings : ScriptableObject
    {
        public string Filename { get => _filename; }
        [SerializeField] private string _filename = "Save";

        public SaveData Data { get; set; } = new SaveData();
    }
}
