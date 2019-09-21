using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mech_MovingObject : MonoBehaviour
{

    public Transform movingObject;
    public Transform pointA;
    public Transform pointB;
    // Start is called before the first frame update

    private bool AtoB=true;
    public float speed = 5f;
    private float lerpTemp = 0;
    void Start()
    {
        movingObject.position = pointA.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (AtoB)
        {
            lerpTemp += +Time.deltaTime * speed;
            movingObject.position = Vector2.Lerp(pointA.position, pointB.position, lerpTemp);
            if (Vector2.Distance(movingObject.position, pointB.position) <= 1f)
            {
                AtoB = false;
                lerpTemp = 0f;
                print("B");

            }
           

        }else if(!AtoB)
        {
            lerpTemp += Time.deltaTime * speed;
            movingObject.position = Vector2.Lerp(pointB.position, pointA.position, lerpTemp + Time.deltaTime * speed);
            if(Vector2.Distance(movingObject.position, pointA.position) <= 1f){
                AtoB = true;
                lerpTemp = 0f;
                print("A");
            }
          
        }
    }
}
