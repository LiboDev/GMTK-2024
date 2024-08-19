using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    //scene
    [SerializeField] private GameObject destruction;
    private Rigidbody2D rb;

    //tracking
    private float damage;

    private Vector2 playerPos;

    

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Break", 10f);

        rb = GetComponent<Rigidbody2D>();

        float angle = Mathf.Atan2(playerPos.y - transform.position.y, playerPos.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + Random.Range(-10f, 10f)));

        rb.velocity = transform.right * 10f;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
    public void PlayerPos(Vector2 pos)
    {
        this.playerPos = pos;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.transform.parent.GetComponent<PlayerController>().Damage(damage);
            Death();
        }
    }

    private void Break()
    {
        Destroy(gameObject);
    }

    private void Death()
    {
        Instantiate(destruction, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
