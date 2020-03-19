using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorIndicator2 : MonoBehaviour
{
    private Rigidbody2D myRb;
    private Vector2 oriPos;
    public float distance;
    public bool isInPlayer = true;

    // Start is called before the first frame update
    void Start()
    {
        
        myRb = PlayerControl1.Instance.gameObject.GetComponent<Rigidbody2D>();
        oriPos = transform.position;



    }

    // Update is called once per frame
    void Update()
    {

        if (isInPlayer)
        {
            if (myRb.velocity.magnitude != 0)
            {
                transform.position = (Vector2)myRb.transform.position + myRb.velocity.normalized * distance;
                transform.rotation = Quaternion.Euler(0, 0, -AngleBetween(Vector2.up, myRb.velocity.normalized));
            }
            else
            {
                transform.position = new Vector2(9999919, 999999);
            }
        }
        else
        {         
            transform.position = (Vector2)oriPos + myRb.velocity.normalized * distance;
            transform.rotation = Quaternion.Euler(0, 0, -AngleBetween(Vector2.up, myRb.velocity.normalized));         
        }
    }

    public static float AngleBetween(Vector2 vectorA, Vector2 vectorB)
    {
        float angle = Vector2.Angle(vectorA, vectorB);
        Vector3 cross = Vector3.Cross(vectorA, vectorB);

        if (cross.z > 0)
        {
            angle = 360 - angle;
        }

        return angle;
    }
}
