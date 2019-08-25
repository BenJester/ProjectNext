using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour {

	public bool active;
	public GameObject player;
	public Rigidbody2D playerBody;
	public PlayerControl1 playerControl;

	void Start() {
		player = GameObject.FindWithTag ("player");
		playerControl = player.GetComponent<PlayerControl1> ();
		playerBody = player.GetComponent<Rigidbody2D> ();
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
