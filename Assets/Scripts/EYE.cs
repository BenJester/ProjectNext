using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EYE : MonoBehaviour
{

    public float eyeMouseDistance = 3;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 thisPos = transform.parent.position;
        Vector3 mousePos = (Input.mousePosition - new Vector3(UnityEngine.Screen.width / 2, UnityEngine.Screen.height / 2, 0)).normalized;
        mousePos.z = 0;
        transform.position = thisPos + mousePos * eyeMouseDistance;
    }
}
