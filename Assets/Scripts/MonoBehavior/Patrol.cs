using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public AnimationCurve platformMovement;

    public float frequency;
    public Vector2 PatrolPos;
    public float offsetSeconds;

    Vector2 DefaultPos;
    float lerp;
    float currentTime;

    private void Awake()
    {
        currentTime = Time.time;
        DefaultPos = transform.position;
    }

    void Update()
    {
        lerp = platformMovement.Evaluate((frequency*(Time.time-currentTime+offsetSeconds)) % 1);
        transform.position = Vector2.Lerp(DefaultPos, DefaultPos + PatrolPos, lerp);
    }
}
