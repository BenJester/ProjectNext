using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        if(m_strSceneName != scene.name && m_strSceneName != null)
        {
            Strawberry[] lstStrawberry = FindObjectsOfType<Strawberry>();
            foreach(Strawberry _strawberry in lstStrawberry)
            {
                Destroy(_strawberry.gameObject);
            }
        }
        m_strSceneName = scene.name;
    }
    void OnSceneUnloaded(Scene scene)
    {
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
}
