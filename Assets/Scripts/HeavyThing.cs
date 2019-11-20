using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class HeavyThing : MonoBehaviour {

	// Use this for initialization

	public bool isFastFall;
	private float velocityY;
	private Rigidbody2D rb;
	public float threshold = 100;
	public float yOffset;
	public GameObject landingParticle;
	private void Awake() {
		rb=GetComponent<Rigidbody2D>();
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		velocityY=rb.velocity.y;
		if (velocityY<=-threshold)
		{
			isFastFall=true;
		}
		if (isFastFall && rb.velocity.y==0)
		{
            if(CameraShaker.Instance != null)
            {
                CameraShaker.Instance.ShakeOnce(15, 1f, 0.02f, 0.05f);
            }
			isFastFall=false;
			//print("HeavyThingLanding!");
			GameObject par = Instantiate(landingParticle,transform.position+new Vector3(0,yOffset,0),Quaternion.identity);
			Destroy(par,1f);
		}
	}
}
