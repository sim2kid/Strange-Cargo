using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Placement
{
    [RequireComponent(typeof(PlayerController))]
    public class PlacementController : MonoBehaviour
    {
        private PlayerController player;
        private Transform headTransform;
        [SerializeField]
        private Shader hologramShader;

        void Start()
        {
            player = GetComponent<PlayerController>();
            headTransform = player.Eyes.transform;
        }


        void Update()
        {

        }
    }
}