using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20;
    private Vector2 movementDirection;

    Rigidbody2D bulletRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bulletRigidbody.velocity = movementDirection * bulletSpeed;
    }

    public void Initialize(Vector2 bulletDir)
    {
        movementDirection = bulletDir;
        if (bulletDir.y < 0)
        {
            transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y * -1);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            
        }
        //Destroy(gameObject);
    }
}
