using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D headRigidbody2D;

    private Vector3 tempScale;
    private Vector3 tempPosition;
    private GameObject body;
    private float speed = 10;
    private bool stretching = false;

    // Start is called before the first frame update
    void Start()
    {
        headRigidbody2D = transform.GetChild(0).GetComponent<Rigidbody2D>();
        tempScale = transform.localScale;
        body = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (true)//Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            float xChange = Time.deltaTime * Input.GetAxis("Horizontal") * speed;
            float yChange = Time.deltaTime * Input.GetAxis("Vertical") * speed;

            tempScale = body.transform.localScale;
            tempScale.x += xChange;
            tempScale.y += yChange;

            tempPosition = body.transform.localPosition;
            tempPosition.x += xChange / 2;
            tempPosition.y += yChange / 2;
        }

        //transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * Time.deltaTime * speed;
        headRigidbody2D.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed;


        if (Input.GetKey(KeyCode.I))
        {
            float xChange = Time.deltaTime;
            float yChange = Time.deltaTime;

            tempScale = body.transform.localScale;
            tempScale.x += xChange;
            tempScale.y += yChange;

            tempPosition = body.transform.localPosition;
            tempPosition.x += xChange / 2;
            tempPosition.y += yChange / 2;
        }
        body.transform.localScale = tempScale;
        body.transform.localPosition = tempPosition;
    }

    private void FixedUpdate()
    {
        
    }
}
