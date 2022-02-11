using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ShiftFocus : MonoBehaviour
{
    [SerializeField]
    public GameObject eyes;
    [SerializeField]
    float defaultGaussianStart = 10;

    void Start()
    {
        GetDepthOfField(Camera.main).gaussianStart.value = defaultGaussianStart;
    }

    // Update is called once per frame
    void Update()
    {
        SetFocalLength();
    }

    void SetFocalLength()
    {
        if (NearestGameObject() != null)
        {
            float distanceToNearestGameObject = (NearestGameObject().transform.position - eyes.transform.position).magnitude;
            GetDepthOfField(Camera.main).gaussianStart.value = distanceToNearestGameObject;
        }
        else
        {
            GetDepthOfField(Camera.main).gaussianStart.value = defaultGaussianStart;
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
    GameObject NearestGameObject()
    {
        Ray ray = new Ray(eyes.transform.position, eyes.transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, defaultGaussianStart);
        if (hits.Length > 0)
        {
            List<RaycastHit> objects = new List<RaycastHit>();
            foreach (RaycastHit h in hits)
            {
                objects.Add(h);
            }
            Queue<GameObject> hitQueue = SortByClosest(objects);
            GameObject nearest = hitQueue.Peek().gameObject;
            return nearest;
        }
        else
        {
            return null;
        }
    }
    public Queue<GameObject> SortByClosest(List<RaycastHit> objs)
    {
        Queue<GameObject> queue = new Queue<GameObject>();
        while (objs.Count > 0)
        {
            RaycastHit obj = GetClosest(objs);
            queue.Enqueue(obj.transform.gameObject);
            objs.Remove(obj);
        }
        return queue;
    }
    public RaycastHit GetClosest(List<RaycastHit> objs)
    {
        RaycastHit toReturn = objs[0];
        float shortest = int.MaxValue;
        foreach (RaycastHit i in objs)
        {
            float distance = i.distance;
            if (distance < shortest)
            {
                shortest = distance;
                toReturn = i;
            }
        }
        return toReturn;
    }
}
