﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour {

	public bool active;
	[HideInInspector]
	public GameObject player;
	[HideInInspector]
	public Rigidbody2D playerBody;
	[HideInInspector]
	public PlayerControl1 playerControl;


	private void Awake() {
		player = GameObject.FindWithTag ("player");
		playerControl = player.GetComponent<PlayerControl1> ();
		playerBody = player.GetComponent<Rigidbody2D> ();
	}
	void Start() {
		
		Init ();
	}

	// 执行
	public virtual void Do(){
	}

	// 检查是否可以执行
	public virtual bool Check(){
		return false;
	}

	public virtual void Init(){
	}
}
