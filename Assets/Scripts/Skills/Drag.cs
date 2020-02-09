using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    public float dragSpeed;
    public float cd;
    public float currcd;
    public float dragThreshold;
    public GameObject dashPointer;

    Swap swap;
    Vector3 startingPoint;

    void Start()
    {
        swap = GetComponent<Swap>();
        currcd = cd;
    }

    void Update()
    {
        Vector2 dir = (Input.mousePosition - startingPoint).normalized;
        if (currcd > 0f)
        {
            currcd -= Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(1))
        {
            startingPoint = Input.mousePosition;

        }
        if (Input.GetMouseButton(1) && swap.col != null && (Input.mousePosition - startingPoint).magnitude > dragThreshold)
        {
            dashPointer.SetActive(true);

            dashPointer.transform.position = (Vector2)swap.col.transform.position + dir * 70f;
            dashPointer.transform.localRotation = Quaternion.Euler(0, 0, -Dash.AngleBetween(Vector2.up, dir));
            
        }
        else if (Input.GetMouseButtonUp(1) || (Input.GetMouseButton(1) && (Input.mousePosition - startingPoint).magnitude < dragThreshold))
        {
            dashPointer.SetActive(false);
        }
        if (Input.GetMouseButtonUp(1) && swap.col != null && currcd <= 0f)
        {
            swap.col.GetComponent<Rigidbody2D>().velocity = dir * dragSpeed;
            currcd = cd;
        }
    }
}
