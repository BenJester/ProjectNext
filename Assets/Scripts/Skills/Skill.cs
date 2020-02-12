using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour {

	public bool active;
	[HideInInspector]
	public GameObject player;
	[HideInInspector]
	public Rigidbody2D playerBody;
    public BoxCollider2D box;
	[HideInInspector]
	public PlayerControl1 playerControl;
    protected float gravity;
    protected AudioSource audioSource;
    public AudioClip clip;

	private void Awake() {
		player = GameObject.FindWithTag ("player");
		playerControl = player.GetComponent<PlayerControl1> ();
		playerBody = player.GetComponent<Rigidbody2D> ();
        box = player.GetComponent<BoxCollider2D>();
        gravity = playerBody.gravityScale;
        audioSource = GetComponent<AudioSource>();
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
