using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature
{
    public class ClosenessSocialBoost : MonoBehaviour
    {
        Player.PlayerController player;
        CreatureController controller;

        float socialBoostPerSecond = 0.3f;

        void Start()
        {
            player = FindObjectOfType<Player.PlayerController>();
            controller = GetComponent<CreatureController>();
        }

        void Update()
        {
            var distance = Vector3.Distance(controller.transform.position, player.transform.position);
            float boost = 0;
            if (distance <= 10 && !Utility.Toolbox.Instance.Pause.Paused) 
            {
                distance = (10 - distance) / 10f;
                boost = function(distance) * socialBoostPerSecond;

                var add = Stats.Needs.Zero;
                add.Social = boost * Time.deltaTime;

                controller.needs += add;
            }
        }

        private float function(float x) 
        {
            return (Mathf.Pow(-x, 2) + 1);
        }
    }
}