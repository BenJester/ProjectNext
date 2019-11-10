using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeLight : MonoBehaviour
{
    // Start is called before the first frame update
    public float angleOffset = 0f;
    Transform player;
    void Start()
    {
        player = transform.parent;
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position;
        Vector3 vecMouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(vecMouseWorldPos);
        float angleToCursor = Mathf.Rad2Deg * PlayerControl1.AngleBetween(player.position, vecMouseWorldPos);
        transform.eulerAngles = new Vector3(0f, 0f, angleToCursor + angleOffset);
    }
}
