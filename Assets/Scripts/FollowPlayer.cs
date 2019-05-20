using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    
    public GameObject player;

    // Use this for initialization


    void Awake()
    {
        
        player = GameObject.FindGameObjectWithTag("player");
        transform.parent = null;
        transform.position = Vector3.zero;

    }
	void Start () {
        transform.position= player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = player.transform.position;
    }
}
