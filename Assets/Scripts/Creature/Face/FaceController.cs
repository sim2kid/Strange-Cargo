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
        string lastKnownEmotion;

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
            foreach(FaceAnimation fA in faceAnimations)
            {
                if (string.IsNullOrEmpty(eyesAssignment))
                {
                    if (!string.IsNullOrEmpty(fA.faceClips[fA.frame].eyesString))
                    {
                        if (fA.faceClips[fA.frame].eyesString != Eyes)
                        {
                            eyesAssignment = fA.faceClips[fA.frame].eyesString;
                        }
                    }
                }
                else
                {
                    return;
                }
                if (string.IsNullOrEmpty(mouthAssignment))
                {
                    if (!string.IsNullOrEmpty(fA.faceClips[fA.frame].mouthString))
                    {
                        if (fA.faceClips[fA.frame].mouthString != Mouth)
                        {
                            mouthAssignment = fA.faceClips[fA.frame].mouthString;
                        }
                    }
                }
                else
                {
                    return;
                }
            }
            if (string.IsNullOrEmpty(eyesAssignment))
            {
                if (emotionCheck.GrabEmotion() != lastKnownEmotion)
                {
                    eyesAssignment = emotionCheck.GrabEmotion();
                }
                else
                {
                    eyesAssignment = lastKnownEmotion;
                }
            }
            if(string.IsNullOrEmpty(mouthAssignment))
            {
                if(emotionCheck.GrabEmotion() != lastKnownEmotion)
                {
                    mouthAssignment = emotionCheck.GrabEmotion();
                }
                else
                {
                    mouthAssignment = lastKnownEmotion;
                }
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
