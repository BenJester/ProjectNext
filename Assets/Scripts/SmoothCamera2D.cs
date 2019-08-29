using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SmoothCamera2D : MonoBehaviour {

	public float dampTime;
	public Vector3 offset;
	private Vector3 velocity = Vector3.zero;
	Transform target;

	// Update is called once per frame
	void Start() 
	{
		target = GameObject.FindWithTag ("player").transform;
		transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
	}
	void Update () 
	{
		if (target)
		{
			Vector3 point = Camera.main.WorldToViewportPoint(target.position);
			Vector3 delta = target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			Vector3 damp = Vector3.SmoothDamp (transform.position, destination + offset, ref velocity, dampTime);
			transform.position = new Vector3(damp.x, offset.y, damp.z);
		}

	}
}