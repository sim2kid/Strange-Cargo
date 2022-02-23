using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Face
{
    public class FaceAnimation
    {
        private List<FaceClip> faceClips;
        private int priority;
        private int frame;
        private float timePassedPerFrame;

        public FaceClip CurrentFrame => faceClips[frame];

        public void Update()
        {

        }
    }
}
