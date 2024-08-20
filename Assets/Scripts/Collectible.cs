using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public void Initialize(UnityEvent<GameObject> waveEnd)
    {
        waveEnd.AddListener(ReturnToCenter);
    }

    private void ReturnToCenter(GameObject enemySpawner)
    {
        StartCoroutine(DriftToCenter());
    }

    private IEnumerator DriftToCenter()
    {
        float temp = 0;
        Vector3 start = transform.position;

        while (transform.position != new Vector3(-42.97f, -3.15f, 0))
        {
            transform.position = Vector2.Lerp(start, new Vector2(-42.97f, -3.15f), temp);
            temp += Time.deltaTime * 0.05f;
            if (temp > 1)
            {
                temp = 1;
            }
            yield return null;
        }
        yield return null;
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
