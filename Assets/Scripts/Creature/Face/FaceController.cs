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
            if(string.IsNullOrEmpty(Eyes))
            {

            }
            if(string.IsNullOrEmpty(Mouth))
            {

            }
        }

        private void SetFaceTexture()
        {
            Queue<FaceAnimation> faceAnimations = SortByPriority(incomingAnimations);
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
