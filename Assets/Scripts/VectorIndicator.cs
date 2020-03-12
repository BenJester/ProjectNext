using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorIndicator : MonoBehaviour
{
    private Rigidbody2D myRb;
    private Vector2 oriPos;
    public float distance;
    private LineRenderer lr;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        myRb = PlayerControl1.Instance.gameObject.GetComponent<Rigidbody2D>();
        oriPos = transform.position;
        lr.SetPosition(0, oriPos);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = oriPos + myRb.velocity.normalized * distance;
        lr.SetPosition(1, transform.position);
    }
}
