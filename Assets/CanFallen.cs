using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanFallen : MonoBehaviour
{
    // Start is called before the first frame update
    public float dropHurtSpeed = 50f;
    public bool canHurt = true;
    public bool isEnemy=true;
    protected Enemy enemy;



    public bool grounded;
    protected Vector2 groundCheckTopLeft;
    protected BoxCollider2D box;
    protected Vector2 groundCheckBottomRight;
    protected float groundCheckBoxIndent = 2f;
    public float groundCheckBoxHeight = 60f;
    public LayerMask floorLayer = 8;
    public LayerMask enemyLayer=10;
    public LayerMask playerLayer=9;
    private Thing thing;
    private SpriteRenderer SR;
    

    void Awake()
    {
        thing = GetComponent<Thing>();
        SR = GetComponent<SpriteRenderer>();

        box = GetComponent<BoxCollider2D>();
        if(isEnemy){
            enemy = GetComponent<Enemy>();
        }
    }
    void Start()
    {
        groundCheckTopLeft = new Vector2
                                 (
                                    -(box.size.x / 2f - groundCheckBoxIndent),
                                    -(box.size.y / 2f - groundCheckBoxHeight / 2f)
                                 );
        groundCheckBottomRight = new Vector2
                                 (
                                    box.size.x / 2f - groundCheckBoxIndent,
                                    -(box.size.y / 2f + groundCheckBoxHeight / 2f)
                                 );
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.OverlapArea
        (
            (Vector2)transform.position + groundCheckTopLeft,
            (Vector2)transform.position + groundCheckBottomRight,
            floorLayer
         );

         if(thing.prevVelocity.y<-dropHurtSpeed){
             SR.color=Color.red;
         }else{
             SR.color = Color.white;
         }
    }


    void OnCollisionEnter2D (Collision2D col) {
        if (thing.prevVelocity.y < -dropHurtSpeed && canHurt && grounded) {
			enemy.TakeDamage(1);
		}
	}

}
