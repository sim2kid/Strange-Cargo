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

        public FaceAnimation(List<FaceClip> clips, int priority = 0) 
        {
            if (clips == null)
            {
                clips = new List<FaceClip>();
            }
            faceClips = clips;
            this.priority = priority;
            frame = 0;
            timePassed = 0;
        }

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
            if (CurrentFrame == null) return;

            // Increment Animation
            timePassed += Time.deltaTime;
            if(Time.deltaTime < CurrentFrame.duration)
            {
                timePassed -= CurrentFrame.duration;
                frame++;
            }
        }

        public FaceAnimation Copy() 
        {
            return new FaceAnimation(faceClips, priority);
        }
    }
}
