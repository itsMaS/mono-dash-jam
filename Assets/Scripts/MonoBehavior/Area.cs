using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Area : MonoBehaviour
{
    public GameObject areaFill;
    public SpriteRenderer border;
    public SpriteRenderer fill;
    public ParticleSystem ps;

    public UnityEvent onEnter;
    public UnityEvent onStay;
    public UnityEvent onExit;

    public Color BorderColor;
    public Color FillColor;
    public int width;
    public int height;

    private void OnValidate()
    {
        areaFill.transform.localScale = new Vector3(width,height,1);
        border.size = new Vector2(width,height);
        border.color = BorderColor;
        fill.color = FillColor;

        var sc = ps.shape;
        sc.scale = new Vector3(width,height,1);
        var em = ps.emission;
        em.rateOverTime = width * height * 0.4f;
    }

    public void ParticlesSwarm(bool swarm)
    {
        var ef = ps.externalForces;
        ef.enabled = swarm;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onEnter.Invoke();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onStay.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            onExit.Invoke();
        }
    }
}
