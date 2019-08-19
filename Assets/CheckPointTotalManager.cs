using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTotalManager : MonoBehaviour {

	public static CheckPointTotalManager instance;
	public Vector3 savedPos;
	public GameObject pivot;
	// Use this for initialization
	private void Awake() {
		savedPos=pivot.transform.position;
		
	}
	void Start () {
		if (!instance)instance=this;
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SaveRespawnPosition(Vector3 playerArrive){
		savedPos=playerArrive;
	}

	public Vector3 SetPlayerPos(){
		return savedPos;
	}
}
