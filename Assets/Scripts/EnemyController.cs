using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //prefabs
    [SerializeField] private int type;
    [SerializeField] private GameObject destruction;
    [SerializeField] private GameObject bulletObject;
    [SerializeField] private GameObject slimeBall;

    //stats
    [SerializeField] private int size = 1;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float range = 10f;
    [SerializeField] private float bulletsPerSecond = 1f;
    [SerializeField] private float bulletDamage = 1;

    //scene
    private Vector3 playerPos;
    private PlayerController playerController;
    private BoxCollider2D playerCollider;
    private Animation enemyAnimation;

    private Rigidbody2D rb;

    //audio
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Sound[] sounds;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerCollider = GameObject.Find("Player/Body").GetComponent<BoxCollider2D>();

        enemyAnimation = GetComponent<Animation>();

        if (type == 0)
        {
            StartCoroutine(Creeper());
            range = Mathf.Sqrt(size) / 2f;
        }
        else if (type == 1)
        {
            range = Mathf.Sqrt(size) / 2f;
            StartCoroutine(Slapper());
        }
        else if (type == 2)
        {
            StartCoroutine(Minigun());
        }

        transform.localScale = new Vector3(Mathf.Sqrt(size), Mathf.Sqrt(size), 1);
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, (transform.position - playerPos).normalized);
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
    public void SetBulletDamage(float bulletDamage)
    {
        this.bulletDamage = bulletDamage;
    }

    private IEnumerator Creeper()
    {
        while (true)
        {
            if (Vector2.Distance(playerPos, transform.position) < range)
            {
                //SFX
                yield return new WaitForSeconds(1f);

                if(Vector2.Distance(playerPos, transform.position) < range)
                {
                    //SFX
                    PlaySFX("Explode", 0.05f, 1f);
                    playerController.Damage(2*size);
                    Death();
                }
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
                PlaySFX("Kiss", 0.05f, 1f);
                playerController.Damage(bulletDamage);
                size+=1;
                range = Mathf.Sqrt(size)/2f;
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
                PlaySFX("Shoot", 0.05f, 1f);
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

    public void Damage(int damage, float knockback)
    {
        size -= damage;

        if(size <=0)
        {
            size = 0;
            var tempBall = Instantiate(slimeBall, transform.position, Quaternion.identity);
            if (transform.GetComponentInParent<EnemySpawner>() != null)
            {
                tempBall.GetComponent<Collectible>().Initialize(transform.GetComponentInParent<EnemySpawner>().GetWaveOverEvent());
            }
            else
            {
                Debug.LogWarning("No Enemy Spawner Found");
            }
            Death();
        }
        else
        {
            PlaySFX("Slap", 0.05f, 1f);
            enemyAnimation.Play();

            for (int i = 0; i < Random.Range(1, damage); i++)
            {
                var tempBall = Instantiate(slimeBall, transform.position, Quaternion.identity);
                if (transform.GetComponentInParent<EnemySpawner>() != null)
                {
                    tempBall.GetComponent<Collectible>().Initialize(transform.GetComponentInParent<EnemySpawner>().GetWaveOverEvent());
                }
                else
                {
                    Debug.LogWarning("No Enemy Spawner Found");
                }
            }

            if(type != 2)
            {
                range = Mathf.Sqrt(size) / 2f;
            }

            transform.localScale = new Vector3(Mathf.Sqrt(size), Mathf.Sqrt(size), 1);
            Vector3 temp = (transform.position - playerPos).normalized * -1 * knockback;
            transform.position -= temp;
        }
    }

    private void PlaySFX(string name, float variation, float volume)
    {
        Sound s = null;

        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == name)
            {
                s = sounds[i];
            }
        }

        audioSource.pitch = Random.Range(1f - variation, 1f + variation);
        audioSource.volume = volume;

        if (s == null)
        {
            Debug.LogError("SoundNotFound");
        }
        else
        {
            audioSource.PlayOneShot(s.clip);
        }
    }

    private void Death()
    {
        //SFX
        Instantiate(destruction, transform.position, Quaternion.identity);

        PlayerPrefs.SetInt("enemiesDefeated", PlayerPrefs.GetInt("enemiesDefeated", 0) + 1);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Body")
        {
            print("Enemy in body");
            StartCoroutine(InBody());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Body")
        {
            StopCoroutine(InBody());
        }
    }

    private IEnumerator InBody()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            Damage(1, 0);
        }
    }
}
