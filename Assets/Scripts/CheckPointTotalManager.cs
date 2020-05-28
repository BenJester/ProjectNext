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
    public List<GameObject> checkpoints;
    public AreaManager[] checkpointsByOrder;
    public List<AreaManager> checkpointsByOrderList;
    public int index;
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
            if(PlayerControl1.Instance != null)
            {
                PlayerControl1.Instance.transform.position = savedPos;
            }
        }
        if (checkpointsByOrder.Length == 0)
        {
            checkpointsByOrder = pivot.transform.parent.GetComponentsInChildren<AreaManager>();
            foreach (var item in checkpointsByOrder)
            {
                checkpointsByOrderList.Add(item);
            }
        }
        
    }
    private void Start()
    {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled 
        Application.targetFrameRate = 60;
    }
	// Update is called once per frame
	void Update () {
        HandleSkipLevel();

    }

    void HandleSkipLevel()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (index == 0) return;
            index -= 1;
            PlayerControl1.Instance.transform.position = checkpointsByOrderList[index].transform.position;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (index == checkpointsByOrderList.Count - 1) return;
            index += 1;
            PlayerControl1.Instance.transform.position = checkpointsByOrderList[index].transform.position;
        }
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
