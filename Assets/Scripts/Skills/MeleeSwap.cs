using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class MeleeSwap : Skill
{

    public float momentumMultiplier;
    public float range;
    public Collider2D col;

    public override void Init()
    {

    }


    public override bool Check()
    {
        if (playerControl.closestObjectToPlayer && playerControl.closestPlayerDistance <= range)
        {
            col = playerControl.closestObjectToPlayer.GetComponent<Collider2D>();
            return true;
        }
        else
            return false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            Do();
    }

    public override void Do()
    {
        if (!active || !Check())
            return;

        CameraShaker.Instance.ShakeOnce(10f, 0.1f, 0.02f, 0.05f);

        Rigidbody2D thingBody = col.gameObject.GetComponent<Rigidbody2D>();
        Thing thing = col.gameObject.GetComponent<Thing>();
        Vector3 pos = player.transform.position;
        Vector3 thingPos = col.transform.position;
        float playerRadiusY = player.GetComponent<BoxCollider2D>().size.y / 2f;
        float heightDiff = (col.GetComponent<BoxCollider2D>().size.y * col.transform.localScale.y - playerRadiusY * 2f) / 2f;

        if (thing.leftX < player.transform.position.x && thing.rightX > player.transform.position.x && thing.lowerY > player.transform.position.y && thing.lowerY < player.transform.position.y + playerRadiusY + 10f)
        {

            Vector3 temp = col.gameObject.transform.position;
            col.gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - playerRadiusY + (thing.upperY - thing.lowerY) / 2f, player.transform.position.z);
            player.transform.position = new Vector3(temp.x, col.gameObject.transform.position.y + playerRadiusY + (thing.upperY - thing.lowerY) / 2f, player.transform.position.z);

        }
        else
        {
            Vector3 tempPos = new Vector3(pos.x, pos.y + heightDiff, pos.z);
            player.transform.position = new Vector3(thingPos.x, thingPos.y - heightDiff, thingPos.z);
            col.gameObject.transform.position = tempPos;
            PostEffectManager.instance.Blink(0.03f);
            print("Exchange!");
        }

        Vector2 tempV = momentumMultiplier * playerBody.velocity;
        playerBody.velocity = momentumMultiplier * thingBody.velocity;
        thingBody.velocity = tempV;


    }
}
