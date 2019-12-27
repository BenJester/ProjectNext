using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class CheckPointTotalManager : MonoBehaviour {

	public static CheckPointTotalManager instance;
	private Vector3 savedPos;
	public GameObject pivot;

	void Awake () {
		if (instance)
        {
			Destroy (gameObject);
        }
        else
        {
            instance = this;
            UIStrawberry _strawberry = FindObjectOfType<UIStrawberry>();
            if(_strawberry != null )
            {
                _strawberry.RegisteLate();
            }
            DontDestroyOnLoad(gameObject);
            savedPos = pivot.transform.position;
        }
    }
    private void Start()
    {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled 
        Application.targetFrameRate = 60;
    }
	// Update is called once per frame
	void Update () {
		
	}

	public void SaveRespawnPosition(Vector3 playerArrive){
		savedPos=playerArrive;
	}

	public Vector3 GetPlayerPos(){
		return savedPos;
	}

    public void SetPlayerPos(Vector3 _vecPos)
    {
        savedPos = _vecPos;
    }
}
