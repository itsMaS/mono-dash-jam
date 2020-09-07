using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float magnetPower = 1;

    Transform player;
    bool magnet = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            magnet = true;
            player = collision.transform;
        }
    }

    private void FixedUpdate()
    {
        if (player && magnet)
        {
            float distance = (player.position - transform.position).magnitude;
            transform.Translate(Mathf.InverseLerp(10,1,distance)*Time.deltaTime*magnetPower*(player.position-transform.position));
        }
        if(player && magnet && Vector2.Distance(player.position,transform.position) < 0.5f)
        {
            player.GetComponent<PlayerController>().Pickup();
            AudioManager.PlaySound("Pickup",0.5f,0.2f);
            Destroy(gameObject);
        }
    }
}
