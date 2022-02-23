using Genetics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Face
{
    [RequireComponent(typeof(FaceTexture))]
    public class FaceController : MonoBehaviour
    {
        FaceTexture faceTexture;
        public IGrabFace grabFace;
        public IEmotionCheck emotionCheck;
        string Eyes;
        string Mouth;
        public List<FaceAnimation> incomingAnimations;

        void Awake()
        {
            faceTexture = GetComponent<FaceTexture>();
            Eyes = string.Empty;
            Mouth = string.Empty;
        }

        void Update()
        {
            SetFaceTexture();
        }

        private void SetFaceTexture()
        {
            List<FaceAnimation> faceAnimations = SortByPriority(incomingAnimations);
            string eyesAssignment = string.Empty;
            string mouthAssignment = string.Empty;
            FaceClip frame = new FaceClip();
            foreach(FaceAnimation fA in faceAnimations)
            {
                fA.Update();
                frame = fA.CurrentFrame;
                if (string.IsNullOrEmpty(eyesAssignment))
                {
                    eyesAssignment = frame.eyesString;
                }
                if (string.IsNullOrEmpty(mouthAssignment))
                {
                    mouthAssignment = frame.mouthString;
                }
            }
            if (string.IsNullOrEmpty(eyesAssignment))
            {
                eyesAssignment = emotionCheck.GrabEmotion();
            }
            if(string.IsNullOrEmpty(mouthAssignment))
            {
                mouthAssignment = emotionCheck.GrabEmotion();
            }
            if(frame.eyesString == Eyes)
            {
                eyesAssignment = null;
            }
            if(frame.mouthString == Mouth)
            {
                mouthAssignment = null;
            }
            faceTexture.SetExpression(grabFace.GrabEyes(eyesAssignment), grabFace.GrabMouth(mouthAssignment));

        }

        public List<FaceAnimation> SortByPriority(List<FaceAnimation> _animations)
        {
            List<FaceAnimation> list = new List<FaceAnimation>();
            while(_animations.Count > 0)
            {
                FaceAnimation animation = GetHighestPriority(_animations);
                list.Add(animation);
                _animations.Remove(animation);
            }
            return list;
        }

        public FaceAnimation GetHighestPriority(List<FaceAnimation> _animations)
        {
            FaceAnimation toReturn = _animations[0];
            float highest = int.MaxValue;
            foreach(FaceAnimation fA in _animations)
            {
                float priority = fA.priority;
                if(priority < highest)
                {
                    highest = priority;
                    toReturn = fA;
                }
            }
            return toReturn;
        }
    }
}
