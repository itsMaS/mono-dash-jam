using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTranform;
    public float followCofficient;

    void Update()
    {
        if(followTranform)
        {
            transform.Translate(followCofficient * (followTranform.position - transform.position));
        }
    }
}
