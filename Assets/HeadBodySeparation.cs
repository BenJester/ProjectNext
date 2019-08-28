using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBodySeparation : MonoBehaviour {

	public Sprite Head;
	public Sprite Body;
	public Sprite PlayerLegs;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Dead(float force){

		print("Dead");
		Vector2 randomV21=Vector2.up+new Vector2((float)Random.Range(-1,1),(float)Random.Range(-1,0)).normalized;
		Vector2 randomV22=Vector2.up+new Vector2((float)Random.Range(-1,1),(float)Random.Range(-1,0)).normalized;

		GameObject headPart  = new GameObject("Head");
		headPart.AddComponent<SpriteRenderer>().sprite= Head;
		headPart.GetComponent<SpriteRenderer>().sortingLayerName="UpText";
		headPart.AddComponent<BoxCollider2D>().isTrigger=true;
		headPart.transform.position=transform.position;
		Rigidbody2D rb1 = headPart.AddComponent<Rigidbody2D>();
		rb1.AddForce(randomV21*force);
		rb1.gravityScale = 100f;

		GameObject bodyPart  = new GameObject("Body");
		bodyPart.AddComponent<SpriteRenderer>().sprite= Body;
		bodyPart.GetComponent<SpriteRenderer>().sortingLayerName="UpText";
		bodyPart.AddComponent<BoxCollider2D>().isTrigger=true;
		bodyPart.transform.position=transform.position;
		Rigidbody2D rb2 = bodyPart.AddComponent<Rigidbody2D>();
		rb2.AddForce(randomV22*force);
		rb2.gravityScale = 100f;

		Destroy(headPart,1f);
		Destroy(bodyPart,1f);

	}

		public void PlayerDead(float force){

		print("Dead");
		Vector2 randomV21=Vector2.up+new Vector2((float)Random.Range(-1,1),(float)Random.Range(-1,0)).normalized;
		Vector2 randomV22=Vector2.up+new Vector2((float)Random.Range(-1,1),(float)Random.Range(-1,0)).normalized;
		Vector2 randomV23=Vector2.up+new Vector2((float)Random.Range(-1,1),(float)Random.Range(-1,0)).normalized;

		GameObject headPart  = new GameObject("Head");
		headPart.AddComponent<SpriteRenderer>().sprite= Head;
		headPart.GetComponent<SpriteRenderer>().sortingLayerName="UpText";
		headPart.AddComponent<BoxCollider2D>().isTrigger=true;
		headPart.transform.position=transform.position;
		Rigidbody2D rb1 = headPart.AddComponent<Rigidbody2D>();
		rb1.AddForce(randomV21*force);
		rb1.gravityScale = 100f;

		GameObject bodyPart  = new GameObject("Body");
		bodyPart.AddComponent<SpriteRenderer>().sprite= Body;
		bodyPart.GetComponent<SpriteRenderer>().sortingLayerName="UpText";
		bodyPart.AddComponent<BoxCollider2D>().isTrigger=true;
		bodyPart.transform.position=transform.position;
		Rigidbody2D rb2 = bodyPart.AddComponent<Rigidbody2D>();
		rb2.AddForce(randomV22*force);
		rb2.gravityScale = 100f;

		GameObject legPart  = new GameObject("Leg");
		legPart.AddComponent<SpriteRenderer>().sprite= PlayerLegs;
		legPart.GetComponent<SpriteRenderer>().sortingLayerName="UpText";
		legPart.AddComponent<BoxCollider2D>().isTrigger=true;
		legPart.transform.position=transform.position;
		Rigidbody2D rb3 = legPart.AddComponent<Rigidbody2D>();
		rb3.AddForce(randomV23*force);
		rb3.gravityScale = 100f;

		Destroy(headPart,1f);
		Destroy(bodyPart,1f);
		Destroy(legPart,1f);

	}
}
