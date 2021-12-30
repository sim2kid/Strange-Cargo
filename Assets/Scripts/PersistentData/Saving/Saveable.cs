using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Saving
{
    [System.Serializable]
    public class Saveable : MonoBehaviour
    {
        [Tooltip("Location of the Prefab in Resources. Keep blank if non available")]
        public string PrefabResourceLocation;
        [Tooltip("This will delete the object before loading in copies. Keep this false for persistant objects.")]
        public bool DeleteBeforeLoad = false;


        public List<ISaveable> LocalInfo = new List<ISaveable>();

        private void Awake()
        {
            
        }

        public bool PreSerialization() 
        {
            bool back = true;
            var mySavables = this.GetComponents<ISaveable>();
            var myKidsSaveables = this.GetComponentsInChildren<ISaveable>(true);
            LocalInfo.Clear();
            LocalInfo.AddRange(mySavables);
            LocalInfo.AddRange(myKidsSaveables);

            foreach (var saveable in mySavables)
                if (!saveable.PreSerialization())
                {
                    back = false;
                    Console.LogError($"There was aa problem saving the component {saveable.GetType()}.");
                }

            return true;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}