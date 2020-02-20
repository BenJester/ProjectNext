using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowStuff : MonoBehaviour
{
    public GameObject LeftClick;
    public GameObject RightClick;
    public GameObject leftSwap;
    public GameObject rightSwap;
    public float speed;
    public float offset;
    PlayerControl1 player;
    public bool leftSwapped;
    public bool rightSwapped;

    void Start()
    {
        player = GetComponent<PlayerControl1>();
    }

    void Throw(GameObject obj, ref GameObject swap)
    {
        Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        GameObject throwObj = Instantiate(obj, transform.position + (Vector3)(dir * offset), Quaternion.identity);
        Rigidbody2D rb = throwObj.GetComponent<Rigidbody2D>();
        rb.velocity = dir * speed;
        swap = throwObj;
    }

    public void Swap(Collider2D target, ref bool swapped)
    {
        if (target == null) return;
        player.rb.velocity = Vector2.zero;
        player.rb.gravityScale = 0f;
        //ScanEnemies(target);
        

        Collider2D _readySwapCol = target;
        Rigidbody2D thingBody = _readySwapCol.gameObject.GetComponent<Rigidbody2D>();
        Thing _swapThing = _readySwapCol.gameObject.GetComponent<Thing>();

        _swapThing.ThingSwap();
        Thing _playerThing = player.gameObject.GetComponent<Thing>();
        _playerThing.ThingSwap();
        if (_swapThing.hasShield) return;
        Vector3 posPlayer = player.transform.position;
        Vector3 _posSwapThing = _readySwapCol.transform.position;

        BoxCollider2D objCol2d = _readySwapCol.GetComponent<BoxCollider2D>();
        float playerRadiusY = player.GetComponent<BoxCollider2D>().bounds.size.y / 2f;
        float heightDiff = (_readySwapCol.GetComponent<BoxCollider2D>().bounds.size.y - playerRadiusY * 2f) / 2f;

        if (_swapThing.GetLeftX() < player.transform.position.x &&
            _swapThing.GetRightX() > player.transform.position.x &&
            _swapThing.GetLowerY() > player.transform.position.y &&
            _swapThing.GetLowerY() < player.transform.position.y + playerRadiusY + 10f)
        {

            Vector3 temp = _readySwapCol.gameObject.transform.position;
            _readySwapCol.gameObject.transform.position = new Vector3(
                player.transform.position.x,
                player.transform.position.y - playerRadiusY + (_swapThing.GetUpperY() - _swapThing.GetLowerY()) / 2f,
                player.transform.position.z);

            player.transform.position = new Vector3(
                temp.x,
                _readySwapCol.gameObject.transform.position.y + playerRadiusY + (_swapThing.GetUpperY() - _swapThing.GetLowerY()) / 2f,
                player.transform.position.z);
        }
        else
        {
            _readySwapCol.gameObject.transform.position = new Vector3(posPlayer.x, _playerThing.GetLowerY() + playerRadiusY + heightDiff, posPlayer.z);
            player.transform.position = new Vector3(_posSwapThing.x, _posSwapThing.y - heightDiff, _posSwapThing.z);
        }

        TriggerInstanceEvent(_swapThing);
        swapped = true;
    }

    void TriggerInstanceEvent(Thing swapThing)
    {
        if (swapThing.GetComponent<TriggerItem_Base>() != null)
        {
            TriggerItem_Base tb = swapThing.GetComponent<TriggerItem_Base>();
            tb.HandleSwapTrigger();
        }
    }

    void HandleInput(bool left)
    {
        if (left)
        {
            if (leftSwap != null)
            {
                if (leftSwapped)
                {
                    leftSwap.GetComponent<Thing>().Die();
                    Throw(RightClick, ref leftSwap);
                    leftSwapped = false;
                }
                else
                    Swap(leftSwap.GetComponent<BoxCollider2D>(), ref leftSwapped);
            }

            else
                Throw(LeftClick, ref leftSwap);
        }
        else
        {
            if (rightSwap != null)
            {
                if (rightSwapped)
                {
                    rightSwap.GetComponent<Thing>().Die();
                    Throw(RightClick, ref rightSwap);
                    rightSwapped = false;
                }
                else
                    Swap(rightSwap.GetComponent<BoxCollider2D>(), ref rightSwapped);
            }
                
            else
                Throw(RightClick, ref rightSwap);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && LeftClick != null)
        {
            HandleInput(true);
        }
        if (Input.GetMouseButtonUp(1) && RightClick != null)
        {
            HandleInput(false);
        }
        if (rightSwap == null)
        {
            rightSwapped = false;
        }
            
        if (leftSwap == null)
        {
            leftSwapped = false;
        }
            
    }
}
