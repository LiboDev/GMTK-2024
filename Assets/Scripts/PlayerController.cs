using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
    private bool canStretch = true;
    private bool stretching = false;
    private bool stretched = false;
    private string xStretchDir = "none";
    private string yStretchDir = "none";
    private bool xStretched = false;
    private bool yStretched = false;

    //Variables relating to the body returning to the head
    Vector2 returnTarget = new Vector2(-1, -1);
    private bool bodyReturning = false;
    private float bodyReturnStartTime;
    private Vector3 startBodyScale;

    //Auxillary variables for manipulating the body
    //The head's initial position before a stretch
    private Vector3 initialPosition;
    //If the head is to the right of the initial position, body is flipped
    [SerializeField] private bool bodyXFlipped = false;
    //If the head is above the initial position, body is flipped
    private bool bodyYFlipped = false;

    //stats
    [SerializeField] private int playerSize = 25;
    [SerializeField] private float bulletInterval = 1.5f;
    [SerializeField] private float range = 10f;
    [SerializeField] private int bulletDamage = 1;
    //
    private bool canShoot = true;
    [SerializeField] private int bulletsPerSecond = 1;

    //Prefabs
    [SerializeField] private GameObject bulletPrefab;

    //General Camera Variables
    private CinemachineVirtualCamera playerCamera;

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
        //Save the head's initial position
        initialPosition = headTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        playerCamera.m_Lens.OrthographicSize = Mathf.Abs(body.transform.localScale.x) + Mathf.Abs(body.transform.localScale.y);
        if (playerCamera.m_Lens.OrthographicSize < 10)
        {
            playerCamera.m_Lens.OrthographicSize = 10;
        }

        PlayerMovement();

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


        if (stretching || stretched)
        {
            bodySpriteRenderer.color = new Color(bodySpriteRenderer.color.r, bodySpriteRenderer.color.g, bodySpriteRenderer.color.b, 0.75f);
        }
        else
        {
            bodySpriteRenderer.color = new Color(bodySpriteRenderer.color.r, bodySpriteRenderer.color.g, bodySpriteRenderer.color.b, 1);
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
            if (bodyYFlipped)
            {
                bulletPos = new Vector2(bulletPos.x, headTransform.position.y + 1);
            }
            else
            {
                bulletPos = new Vector2(bulletPos.x, headTransform.position.y - body.transform.localScale.y);
            }
            GameObject temp = Instantiate(bulletPrefab, bulletPos, Quaternion.LookRotation(new Vector3(0, 0, 1)));
            temp.GetComponent<PlayerBullet>().Initialize(Vector2.up);
        }
        else
        {
            for (int i = 0; i < Mathf.Abs((int)(body.transform.localScale.x / bulletInterval)); i++)
            {
                if (bodyXFlipped)
                {
                    bulletPos = new Vector2(headTransform.position.x - (bulletInterval * i), bulletPos.y);
                }
                else
                {
                    bulletPos = new Vector2(headTransform.position.x + (bulletInterval * i), bulletPos.y);
                }
                if (bodyYFlipped)
                {
                    bulletPos = new Vector2(bulletPos.x, headTransform.position.y + 1);
                }
                else
                {
                    bulletPos = new Vector2(bulletPos.x, headTransform.position.y - body.transform.localScale.y);
                }
                GameObject temp = Instantiate(bulletPrefab, bulletPos, Quaternion.LookRotation(new Vector3(0, 0, 1)));
                temp.GetComponent<PlayerBullet>().Initialize(Vector2.up);
            }
        }
    }

    private void FireDown()
    {
        Vector2 bulletPos = headTransform.position;
        if ((int)(body.transform.localScale.x / bulletInterval) == 0)
        {
            if (bodyYFlipped)
            {
                bulletPos = new Vector2(bulletPos.x, headTransform.position.y + body.transform.localScale.y);
            }
            else
            {
                bulletPos = new Vector2(bulletPos.x, headTransform.position.y - 1);
            }
            GameObject temp = Instantiate(bulletPrefab, bulletPos, Quaternion.LookRotation(new Vector3(0, 0, -1)));
            temp.GetComponent<PlayerBullet>().Initialize(Vector2.down);
        }
        else
        {
            for (int i = 0; i < Mathf.Abs((int)(body.transform.localScale.x / bulletInterval)); i++)
            {
                if (bodyXFlipped)
                {
                    bulletPos = new Vector2(headTransform.position.x - (bulletInterval * i), bulletPos.y);
                }
                else
                {
                    bulletPos = new Vector2(headTransform.position.x + (bulletInterval * i), bulletPos.y);
                }
                if (bodyYFlipped)
                {
                    bulletPos = new Vector2(bulletPos.x, headTransform.position.y - body.transform.localScale.y);
                }
                else
                {
                    bulletPos = new Vector2(bulletPos.x, headTransform.position.y - 1);
                }
                GameObject temp = Instantiate(bulletPrefab, bulletPos, Quaternion.LookRotation(new Vector3(0, 0, -1)));
                temp.GetComponent<PlayerBullet>().Initialize(Vector2.down);
            }
        }
    }

    private void FireRight()
    {
        Vector2 bulletPos = headTransform.position;
        if ((int)(body.transform.localScale.y / bulletInterval) == 0)
        {
            if (bodyXFlipped)
            {
                bulletPos = new Vector2(headTransform.position.x + 1, bulletPos.y);
            }
            else
            {
                bulletPos = new Vector2(headTransform.position.x - body.transform.localScale.x, bulletPos.y);
            }
            GameObject temp = Instantiate(bulletPrefab, bulletPos, Quaternion.LookRotation(Vector3.forward, Vector3.right));
            temp.GetComponent<PlayerBullet>().Initialize(Vector2.right);
        }
        else
        {
            for (int i = 0; i < Mathf.Abs((int)(body.transform.localScale.y / bulletInterval)); i++)
            {
                if (bodyXFlipped)
                {
                    bulletPos = new Vector2(headTransform.position.x + 1, bulletPos.y);
                }
                else
                {
                    bulletPos = new Vector2(headTransform.position.x - body.transform.localScale.x, bulletPos.y);
                }
                if (bodyYFlipped)
                {
                    bulletPos = new Vector2(bulletPos.x, headTransform.position.y - (bulletInterval * i));
                }
                else
                {
                    bulletPos = new Vector2(bulletPos.x, headTransform.position.y + (bulletInterval * i));
                }
                GameObject temp = Instantiate(bulletPrefab, bulletPos, Quaternion.LookRotation(Vector3.forward, Vector3.right));
                temp.GetComponent<PlayerBullet>().Initialize(Vector2.right);
            }
        }
    }

    private void FireLeft()
    {
        Vector2 bulletPos = headTransform.position;
        if ((int)(body.transform.localScale.y / bulletInterval) == 0)
        {
            if (bodyXFlipped)
            {
                bulletPos = new Vector2(headTransform.position.x - body.transform.localScale.x, bulletPos.y);
            }
            else
            {
                bulletPos = new Vector2(headTransform.position.x - 1, bulletPos.y);
            }
            GameObject temp = Instantiate(bulletPrefab, bulletPos, Quaternion.LookRotation(Vector3.forward, Vector3.left));
            temp.GetComponent<PlayerBullet>().Initialize(Vector2.left);
        }
        else
        {
            for (int i = 0; i < Mathf.Abs((int)(body.transform.localScale.y / bulletInterval)); i++)
            {
                if (bodyXFlipped)
                {
                    bulletPos = new Vector2(headTransform.position.x - body.transform.localScale.x, bulletPos.y);
                }
                else
                {
                    bulletPos = new Vector2(headTransform.position.x - 1, bulletPos.y);
                }
                if (bodyYFlipped)
                {
                    bulletPos = new Vector2(bulletPos.x, headTransform.position.y - (bulletInterval * i));
                }
                else
                {
                    bulletPos = new Vector2(bulletPos.x, headTransform.position.y + (bulletInterval * i));
                }
                GameObject temp = Instantiate(bulletPrefab, bulletPos, Quaternion.LookRotation(Vector3.forward, Vector3.left));
                temp.GetComponent<PlayerBullet>().Initialize(Vector2.left);
            }
        }
    }

    private void PlayerMovement()
    {
        //If the player presses any movement key while not currently stretched, set all the movement stuff in that/those direction(s)
        if (!stretched && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
        {
            if (xStretchDir == "none" && !xStretched)
            {
                if (Input.GetKey(KeyCode.D))
                {
                    xStretchDir = "right";
                    bodyXFlipped = true;
                    returnTarget = new Vector2(1, returnTarget.y);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    xStretchDir = "left";
                    bodyXFlipped = false;
                    returnTarget = new Vector2(-1, returnTarget.y);
                }
                stretching = true;
            }

            if (yStretchDir == "none" && !yStretched)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    yStretchDir = "up";
                    bodyYFlipped = true;
                    returnTarget = new Vector2(returnTarget.x, 1);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    yStretchDir = "down";
                    bodyYFlipped = false;
                    returnTarget = new Vector2(returnTarget.x, -1);
                }
                stretching = true;
            }


        }

        if (stretching && (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S)))
        {
            switch (xStretchDir)
            {
                case "right":
                    if (Input.GetKeyUp(KeyCode.D))
                    {
                        if (yStretchDir != "none")
                        {
                            xStretchDir = "none";
                            xStretched = true;
                        }
                        else
                        {
                            StopBodyStretching();
                        }
                    }
                    break;
                case "left":
                    if (Input.GetKeyUp(KeyCode.A))
                    {
                        if (yStretchDir != "none")
                        {
                            xStretchDir = "none";
                            xStretched = true;
                        }
                        else
                        {
                            StopBodyStretching();
                        }
                    }
                    break;
            }
            switch (yStretchDir)
            {
                case "up":
                    if (Input.GetKeyUp(KeyCode.W))
                    {
                        if (xStretchDir != "none")
                        {
                            yStretchDir = "none";
                            yStretched = true;
                        }
                        else
                        {
                            StopBodyStretching();
                        }
                    }
                    break;
                case "down":
                    if (Input.GetKeyUp(KeyCode.S))
                    {
                        if (xStretchDir != "none")
                        {
                            yStretchDir = "none";
                            yStretched = true;
                        }
                        else
                        {
                            StopBodyStretching();
                        }
                    }
                    break;
            }
        }

        if (bodyReturning)
        {
            float fracComplete = (Time.time - bodyReturnStartTime) / 0.5f;
            body.transform.localScale = Vector2.Lerp(startBodyScale, returnTarget, fracComplete);
            if (bodyXFlipped && bodyYFlipped)
            {
                body.transform.localPosition = headTransform.localPosition - ((body.transform.localScale) / 2) + new Vector3(0.5f, 0.5f, 0);
            }
            else if (bodyXFlipped)
            {
                body.transform.localPosition = headTransform.localPosition - ((body.transform.localScale) / 2) + new Vector3(0.5f, -0.5f, 0);
            }
            else if (bodyYFlipped)
            {
                body.transform.localPosition = headTransform.localPosition - ((body.transform.localScale) / 2) + new Vector3(-0.5f, 0.5f, 0);
            }
            else
            {
                body.transform.localPosition = headTransform.localPosition - ((body.transform.localScale) / 2) - new Vector3(0.5f, 0.5f, 0);
            }

            if (fracComplete >= 1)
            {
                xStretchDir = "none";
                yStretchDir = "none";
                stretched = false;
                bodyReturning = false;
                canStretch = true;
                initialPosition = headTransform.localPosition;
                body.transform.localPosition = initialPosition;
                body.transform.localScale = Vector2.one;
            }
        }
        else if (canStretch)
        {
            Vector3 changeFromStartingPos = headTransform.localPosition - initialPosition - new Vector3(0.5f, 0.5f, 0);
            if (bodyXFlipped && bodyYFlipped)
            {
                changeFromStartingPos = headTransform.localPosition - initialPosition + new Vector3(0.5f, 0.5f, 0);
                body.transform.localScale = changeFromStartingPos + new Vector3(0.5f, 0.5f, 0);
                body.transform.localPosition = ((changeFromStartingPos) / 2) - new Vector3(0.25f, 0.25f, 0);
            }
            else if (bodyXFlipped)
            {
                changeFromStartingPos = headTransform.localPosition - initialPosition + new Vector3(0.5f, -0.5f, 0);
                body.transform.localScale = changeFromStartingPos + new Vector3(0.5f, -0.5f, 0);
                body.transform.localPosition = ((changeFromStartingPos) / 2) - new Vector3(0.25f, -0.25f, 0);
            }
            else if (bodyYFlipped)
            {
                changeFromStartingPos = headTransform.localPosition - initialPosition + new Vector3(-0.5f, 0.5f, 0);
                body.transform.localScale = changeFromStartingPos + new Vector3(-0.5f, 0.5f, 0);
                body.transform.localPosition = ((changeFromStartingPos) / 2) - new Vector3(-0.25f, 0.25f, 0);
            }
            else
            {
                body.transform.localScale = changeFromStartingPos - new Vector3(0.5f, 0.5f, 0);
                body.transform.localPosition = ((changeFromStartingPos) / 2) + new Vector3(0.25f, 0.25f, 0);
            }
            body.transform.position += initialPosition;
        }

        if (!stretched && canStretch)
        {
            switch (xStretchDir)
            {
                case "right":
                    headRigidbody2D.velocity = new Vector2(Mathf.Max(Input.GetAxisRaw("Horizontal"), 0), headRigidbody2D.velocity.y);
                    break;
                case "left":
                    headRigidbody2D.velocity = new Vector2(Mathf.Min(Input.GetAxisRaw("Horizontal"), 0), headRigidbody2D.velocity.y);
                    break;
                default:
                    headRigidbody2D.velocity = new Vector2(0, headRigidbody2D.velocity.y);
                    break;
            }

            switch (yStretchDir)
            {
                case "up":
                    headRigidbody2D.velocity = new Vector2(headRigidbody2D.velocity.x, Mathf.Max(Input.GetAxisRaw("Vertical"), 0));
                    break;
                case "down":
                    headRigidbody2D.velocity = new Vector2(headRigidbody2D.velocity.x, Mathf.Min(Input.GetAxisRaw("Vertical"), 0));
                    break;
                default:
                    headRigidbody2D.velocity = new Vector2(headRigidbody2D.velocity.x, 0);
                    break;
            }

            headRigidbody2D.velocity *= speed;
        }
        else if (!stretched)
        {
            Vector2 headTarget = new Vector2(1, 1);

            if (bodyXFlipped)
            {
                headTarget = new Vector2(body.transform.localPosition.x + (body.transform.localScale.x / 2) - 0.5f, headTarget.y);
            }
            else
            {
                headTarget = new Vector2(body.transform.localPosition.x + (body.transform.localScale.x / 2) + 0.5f, headTarget.y);
            }

            if (bodyYFlipped)
            {
                headTarget = new Vector2(headTarget.x, body.transform.localPosition.y + (body.transform.localScale.y / 2) - 0.5f);
            }
            else
            {
                headTarget = new Vector2(headTarget.x, body.transform.localPosition.y + (body.transform.localScale.y / 2) + 0.5f);
            }

            if (bodyXFlipped && bodyYFlipped)
            {
                if (headTransform.localPosition.x >= headTarget.x && headTransform.localPosition.y >= headTarget.y)
                {
                    headRigidbody2D.velocity = Vector2.zero;
                    headTransform.localPosition = headTarget;
                }
            }
            else if (bodyXFlipped)
            {
                if (headTransform.localPosition.x >= headTarget.x && headTransform.localPosition.y <= headTarget.y)
                {
                    headRigidbody2D.velocity = Vector2.zero;
                    headTransform.localPosition = headTarget;
                }
            }
            else if (bodyYFlipped)
            {
                if (headTransform.localPosition.x <= headTarget.x && headTransform.localPosition.y >= headTarget.y)
                {
                    headRigidbody2D.velocity = Vector2.zero;
                    headTransform.localPosition = headTarget;
                }
            }
            else
            {
                if (headTransform.localPosition.x <= headTarget.x && headTransform.localPosition.y <= headTarget.y)
                {
                    headRigidbody2D.velocity = Vector2.zero;
                    headTransform.localPosition = headTarget;
                }
            }
        }
        else
        {
            headRigidbody2D.velocity = Vector2.zero;
        }

        if ((Mathf.Abs(body.transform.localScale.x) * Mathf.Abs(body.transform.localScale.y)) >= playerSize)
        {
            canStretch = false;
        }

        void StopBodyStretching()
        {
            stretching = false;
            stretched = true;
            bodyReturning = true;
            bodyReturnStartTime = Time.time;
            startBodyScale = body.transform.localScale;
            xStretched = false;
            yStretched = false;
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
                if (bodyXFlipped && bodyYFlipped)
                {
                    float bodyScaleXDifference = body.transform.localScale.x - 1;
                    float bodyScaleYDifference = body.transform.localScale.y - 1;
                    if ((body.transform.localScale.x <= 1 + Mathf.Sqrt(1)) && body.transform.localScale.y <= 1 + Mathf.Sqrt(1))
                    {
                        body.transform.localScale = new Vector3(1, 1, body.transform.localScale.z);
                        body.transform.localPosition -= new Vector3(bodyScaleXDifference, bodyScaleYDifference, 0);
                        headTransform.localPosition = body.transform.localPosition;
                    }
                    else if (body.transform.localScale.x <= 1 + Mathf.Sqrt(1))
                    {
                        body.transform.localScale = new Vector3(1, body.transform.localScale.y - 1, body.transform.localScale.z);
                        body.transform.localPosition -= new Vector3(bodyScaleXDifference, 0.5f, 0);
                        headTransform.localPosition -= new Vector3(bodyScaleXDifference, 0.5f, 0);
                    }
                    else if (body.transform.localScale.y <= 1 + Mathf.Sqrt(1))
                    {
                        body.transform.localScale = new Vector3(body.transform.localScale.x - 1, 1, body.transform.localScale.z);
                        body.transform.localPosition -= new Vector3(0.5f, bodyScaleYDifference, 0);
                        headTransform.localPosition -= new Vector3(0.5f, bodyScaleYDifference, 0);
                    }
                    else
                    {
                        body.transform.localScale -= new Vector3(Mathf.Sqrt(1), Mathf.Sqrt(1), 0);
                        body.transform.localPosition -= (new Vector3(Mathf.Sqrt(1), Mathf.Sqrt(1), 0)) / 2;
                        headTransform.localPosition -= (new Vector3(Mathf.Sqrt(1), Mathf.Sqrt(1), 0)) / 2;
                    }
                }
                else if (bodyXFlipped)
                {
                    float bodyScaleXDifference = body.transform.localScale.x - 1;
                    float bodyScaleYDifference = body.transform.localScale.y + 1;
                    if ((body.transform.localScale.x <= 1 + Mathf.Sqrt(1)) && body.transform.localScale.y >= -1 - Mathf.Sqrt(1))
                    {
                        body.transform.localScale = new Vector3(1, -1, body.transform.localScale.z);
                        body.transform.localPosition += new Vector3(bodyScaleXDifference, bodyScaleYDifference, body.transform.localPosition.z);
                        headTransform.localPosition = body.transform.localPosition;
                    }
                    else if (body.transform.localScale.x <= 1 + Mathf.Sqrt(1))
                    {
                        body.transform.localScale = new Vector3(1, body.transform.localScale.y + 1, body.transform.localScale.z);
                        body.transform.localPosition += new Vector3(bodyScaleXDifference, 0.5f, 0);
                        headTransform.localPosition += new Vector3(bodyScaleXDifference, 0.5f, 0);
                    }
                    else if (body.transform.localScale.y >= -1 - Mathf.Sqrt(1))
                    {
                        body.transform.localScale = new Vector3(body.transform.localScale.x - 1, 1, body.transform.localScale.z);
                        body.transform.localPosition += new Vector3(-0.5f, bodyScaleYDifference, 0);
                        headTransform.localPosition += new Vector3(-0.5f, bodyScaleYDifference, 0);
                    }
                    else
                    {
                        body.transform.localScale += new Vector3(-Mathf.Sqrt(1), Mathf.Sqrt(1), 0);
                        body.transform.localPosition += (new Vector3(-Mathf.Sqrt(1), Mathf.Sqrt(1), 0)) / 2;
                        headTransform.localPosition += (new Vector3(-Mathf.Sqrt(1), Mathf.Sqrt(1), 0)) / 2;
                    }
                }
                else if (bodyYFlipped)
                {
                    float bodyScaleXDifference = body.transform.localScale.x + 1;
                    float bodyScaleYDifference = body.transform.localScale.y - 1;
                    if ((body.transform.localScale.x >= -1 - Mathf.Sqrt(1)) && body.transform.localScale.y <= 1 + Mathf.Sqrt(1))
                    {
                        body.transform.localScale = new Vector3(-1, 1, body.transform.localScale.z);
                        body.transform.localPosition += new Vector3(bodyScaleXDifference, bodyScaleYDifference, 0);
                        headTransform.localPosition = body.transform.localPosition;
                    }
                    else if (body.transform.localScale.x >= -1 + Mathf.Sqrt(1))
                    {
                        body.transform.localScale = new Vector3(1, body.transform.localScale.y - 1, body.transform.localScale.z);
                        body.transform.localPosition += new Vector3(bodyScaleXDifference, -0.5f, 0);
                        headTransform.localPosition += new Vector3(bodyScaleXDifference, -0.5f, 0);
                    }
                    else if (body.transform.localScale.y <= 1 + Mathf.Sqrt(1))
                    {
                        body.transform.localScale = new Vector3(body.transform.localScale.x + 1, 1, body.transform.localScale.z);
                        body.transform.localPosition += new Vector3(0.5f, bodyScaleYDifference, 0);
                        headTransform.localPosition += new Vector3(0.5f, bodyScaleYDifference, 0);
                    }
                    else
                    {
                        body.transform.localScale += new Vector3(Mathf.Sqrt(1), -Mathf.Sqrt(1), 0);
                        body.transform.localPosition += (new Vector3(Mathf.Sqrt(1), -Mathf.Sqrt(1), 0)) / 2;
                        headTransform.localPosition += (new Vector3(Mathf.Sqrt(1), -Mathf.Sqrt(1), 0)) / 2;
                    }
                }
                else
                {
                    float bodyScaleXDifference = body.transform.localScale.x + 1;
                    float bodyScaleYDifference = body.transform.localScale.y + 1;
                    if ((body.transform.localScale.x >= -1 - Mathf.Sqrt(1)) && body.transform.localScale.y >= -1 - Mathf.Sqrt(1))
                    {
                        body.transform.localScale = new Vector3(-1, -1, body.transform.localScale.z);
                        body.transform.localPosition += new Vector3(bodyScaleXDifference, bodyScaleYDifference, 0);
                        headTransform.localPosition = body.transform.localPosition;
                    }
                    else if (body.transform.localScale.x >= -1 + Mathf.Sqrt(1))
                    {
                        body.transform.localScale = new Vector3(1, body.transform.localScale.y + 1, body.transform.localScale.z);
                        body.transform.localPosition += new Vector3(bodyScaleXDifference, 0.5f, 0);
                        headTransform.localPosition += new Vector3(bodyScaleXDifference, 0.5f, 0);
                    }
                    else if (body.transform.localScale.y >= -1 + Mathf.Sqrt(1))
                    {
                        body.transform.localScale = new Vector3(body.transform.localScale.x + 1, 1, body.transform.localScale.z);
                        body.transform.localPosition += new Vector3(0.5f, bodyScaleYDifference, 0);
                        headTransform.localPosition += new Vector3(0.5f, bodyScaleYDifference, 0);
                    }
                    else
                    {
                        body.transform.localScale += new Vector3(Mathf.Sqrt(1), Mathf.Sqrt(1), 0);
                        body.transform.localPosition += (new Vector3(Mathf.Sqrt(1), Mathf.Sqrt(1), 0)) / 2;
                        headTransform.localPosition += (new Vector3(Mathf.Sqrt(1), Mathf.Sqrt(1), 0)) / 2;
                    }
                }
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
