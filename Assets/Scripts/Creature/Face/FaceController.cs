using Genetics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Creature.Face
{
    [RequireComponent(typeof(FaceTexture))]
    public class FaceController : MonoBehaviour, IGrabFace, EmotionCheck
    {
        public Dictionary<string, Texture> faces;
        FaceTexture faceTexture;
        public UnityEvent OnEmote;

        void Awake()
        {
            faceTexture = GetComponent<FaceTexture>();
        }

        void ChangeFacialExpression()
        {
            OnEmote.Invoke();

            faceTexture.SetExpression(GrabEyes(GrabEmotion()), GrabMouth(GrabEmotion()));
        }

        public string GrabEmotion()
        {
            throw new System.NotImplementedException();
        }

        public void Hydrate(DNA creatureDna)
        {
            throw new System.NotImplementedException();
        }

        public Texture2D GrabEyes(string action)
        {
            throw new System.NotImplementedException();
        }

        public Texture2D GrabMouth(string action)
        {
            throw new System.NotImplementedException();
        }
    }
}
