using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound.Player;
using Player.Movement;
using Sound.Structure;
using Sound.Source.Internal;
using Sound.Source;

[RequireComponent(typeof(AudioPlayer))]
[RequireComponent(typeof(MovementController))]
public class Footsteps : MonoBehaviour
{
    public float StepRate = 1.4f;
    public string StepRepository;

    private float timer;
    private AudioPlayer ap;
    private MovementController mover;

    private SwitchContainer stepSound;

    [SerializeField]
    private Environment.Material materialType;

    void Start()
    {
        ap = GetComponent<AudioPlayer>();
        mover = GetComponent<MovementController>();
        timer = 0;
        stepSound = new SwitchContainer();
        foreach (string name in System.Enum.GetNames(typeof(Environment.Material)))
            if (!name.ToLower().Equals("none"))
            {
                RandomContainer r = new RandomContainer();
                r.Containers.Add(new ResourceList(StepRepository + "/" + name));
                stepSound.Containers.Add(r);
            }
            else 
            {
                stepSound.Containers.Add(new SilentSource());
            }
        stepSound.Selection = 0;
        materialType = Environment.Material.None;
    }


    void FixedUpdate()
    {
        PlaySound();
        SetFloorSound();
    }

    void PlaySound() 
    {
        if (timer > 0)
        {
            timer -= Time.fixedDeltaTime;
            if (timer < 0)
                timer = 0;
        }
        else if (mover.IsOnGround)
        {
            Vector3 vector = mover.Velocity;
            vector.y = 0;
            if (vector.magnitude > 0)
            {
                stepSound.Selection = (int)materialType;
                ap.PlayOneShot(stepSound);
                timer += StepRate;
            }
        }
    }

    void SetFloorSound() 
    {
        // Try raycast directly under player
        if (Physics.Raycast(mover.GetFootOrigin(), Vector3.down, out RaycastHit hitInfo, mover.GetRadius(), mover.LayerMask)) 
        {
            var sm = hitInfo.collider.gameObject.GetComponent<Environment.SurfaceMaterial>();
            if (sm != null)
            {
                materialType = sm.Material;
                return;
            }
        }

        // If that doesn't work, try everything else
        Collider[] colliders = Physics.OverlapSphere(mover.GetFootOrigin(), mover.GetRadius());
        foreach (Collider c in colliders)
        {
            Environment.SurfaceMaterial sm = c.gameObject.GetComponent<Environment.SurfaceMaterial>();
            if (sm != null)
            {
                materialType = sm.Material;
                return;
            }
        }
        materialType = Environment.Material.None;
    }
}
