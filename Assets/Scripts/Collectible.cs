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

        rb.velocity = new Vector2(Random.Range(-3, 3), Random.Range(-3, 3));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.parent.GetComponent<PlayerController>().Grow(1);
            Death();
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
