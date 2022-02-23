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
            if(string.IsNullOrEmpty(Eyes) || string.IsNullOrEmpty(Mouth))
            {
                SetFaceTexture();
            }
            string currentEmotion = emotionCheck.GrabEmotion();
            if(lastKnownEmotion != currentEmotion)
            {
                SetFaceTexture();
            }
            lastKnownEmotion = currentEmotion;
        }

        private void SetFaceTexture()
        {
            Queue<FaceAnimation> faceAnimations = SortByPriority(incomingAnimations);
            int currentFrame = faceAnimations.Peek().GetFrame();
            Eyes = faceAnimations.Peek().faceClips[currentFrame].eyesString;
            Mouth = faceAnimations.Peek().faceClips[currentFrame].mouthString;
            if(string.IsNullOrEmpty(Eyes))
            {
                Eyes = emotionCheck.GrabEmotion();
            }
            if(string.IsNullOrEmpty(Mouth))
            {
                Mouth = emotionCheck.GrabEmotion();
            }
            faceTexture.SetExpression(grabFace.GrabEyes(Eyes),grabFace.GrabMouth(Mouth));

        }

        public Queue<FaceAnimation> SortByPriority(List<FaceAnimation> _animations)
        {
            Queue<FaceAnimation> queue = new Queue<FaceAnimation>();
            while(_animations.Count > 0)
            {
                FaceAnimation animation = GetHighestPriority(_animations);
                queue.Enqueue(animation);
                _animations.Remove(animation);
            }
            return queue;
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
