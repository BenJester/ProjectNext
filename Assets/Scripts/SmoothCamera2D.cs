using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SmoothCamera2D : MonoBehaviour {

	public float dampTime;
	public Vector3 offset;
	private Vector3 velocity = Vector3.zero;
	Transform target;
    public bool followY;
    bool init;
	// Update is called once per frame
	void Init () {
        if (init) return;
        init = true;
        GameObject objPlayer = GameObject.FindWithTag("player");
        PlayerControl1 _playerCtrl = objPlayer.GetComponent<PlayerControl1>();
        _playerCtrl.RegisteDieAction(PlayerDieAction);
        target = objPlayer.transform;
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }
	void Update () {
        Init();
		//跟随玩家移动
		if (target) {
			Vector3 point = Camera.main.WorldToViewportPoint (target.position);
			Vector3 delta = target.position - Camera.main.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
			Vector3 destination = transform.position + delta;
			Vector3 damp = Vector3.SmoothDamp (transform.position, destination + offset, ref velocity, dampTime);
			transform.position = new Vector3 (damp.x, followY ? damp.y : offset.y, damp.z);
		}

	}

    public void PlayerDieAction(PlayerControl1 _ctrl)
    {
        target = null;
        transform.position = new Vector3(CheckPointTotalManager.instance.GetPlayerPos().x, CheckPointTotalManager.instance.GetPlayerPos().y, transform.position.z);
    }

	public void offsetLery (Vector3 offsetTarget) {
		StartCoroutine (lerpv3 (offset, offsetTarget));
	}

	IEnumerator lerpv3 (Vector3 offsetit, Vector3 offsetTarget) {
		if (offsetit != offsetTarget) {
			offsetit = offsetTarget;
			offset = offsetTarget;
//			offsetit = Vector3.Lerp (offsetit, offsetTarget, 0.1f);
			yield return new WaitForSeconds (0.02f);
//			offset = offsetit;

		}
	}
}