using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    public bool triggered;
    public float speed;
    public Vector2 dir;
    public bool canShoot;
    public bool canSwap;
    Rigidbody2D rb;
    BoxCollider2D box;
    public Collider2D target;
    PlayerControl1 player;
    public bool swapDamageOn;
    public int swapDamage;
    public float scanBoxHeight;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        
        //Time.fixedDeltaTime = 0.003f;
    }
    private void Start()
    {
        player = PlayerControl1.Instance;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(Time.fixedDeltaTime);
    }

    public void HandleInput(Vector2 dir)
    {
        if (target != null)
            Swap();
        else
            Shoot(dir);
    }

    public void Shoot(Vector2 dir)
    {
        transform.position = player.transform.position;
        rb.velocity = dir.normalized * speed;
        this.dir = dir;
    }

    public void Swap()
    {
        if (target == null) return;
        player.rb.velocity = Vector2.zero;
        player.rb.gravityScale = 0f;
        ScanEnemies(target);
        if (target == box)
        {
            player.transform.position = transform.position;
            Reset();
            
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
        
        Reset();
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
            
        }
        else if (col.collider.CompareTag("floor") || col.collider.GetComponent<Thing>().hasShield)
        {
            target = box;
            rb.velocity = Vector2.zero;
            //rb.bodyType = RigidbodyType2D.Kinematic;
            triggered = true;
            
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
            if (enemy != null)
            {
                enemy.TakeDamage(swapDamage);
            }
        }


        Destroy(temp);
        Destroy(scan);
    }
}
