using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static bool ScreenShake = true;

    Vector3 previousPos;
    Vector3 newPos;

    public float shakeAmount;
    public float shakeFrequency;
    public float shakeFalloff;
    public float shakeClamp;

    float defaultZoom;
    float charge;
    float lerp;

    private void Update()
    {
        if(ScreenShake)
        {
            lerp = Mathf.Clamp(lerp + Time.deltaTime * shakeFrequency, 0, 1);
            shakeAmount = Mathf.Clamp(shakeAmount - shakeFalloff * Time.deltaTime, 0, shakeClamp);
            transform.localPosition = Vector3.Lerp(previousPos, newPos, lerp);
            if (lerp == 1)
            {
                lerp = 0;
                previousPos = transform.localPosition;
                newPos = shakeAmount * new Vector3(Random.value, Random.value) + new Vector3(0, 0, -10);
            }
        }
    }

    public void Shake(float amount)
    {
        shakeAmount += amount;
    }
}
