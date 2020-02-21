using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    public bool triggered;
    public float speed;
    public Vector2 dir;
    Rigidbody2D rb;
    BoxCollider2D box;
    public Collider2D target;
    PlayerControl1 player;
    public bool swapDamageOn;
    public int swapDamage;
    public float scanBoxHeight;
    public bool noThrow;
    public float dashSpeed;
    public Kunai other;
    LineRenderer lr;
    public bool buffered;
    public bool ready;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        lr = GetComponent<LineRenderer>();
        //Time.fixedDeltaTime = 0.003f;
        transform.position = Vector3.zero;
    }
    private void Start()
    {
        player = PlayerControl1.Instance;
    }
    
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > 3000f)
            Reset();
        if (target == null && !ready && rb.velocity.magnitude < speed - 200f)
            Reset();
        if (triggered)
        {
            lr.enabled = true;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, player.transform.position);
        }
        else
        {
            lr.enabled = false;
            Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.transform.position).normalized * 1500f;
            //lr.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            //lr.SetPosition(1, player.transform.position + (Vector3) dir);
            //lr.SetPosition(0, player.transform.position);
        }
            
    }

    public void HandleInput(Vector2 dir)
    {
        if (target != null)
            Swap();
        else if (ready)
            Shoot(dir);
        else
            buffered = true;
    }

    public void Shoot(Vector2 dir)
    {
        ready = false;
        if (noThrow)
        {
            transform.position = player.transform.position;
            target = box;
            return;
        }
        transform.position = player.transform.position;
        rb.velocity = dir.normalized * speed;
        this.dir = dir;
    }

    IEnumerator Dash(Vector3 pos)
    {
        other.StopAllCoroutines();
        player.box.isTrigger = true;
        player.disableAirControl = true;
        player.rb.bodyType = RigidbodyType2D.Kinematic;
        
        player.rb.gravityScale = 0f;
        //yield return new WaitForSeconds(Vector3.Distance(pos, player.transform.position) / dashSpeed);
        while (Vector3.Distance(player.transform.position, transform.position) > 65f)
        {
            player.rb.velocity = (transform.position - player.transform.position).normalized * dashSpeed;
            yield return new WaitForEndOfFrame();
        }
        player.transform.position = transform.position;
        player.box.isTrigger = false;
        player.disableAirControl = false;
        player.rb.velocity = Vector2.zero;
        
        player.rb.bodyType = RigidbodyType2D.Dynamic;
        player.rb.gravityScale = 0f;
        Reset();
    }

    public void Swap()
    {
        if (target == null) return;
        player.rb.velocity = Vector2.zero;
        player.rb.gravityScale = 0f;
        ScanEnemies(target);
        if (target == box)
        {
            //player.transform.position = transform.position;
            
            StartCoroutine(Dash(target.transform.position));
            return;
        }
        
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

        Reset();
    }

    void TriggerInstanceEvent(Thing swapThing)
    {
        if (swapThing.GetComponent<TriggerItem_Base>() != null)
        {
            TriggerItem_Base tb = swapThing.GetComponent<TriggerItem_Base>();
            tb.HandleSwapTrigger();
        }
    }

    private void FixedUpdate()
    {
        //RaycastHit2D hit2D = Physics2D.Raycast(transform.position, dir, 20, 1 << 8);
        //if (hit2D)
        //{
        //    transform.position = (Vector2)transform.position + (Vector2)dir * 15;
        //    rb.velocity = Vector2.zero;
        //    rb.freezeRotation = true;
        //    print("Hit");
        //    //my_rb.bodyType=RigidbodyType2D.Static;
        //    //isTrigger = false;
        //}
    }

    private void Reset()
    {
        transform.parent = null;
        target = null;
        triggered = false;
        transform.position = Vector3.zero;
        buffered = false;
        ready = true;
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (triggered) return;
        if (col.collider.CompareTag("thing") && !col.collider.GetComponent<Thing>().hasShield && !col.collider.CompareTag("player"))
        {
            target = col.collider;
            rb.velocity = Vector2.zero;
            transform.parent = target.transform;
            triggered = true;
            if (buffered)
                Swap();
        }
        else if (col.collider.CompareTag("floor") || (col.collider.CompareTag("thing") && col.collider.GetComponent<Thing>().hasShield))
        {
            target = box;
            rb.velocity = Vector2.zero;
            //rb.bodyType = RigidbodyType2D.Kinematic;
            triggered = true;
            if (buffered)
                Swap();

        }
        else if (col.collider.CompareTag("metal"))
        {
            Reset();
        }
    }

    void ScanEnemies(Collider2D _swapCol)
    {
        if (!swapDamageOn)
            return;

        Vector2 midPoint = (player.transform.position + _swapCol.transform.position) / 2f;
        Vector2 size = new Vector2(Vector2.Distance(player.transform.position, _swapCol.transform.position), scanBoxHeight);
        float angle = Vector2.SignedAngle(Vector2.right, (Vector2)player.transform.position - (Vector2)target.transform.position);

        GameObject temp = new GameObject();
        GameObject scan = Instantiate(temp, midPoint, Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.right, (Vector2)player.transform.position - (Vector2)target.transform.position)));
        scan.transform.position = midPoint;
        BoxCollider2D scanBox = scan.AddComponent<BoxCollider2D>();
        scanBox.isTrigger = true;
        scanBox.size = size;
        Collider2D[] cols = new Collider2D[32];
        int count = Physics2D.OverlapCollider(scanBox, new ContactFilter2D(), cols);
        for (int i = 0; i < count; i++)
        {
            if (cols[i] == _swapCol)
                continue;
            Enemy enemy = cols[i].GetComponent<Enemy>();
            if (enemy != null && enemy.canBeDamagedByKunaiDash)
            {
                enemy.TakeDamage(swapDamage);
            }
        }


        Destroy(temp);
        Destroy(scan);
    }
}
