using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature
{
    public class ClosenessSocialBoost : MonoBehaviour
    {
        Player.PlayerController player;
        CreatureController controller;

        float socialBoostPerSecond = 5;

        void Start()
        {
            player = FindObjectOfType<Player.PlayerController>();
            controller = GetComponent<CreatureController>();
        }

        void Update()
        {
            var distance = Vector3.Distance(controller.transform.position, player.transform.position);
            float boost = 0;
            if (distance <= 10) 
            {
                distance = (10 - distance) / 10f;
                boost = function(socialBoostPerSecond, distance);

                controller.needs += new Stats.Needs() { Social = boost / Time.deltaTime };
            }
        }

        private float function(float x, float pos) 
        {
            return (Mathf.Pow(-pos, 2) + 1) * x;
        }
    }
}