using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D headRigidbody2D;
    private Transform headTransform;

    private GameObject body;
    private float speed = 10;

    private bool stretching = false;
    private bool stretched = false;
    public string xStretchDir = "none";
    public string yStretchDir = "none";

    [SerializeField] Vector2 returnTarget = new Vector2(-1, -1);
    public bool bodyReturning = false;
    private float bodyReturnStartTime;
    private Vector3 startBodyPos;
    private Vector3 startBodyScale;

    private Vector3 initialPosition;
    public bool bodyXFlipped = false;
    public bool bodyYFlipped = false;

    //stats
    private int size = 10;




    // Start is called before the first frame update
    void Start()
    {
        headRigidbody2D = transform.GetChild(0).GetComponent<Rigidbody2D>();
        headTransform = transform.GetChild(0).GetComponent<Transform>();
        body = transform.GetChild(1).gameObject;
        initialPosition = headTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
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
            startBodyPos = body.transform.localPosition;
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
