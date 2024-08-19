using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private GameObject destruction;

    [SerializeField] float bulletSpeed = 20;
    private Vector2 movementDirection;

    //How long should the bullet last before despawning? (In seconds)
    private float despawnTime = 2f;
    private float despawnTimer = 0f;

    [SerializeField] private int damage = 1;

    [SerializeField] private float knockback = 0;

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

        despawnTimer += Time.deltaTime;

        if (despawnTimer >= despawnTime)
        {
            Destroy(this.gameObject);
        }
    }

    public void Initialize(Vector2 bulletDir, int damage, float knockback, float range)
    {
        SetDamage(damage);
        SetKnockback(knockback);
        despawnTime = range;
        movementDirection = bulletDir;
        if (bulletDir.y < 0)
        {
            transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y * -1);
        }
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public void SetKnockback(float knockback)
    {
        this.knockback = knockback;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.transform.GetComponent<EnemyController>().Damage(damage, knockback);
            Death();
        }
    }

    private void Death()
    {
        Instantiate(destruction, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
