using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ShiftFocus : MonoBehaviour
{
    private GameObject eyes;
    [SerializeField]
    LayerMask layerMask;
    [SerializeField]
    float defaultGaussianStart = 10;

    void Start()
    {
        GetDepthOfField(Camera.main).gaussianStart.value = defaultGaussianStart;
        eyes = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        SetFocalLength();
    }

    void SetFocalLength()
    {
        if (NearestPoint() != Vector3.zero)
        {
            float distanceToNearestGameObject = Vector3.Distance(NearestPoint(), eyes.transform.position);
            GetDepthOfField(Camera.main).gaussianStart.value = distanceToNearestGameObject + defaultGaussianStart;
        }
        else
        {
            GetDepthOfField(Camera.main).gaussianStart.value = float.MaxValue;
        }
    }
    DepthOfField GetDepthOfField(Camera _cam)
    {
        VolumeProfile profile = _cam.GetComponent<Volume>().sharedProfile;
        if(!profile.TryGet<DepthOfField>(out var depthOfField))
        {
            depthOfField = profile.Add<DepthOfField>(false);
        }
        return depthOfField;
    }
    Vector3 NearestPoint()
    {
        Ray ray = new Ray(eyes.transform.position, eyes.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, layerMask, QueryTriggerInteraction.Ignore)) 
        {
            return hitInfo.point;
        }
        return Vector3.zero;
    }
}
