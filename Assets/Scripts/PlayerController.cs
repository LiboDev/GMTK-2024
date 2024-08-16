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
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        headRigidbody2D = transform.GetChild(0).GetComponent<Rigidbody2D>();
        headTransform = transform.GetChild(0).GetComponent<Transform>();
        body = transform.GetChild(1).gameObject;
        initialPosition = headTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        body.transform.localScale = headTransform.position - initialPosition - new Vector3(0.5f, 0.5f, 0);
        body.transform.localPosition = (headTransform.position - initialPosition - new Vector3(0.5f, 0.5f, 0)) / 2;

        headRigidbody2D.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed;
    }
}
