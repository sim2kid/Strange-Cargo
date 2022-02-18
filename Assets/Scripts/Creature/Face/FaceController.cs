using Genetics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Creature.Face
{
    [RequireComponent(typeof(FaceTexture))]
    public class FaceController : MonoBehaviour
    {
        public Dictionary<string, Texture> faces;
        FaceTexture faceTexture;
        public IGrabFace grabFace;
        public IEmotionCheck emotionCheck;
        public UnityEvent OnEmote;

        void Awake()
        {
            faceTexture = GetComponent<FaceTexture>();
        }

        void ChangeFacialExpression()
        {
            OnEmote.Invoke();

            faceTexture.SetExpression(grabFace.GrabEyes(emotionCheck.GrabEmotion()), grabFace.GrabMouth(emotionCheck.GrabEmotion()));
        }
    }
}
