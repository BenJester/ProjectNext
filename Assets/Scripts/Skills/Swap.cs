using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swap : Skill {

	public bool swapDamageOn;
	public int swapDamage;
	public bool smokeOn;
	public Collider2D col;

	public GameObject dashParticle;
	public Vector3 smokeOffset;
	public float scanBoxHeight;

	public override void Do ()
	{
		if (!active || !col || col.GetComponent<Thing>().dead)
			return;

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

		} else {
			Vector3 tempPos = new Vector3 (pos.x, pos.y + heightDiff, pos.z);

			player.transform.position = new Vector3 (thingPos.x, thingPos.y - heightDiff, thingPos.z);
			col.gameObject.transform.position = tempPos;
			//交换的瞬间

			PostEffectManager.instance.Blink(0.03f);
		}

		Vector2 tempV = playerBody.velocity;
		playerBody.velocity = thingBody.velocity;
		thingBody.velocity = tempV;

		ScanEnemies ();
	}

	void ScanEnemies()
	{
		if (!swapDamageOn)
			return;
		Collider2D[] cols = Physics2D.OverlapAreaAll((Vector2) (player.transform.position + scanBoxHeight / 2f * Vector3.up), (Vector2) (col.transform.position - scanBoxHeight / 2f * Vector3.down));
		foreach (var item in cols) {
			if (item == col)
				continue;
			Enemy enemy = item.GetComponent<Enemy> ();
			if (enemy != null) {
				enemy.TakeDamage (swapDamage);
			}
		}
	}

	void Smoke()
	{
		if (!smokeOn)
			return;
		Vector3 pos = player.transform.position;
		Vector3 thingPos = col.transform.position;
		GameObject par1 = Instantiate(dashParticle,player.transform.position+smokeOffset,Quaternion.identity);
		GameObject par2 = Instantiate(dashParticle,thingPos+smokeOffset,Quaternion.identity);
		Destroy(par1,1f);
		Destroy(par2,1f);
	}
}
