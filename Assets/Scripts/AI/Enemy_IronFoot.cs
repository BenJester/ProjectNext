using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_IronFoot : Enemy_Base {

	// Use this for initialization



	Animator anim;
	private float velocityY;
	private Rigidbody2D rb;
	public float threshold=50f;
	public bool isFalling=false;

    public LayerMask FloorLayerMask;

    public float RayCastDistance;
    public float FallingSpeed;

    private Collider2D m_collider2d;

    private bool m_bFalling;
	private void Awake() {
		anim=GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();

    }
    private bool _isHit()
    {
        bool bRes = false;
        RaycastHit2D[] _arrayHit = Physics2D.RaycastAll(transform.position, -transform.up, RayCastDistance, FloorLayerMask, 0);
        foreach (RaycastHit2D _hit in _arrayHit)
        {
            if (_hit.collider.gameObject != gameObject)
            {
                bRes = true;
                break;
            }
        }
        return bRes;
    }
	void Start ()
    {
        m_collider2d = GetComponent<Collider2D>();

        m_bFalling = !_isHit();
        _processFalling();
        PlayerControl1.Instance.swap.OnOverhead += HandleOverhead;
        PlayerControl1.Instance.swap.OnDrop += HandleDrop;
        GetComponent<Thing>().OnDie += OnDie;
    }
	void OnDie()
    {
        PlayerControl1.Instance.swap.OnOverhead -= HandleOverhead;
    }
    void HandleOverhead()
    {
        if (PlayerControl1.Instance.swap.col != null && gameObject != null && PlayerControl1.Instance.swap.col.gameObject == gameObject)
            PlayerControl1.Instance.overhead.SwitchState(OverheadState.Ironfoot);
    }

    void HandleDrop()
    {
        //if (PlayerControl1.Instance.swap.col.gameObject == gameObject)
            //PlayerControl1.Instance.overhead.SwitchState(OverheadState.None);
    }
    // Update is called once per frame
    void Update ()
    {
    }
    private void FixedUpdate()
    {
        bool bNewFalling = !_isHit();
        m_bFalling = bNewFalling;
        isFalling=m_bFalling;
        _processFalling();
        if (rb.velocity.y < 0f)
        {
            //originalGravity = playerBody.gravityScale;
            //playerBody.gravityScale = 0;
            rb.velocity = new Vector2(rb.velocity.x, -100f);
            //switched = true;
        }
    }
    private void _processFalling()
    {
        if( m_bFalling == true )
        {
            anim.CrossFade("Enemy_IronFoot", 0.01f);
            //rb.gravityScale = 0;
            //rb.drag = 0f;
            //rb.velocity = new Vector2(rb.velocity.x, -FallingSpeed);
        }
        else
        {
            anim.CrossFade("Enemy_IronFoot_idle", 0.01f);
            //rb.isKinematic = false;
            //rb.gravityScale = 200;
            //rb.drag = 0f;
        }
    }
 //   void Falling(bool isFall){
	//	if(isFall){
	//		anim.CrossFade("Enemy_IronFoot",0.01f);
	//		rb.gravityScale=0.1f;
	//		rb.drag=0.2f;
	//	}else
	//	{
	//		anim.CrossFade("Enemy_IronFoot_idle",0.01f);
	//		rb.gravityScale=200;
	//		rb.drag=0f;
	//	}
	//}



}
