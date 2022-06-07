namespace Undefinable.Data
{

    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    public class DataManagement {
        /// <summary>
        /// Load a file, be sure to cast what you want back from it
        /// </summary>
        /// <param name="fileName">File name (Include filetype)</param>
        /// <param name="obj">Where to load the data into</param>
        public static object LoadData(string fileName, object obj) {
            if (File.Exists(Application.persistentDataPath + "/" + fileName)) { 
                try {
                    //Modify obj with data you need
                    JsonUtility.FromJsonOverwrite(File.ReadAllText(Application.persistentDataPath + "/" + fileName), obj);
                }
                catch {
                    // If there's something wrong with the remap file,
                    // Ignore and generate a new one with the current mappings.
                }
            }
            //Generate new savedata
            SaveData(fileName, obj);
            return obj;
        }

        /// <summary>
        /// Save a file
        /// </summary>
        /// <param name="fileName">File name (Include filetype)</param>
        /// <param name="obj">What data to use</param>
        public static void SaveData(string fileName, object obj) {
            File.WriteAllText(Application.persistentDataPath + "/" + fileName, JsonUtility.ToJson(obj));
            Debug.Log($"Saved to: {Application.persistentDataPath}");
        }
    }
}