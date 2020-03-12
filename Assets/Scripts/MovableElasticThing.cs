using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ben;
public class MovableElasticThing : MonoBehaviour {
    // Start is called before the first frame update

    public Transform parent;
    private Vector2 originalPos;
    public float force=1f;
    private Rigidbody2D rb;

    private LineRenderer lr;

    
    void Awake()
    {
        rb =GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
    }
    void Start () {
        parent = transform.parent;
        originalPos = parent.position;
    }

    // Update is called once per frame
    void Update () {
        if ((Vector2) transform.position != originalPos) {
            lr.enabled = true;
            lr.SetPosition(0,originalPos);
            lr.SetPosition(1,transform.position);

            rb.velocity += (Vector2)(originalPos-(Vector2)transform.position).normalized * force;
            Vector2 vec = rb.velocity;
            Debug.Log(string.Format("{0}", vec.normalized));
        }
    }

  
    public void OnTriggerEnter2D(Collider2D col) {

		if (col.CompareTag ("thing")) {
			if (col.GetComponent<Thing> ().type != Type.box && col.GetComponent<Thing>().type != Type.invincible)
            {
                if (col.GetComponent<Enemy>() != null)
                {
                    col.GetComponent<Enemy>().TakeDamage(2);
                }
            }
            col.GetComponent<Thing>().TriggerMethod?.Invoke();

            //Deactivate();


        } else if (col.CompareTag ("player")) {
			
			PlayerControl1.Instance.Die ();
			//Deactivate ();
		}
		
	}
}