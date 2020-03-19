using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ben;
public class Box : MonoBehaviour {

	public float killDropSpeed;
	public float killRange;
	Rigidbody2D body;
	Thing _boxThing;
	Vector2 prevVelocity;
    public int damage;
    public bool useDamage;
    public Color spikeColor;

	void Start () {
		body = GetComponent<Rigidbody2D> ();
		_boxThing = GetComponent<Thing> ();	
	}
	
	void FixedUpdate () {
        StartCoroutine(Late());
	}

    IEnumerator Late()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();
        prevVelocity = body.velocity;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("thing"))
        {
            Thing colThing = col.gameObject.GetComponent<Thing>();
            //这里逻辑有点奇怪
            //if (colThing.type == Type.enemy && prevVelocity.y < -killDropSpeed && _boxThing.GetLowerY() <= colThing.GetUpperY() + killRange) {
            if (body.gravityScale > 0)
            {
                if (colThing.type == Type.enemy && prevVelocity.y < -killDropSpeed)//&& _boxThing.GetLowerY() >= colThing.GetUpperY())
                {
                    if (colThing.GetLowerY() > _boxThing.GetLowerY())
                    {
                        //碰撞物在箱子上面。就不处理。
                    }
                    else
                    {
                        if (useDamage && colThing.GetComponent<Enemy>() != null)
                        {
                            //colThing.hasShield = false;
                            //colThing.GetComponent<Enemy>().canBeDamagedByKunaiDash = true;
                            colThing.GetComponent<Enemy>().TakeDamage(damage);
                            body.velocity = new Vector2(400f, 500f);
                        }
                        else
                            colThing.Die();

                    }
                }
                else
                {
                    //Debug.Assert(false);
                }
                if (body != null)
                {
                    //body.velocity = prevVelocity;
                }
            }
            else
            {
                if (colThing.type == Type.enemy && prevVelocity.y > killDropSpeed)
                {
                    if (colThing.GetUpperY() < _boxThing.GetUpperY())
                    {
                        //碰撞物在箱子上面。就不处理。
                    }
                    else
                    {
                        if (useDamage && colThing.GetComponent<Enemy>() != null)
                        {
                            //colThing.hasShield = false;
                            //colThing.GetComponent<Enemy>().canBeDamagedByKunaiDash = true;
                            colThing.GetComponent<Enemy>().TakeDamage(damage);
                            body.velocity = new Vector2(400f, 500f);
                        }
                        else
                            colThing.Die();

                    }
                }
                else
                {
                    //Debug.Assert(false);
                }
                if (body != null)
                {
                    //body.velocity = prevVelocity;
                }
            }

        }
    }
    void OnCollisionStay2D(Collision2D col) {
		if (col.gameObject.CompareTag("thing")) {
			Thing colThing = col.gameObject.GetComponent<Thing> ();
            //这里逻辑有点奇怪
            //if (colThing.type == Type.enemy && prevVelocity.y < -killDropSpeed && _boxThing.GetLowerY() <= colThing.GetUpperY() + killRange) {
            if (body.gravityScale > 0)
            {
                if (colThing.type == Type.enemy && prevVelocity.y < -killDropSpeed )//&& _boxThing.GetLowerY() >= colThing.GetUpperY())
                {
                    if (colThing.GetLowerY() > _boxThing.GetLowerY())
                    {
                        //碰撞物在箱子上面。就不处理。
                    }
                    else
                    {
                        if (useDamage && colThing.GetComponent<Enemy>() != null)
                        {
                            //colThing.hasShield = false;
                            //colThing.GetComponent<Enemy>().canBeDamagedByKunaiDash = true;
                        }
                        else
                            colThing.Die();

                    }
                }
                else
                {
                    //Debug.Assert(false);
                }
                if (body != null)
                {
                    //body.velocity = prevVelocity;
                }
            }
            else
            {
                if (colThing.type == Type.enemy && prevVelocity.y > killDropSpeed)
                {
                    if (colThing.GetUpperY() < _boxThing.GetUpperY())
                    {
                        //碰撞物在箱子上面。就不处理。
                    }
                    else
                    {
                        if (useDamage && colThing.GetComponent<Enemy>() != null)
                        {
                            colThing.hasShield = false;
                            colThing.GetComponent<Enemy>().canBeDamagedByKunaiDash = true;
                        }
                        else
                            colThing.Die();

                    }
                }
                else
                {
                    //Debug.Assert(false);
                }
                if (body != null)
                {
                    //body.velocity = prevVelocity;
                }
            }
            
		}
	}

    public void GetSpike()
    {
        GetComponent<SpriteRenderer>().color = spikeColor;

    }

    public void OutSpike()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
