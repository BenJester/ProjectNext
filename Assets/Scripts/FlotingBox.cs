using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlotingBox : MonoBehaviour {
    // Start is called before the first frame update

    public bool isFloating = false;
    public Vector2 dir;
    public float speed;
    Rigidbody2D rb;
    void Start () {
        rb = GetComponent<Rigidbody2D> ();
    }

    // Update is called once per frame
    void Update () {
        if (isFloating) {
            rb.velocity = dir * speed;
        }
    }

    public void SetFloating () {
        isFloating = true;
    }

}