﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTestCapacity : MonoBehaviour {


	public float movespeed=2;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(1))
		{
			GetComponent<Rigidbody2D>().velocity += movespeed*(Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position).normalized;
		}
	}
}
