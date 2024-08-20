using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //General head variables
    private Rigidbody2D headRigidbody2D;
    private Transform headTransform;
    private Transform brainTransform;

    //Head movement variables
    public float speed = 10;
    public float maxSpeed = 100;

    //General body variables
    private GameObject body;
    private SpriteRenderer bodySpriteRenderer;
    private Animation myAnimation;

    //Variables related to the body stretching
    private Vector3 startPos;

    //Variables relating to the body returning to the head
    [SerializeField] public float returnTimeModifier = 1;
    public float maxReturnTimeModifier = 2;

    //stats
    [SerializeField] private float playerSize = 25;
    public int maxSize = 200;
    [SerializeField] public float bulletInterval = 1.5f;
    [SerializeField] public float range = 10f;
    public int maxRange = 100;
    [SerializeField] public int bulletDamage = 1;
    public int maxDamage = 10;
    [SerializeField] public float bulletKnockback = 0;
    public int maxKnockback = 10;
    [SerializeField] public float damageReduction = 0;
    public float maxDamageReduction = 0.5f;
    //
    private bool canShoot = true;
    [SerializeField] public int bulletsPerSecond = 1;
    public int maxBPS = 10;

    //Prefabs
    [SerializeField] private GameObject bulletPrefab;

    //General Camera Variables
    private CinemachineVirtualCamera playerCamera;

    //Objects
    [SerializeField] private Slider gooBar;
    [SerializeField] private Slider sizeBar;
    [SerializeField] private GameObject upgradePanel;

    //audio
    [SerializeField] private Sound[] sounds;
    [SerializeField] private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        headRigidbody2D = transform.GetChild(0).GetComponent<Rigidbody2D>();
        headTransform = transform.GetChild(0).GetComponent<Transform>();
        brainTransform = transform.GetChild(2);

        playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<CinemachineVirtualCamera>();
        body = transform.GetChild(1).gameObject;
        bodySpriteRenderer = body.GetComponent<SpriteRenderer>();
        myAnimation = body.GetComponent<Animation>();

        startPos = headTransform.position;

        PlayerPrefs.SetFloat("sizeScore", playerSize);

        StartCoroutine(Stretch());
    }

    // Update is called once per frame
    void Update()
    {
        playerCamera.m_Lens.OrthographicSize = Mathf.Abs(body.transform.localScale.x) + Mathf.Abs(body.transform.localScale.y);
        if (playerCamera.m_Lens.OrthographicSize < 10)
        {
            playerCamera.m_Lens.OrthographicSize = 10;
        }

        if (gooBar != null)
        {
            gooBar.value = (body.transform.localScale.x * body.transform.localScale.y) / playerSize;
        }

        if (sizeBar != null)
        {
            sizeBar.value = playerSize / maxSize;
        }

        body.transform.position = (headTransform.position + startPos) / 2;
        body.transform.localScale = new Vector3(Mathf.Abs(headTransform.position.x - startPos.x) + 2, Mathf.Abs(headTransform.position.y - startPos.y) + 2, 0);
        brainTransform.position = startPos;

        if (canShoot)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                Shoot();
                FireUp();
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                Shoot();
                FireDown();
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                Shoot();
                FireRight();
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                Shoot();
                FireLeft();
            }
        }

        bodySpriteRenderer.color = new Color(bodySpriteRenderer.color.r, bodySpriteRenderer.color.g, bodySpriteRenderer.color.b, Mathf.Clamp(1 - ((body.transform.localScale.x * body.transform.localScale.y) / playerSize), 0.2f, 0.9f));
    }

    private void Reload()
    {
        canShoot = true;
    }

    private void Shoot()
    {
        playerSize -= 0.1f;
        ShrinkPlayer(0.1f);
        canShoot = false;
        Invoke("Reload", 1f / bulletsPerSecond);

        PlaySFX("Shoot", 0.05f, 1f);
    }

    private void FireUp()
    {
        Vector2 bulletPos = headTransform.position;
        int numberOfBullets = (int) (body.transform.localScale.x / bulletInterval);

        if (numberOfBullets == 0 || numberOfBullets == 1)
        {
            if (headTransform.position.y > startPos.y)
            {
                bulletPos = new Vector2(bulletPos.x, headTransform.position.y + 1);
            }
            else
            {
                bulletPos = new Vector2(bulletPos.x, headTransform.position.y + body.transform.localScale.y);
            }
            GameObject temp = Instantiate(bulletPrefab, bulletPos, Quaternion.LookRotation(new Vector3(0, 0, 1)));
            temp.GetComponent<PlayerBullet>().Initialize(Vector2.up, bulletDamage, bulletKnockback, range);
        }
        else
        {
            float bulletLength = (numberOfBullets * bulletInterval);
            float remainder = body.transform.localScale.x - bulletLength;

            for (int i = 0; i < numberOfBullets; i++)
            {
                if (headTransform.position.x > startPos.x)
                {
                    bulletPos = new Vector2(headTransform.position.x - (bulletInterval * i) - (remainder / 2), bulletPos.y);
                }
                else
                {
                    bulletPos = new Vector2(headTransform.position.x + (bulletInterval * i) + (remainder / 2), bulletPos.y);
                }
                if (headTransform.position.y > startPos.y)
                {
                    bulletPos = new Vector2(bulletPos.x, headTransform.position.y + 1);
                }
                else
                {
                    bulletPos = new Vector2(bulletPos.x, headTransform.position.y + body.transform.localScale.y);
                }

                GameObject temp = Instantiate(bulletPrefab, bulletPos, Quaternion.LookRotation(new Vector3(0, 0, 1)));
                temp.GetComponent<PlayerBullet>().Initialize(Vector2.up, bulletDamage, bulletKnockback, range);
            }
        }
    }

    private void FireDown()
    {
        Vector2 bulletPos = headTransform.position;
        int numberOfBullets = (int) (body.transform.localScale.x / bulletInterval);

        if (numberOfBullets == 0 || numberOfBullets == 1)
        {
            if (headTransform.position.y > startPos.y)
            {
                bulletPos = new Vector2(bulletPos.x, headTransform.position.y + body.transform.localScale.y);
            }
            else
            {
                bulletPos = new Vector2(bulletPos.x, headTransform.position.y - 1);
            }
            GameObject temp = Instantiate(bulletPrefab, bulletPos, Quaternion.LookRotation(new Vector3(0, 0, -1)));
            temp.GetComponent<PlayerBullet>().Initialize(Vector2.down, bulletDamage, bulletKnockback, range);
        }
        else
        {
            float bulletLength = numberOfBullets * bulletInterval;
            float remainder = body.transform.localScale.x - bulletLength;

            for (int i = 0; i < numberOfBullets; i++)
            {
                if (headTransform.position.x > startPos.x)
                {
                    bulletPos = new Vector2(headTransform.position.x - (bulletInterval * i) - (remainder / 2), bulletPos.y);
                }
                else
                {
                    bulletPos = new Vector2(headTransform.position.x + (bulletInterval * i) + (remainder / 2), bulletPos.y);
                }
                if (headTransform.position.y > startPos.y)
                {
                    bulletPos = new Vector2(bulletPos.x, headTransform.position.y - body.transform.localScale.y);
                }
                else
                {
                    bulletPos = new Vector2(bulletPos.x, headTransform.position.y - 1);
                }

                GameObject temp = Instantiate(bulletPrefab, bulletPos, Quaternion.LookRotation(new Vector3(0, 0, -1)));
                temp.GetComponent<PlayerBullet>().Initialize(Vector2.down, bulletDamage, bulletKnockback, range);
            }
        }
    }

    private void FireRight()
    {
        Vector2 bulletPos = headTransform.position;
        int numberOfBullets = (int) (body.transform.localScale.y / bulletInterval);

        if (numberOfBullets == 0 || numberOfBullets == 1)
        {
            if (headTransform.position.x > startPos.x)
            {
                bulletPos = new Vector2(headTransform.position.x + 1, bulletPos.y);
            }
            else
            {
                bulletPos = new Vector2(headTransform.position.x + body.transform.localScale.x, bulletPos.y);
            }
            GameObject temp = Instantiate(bulletPrefab, bulletPos, Quaternion.LookRotation(Vector3.forward, Vector3.right));
            temp.GetComponent<PlayerBullet>().Initialize(Vector2.right, bulletDamage, bulletKnockback, range);
        }
        else
        {
            float bulletLength = numberOfBullets * bulletInterval;
            float remainder = body.transform.localScale.y - bulletLength;

            for (int i = 0; i < numberOfBullets; i++)
            {
                if (headTransform.position.x > startPos.x)
                {
                    bulletPos = new Vector2(headTransform.position.x + 1, bulletPos.y);
                }
                else
                {
                    bulletPos = new Vector2(headTransform.position.x + body.transform.localScale.x, bulletPos.y);
                }
                if (headTransform.position.y > startPos.y)
                {
                    bulletPos = new Vector2(bulletPos.x, headTransform.position.y - (bulletInterval * i) - (remainder / 2));
                }
                else
                {
                    bulletPos = new Vector2(bulletPos.x, headTransform.position.y + (bulletInterval * i) + (remainder / 2));
                }

                GameObject temp = Instantiate(bulletPrefab, bulletPos, Quaternion.LookRotation(Vector3.forward, Vector3.right));
                temp.GetComponent<PlayerBullet>().Initialize(Vector2.right, bulletDamage, bulletKnockback, range);
            }
        }
    }

    private void FireLeft()
    {
        Vector2 bulletPos = headTransform.position;
        int numberOfBullets = (int)(body.transform.localScale.y / bulletInterval);

        if (numberOfBullets == 0 || numberOfBullets == 1)
        {
            if (headTransform.position.x > startPos.x)
            {
                bulletPos = new Vector2(headTransform.position.x - body.transform.localScale.x, bulletPos.y);
            }
            else
            {
                bulletPos = new Vector2(headTransform.position.x - 1, bulletPos.y);
            }
            GameObject temp = Instantiate(bulletPrefab, bulletPos, Quaternion.LookRotation(Vector3.forward, Vector3.left));
            temp.GetComponent<PlayerBullet>().Initialize(Vector2.left, bulletDamage, bulletKnockback, range);
        }
        else
        {
            float bulletLength = numberOfBullets * bulletInterval;
            float remainder = body.transform.localScale.y - bulletLength;

            for (int i = 0; i < numberOfBullets; i++)
            {
                if (headTransform.position.x > startPos.x)
                {
                    bulletPos = new Vector2(headTransform.position.x - body.transform.localScale.x, bulletPos.y);
                }
                else
                {
                    bulletPos = new Vector2(headTransform.position.x - 1, bulletPos.y);
                }
                if (headTransform.position.y > startPos.y)
                {
                    bulletPos = new Vector2(bulletPos.x, headTransform.position.y - (bulletInterval * i) - (remainder / 2));
                }
                else
                {
                    bulletPos = new Vector2(bulletPos.x, headTransform.position.y + (bulletInterval * i) + (remainder / 2));
                }
                GameObject temp = Instantiate(bulletPrefab, bulletPos, Quaternion.LookRotation(Vector3.forward, Vector3.left));
                temp.GetComponent<PlayerBullet>().Initialize(Vector2.left, bulletDamage, bulletKnockback, range);
            }
        }
    }

    private IEnumerator Stretch()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))
            {
                startPos = headTransform.position;

                while (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))
                {
                    Vector2 movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                    movementVector.Normalize();
                    headTransform.rotation = Quaternion.LookRotation(Vector3.forward, movementVector);
                    if (!((Mathf.Abs(body.transform.localScale.x) * Mathf.Abs(body.transform.localScale.y)) >= playerSize) || Mathf.Abs(headTransform.position.x - startPos.x) > Mathf.Abs(headTransform.position.x - startPos.x + movementVector.x) || Mathf.Abs(headTransform.position.y - startPos.y) > Mathf.Abs(headTransform.position.y - startPos.y + movementVector.y))
                    {
                        Vector3 posChange = movementVector * Time.deltaTime * speed;
                        if (!((headTransform.position.y + posChange.y > 65) || (headTransform.position.y + posChange.y < -79.5) || (headTransform.position.x + posChange.x > 115.5) || (headTransform.position.x + posChange.x < -213.5)))
                        {
                            headTransform.position += posChange;
                        }
                    }

                    yield return null;
                }
            }

            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.W))
            {
                Vector2 initialPos = startPos;
                float time = 0;

                while (new Vector2(body.transform.localScale.x, body.transform.localScale.y) != new Vector2(2,2))
                {
                    startPos = Vector2.Lerp(initialPos, headTransform.position, time);
                    time += Time.deltaTime * returnTimeModifier;
                    yield return null;
                }
                body.transform.localScale = new Vector3(2, 2, 0);
            }
            yield return null;
        }
    }

    public void StartUpgrade(GameObject spawner)
    {
        upgradePanel.SetActive(true);
        upgradePanel.GetComponent<UpgradeMenu>().SetEnemySpawner(spawner);
    }

    public void Damage(float enemyDamage)
    {
        float damage = enemyDamage * (1 - damageReduction);

        if (damage < 0)
        {
            damage = 0;
            return;
        }

        //SFX
        PlaySFX("Damage", 0.05f, 1f);

        playerSize -= damage;
        myAnimation.Play("playerHit");

        if (playerSize <= 4)
        {
            playerSize = 4;

            Debug.Log("GameOver");

            GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>().GameOver(false);

            enabled = false;
        }
        else
        {
            ShrinkPlayer(damage);
        }
    }

    private void ShrinkPlayer(float damage)
    {
        if (Mathf.Abs(body.transform.localScale.x) >= 1 && Mathf.Abs(body.transform.localScale.y) >= 1)
        {
            if ((Mathf.Abs(body.transform.localScale.x) * Mathf.Abs(body.transform.localScale.y)) >= playerSize)
            {
                float sizeMod = playerSize / (playerSize + damage);
                print("Size: " + playerSize + "\nDamage: " + damage + "\nSize + Damage: " + (playerSize + damage) + "\nMod: " + sizeMod);
                print("Pre-mod: " + headTransform.position);
                Vector2 headPos = headTransform.position;
                Vector2 absStartPos = new Vector2(Mathf.Abs(startPos.x), Mathf.Abs(startPos.y));
                if (Mathf.Abs(headPos.x * sizeMod - startPos.x) >= 1 && Mathf.Abs(headPos.y * sizeMod - startPos.y) >= 1)
                {
                    print("Normal change");
                    headTransform.position = (headTransform.position - startPos) * sizeMod + startPos;
                }
                else if (Mathf.Abs(headPos.x * sizeMod - startPos.x) >= 1)
                {
                    print("Y is too small");
                    headTransform.position = new Vector2((headTransform.position.x - startPos.x) * sizeMod + startPos.x, startPos.y);
                }
                else if (Mathf.Abs(headPos.y * sizeMod - startPos.y) >= 1)
                {
                    print("X is too small");
                    headTransform.position = new Vector2(startPos.x, (headTransform.position.y - startPos.y) * sizeMod + startPos.y);
                }
                else
                {
                    print("X & Y are too small");
                    headTransform.position = startPos;
                }
                print("Post-mod: " + headTransform.position);
            }
        }
        else
        {
            body.transform.localScale = new Vector2(1, 1);
        }
    }

    public void Grow(float num)
    {
        //SFX
        PlaySFX("Grow", 0f, 0.5f);
        myAnimation.Play("playerGrow");

        playerSize += num;

        if (playerSize > PlayerPrefs.GetFloat("sizeScore", 0))
        {
            int temp = (int) (playerSize * 100);
            PlayerPrefs.SetFloat("sizeScore", temp / 100f);
        }

        if(playerSize >= 100)
        {
            playerSize = 100;
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
}
