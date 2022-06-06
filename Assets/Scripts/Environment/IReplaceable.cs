using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReplaceable
{
    public GameObject GameObject { get; }

    public Vector3 rotationOffset { get; }

    public void Pickup();
    public void Putdown();
}
