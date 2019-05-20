using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

    // Use this for initialization

    private Rigidbody2D rb;

    public Vector2 firstPosition;
    public Vector2 secondPosition;
    
    public float time;
    public float time2;


    private float timer;

    public bool isLeft=true;


	void Start () {
        rb = GetComponent<Rigidbody2D>();
        timer = 0;
        //rb.velocity = new Vector2(100f, 0f);
    }
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;

        if (isLeft && timer < time)
        {
            rb.velocity = firstPosition;
        }
        else if (isLeft && timer>=time)
        {
            isLeft = !isLeft;
        }
        else if (!isLeft && timer<time+time2)
        {
            rb.velocity = secondPosition;

        }
        else if (!isLeft && timer>time+time2)
        {
            isLeft = !isLeft;
            timer = 0;
        }

       

    }
}
