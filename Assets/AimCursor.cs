using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCursor : MonoBehaviour {

	// Use this for initialization

	private Vector3 Pos;
	public bool isAim=false;
	public Sprite normalState;
	public Sprite aimState;
	private SpriteRenderer sr;
	private void Awake() {
		sr=GetComponent<SpriteRenderer>();
		
    	Cursor.visible = false;
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Pos=Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Pos.z=0;
		transform.position=Pos;
		
	}


	//通过传入切换图标
	public void SetAim(bool aim){
		isAim =  aim;
		if (isAim)
		{
			sr.sprite = aimState;
		}else
		{
			sr.sprite = normalState;
		}
	}
}
