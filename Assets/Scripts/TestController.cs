using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    //heirarchy
    private Transform head;
    private Transform body;

    //tracking
    private Vector3 startPos;

    //stats
    private float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        head = transform.GetChild(0);
        body = transform.GetChild(1);

        startPos = head.position;

        StartCoroutine(Stretch());
    }

    // Update is called once per frame
    void Update()
    {
        body.position = (head.position + startPos) / 2;
        body.localScale = new Vector3(Mathf.Abs(head.position.x - startPos.x) + 1, Mathf.Abs(head.position.y - startPos.y) + 1, 0);
    }

    private IEnumerator Stretch()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))
            {
                startPos = head.position;

                while (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))
                {
                    Vector2 movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                    movementVector.Normalize();

                    head.position += new Vector3(movementVector.x, movementVector.y, 0) * Time.deltaTime * speed;

                    yield return null;
                }
            }

            if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.W))
            {
                Vector2 initialPos = startPos;
                float time = 0;

                while (new Vector2(body.localScale.x, body.localScale.y) != new Vector2(1, 1))
                {
                    startPos = Vector2.Lerp(initialPos, head.position, time);
                    time += Time.deltaTime;
                    yield return null;
                }
                body.localScale = new Vector3(1, 1, 0);
            }
            yield return null;
        }
    }
}
