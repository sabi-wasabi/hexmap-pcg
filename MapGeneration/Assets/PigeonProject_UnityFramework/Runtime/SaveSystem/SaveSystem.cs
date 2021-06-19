using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace PigeonProject
{
    [CreateAssetMenu(menuName = "PigeonProject/SaveSystem/SaveSystem")]
    public class SaveSystem : ScriptableObject
    {
        // TODO: update support for multiple save files and easy switching
        // TODO: maybe enable loading of savedata into its instance (for JSON representation) without actually updating game state
        // TOOD: add option for automatic filename generation via the SaveSystem

        #region Editor Properties
        [Header("Events")]
        [SerializeField] GameEvent _beforeSaveEvent = default;
        [SerializeField] GameEvent _afterLoadEvent = default;
        [Header("Settings")]
        [SerializeField] string _fileEnding = "bomboclaat";
        [Header("Saves")]
        [SerializeField] SaveDataSettings _activeSaveData = default;
        [SerializeField] SaveDataSettings[] _saveDataSlots = default;
        #endregion

        #region Static Properties
        static public SaveSystem Instance { get => _instance; }
        static SaveSystem _instance = default;
        #endregion


        private void OnEnable()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                DestroyImmediate(this);
            }

            if (_activeSaveData == null && _saveDataSlots.Length > 0)
                _activeSaveData = _saveDataSlots[0];
        }


        #region Saving and Loading
        /// <summary>
        /// Raises the BeforeSave game event which allows all listeners to push data to the active SaveData instance.
        /// Afterwards the SaveData gets written to a binary file.
        /// </summary>
        public void SaveGame()
        {
            _beforeSaveEvent.Raise(_activeSaveData.Data);
            string path = $"{Application.persistentDataPath}/{_activeSaveData.Filename}.{_fileEnding}";

            var formatter = CreateBinaryFormatter();
            using FileStream fs = new FileStream(path, FileMode.Create);
            formatter.Serialize(fs, _activeSaveData.Data);

            Debug.Log("Game saved at path: " + path);
        }

        /// <summary>
        /// Loads the given SaveData from file.
        /// </summary>
        /// <remarks>
        /// This only fills the SaveData scripo. For actually loading the data into the game the AfterLoad event needs to be raised.
        /// </remarks>
        /// <param name="saveData">the SaveData instance to be loaded.</param>
        public void LoadSaveData(SaveDataSettings saveData)
        {
            string path = $"{Application.persistentDataPath}/{saveData.Filename}.{_fileEnding}";
            if (File.Exists(path))
            {
                var formatter = CreateBinaryFormatter();
                using FileStream fs = new FileStream(path, FileMode.Open);
                saveData.Data = formatter.Deserialize(fs) as SaveData;

                Debug.Log($"Save at {path} loaded!");
            }
            else
            {
                Debug.LogWarning($"Save file at {path} not found!");
            }
        }
        /// <summary>
        /// Loads all registered save data slots from file.
        /// </summary>
        public void LoadSaveDataAll()
        {
            foreach (var saveData in _saveDataSlots)
                LoadSaveData(saveData);
        }

        /// <summary>
        /// Loads the active SaveData into the game by raising the AfterLoad event.
        /// Listeners can then retrieve their data from the updated SaveData instance.
        /// </summary>
        public void LoadGame()
        {
            _afterLoadEvent.Raise(_activeSaveData.Data);

            Debug.Log($"Game loaded with {_activeSaveData.Filename}");
        }
        /// <summary>
        /// Loads the game with the given save data.
        /// </summary>
        /// <remarks>
        /// This only loads the local state of the save data into the game.
        /// For loading the save data from file you first have to call LoadSaveData().
        /// </remarks>
        /// <param name="saveData">the save data to be loaded.</param>
        public void LoadGame(SaveData saveData)
        {
            _afterLoadEvent.Raise(saveData);
        }
        #endregion

        /// <summary>
        /// Sets the active save data to another slot.
        /// </summary>
        /// <param name="index">The index of the save data to be set active.</param>
        /// <returns>true if the index is valid; false otherwise.</returns>
        public bool SetActiveSlot(int index)
        {
            if (index >= 0 && index <_saveDataSlots.Length)
            {
                _activeSaveData = _saveDataSlots[index];
                return true;
            }
            return false;
        }


        private BinaryFormatter CreateBinaryFormatter()
        {
            var formatter = new BinaryFormatter();
            return formatter;
        }
    }
}
