using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DeadSimpleDoor : MonoBehaviour
{
    public UnityEvent OnUse;
    private Vector3 currentPosition;
    private Vector3 currentRotation;

    public Vector3 DesiredPosition;

    public float ease = 1f;

    public bool isOpen;

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = transform.localPosition;
        currentRotation = transform.localRotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(isOpen)
            transform.localPosition -= (transform.localPosition - currentPosition) * ease * Time.deltaTime;
        else
            transform.localPosition -= (transform.localPosition - DesiredPosition) * ease * Time.deltaTime;
        //transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + ((transform.localRotation.eulerAngles - DesiredRotation) * ease * Time.deltaTime));

    }

    public void Use(bool noSignal = false) 
    {
        isOpen = !isOpen;
        if(!noSignal)
            OnUse.Invoke();
    }


}
