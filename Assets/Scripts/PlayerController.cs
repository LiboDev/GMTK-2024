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

    //Head movement variables
    private float speed = 10;

    //General body variables
    private GameObject body;
    private SpriteRenderer bodySpriteRenderer;

    //Variables related to the body stretching
    private Vector3 startPos;

    //Variables relating to the body returning to the head
    [SerializeField] private float returnTimeModifier = 1;

    //stats
    [SerializeField] private int playerSize = 25;
    [SerializeField] private float bulletInterval = 1.5f;
    [SerializeField] private float range = 10f;
    [SerializeField] private int bulletDamage = 1;
    [SerializeField] private float bulletKnockback = 0;
    //
    private bool canShoot = true;
    [SerializeField] private int bulletsPerSecond = 1;

    //Prefabs
    [SerializeField] private GameObject bulletPrefab;

    //General Camera Variables
    private CinemachineVirtualCamera playerCamera;

    //Objects
    [SerializeField] private Slider gooBar;

    //audio
    [SerializeField] private Sound[] sounds;
    [SerializeField] private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        headRigidbody2D = transform.GetChild(0).GetComponent<Rigidbody2D>();
        headTransform = transform.GetChild(0).GetComponent<Transform>();
        playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<CinemachineVirtualCamera>();
        body = transform.GetChild(1).gameObject;
        bodySpriteRenderer = body.GetComponent<SpriteRenderer>();

        startPos = headTransform.position;

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

        body.transform.position = (headTransform.position + startPos) / 2;
        body.transform.localScale = new Vector3(Mathf.Abs(headTransform.position.x - startPos.x) + 1, Mathf.Abs(headTransform.position.y - startPos.y) + 1, 0);

        if (canShoot)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                FireUp();
                Shoot();
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                FireDown();
                Shoot();
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                FireRight();
                Shoot();
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                FireLeft();
                Shoot();
            }
        }
    }

    private void Reload()
    {
        canShoot = true;
    }

    private void Shoot()
    {
        canShoot = false;
        Invoke("Reload", 1f / bulletsPerSecond);

        PlaySFX("Shoot", 0.05f, 1f);
    }

    private void FireUp()
    {
        Vector2 bulletPos = headTransform.position;
        if ((int)(body.transform.localScale.x / bulletInterval) == 0)
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
            for (int i = 0; i < (int)(body.transform.localScale.x / bulletInterval); i++)
            {
                if (headTransform.position.x > startPos.x)
                {
                    bulletPos = new Vector2(headTransform.position.x - (bulletInterval * i), bulletPos.y);
                }
                else
                {
                    bulletPos = new Vector2(headTransform.position.x + (bulletInterval * i), bulletPos.y);
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
        if ((int)(body.transform.localScale.x / bulletInterval) == 0)
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
            for (int i = 0; i < Mathf.Abs((int)(body.transform.localScale.x / bulletInterval)); i++)
            {
                if (headTransform.position.x > startPos.x)
                {
                    bulletPos = new Vector2(headTransform.position.x - (bulletInterval * i), bulletPos.y);
                }
                else
                {
                    bulletPos = new Vector2(headTransform.position.x + (bulletInterval * i), bulletPos.y);
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
        if ((int)(body.transform.localScale.y / bulletInterval) == 0)
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
            for (int i = 0; i < Mathf.Abs((int)(body.transform.localScale.y / bulletInterval)); i++)
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
                    bulletPos = new Vector2(bulletPos.x, headTransform.position.y - (bulletInterval * i));
                }
                else
                {
                    bulletPos = new Vector2(bulletPos.x, headTransform.position.y + (bulletInterval * i));
                }
                GameObject temp = Instantiate(bulletPrefab, bulletPos, Quaternion.LookRotation(Vector3.forward, Vector3.right));
                temp.GetComponent<PlayerBullet>().Initialize(Vector2.right, bulletDamage, bulletKnockback, range);
            }
        }
    }

    private void FireLeft()
    {
        Vector2 bulletPos = headTransform.position;
        if ((int)(body.transform.localScale.y / bulletInterval) == 0)
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
            for (int i = 0; i < Mathf.Abs((int)(body.transform.localScale.y / bulletInterval)); i++)
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
                    bulletPos = new Vector2(bulletPos.x, headTransform.position.y - (bulletInterval * i));
                }
                else
                {
                    bulletPos = new Vector2(bulletPos.x, headTransform.position.y + (bulletInterval * i));
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
                    if (!((Mathf.Abs(body.transform.localScale.x) * Mathf.Abs(body.transform.localScale.y)) >= playerSize) || Mathf.Abs(headTransform.position.x - startPos.x) > Mathf.Abs(headTransform.position.x - startPos.x + movementVector.x) || Mathf.Abs(headTransform.position.y - startPos.y) > Mathf.Abs(headTransform.position.y - startPos.y + movementVector.y))
                    {
                        headTransform.position += new Vector3(movementVector.x, movementVector.y, 0) * Time.deltaTime * speed;
                    }

                    yield return null;
                }
            }

            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.W))
            {
                Vector2 initialPos = startPos;
                float time = 0;

                while (new Vector2(body.transform.localScale.x, body.transform.localScale.y) != new Vector2(1, 1))
                {
                    startPos = Vector2.Lerp(initialPos, headTransform.position, time);
                    time += Time.deltaTime * returnTimeModifier;
                    yield return null;
                }
                body.transform.localScale = new Vector3(1, 1, 0);
            }
            yield return null;
        }
    }

    public void Damage(int damage)
    {
        //SFX
        PlaySFX("Damage", 0.05f, damage/2f);

        playerSize -= damage;

        if (playerSize <= 0)
        {
            playerSize = 0;

            Debug.Log("GameOver");
        }
        else
        {
            if (Mathf.Abs(body.transform.localScale.x) >= 1 && Mathf.Abs(body.transform.localScale.y) >= 1)
            {
                if ((Mathf.Abs(body.transform.localScale.x) * Mathf.Abs(body.transform.localScale.y)) >= playerSize)
                {
                    float moddedSize = playerSize + damage;
                    float sizeMod = playerSize / moddedSize;
                    print("Size: " + playerSize + "\nDamage: " + damage + "\nSize + Damage: " + (playerSize + damage) + "\nCalculation: " + (playerSize / moddedSize) + "\nMod: " + sizeMod);
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
    }

    public void Grow(int num)
    {
        //SFX
        PlaySFX("Grow", 0f, 0.5f);

        playerSize += num;

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
