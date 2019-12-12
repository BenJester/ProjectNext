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
    // Use this for initialization
    public int strawberryCount = 0;
    public int maxStrawberryCount = 0;

    private UnityAction<int, int> m_actStrawBerry;
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

            SetStrawBerryNum();
            SetStrawBerryText();
        }
    }
    private void Start()
    {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled 
        Application.targetFrameRate = 60;
        SetStrawBerryNum();
        SetStrawBerryText();
    }

    public void SetStrawBerryText()
    {
        if(m_actStrawBerry != null)
        {
            m_actStrawBerry.Invoke(strawberryCount, maxStrawberryCount);
        }
    }
    public void RegisteStrawberryUI(UnityAction<int,int> ua)
    {
        m_actStrawBerry += ua;
    }
    public void UnregisteStrawberryUI(UnityAction<int, int> ua)
    {
        m_actStrawBerry -= ua;
    }
    public void DescreaseAndUpdate()
    {
        maxStrawberryCount--;
        SetStrawBerryText();
    }
	private void SetStrawBerryNum()
    {
        Strawberry[] strawberries = FindObjectsOfType<Strawberry>();
        maxStrawberryCount=strawberries.Length;
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
