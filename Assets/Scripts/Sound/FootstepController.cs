using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enviroment;

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
            RaycastHit hit;

            Enviroment.Material myMaterial = Enviroment.Material.None;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 2))
            {
                IMaterial m = hit.transform.gameObject.GetComponent<IMaterial>();
                if (m != null)
                    myMaterial = m.Material;
            }

            string newAudioMap = SoundRepository.EnviromentSoundBank(myMaterial);
            player.Sound.LoadAudio(newAudioMap);
        }
    }
}