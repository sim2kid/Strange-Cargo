using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Face
{
    public class FaceAnimation
    {
        public List<FaceClip> faceClips;
        public int priority;
        private int frame;
        private float timePassed;

        public FaceClip CurrentFrame
        {
            get
            {
                if (frame >= faceClips.Count)
                {
                    return null;
                }
                else
                {
                    return faceClips[frame];
                }
            }
        }

        public void Update()
        {
            timePassed += Time.deltaTime;
            if(Time.deltaTime < CurrentFrame.duration)
            {
                timePassed -= CurrentFrame.duration;
                frame++;
            }
        }
    }
}
