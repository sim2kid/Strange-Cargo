using Creature.Stats;
using DataType;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    [RequireComponent(typeof(AreaZone))]
    public class PoopZone : NeedSource, IObject
    {
        [SerializeField]
        public List<GameObject> PoopModels = new List<GameObject>();

        [SerializeField]
        public UnityEvent OnPoop;


        public string Name { get => gameObject.name; set { gameObject.name = value; } }

        private string _guid;
        public string Guid => _guid;

        public bool Equals(IObject other)
        {
            return (string.Equals(this.Name, other.Name) && string.Equals(this.Guid, other.Guid));
        }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_guid)) 
            {
                _guid = new System.Guid().ToString();
            }
        }

        public Vector3 GetPoint() 
        {
            return GetComponent<AreaZone>().GetRandomPoint();
        }

        public void Poop(Vector3 location) 
        {
            GameObject model = PoopModels[Random.Range(0, PoopModels.Count)];
            Instantiate(model, location, Quaternion.identity);
        }
    }
}