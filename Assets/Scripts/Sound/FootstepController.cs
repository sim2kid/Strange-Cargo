using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sound
{
    [RequireComponent(typeof(MovementController))]
    [RequireComponent(typeof(AudioPlayer))]
    public class FootstepController : MonoBehaviour
    {
        private Vector3 lastPosition;
        [SerializeField]
        private float StepsRate = 1;
        private float speed;
        private MovementController pc;
        private AudioPlayer player;

        // Start is called before the first frame update
        void Start()
        {
            lastPosition = transform.position;
            pc = GetComponent<MovementController>();
            player = GetComponent<AudioPlayer>();
            player.enabled = true;
            player.DelayAfter = true;
        }

        // Update is called once per frame
        void Update()
        {
            speed = Vector3.Distance(transform.position, lastPosition) * (50 / pc.MoveSpeed);
            lastPosition = transform.position;

            if (pc.isOnGround && speed > 0.1f)
            {
                CheckFloorType();
                player.Sound.Delay.Value = StepsRate;
                player.Play();
            }
        }

        private void CheckFloorType() 
        {
            // TO-DO
            string newAudioMap = SoundRepository.EnviromentSoundBank(EnviromentSound.Wood);
            player.Sound.LoadAudio(newAudioMap);
        }
    }
}