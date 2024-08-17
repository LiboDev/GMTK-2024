using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //General body variables
    private Rigidbody2D headRigidbody2D;
    private Transform headTransform;

    //Head movement variables
    private float speed = 10;

    //General body variables
    private GameObject body;

    //Variables related to the body stretching
    private bool stretching = false;
    private bool stretched = false;
    private string xStretchDir = "none";
    private string yStretchDir = "none";

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
    private int size = 10;
    private float bulletInterval = 1.5f;

    //Prefabs
    [SerializeField] private GameObject bulletPrefab;


    // Start is called before the first frame update
    void Start()
    {
        headRigidbody2D = transform.GetChild(0).GetComponent<Rigidbody2D>();
        headTransform = transform.GetChild(0).GetComponent<Transform>();
        body = transform.GetChild(1).gameObject;
        //Save the head's initial position
        initialPosition = headTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            FireUp();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            FireDown();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            FireRight();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            FireLeft();
        }
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
            if (xStretchDir == "none")
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

            if (yStretchDir == "none")
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
                        stretching = false;
                        stretched = true;
                    }
                    break;
                case "left":
                    if (Input.GetKeyUp(KeyCode.A))
                    {
                        stretching = false;
                        stretched = true;
                    }
                    break;
            }
            switch (yStretchDir)
            {
                case "up":
                    if (Input.GetKeyUp(KeyCode.W))
                    {
                        stretching = false;
                        stretched = true;
                    }
                    break;
                case "down":
                    if (Input.GetKeyUp(KeyCode.S))
                    {
                        stretching = false;
                        stretched = true;
                    }
                    break;
            }

            bodyReturning = true;
            bodyReturnStartTime = Time.time;
            startBodyScale = body.transform.localScale;
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
                initialPosition = headTransform.localPosition;
                body.transform.localPosition = initialPosition;
                body.transform.localScale = Vector2.one;
            }
        }
        else
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

        if (!stretched)
        {
            switch (xStretchDir)
            {
                case "right":
                    headRigidbody2D.velocity = new Vector2(Mathf.Max(Input.GetAxisRaw("Horizontal"), 0), headRigidbody2D.velocity.y);
                    break;
                case "left":
                    headRigidbody2D.velocity = new Vector2(Mathf.Min(Input.GetAxisRaw("Horizontal"), 0), headRigidbody2D.velocity.y);
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
            }

            headRigidbody2D.velocity *= speed;
        }
        else
        {
            headRigidbody2D.velocity = Vector2.zero;
        }
    }

    public void Damage(int damage)
    {
        //SFX

        size -= damage;
        
        if(size <= 0)
        {
            size = 0;

            Debug.Log("GameOver");
        }
    }

    public void Grow(int num)
    {
        //SFX
        size += num;

        if(size >= 100)
        {
            size = 100;
        }

    }
}
