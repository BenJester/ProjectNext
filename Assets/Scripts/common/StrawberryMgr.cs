using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StrawberryMgr : MonoBehaviour
{
    public static StrawberryMgr instance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Strawberry[] lst = FindObjectsOfType<Strawberry>();
        int a = 0;
    }
    void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("OnSceneUnloaded: " + scene.name);
    }
}
