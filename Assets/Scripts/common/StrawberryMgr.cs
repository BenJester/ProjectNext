using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class StrawberryInScene
{
    public string SceneName;
    public List<bool> LstStrawberryInScene;
    public List<bool> LstStrawberryInit;
    public int IndexOfStrawBerry;

    public int GetCurrentIndex()
    {
        return IndexOfStrawBerry++;
    }
    public void ResetIndex()
    {
        IndexOfStrawBerry = 0;
    }
    public void TakeStrawberry(int nIdx)
    {
        LstStrawberryInScene[nIdx] = true;
    }
    public void InitStrawBerry(int nIdx)
    {
        LstStrawberryInit[nIdx] = true;
    }
    public bool IsStrawBerryInit(int nIdx)
    {
        return LstStrawberryInit[nIdx];
    }
    public bool IsIndexTaken(int nStrawberryIdx)
    {
        int nIdx = 0;
        foreach( bool bTaken in LstStrawberryInScene)
        {
            if( nIdx == nStrawberryIdx )
            {
                if( bTaken == true)
                {
                    return true;
                }
            }
            nIdx++;
        }
        return false;
    }
}
public class StrawberryMgr : MonoBehaviour
{
    public static StrawberryMgr instance;
    public List<StrawberryInScene> LstScenes;


    private StrawberryInScene m_curScene;
    private string m_strSceneName;
    private int m_nStrawberryCount;
    private int m_nMaxStrawberryCount;
    private UnityAction<int, int> m_actStrawBerry;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddStrawBerry()
    {
        m_nStrawberryCount++;
    }
    public int GetStrawberryCounts()
    {
        return m_nStrawberryCount;
    }
    public void UpdateStrawberryCoutns(int nCounts)
    {
        m_nStrawberryCount = nCounts;
    }
    public int GetMaxCounts()
    {
        return m_nMaxStrawberryCount;
    }
    public void SetStrawBerryText()
    {
        if(m_actStrawBerry != null)
        {
            m_actStrawBerry(m_nStrawberryCount, m_nMaxStrawberryCount);
        }
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
            SceneManager.activeSceneChanged += OnSceneChange;
            LstScenes = new List<StrawberryInScene>();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool bFindScene = false;
        foreach(StrawberryInScene _scene in LstScenes)
        {
            if( _scene.SceneName.CompareTo(scene.name) == 0 )
            {
                bFindScene = true;
                _scene.ResetIndex();
                m_curScene = _scene;
                break;
            }
        }
        if(bFindScene == true)
        {

        }
        else
        {
            StrawberryInScene _newScene = new StrawberryInScene();
            _newScene.SceneName = scene.name;
            _newScene.LstStrawberryInScene = new List<bool>();
            _newScene.LstStrawberryInit = new List<bool>();

            Strawberry[] _lstStrawBerry = FindObjectsOfType<Strawberry>();
            foreach(Strawberry _strawberry in _lstStrawBerry)
            {
                _newScene.LstStrawberryInScene.Add(false);
                _newScene.LstStrawberryInit.Add(false);
            }
            LstScenes.Add(_newScene);
            m_curScene = _newScene;
        }

        //if(m_strSceneName != scene.name && m_strSceneName != null)
        //{
        //    Strawberry[] lstStrawberry = FindObjectsOfType<Strawberry>();
        //    foreach(Strawberry _strawberry in lstStrawberry)
        //    {
        //        Destroy(_strawberry.gameObject);
        //    }
        //}
        m_strSceneName = scene.name;
    }
    void OnSceneChange(Scene _scene1, Scene _scene2)
    {
        int nCounts = 0;
        Strawberry[] lstStrawberry = FindObjectsOfType<Strawberry>();
        foreach (Strawberry _strawberry in lstStrawberry)
        {
            if(_strawberry.sceneName == null || _strawberry.sceneName == "")
            {
                _strawberry.sceneName = _scene2.name;
                nCounts++;
            }
            else
            {
                if(_strawberry.sceneName.CompareTo( _scene2.name) != 0)
                {
                    Destroy(_strawberry.gameObject);
                }
            }
        }
        int a = 0;
        m_nMaxStrawberryCount = nCounts;
        SetStrawBerryText();
    }
    void OnSceneUnloaded(Scene scene)
    {
        Strawberry[] lstStrawberry = FindObjectsOfType<Strawberry>();
        foreach (Strawberry _strawberry in lstStrawberry)
        {
            int a = 0;
        }
    }
    public int GetCurrentIndex()
    {
        int nRetIndex = -1;
        if(m_curScene != null)
        {
            nRetIndex = m_curScene.GetCurrentIndex();
        }
        else
        {
            Debug.Assert(false);
        }
        return nRetIndex;
    }

    public bool IsIndexHasBeenTake(int nIndex)
    {
        bool bres = false;
        bres = m_curScene.IsIndexTaken(nIndex);
        return bres;
    }
    public void TakeStrawberry(int nIndex)
    {
        m_curScene.TakeStrawberry(nIndex);
    }
    public void InitStrawberry(int nIdx)
    {
        m_curScene.InitStrawBerry(nIdx);
    }

    public bool IsSceneStrawberryInit(int nIdx)
    {
        return m_curScene.IsStrawBerryInit(nIdx);
    }
    public void RegisteStrawberryUI(UnityAction<int, int> ua)
    {
        m_actStrawBerry += ua;
    }
    public void UnregisteStrawberryUI(UnityAction<int, int> ua)
    {
        m_actStrawBerry -= ua;
    }
}
