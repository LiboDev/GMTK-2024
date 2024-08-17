using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private int damage;

    private Vector2 playerPos;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        float angle = Mathf.Atan2(playerPos.y - transform.position.y, playerPos.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        rb.velocity = transform.right * 10f;
    }

    public void SetDamage(int damage)
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
            Destroy(gameObject);
        }
    }
}
