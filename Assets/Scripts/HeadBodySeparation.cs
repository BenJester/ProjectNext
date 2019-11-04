using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HeadBodySeparation : MonoBehaviour {

	public Sprite Head;
	public Sprite Body;
	public Sprite PlayerLegs;

    AudioSource audioSource;
    public AudioClip clip;

	void Start () {
        audioSource = gameObject.AddComponent<AudioSource>();
        clip = Resources.Load<AudioClip>("Sounds/Hit_SlimeSplat1");
    }

	// Update is called once per frame
	void Update () {

	}

	public void Dead (float force) {

		//print ("Dead");
		float x1 = Random.Range (-1f, 1f);
		float x2 = Random.Range (-1f, 1f);

		Vector2 randomV21 = new Vector2 (x1, 1).normalized;
		Vector2 randomV22 = new Vector2 (x2, 1).normalized;

		GameObject headPart = new GameObject ("Head");
		headPart.AddComponent<SpriteRenderer> ().sprite = Head;
		headPart.GetComponent<SpriteRenderer> ().sortingLayerName = "UpText";
		headPart.AddComponent<BoxCollider2D> ().isTrigger = true;
		headPart.AddComponent<SelfRotate>();
		headPart.transform.position = transform.position;
		Rigidbody2D rb1 = headPart.AddComponent<Rigidbody2D> ();
		rb1.AddForce (randomV21 * force);
		rb1.gravityScale = 100f;

		GameObject bodyPart = new GameObject ("Body");
		bodyPart.AddComponent<SpriteRenderer> ().sprite = Body;
		bodyPart.GetComponent<SpriteRenderer> ().sortingLayerName = "UpText";
		bodyPart.AddComponent<BoxCollider2D> ().isTrigger = true;
		bodyPart.AddComponent<SelfRotate>();
		bodyPart.transform.position = transform.position;
		Rigidbody2D rb2 = bodyPart.AddComponent<Rigidbody2D> ();
		rb2.AddForce (randomV22 * force);
		rb2.gravityScale = 100f;

		Destroy (headPart, 2.5f);
		Destroy (bodyPart, 2.5f);

        audioSource.PlayOneShot(clip, 0.5f);

    }

	public void PlayerDead (float force) {

		float x1 = Random.Range (-0.5f, 0.5f);

		float x2 = Random.Range (-.5f, .5f);
		float x3 = Random.Range (-.5f, .5f);
		//print (x1);
		//print (x2);
		//print (x3);

		Vector2 randomV21 = new Vector2 (x1, 1).normalized;
		Vector2 randomV22 = new Vector2 (x2, 1).normalized;
		Vector2 randomV23 = new Vector2 (x3, 1).normalized;

		GameObject headPart = new GameObject ("Head");
		headPart.AddComponent<SpriteRenderer> ().sprite = Head;
		headPart.GetComponent<SpriteRenderer> ().sortingLayerName = "UpText";
		headPart.AddComponent<BoxCollider2D> ().isTrigger = true;
		headPart.AddComponent<SelfRotate>();
		headPart.transform.position = transform.position;
		Rigidbody2D rb1 = headPart.AddComponent<Rigidbody2D> ();
		rb1.AddForce (randomV21 * force);
		rb1.gravityScale = 100f;

		GameObject bodyPart = new GameObject ("Body");
		bodyPart.AddComponent<SpriteRenderer> ().sprite = Body;
		bodyPart.GetComponent<SpriteRenderer> ().sortingLayerName = "UpText";
		bodyPart.AddComponent<BoxCollider2D> ().isTrigger = true;
		bodyPart.transform.position = transform.position;
		bodyPart.AddComponent<SelfRotate>();
		Rigidbody2D rb2 = bodyPart.AddComponent<Rigidbody2D> ();
		rb2.AddForce (randomV22 * force);
		rb2.gravityScale = 100f;

		GameObject legPart = new GameObject ("Leg");
		legPart.AddComponent<SpriteRenderer> ().sprite = PlayerLegs;
		legPart.GetComponent<SpriteRenderer> ().sortingLayerName = "UpText";
		legPart.AddComponent<BoxCollider2D> ().isTrigger = true;
		legPart.transform.position = transform.position;
		legPart.AddComponent<SelfRotate>();
		Rigidbody2D rb3 = legPart.AddComponent<Rigidbody2D> ();
		rb3.AddForce (randomV23 * force);
		rb3.gravityScale = 100f;

		Destroy (headPart, 3f);
		Destroy (bodyPart, 3f);
		Destroy (legPart, 3f);

	}
}