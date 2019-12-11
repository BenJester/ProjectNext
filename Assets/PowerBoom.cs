using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBoom : MonoBehaviour
{

    private Rigidbody2D myRb;
    public bool isBurst = false;
    [Range(0,1)]
    public float horizontalRatio;
    [Range(0, 1)]
    public float verticalRatio;

    public float burstPower;
    public float targetPower;

    // Start is called before the first frame update
    private void Awake()
    {
        
    }
    void Start()
    {
        myRb = PlayerControl1.Instance.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = new Vector2(myRb.velocity.x, myRb.velocity.y).normalized;
        dir = new Vector2(dir.x * horizontalRatio, dir.y * verticalRatio);

        if (isBurst)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                myRb.velocity += dir * burstPower;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (myRb.velocity.y >= 0)
                {
                    myRb.velocity = Vector2.Lerp(myRb.velocity, new Vector2(dir.x * targetPower, dir.y * targetPower), 0.6f);
                }
                else
                {
                    myRb.velocity = Vector2.Lerp(myRb.velocity, new Vector2(dir.x * targetPower, myRb.velocity.y), 0.6f);
                }
               
            }
        }
        
    }
}
