using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Face {
    public class Blink : MonoBehaviour
    {
        static FaceAnimation blinkAnimation;
        FaceController face;
        float cooldown;

        private void Awake()
        {
            if (blinkAnimation == null)
            {
                blinkAnimation = AnimationBuilder.LoadFromResources("Data/FaceAnimations/Blink");
            }
        }

        void Start()
        {
            face = GetComponent<FaceController>();
            if (face == null)
            {
                Console.LogWarning($"No face controller was found on \"{gameObject.name}\". Blinking script will be deleted.");
                Destroy(this);
            }
            cooldown = 0;
        }

        void Update()
        {
            if (!Utility.Toolbox.Instance.Pause.Paused)
            {
                if (cooldown <= 0)
                {
                    cooldown += Random.Range(2f, 7f);
                    CloseEyes();
                }
                else 
                {
                    cooldown -= Time.deltaTime;
                }
            }
        }

        void CloseEyes() 
        {
            face.PlayAnimation(blinkAnimation);
        }
    }
}