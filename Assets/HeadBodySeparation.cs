﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBodySeparation : MonoBehaviour {

	public Sprite Head;
	public Sprite Body;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Dead(float force){
		
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
}
