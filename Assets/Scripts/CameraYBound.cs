using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraYBound : MonoBehaviour {

	public Vector3 offset;
	public float height;
	public float weight;

	[HideInInspector]
	public SmoothCamera2D followCamera;

	private void Awake () {
		followCamera = Camera.main.GetComponent<SmoothCamera2D> ();
		//offset = offset+transform.position;

	}
	void Start () {
		GetComponent<BoxCollider2D> ().size = new Vector2 (weight, height);
	}

	void Update () {

	}

	private void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "player") {
			print ("PlayerEnter!");
			followCamera.offsetLery (followCamera.offset + offset);
		}
	}

	private void OnTriggerExit2D (Collider2D other) {
		if (other.tag == "player") {
			print ("PlayerExit!");
			followCamera.offsetLery (followCamera.offset - offset);
		}
	}

	void OnDrawGizmos () {
		Gizmos.DrawLine (transform.position + new Vector3 ((weight / 2), (height / 2), 0), transform.position + new Vector3 (-(weight / 2), (height / 2), 0));
		Gizmos.DrawLine (transform.position + new Vector3 ((weight / 2), (height / 2), 0), transform.position + new Vector3 ((weight / 2), -(height / 2), 0));
		Gizmos.DrawLine (transform.position + new Vector3 (-(weight / 2), -(height / 2), 0), transform.position + new Vector3 (-(weight / 2), (height / 2), 0));
		Gizmos.DrawLine (transform.position + new Vector3 (-(weight / 2), -(height / 2), 0), transform.position + new Vector3 ((weight / 2), -(height / 2), 0));
	}
}