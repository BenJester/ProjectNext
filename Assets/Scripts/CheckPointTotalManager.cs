﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPointTotalManager : MonoBehaviour {

	public static CheckPointTotalManager instance;
	public Vector3 savedPos;
	public GameObject pivot;
    // Use this for initialization
    public int strawberryCount = 0;
    public int maxStrawberryCount = 0;
    public Text strawberryText;
	void Awake () {
		if (instance)
			Destroy (gameObject);
		if (!instance)instance=this;
		DontDestroyOnLoad(gameObject);
		savedPos=pivot.transform.position;
        SetStrawBerryText();

    }
    private void Start()
    {
        SetStrawBerryText();
    }

    public void SetStrawBerryText()
    {
        strawberryText.text = strawberryCount.ToString() + "/" + maxStrawberryCount.ToString();
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
