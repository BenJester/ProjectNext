using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandPlatform : MonoBehaviour
{
    private GameObject player;
    private bool isStand;
    private float delta;
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "player")
        {
            player = collision.gameObject;
            delta = transform.position.x - player.transform.position.x;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "player")
        {
            if (collision.transform.position.y > transform.position.y)
                isStand = true;
            else
                isStand = false;
            collision.transform.SetParent(transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "player")
        {
            isStand = false;
            collision.transform.SetParent(null);
        }
    }
    private void Update()
    {
        if (isStand)
        {
            if (PlayerControl1.Instance.player.GetAxisRaw("MoveHorizontal") ==0)
                player.transform.position = new Vector3(transform.position.x - delta, player.transform.position.y, player.transform.position.z);
            else
                delta= transform.position.x - player.transform.position.x;
        }
    }
}
