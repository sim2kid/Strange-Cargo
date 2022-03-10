using Genetics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Creature.Face
{
    [RequireComponent(typeof(FaceTexture))]
    public class FaceController : MonoBehaviour
    {
        FaceTexture faceTexture;
        public IGrabFace GrabFace;
        public IEmotionCheck EmotionCheck;
        private string Eyes;
        private string Mouth;
        private List<FaceAnimation> incomingAnimations = new List<FaceAnimation>();

        void Start()
        {
            faceTexture = GetComponent<FaceTexture>();
            Eyes = string.Empty;
            Mouth = string.Empty;
        }

        void Update()
        {
            if (faceTexture != null)
            {
                SetFaceTexture();
            }
        }

        public void PlayAnimation(FaceAnimation animation) 
        {
            incomingAnimations.Add(animation);
            // Sort from highest priority to lowest
            incomingAnimations = incomingAnimations.OrderBy(x => -x.priority).ToList();
        }

        private void SetFaceTexture()
        {
            string eyesAssignment = string.Empty;
            string mouthAssignment = string.Empty;
            // List of animations to remove
            List<FaceAnimation> finished = new List<FaceAnimation>();
            // Loop through each animation
            foreach (FaceAnimation fA in incomingAnimations)
            {
                // Get frame
                FaceClip frame = fA.CurrentFrame;
                if (frame == null)
                {
                    // Mark to discard animation if frame is used up
                    finished.Add(fA);
                    continue;
                }
                // Set frame is needed
                if (string.IsNullOrEmpty(eyesAssignment))
                {
                    eyesAssignment = frame.eyes;
                }
                if (string.IsNullOrEmpty(mouthAssignment))
                {
                    mouthAssignment = frame.mouth;
                }
                // Update animation
                fA.Update();
            }
            // Remove dead animations
            foreach (var f in finished) 
            {
                incomingAnimations.Remove(f);
            }

            // Assign base state if null
            if (string.IsNullOrEmpty(eyesAssignment))
            {
                eyesAssignment = EmotionCheck.GrabEmotion();
            }
            if(string.IsNullOrEmpty(mouthAssignment))
            {
                mouthAssignment = EmotionCheck.GrabEmotion();
            }

            // Remove assignment if unchanged or update last assignment if changed
            if (eyesAssignment == Eyes)
            {
                eyesAssignment = null;
            }
            else 
            {
                Eyes = eyesAssignment;
            }
            if(mouthAssignment == Mouth)
            {
                mouthAssignment = null;
            }
            else
            {
                Mouth = mouthAssignment;
            }

            // Set expression
            faceTexture.SetExpression(GrabFace.GrabEyes(eyesAssignment), GrabFace.GrabMouth(mouthAssignment));
        }
    }
}
