using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Swap : Skill {

	public bool swapDamageOn;
	public int swapDamage;
	public bool smokeOn;
	public bool damageEffectOn = true;

	public Collider2D col;

	public GameObject smokeParticle;
	public GameObject damageParticle;
	public Vector3 smokeOffset;
	public float scanBoxHeight;

	public override void Do () {
		if (!active || !col || col.GetComponent<Thing> ().dead)
			return;


		//就是放在这边，有些时候不知道为什么没有立刻触发
		StartCoroutine (SwapDamageEffect ());

		//屏幕震动	
		CameraShaker.Instance.ShakeOnce(10f,0.1f,0.02f,0.05f);

		ScanEnemies ();

		Rigidbody2D thingBody = col.gameObject.GetComponent<Rigidbody2D> ();
		Thing thing = col.gameObject.GetComponent<Thing> ();
		Vector3 pos = player.transform.position;
		Vector3 thingPos = col.transform.position;
		float playerRadiusY = player.GetComponent<BoxCollider2D> ().size.y / 2f;
		float heightDiff = (col.GetComponent<BoxCollider2D> ().size.y * col.transform.localScale.y - playerRadiusY * 2f) / 2f;

		if (thing.leftX < player.transform.position.x && thing.rightX > player.transform.position.x && thing.lowerY > player.transform.position.y && thing.lowerY < player.transform.position.y + playerRadiusY + 10f) {
			
			Vector3 temp = col.gameObject.transform.position;
			col.gameObject.transform.position = new Vector3 (player.transform.position.x, player.transform.position.y - playerRadiusY + (thing.upperY - thing.lowerY) / 2f, player.transform.position.z);
			player.transform.position = new Vector3 (temp.x, col.gameObject.transform.position.y + playerRadiusY + (thing.upperY - thing.lowerY) / 2f, player.transform.position.z);
			
			Smoke ();
		} else {
			
			Vector3 tempPos = new Vector3 (pos.x, pos.y + heightDiff, pos.z);
			player.transform.position = new Vector3 (thingPos.x, thingPos.y - heightDiff, thingPos.z);
			col.gameObject.transform.position = tempPos;
			PostEffectManager.instance.Blink (0.03f);
			print ("Exchange!");
			Smoke ();
		}

		Vector2 tempV = playerBody.velocity;
		playerBody.velocity = thingBody.velocity;
		thingBody.velocity = tempV;


	}
		
	void ScanEnemies () {
		if (!swapDamageOn)
			return;

		
		//Collider2D[] cols = Physics2D.OverlapAreaAll ((Vector2) (player.transform.position + scanBoxHeight / 2f * Vector3.up), (Vector2) (col.transform.position - scanBoxHeight / 2f * Vector3.down));
		Vector2 midPoint = (player.transform.position + col.transform.position) / 2f;
		Vector2 size = new Vector2 (Vector2.Distance ((Vector2)player.transform.position, (Vector2)col.transform.position)/2f, scanBoxHeight/2f);
		float angle = Vector2.SignedAngle (player.transform.position, col.transform.position);
		//RaycastHit2D[] cols = Physics2D.LinecastAll ((Vector2)player.transform.position, (Vector2)col.transform.position);

		Collider2D[] cols = Physics2D.OverlapBoxAll (midPoint, size, angle);

		foreach (var item in cols) {
			if (item == col)
				continue;
			Enemy enemy = item.GetComponent<Enemy> ();
			if (enemy != null) {
				enemy.TakeDamage (swapDamage);
			}
		}
	}

	void Smoke () {
		if (!smokeOn)
			return;
		Vector3 pos = player.transform.position;
		Vector3 thingPos = col.transform.position;
		GameObject par1 = Instantiate (smokeParticle, player.transform.position + smokeOffset, Quaternion.identity);
		GameObject par2 = Instantiate (smokeParticle, thingPos + smokeOffset, Quaternion.identity);
		Destroy (par1, 1f);
		Destroy (par2, 1f);
	}

	//刀光，加入伤害的粒子效果
	IEnumerator SwapDamageEffect () {
		if (damageEffectOn) {
			GameObject par3 =  Instantiate(damageParticle,transform.position,Quaternion.identity);
			
			yield return new WaitForSeconds (0.1f);
			par3.transform.SetParent(null);
			Destroy(par3,1f);
		} else {
			yield return null;
		}

	}
}