using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private int type;
    [SerializeField] private GameObject destruction;
    [SerializeField] private GameObject bulletObject;

    [SerializeField] private int size = 1;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float range = 10f;
    [SerializeField] private float bulletsPerSecond = 1f;
    [SerializeField] private int bulletDamage = 1;

    private Vector3 playerPos;
    private PlayerController playerController;
    private BoxCollider2D playerCollider;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();


        /*player = GameObject.Find("Player/Body").transform;*/
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerCollider = GameObject.Find("Player/Body").GetComponent<BoxCollider2D>();

        if (type == 0)
        {
            StartCoroutine(Creeper());
        }
        else if (type == 1)
        {
            StartCoroutine(Slapper());
        }
        else if (type == 2)
        {
            StartCoroutine(Minigun());
        }
    }

    public void SetSize(int size)
    {
        this.size = size;
    }
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
    public void SetRange(float range)
    {
        this.range = range;
    }
    public void SetBulletsPerSecond(float bulletsPerSecond)
    {
        this.bulletsPerSecond = bulletsPerSecond;
    }
    public void SetBulletDamage(int bulletDamage)
    {
        this.bulletDamage = bulletDamage;
    }

    private IEnumerator Creeper()
    {
        while (true)
        {
            if (Vector2.Distance(playerPos, transform.position) < range)
            {
                playerController.Damage(5);
                //Instantiate(destruction, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
            yield return null;
        }
    }

    private IEnumerator Slapper()
    {
        while (true)
        {
            if (Vector2.Distance(playerPos, transform.position) < range)
            {
                playerController.Damage(1);
                size++;
                range = Mathf.Sqrt(size);
                transform.localScale = new Vector3(Mathf.Sqrt(size), Mathf.Sqrt(size), 1);

                yield return new WaitForSeconds(1f/bulletsPerSecond);
            }

            yield return null;
        }
    }

    private IEnumerator Minigun()
    {
        while (true)
        {
            if (Vector2.Distance(playerPos, transform.position) < range)
            {
                GameObject bullet = Instantiate(bulletObject, transform.position, Quaternion.identity);
                BulletController bulletController = bullet.GetComponent<BulletController>();
                bulletController.SetDamage(bulletDamage);
                bulletController.PlayerPos(playerPos);

                yield return new WaitForSeconds(1f / bulletsPerSecond);
            }

            yield return null;
        }
    }

    void FixedUpdate()
    {
        playerPos = playerCollider.ClosestPoint(transform.position);

        if (Vector2.Distance(playerPos, transform.position) > range)
        {
            Vector3 direction = playerPos - transform.position;
            direction.Normalize();
            rb.velocity = direction * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
}
