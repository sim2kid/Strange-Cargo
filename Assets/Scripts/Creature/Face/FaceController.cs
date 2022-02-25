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
        private List<FaceAnimation> incomingAnimations;

        void Start()
        {
            faceTexture = GetComponent<FaceTexture>();
            Eyes = string.Empty;
            Mouth = string.Empty;
        }

        void Update()
        {
            SetFaceTexture();
        }

        public void PlayAnimation(FaceAnimation animation) 
        {
            incomingAnimations.Add(animation);
            // Sort from highest to lowest
            incomingAnimations = incomingAnimations.OrderBy(x => -x.priority).ToList();
        }

        private void SetFaceTexture()
        {
            string eyesAssignment = string.Empty;
            string mouthAssignment = string.Empty;
            List<FaceAnimation> finished = new List<FaceAnimation>();
            foreach (FaceAnimation fA in incomingAnimations)
            {
                fA.Update();
                FaceClip frame = fA.CurrentFrame;
                if (frame == null)
                {
                    finished.Add(fA);
                    continue;
                }
                if (string.IsNullOrEmpty(eyesAssignment))
                {
                    eyesAssignment = frame.eyesString;
                }
                if (string.IsNullOrEmpty(mouthAssignment))
                {
                    mouthAssignment = frame.mouthString;
                }
            }
            foreach (var f in finished) 
            {
                incomingAnimations.Remove(f);
            }

            if (string.IsNullOrEmpty(eyesAssignment))
            {
                eyesAssignment = EmotionCheck.GrabEmotion();
            }
            if(string.IsNullOrEmpty(mouthAssignment))
            {
                mouthAssignment = EmotionCheck.GrabEmotion();
            }

            if(eyesAssignment == Eyes)
            {
                eyesAssignment = null;
            }
            if(mouthAssignment == Mouth)
            {
                mouthAssignment = null;
            }
            faceTexture.SetExpression(GrabFace.GrabEyes(eyesAssignment), GrabFace.GrabMouth(mouthAssignment));

        }
    }
}
