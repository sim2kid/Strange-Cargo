using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Importing
{
    [System.Serializable]
    public class Database : ScriptableObject
    {
        [SerializeField]
        private List<string> keys;
        [SerializeField]
        private List<Folder> values;

        /// <summary>
        /// Disctionary of < "Folder Location" ,Folder>
        /// </summary>
        public Dictionary<string, Folder> Folders = new Dictionary<string, Folder>();

        public void Serialize() 
        {
            keys = new List<string>();
            values = new List<Folder>();

            foreach (KeyValuePair<string, Folder> kvp in Folders) 
            {
                keys.Add(kvp.Key);
                values.Add(kvp.Value);
            }
        }

        public void DeSerialize()
        {
            Folders = new Dictionary<string, Folder>();
            for (int i = 0; i < keys.Count; i++)
            {
                Folders.Add(keys[i], values[i]);
            }
        }
    }
}
