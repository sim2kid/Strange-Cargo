using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Environment;
using UnityEngine.Events;

namespace Sound
{
    [RequireComponent(typeof(AudioPlayer))]
    public class FootstepController : MonoBehaviour
    {
        [Tooltip("The speed at which steps are played")]
        [SerializeField]
        public float StepsRate = 1;
        [Tooltip("Adjust so that 'IsGrounded' is false during a jump")]
        [SerializeField]
        private float GroundCheckDistance = 1.5f;
        [SerializeField]
        private bool isPlayer = false;

        private Vector3 lastPosition;
        [Header("View Only")]
        [SerializeField]
        private float speed;
        [SerializeField]
        bool isGrounded;

        [SerializeField]
        public UnityEvent OnStep;
        [SerializeField]
        float timeOffset = 0.01f;

        private AudioPlayer player;

        // Start is called before the first frame update
        void Start()
        {
            lastPosition = transform.position;
            player = GetComponent<AudioPlayer>();
            player.enabled = true;
            player.DelayAfter = true;
            player.OnPlay.AddListener(() => 
            {
                Invoke("RunOnStep", timeOffset);
            });
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            speed = (transform.position - lastPosition).magnitude;
            lastPosition = transform.position;

            isGrounded = GoundCheck();

            if (isGrounded && speed > 0.001f)
            {
                CheckFloorType();
                player.Sound.Delay = StepsRate;
                player.Play();
            }
        }

        void RunOnStep() 
        {
            OnStep.Invoke();
        }

        private bool GoundCheck() 
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, GroundCheckDistance)) 
            {
                return !hit.collider.isTrigger;
            }
            return false;
        }

        private void CheckFloorType() 
        {
            RaycastHit hit;

            Environment.Material myMaterial = Environment.Material.None;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, GroundCheckDistance))
            {
                IMaterial m = hit.transform.gameObject.GetComponent<IMaterial>();
                if (m != null)
                    myMaterial = m.Material;
            }

            string newAudioMap = SoundRepository.EnviromentSoundBank(myMaterial, !isPlayer);
            player.Sound.LoadAudio(newAudioMap);
        }
    }
}