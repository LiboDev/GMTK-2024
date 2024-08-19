using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    //scene
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 vel = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        vel.Normalize();
        rb.velocity = vel * Random.Range(1f,3f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.parent.GetComponent<PlayerController>().Grow(0.5f);
            Death();
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
