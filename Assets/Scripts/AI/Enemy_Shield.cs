using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shield : MonoBehaviour
{
    Enemy enemy;
    float offsetX;

    private void Start()
    {
        enemy = transform.parent.GetComponent<Enemy>();
        offsetX = transform.localPosition.x;
    }

    private void Update()
    {
        if (enemy != null)
        {
            transform.localPosition = new Vector3(enemy.faceRight ? offsetX : -offsetX, transform.localPosition.y, transform.localPosition.z);
        }
    }
}
